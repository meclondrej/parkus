using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;

namespace parkus
{
    public class RemoteKeycard
    {
        public static bool HasKeycardPermission(Player player, KeycardPermissions perms)
        {
            // strips ScpOverride off of the required perms, not meant for cards
            KeycardPermissions stripped_perms = perms & ~KeycardPermissions.ScpOverride;
            return player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(stripped_perms));
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed
                || ev.Door.Base.ActiveLocks > 0
                || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions);
        }

        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.IsAllowed
                || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, KeycardPermissions.AlphaWarhead);
        }

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.IsAllowed
                || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, ev.Generator.Base._requiredPermission);
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.IsAllowed
                || ev.Chamber == null
                || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, ev.Chamber.RequiredPermissions);
        }
    }
}