using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Doors;

namespace parkus.Features
{
public class LockableDoors
{
    public bool OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (ev.Player.CurrentItem == null
                || ev.Player.CurrentItem.Type != ItemType.Coin
                || ev.Door.IsElevator
                || ev.Door.IsPartOfCheckpoint
                || ev.Door.IsLocked
                || (ev.Door is BreakableDoor breakableDoor && breakableDoor.IsDestroyed)
                || (!ev.Door.IsFullyOpen && !ev.Door.IsFullyClosed)
                || (!ev.IsAllowed && !RemoteKeycard.HasKeycardPermission(ev.Player, ev.Door.KeycardPermissions)))
            return true;
        ev.IsAllowed = false;
        if (ev.Door.IsFullyOpen)
            ev.Door.IsOpen = false;
        ev.Door.Lock(30, Exiled.API.Enums.DoorLockType.NoPower);
        ev.Player.RemoveItem(ev.Player.CurrentItem);
        return false;
    }
}
}