using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Handlers = Exiled.Events.Handlers;

namespace parkus.Features
{
    public class ConnectionStatusBroadcast : IHandler
    {
        public void OnPreAuthenticating(PreAuthenticatingEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast("Hráč se připojuje...", 10), true);
        }

        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast($"{ev.Player.Nickname} se připojil do hry.", 10), true);
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast($"{ev.Player.Nickname} opustil hru.", 10), true);
        }

        public void RegisterEvents()
        {
            Handlers.Player.PreAuthenticating += OnPreAuthenticating;
            Handlers.Player.Joined += OnPlayerJoined;
            Handlers.Player.Left += OnPlayerLeft;
        }

        public void UnregisterEvents()
        {
            Handlers.Player.PreAuthenticating -= OnPreAuthenticating;
            Handlers.Player.Joined -= OnPlayerJoined;
            Handlers.Player.Left -= OnPlayerLeft;
        }
    }
}