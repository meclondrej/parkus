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
            Messager.Send(
                $"Zabil jsi {ev.Player.DisplayNickname}! Killstreak: {entries[ev.Attacker.Id]}",
                ev.Attacker,
                3000
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
            Messager.Send($"Killstreak: {entries[ev.Player.Id]}", ev.Player, 10000);
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
