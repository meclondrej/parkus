using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public class ConnectionStatusBroadcast
    {
        public void OnPreAuthenticating(PreAuthenticatingEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast("Hráč se připojuje...", 10), true);
        }

        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast($"{ev.Player.Nickname} se připojil do hry.", 10), true);
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            foreach (Player player in Player.List)
                player.Broadcast(new Exiled.API.Features.Broadcast($"{ev.Player.Nickname} opustil hru.", 10), true);
        }
    }
}