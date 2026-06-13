using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace parkus.Features
{
    public static class Killcounter
    {
        private static readonly Dictionary<int, uint> entries = new Dictionary<int, uint>();

        public static void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player.Id == ev.Attacker.Id)
                return;
            if (entries.ContainsKey(ev.Attacker.Id))
                entries[ev.Attacker.Id]++;
            else
                entries.Add(ev.Attacker.Id, 1);
            ev.Attacker.Broadcast(
                new Exiled.API.Features.Broadcast(
                    $"Killstreak: {entries[ev.Attacker.Id]}\n+1 Kill",
                    3
                )
            );
        }

        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (
                !entries.ContainsKey(ev.Player.Id)
                || (
                    ev.NewRole != RoleTypeId.Spectator
                    && ev.Player.Role.Type != RoleTypeId.Spectator
                )
            )
                return;
            ev.Player.Broadcast(
                new Exiled.API.Features.Broadcast($"Killstreak: {entries[ev.Player.Id]}", 10)
            );
            entries.Remove(ev.Player.Id);
        }

        public static void OnPlayerLeft(LeftEventArgs ev)
        {
            entries.Remove(ev.Player.Id);
        }

        public static void OnRoundStarted()
        {
            entries.Clear();
        }
    }
}
