using Exiled.Events.EventArgs.Player;
using parkus.Features;

namespace parkus
{
    public class Handlers
    {
        private readonly Killcounter killcounter;
        private readonly ConnectionStatusBroadcast connectionStatusBroadcast;
        private readonly RemoteKeycard remoteKeycard;

        public Handlers()
        {
            killcounter = new Killcounter();
            connectionStatusBroadcast = new ConnectionStatusBroadcast();
            remoteKeycard = new RemoteKeycard();
        }

        private void OnPlayerJoined(JoinedEventArgs ev)
        {
            connectionStatusBroadcast.OnPlayerJoined(ev);
            killcounter.OnPlayerJoined(ev);
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

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Joined += OnPlayerJoined;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Player.PreAuthenticating += OnPreAuthenticating;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Joined -= OnPlayerJoined;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Player.PreAuthenticating -= OnPreAuthenticating;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
        }
    }
}