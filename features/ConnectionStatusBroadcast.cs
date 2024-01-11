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
            Server.SendBroadcast("A player is joining...", 10, Broadcast.BroadcastFlags.Normal, true);
        }
        [PluginEvent]
        public void OnPlayerJoined(PlayerJoinedEvent ev)
        {
            Server.SendBroadcast($"{ev.Player.Nickname} joined the server.", 10, Broadcast.BroadcastFlags.Normal, true);
        }
    }
}