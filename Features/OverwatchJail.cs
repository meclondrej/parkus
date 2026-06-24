using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using PlayerRoles;

namespace parkus.Features
{
    public static class OverwatchJail
    {
        private static readonly Dictionary<string, string> jailedPlayers =
            new Dictionary<string, string>();

        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!jailedPlayers.ContainsKey(ev.Player.UserId) || ev.Reason == SpawnReason.ForceClass)
                return;
            ev.IsAllowed = false;
            if (ev.Player.Role.Type != RoleTypeId.Overwatch)
                ev.Player.Role.Set(RoleTypeId.Overwatch);
        }

        public static bool Jail(Player player)
        {
            if (jailedPlayers.ContainsKey(player.UserId))
                return false;
            jailedPlayers.Add(player.UserId, player.Nickname);
            player.Role.Set(RoleTypeId.Overwatch);
            return true;
        }

        public static bool Unjail(Player player)
        {
            if (!jailedPlayers.ContainsKey(player.UserId))
                return false;
            jailedPlayers.Remove(player.UserId);
            player.Role.Set(RoleTypeId.Spectator);
            return true;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetJailedPlayers()
        {
            return jailedPlayers.ToArray();
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class OverwatchJailCommand : ICommand, IUsageProvider
    {
        public string Command => "overwatchjail";

        public string[] Aliases => new string[] { "owj" };

        public string Description => "ovladani overwatch jailu";

        public string[] Usage =>
            new string[]
            {
                "overwatchjail list -> vylistuje jailnuty hrace",
                "overwatchjail jail <id/userid> -> jailne hrace",
                "overwatchjail unjail <id/userid> -> unjailne hrace",
            };

        private static bool TryGetPlayerFromIdentifier(string ident, out Player outPlayer)
        {
            if (int.TryParse(ident, out int id))
            {
                foreach (Player player in Player.List)
                    if (player.Id == id)
                    {
                        outPlayer = player;
                        return true;
                    }
            }
            foreach (Player player in Player.List)
                if (player.UserId == ident)
                {
                    outPlayer = player;
                    return true;
                }
            outPlayer = null;
            return false;
        }

        public bool Execute(
            ArraySegment<string> arguments,
            ICommandSender sender,
            out string response
        )
        {
            if (!sender.CheckPermission("parkus.overwatchjail"))
            {
                response = "nedostatecne permy";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "nedostatek parametru";
                return false;
            }

            switch (arguments.At(0))
            {
                case "list":
                    StringBuilder res = new StringBuilder();
                    foreach (KeyValuePair<string, string> kvp in OverwatchJail.GetJailedPlayers())
                        res.Append($"{kvp.Key}: {kvp.Value}\n");
                    response = res.ToString();
                    return true;
                case "jail":
                {
                    if (arguments.Count < 2)
                    {
                        response = "zadej hrace ktereho chces jailnout do parametru";
                        return false;
                    }
                    if (!TryGetPlayerFromIdentifier(arguments.At(1), out Player player))
                    {
                        response = "nelze najit hrace";
                        return false;
                    }
                    if (!OverwatchJail.Jail(player))
                    {
                        response = $"hrac {player.Nickname} uz je v overwatch jailu";
                        return false;
                    }
                    response = $"hrac {player.Nickname} presunut do overwatch jailu";
                    return true;
                }
                case "unjail":
                {
                    if (arguments.Count < 2)
                    {
                        response = "zadej hrace ktereho chces unjailnout do parametru";
                        return false;
                    }
                    if (!TryGetPlayerFromIdentifier(arguments.At(1), out Player player))
                    {
                        response = "nelze najit hrace";
                        return false;
                    }
                    if (!OverwatchJail.Unjail(player))
                    {
                        response = $"hrac {player.Nickname} neni v overwatch jailu";
                        return false;
                    }
                    response = $"hrac {player.Nickname} odstranen z overwatch jailu";
                    return true;
                }
                default:
                    response = "neplatny parametr. zadej list, jail nebo unjail";
                    return false;
            }
        }
    }
}
