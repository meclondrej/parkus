using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public static class RemoteKeycard
    {
        public static bool HasKeycardPermission(Player player, KeycardPermissions perms)
        {
            // strips ScpOverride and invalid permission bits off of the required perms
            KeycardPermissions stripped_perms =
                perms & ~KeycardPermissions.ScpOverride & (KeycardPermissions)0x7FF;
            return player.Items.Any(item =>
                item is Keycard keycard
                && keycard.Permissions.HasFlag(stripped_perms)
                && keycard.Type != ItemType.SurfaceAccessPass
            );
        }

        public static void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed || ev.Door.Base.ActiveLocks > 0 || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, ev.Door.KeycardPermissions);
        }

        public static void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.IsAllowed || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(ev.Player, KeycardPermissions.AlphaWarhead);
        }

        public static void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.IsAllowed || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(
                ev.Player,
                (KeycardPermissions)ev.Generator.Base._requiredPermission
            );
        }

        public static void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.IsAllowed || ev.InteractingChamber == null || ev.Player.IsScp)
                return;
            ev.IsAllowed = HasKeycardPermission(
                ev.Player,
                ev.InteractingChamber.RequiredPermissions
            );
        }
    }
}
