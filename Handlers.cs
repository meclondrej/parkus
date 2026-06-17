using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using parkus.Features;

namespace parkus
{
    public static class Handlers
    {
        public static void OnDisabled()
        {
            RespawnTimer.OnDisabled();
        }

        private static void OnPlayerVerified(VerifiedEventArgs ev)
        {
            ConnectionStatusBroadcast.OnPlayerVerified(ev);
        }

        private static void OnPlayerLeft(LeftEventArgs ev)
        {
            ConnectionStatusBroadcast.OnPlayerLeft(ev);
            Killcounter.OnPlayerLeft(ev);
        }

        private static void OnPlayerDied(DiedEventArgs ev)
        {
            Killcounter.OnPlayerDied(ev);
            RespawnTimer.OnPlayerDied(ev);
            AutoReport.OnPlayerDied(ev);
        }

        private static void OnPreAuthenticating(PreAuthenticatingEventArgs ev)
        {
            ConnectionStatusBroadcast.OnPreAuthenticating();
        }

        private static void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!LockableDoors.OnInteractingDoor(ev))
                return;
            RemoteKeycard.OnInteractingDoor(ev);
        }

        private static void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            RemoteKeycard.OnActivatingWarheadPanel(ev);
        }

        private static void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            RemoteKeycard.OnUnlockingGenerator(ev);
        }

        private static void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            RemoteKeycard.OnInteractingLocker(ev);
        }

        private static void OnRoundStarted()
        {
            Killcounter.OnRoundStarted();
            RespawnTimer.OnRoundStarted();
        }

        private static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            RespawnTimer.OnRoundEnded();
        }

        private static void OnRestartingRound()
        {
            Messager.OnRestartingRound();
            RespawnTimer.OnRestartingRound();
        }

        private static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            OverwatchJail.OnChangingRole(ev);
            DefaultLoot.OnChangingRole(ev);
            Killcounter.OnChangingRole(ev);
        }

        private static void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            CoinGamble.OnFlippingCoin(ev);
        }

        private static void OnWaitingForPlayers()
        {
            Messager.OnWaitingForPlayers();
        }

        public static void RegisterEvents()
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
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        public static void UnregisterEvents()
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
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
        }
    }
}
