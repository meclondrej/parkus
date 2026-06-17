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
            Messager.SendToAll($"{ev.Player.Nickname} se připojil do hry.", 10000);
        }

        public static void OnPlayerLeft(LeftEventArgs ev)
        {
            Messager.SendToAll($"{ev.Player.Nickname} opustil hru.", 10000);
        }
    }
}
