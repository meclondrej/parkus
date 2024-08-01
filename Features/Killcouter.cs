using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public class Killcounter
    {
        private static readonly Dictionary<int, uint> entries = new Dictionary<int, uint>();

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            entries.Add(ev.Player.Id, 0);
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            if (entries.ContainsKey(ev.Player.Id))
                entries.Remove(ev.Player.Id);
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null
                && ev.Player.Id != ev.Attacker.Id
                && entries.ContainsKey(ev.Attacker.Id))
            {
                entries[ev.Attacker.Id]++;
                ev.Attacker.Broadcast(new Exiled.API.Features.Broadcast("+1 Kill", 3));
            }
            if (entries.ContainsKey(ev.Player.Id))
            {
                entries[ev.Player.Id] = 0;
                ev.Player.Broadcast(new Exiled.API.Features.Broadcast($"Killy: {entries[ev.Player.Id]}", 10));
            }
        }
    }
}