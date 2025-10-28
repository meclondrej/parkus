using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace parkus.Features
{
    public class OverwatchJail
    {
        public HashSet<string> jailedIds = new HashSet<string>();

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.Overwatch) // jail
                jailedIds.Add(ev.Player.UserId);
            else if (ev.Reason == SpawnReason.ForceClass) // unjail
                jailedIds.Remove(ev.Player.UserId);
            else if (jailedIds.Contains(ev.Player.UserId))
            { // keep player in jail
                ev.IsAllowed = false;
                ev.Player.Role.Set(RoleTypeId.Overwatch);
            }
        }
    }
}
