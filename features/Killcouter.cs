using System.Collections.Generic;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace parkus
{
    public class Killcounter
    {
        private static Dictionary<string, int> entries = new Dictionary<string, int>();

        [PluginEvent]
        public void OnPlayerJoined(PlayerJoinedEvent ev)
        {
            entries.Add(ev.Player.UserId, 0);
        }

        public void OnPlayerLeft(PlayerLeftEvent ev)
        {
            if (entries.ContainsKey(ev.Player.UserId))
                entries.Remove(ev.Player.UserId);
        }

        [PluginEvent]
        public void OnPlayerDeath(PlayerDeathEvent ev)
        {
            if (ev.Attacker == null || ev.Player.UserId == ev.Attacker.UserId)
                return;
            if (!entries.ContainsKey(ev.Attacker.UserId))
                return;
            entries[ev.Attacker.UserId]++;
            ev.Attacker.SendBroadcast("+1 Kill", 3);
            ev.Player.SendBroadcast($"Killy: {entries[ev.Player.UserId].ToString()}", 10);
            entries[ev.Player.UserId] = 0;
        }
    }
}