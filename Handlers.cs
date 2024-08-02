using Exiled.Events.EventArgs.Player;
using parkus.Features;

namespace parkus
{
    public class Handlers
    {
        private readonly Killcounter killcounter;
        private readonly ConnectionStatusBroadcast connectionStatusBroadcast;
        private readonly RemoteKeycard remoteKeycard;
        private readonly LockableDoors lockableDoors;

        public Handlers()
        {
            killcounter = new Killcounter();
            connectionStatusBroadcast = new ConnectionStatusBroadcast();
            remoteKeycard = new RemoteKeycard();
            lockableDoors = new LockableDoors();
        }

        private void OnPlayerVerified(VerifiedEventArgs ev)
        {
            connectionStatusBroadcast.OnPlayerVerified(ev);
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
            connectionStatusBroadcast.OnPlayerLeft(ev);
            killcounter.OnPlayerLeft(ev);
        }

        private void OnPlayerDied(DiedEventArgs ev)
        {
            killcounter.OnPlayerDied(ev);
        }

        private void OnPreAuthenticating(PreAuthenticatingEventArgs ev)
        {
            connectionStatusBroadcast.OnPreAuthenticating(ev);
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!lockableDoors.OnInteractingDoor(ev))
                return;
            remoteKeycard.OnInteractingDoor(ev);
        }

        private void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            remoteKeycard.OnActivatingWarheadPanel(ev);
        }

        private void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            remoteKeycard.OnUnlockingGenerator(ev);
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            remoteKeycard.OnInteractingLocker(ev);
        }

        private void OnRoundStarted()
        {
            killcounter.OnRoundStarted();
        }

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Verified += OnPlayerVerified;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Player.PreAuthenticating += OnPreAuthenticating;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnPlayerVerified;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Player.PreAuthenticating -= OnPreAuthenticating;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }
    }
}