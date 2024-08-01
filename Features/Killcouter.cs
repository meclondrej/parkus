using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using Handlers = Exiled.Events.Handlers;

namespace parkus.Features
{
    public class Killcounter : IHandler
    {
        private static readonly Dictionary<string, int> entries = new Dictionary<string, int>();

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            entries.Add(ev.Player.UserId, 0);
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            if (ev.Player.UserId == null)
                return;
            if (entries.ContainsKey(ev.Player.UserId))
                entries.Remove(ev.Player.UserId);
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null
                && ev.Player.UserId != ev.Attacker.UserId
                && entries.ContainsKey(ev.Attacker.UserId))
            {
                entries[ev.Attacker.UserId]++;
                ev.Attacker.Broadcast(new Exiled.API.Features.Broadcast("+1 Kill", 3));
            }
            if (entries.ContainsKey(ev.Player.UserId))
            {
                entries[ev.Player.UserId] = 0;
                ev.Player.Broadcast(new Exiled.API.Features.Broadcast($"Killy: {entries[ev.Player.UserId]}", 10));
            }
        }

        public void RegisterEvents()
        {
            Handlers.Player.Joined += OnPlayerJoined;
            Handlers.Player.Left += OnPlayerLeft;
            Handlers.Player.Died += OnPlayerDied;
        }

        public void UnregisterEvents()
        {
            Handlers.Player.Joined -= OnPlayerJoined;
            Handlers.Player.Left -= OnPlayerLeft;
            Handlers.Player.Died -= OnPlayerDied;
        }
    }
}