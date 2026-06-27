using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using MEC;

namespace parkus.Features
{
    public static class Messager
    {
        private readonly struct Message
        {
            public Message(Guid guid, string content, int[] playerIds, long expiry)
            {
                this.guid = guid;
                this.content = content;
                this.playerIds = playerIds;
                this.expiry = expiry;
            }

            public readonly Guid guid;
            public readonly string content;

            // null means broadcast
            public readonly int[] playerIds;

            // zero means forever
            public readonly long expiry;
        }

        private static readonly LinkedList<Message> messages = new LinkedList<Message>();
        private static CoroutineHandle? mainLoopHandle = null;

        public static Guid Send(
            string content,
            IEnumerable<Player> players,
            long durationMilliseconds
        )
        {
            Guid guid = Guid.NewGuid();
            long now = DateTime.Now.ToUniversalTime().Ticks / 10000;
            messages.AddLast(
                new Message(
                    guid,
                    content,
                    players.Select(p => p.Id).ToArray(),
                    now + durationMilliseconds
                )
            );
            foreach (Player player in players)
                RenderPlayer(player);
            return guid;
        }

        public static Guid Send(string content, Player player, long durationMilliseconds) =>
            Send(content, new[] { player }, durationMilliseconds);

        public static Guid SendToAll(string content, long durationMilliseconds)
        {
            Guid guid = Guid.NewGuid();
            long now = DateTime.Now.ToUniversalTime().Ticks / 10000;
            messages.AddLast(new Message(guid, content, null, now + durationMilliseconds));
            RenderAllPlayers();
            return guid;
        }

        public static bool Cancel(Guid guid)
        {
            for (LinkedListNode<Message> node = messages.First; node != null; node = node.Next)
                if (node.Value.guid == guid)
                {
                    messages.Remove(node);
                    if (node.Value.playerIds == null)
                        RenderAllPlayers();
                    foreach (
                        Player player in node
                            .Value.playerIds.Select(id => Player.Get(id))
                            .Where(p => p != null)
                    )
                        RenderPlayer(player);
                    return true;
                }
            return false;
        }

        public static void OnWaitingForPlayers()
        {
            if (mainLoopHandle.HasValue)
                return;
            mainLoopHandle = Timing.RunCoroutine(MainLoop());
        }

        public static void OnRestartingRound()
        {
            messages.Clear();
            RenderAllPlayers();
            if (!mainLoopHandle.HasValue)
                return;
            Timing.KillCoroutines(mainLoopHandle.Value);
            mainLoopHandle = null;
        }

        private const long MAXIMUM_REFRESH_TIME_MS = 100;
        private const float MAXIMUM_REFRESH_TIME_SEC = (float)MAXIMUM_REFRESH_TIME_MS / 1000;
        private const ushort VISIBLE_DURATION_SEC = 2;

        private static IEnumerator<float> MainLoop()
        {
            while (true)
            {
                long now = DateTime.Now.ToUniversalTime().Ticks / 10000;

                // time until the next refresh is needed
                // null if no refresh is needed
                long? time_to_next_update = null;
                foreach (Message msg in messages)
                {
                    if (msg.expiry == 0)
                        continue;
                    long time_to_msg_update = msg.expiry - now;
                    if (time_to_msg_update < 0)
                        continue;
                    if (
                        !time_to_next_update.HasValue
                        || time_to_next_update.Value > time_to_msg_update
                    )
                        time_to_next_update = time_to_msg_update;
                }

                // skip this loop if no update is needed
                if (!time_to_next_update.HasValue)
                {
                    yield return Timing.WaitForSeconds(MAXIMUM_REFRESH_TIME_SEC);
                    continue;
                }

                float secs =
                    time_to_next_update.Value > MAXIMUM_REFRESH_TIME_MS
                        ? MAXIMUM_REFRESH_TIME_SEC
                        : (float)time_to_next_update.Value / 1000;
                yield return Timing.WaitForSeconds(secs);

                // remove all expired messages
                for (LinkedListNode<Message> node = messages.First; node != null; )
                {
                    if (node.Value.expiry > now)
                    {
                        node = node.Next;
                        continue;
                    }
                    LinkedListNode<Message> expiredNode = node;
                    node = node.Next;
                    messages.Remove(expiredNode);
                }

                RenderAllPlayers();
            }
        }

        private static void RenderPlayer(Player player)
        {
            player.ClearBroadcasts();

            StringBuilder playerString = null;
            foreach (Message msg in messages)
                if (msg.playerIds == null || msg.playerIds.Contains(player.Id))
                    if (playerString == null)
                        playerString = new StringBuilder(msg.content);
                    else
                        playerString.Append('\n').Append(msg.content);

            if (playerString == null)
                return;

            player.Broadcast(
                new Exiled.API.Features.Broadcast(playerString.ToString(), VISIBLE_DURATION_SEC)
            );
        }

        private static void RenderAllPlayers()
        {
            // clear all players' broadcasts
            foreach (Player player in Player.List)
                player.ClearBroadcasts();

            // construct new broadcast strings
            Dictionary<Player, StringBuilder> playerStrings = new Dictionary<Player, StringBuilder>(
                Player.Count
            );
            foreach (Message msg in messages)
            foreach (
                Player player in msg.playerIds == null
                    ? Player.List
                    : msg.playerIds.Select(id => Player.Get(id)).Where(p => p != null)
            )
                if (!playerStrings.ContainsKey(player))
                    playerStrings.Add(player, new StringBuilder(msg.content));
                else
                    playerStrings[player].Append('\n').Append(msg.content);

            // display broadcast strings
            foreach (Player player in Player.List)
                if (playerStrings.ContainsKey(player))
                    player.Broadcast(
                        new Exiled.API.Features.Broadcast(
                            playerStrings[player].ToString(),
                            VISIBLE_DURATION_SEC
                        ),
                        true
                    );
        }
    }
}
