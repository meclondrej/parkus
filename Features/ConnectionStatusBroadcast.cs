using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public static class ConnectionStatusBroadcast
    {
        public static void OnPreAuthenticating()
        {
            Messager.SendToAll("Hráč se připojuje...", 10000);
        }

        public static void OnPlayerVerified(VerifiedEventArgs ev)
        {
            foreach (Player player in Player.List)
                if (player.Id != ev.Player.Id)
                    Messager.Send($"{ev.Player.Nickname} se připojil do hry.", player, 10000);
        }

        public static void OnPlayerLeft(LeftEventArgs ev)
        {
            foreach (Player player in Player.List)
                if (player.Id != ev.Player.Id)
                    Messager.Send($"{ev.Player.Nickname} opustil hru.", player, 10000);
        }
    }
}
