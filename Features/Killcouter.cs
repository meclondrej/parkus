using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public class Killcounter
    {
        private static readonly Dictionary<int, uint> entries = new Dictionary<int, uint>();

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (!entries.ContainsKey(ev.Player.Id))
                entries.Add(ev.Player.Id, 0);
            ev.Player.Broadcast(new Exiled.API.Features.Broadcast($"Killstreak: {entries[ev.Player.Id]}", 10));
            entries[ev.Player.Id] = 0;
            if (ev.Attacker == null
                    || ev.Player.Id == ev.Attacker.Id)
                return;
            if (entries.ContainsKey(ev.Attacker.Id))
                entries[ev.Attacker.Id]++;
            else
                entries.Add(ev.Attacker.Id, 1);
            ev.Attacker.Broadcast(new Exiled.API.Features.Broadcast($"Killstreak: {entries[ev.Attacker.Id]}\n+1 Kill", 3));
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            if (entries.ContainsKey(ev.Player.Id))
                entries.Remove(ev.Player.Id);
        }

        public void OnRoundStarted()
        {
            entries.Clear();
        }
    }
}