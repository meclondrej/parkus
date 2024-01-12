using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace parkus.features
{
    public class JoinBroadcast
    {
        [PluginEvent]
        public void OnPlayerPreauth(PlayerPreauthEvent ev)
        {
            Server.SendBroadcast("Hráč se připojuje...", 10, Broadcast.BroadcastFlags.Normal, true);
        }
        [PluginEvent]
        public void OnPlayerJoined(PlayerJoinedEvent ev)
        {
            Server.SendBroadcast($"{ev.Player.Nickname} se připojil do hry.", 10, Broadcast.BroadcastFlags.Normal, true);
        }
        [PluginEvent]
        public void OnPlayerLeft(PlayerLeftEvent ev)
        {
            Server.SendBroadcast($"{ev.Player.Nickname} opustil hru.", 10, Broadcast.BroadcastFlags.Normal, true);
        }
    }
}