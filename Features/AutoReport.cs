using System;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace parkus.Features
{
public class AutoReport
{
    public static void Report(string msg)
    {
        Log.Info($"AUTOREPORT: {msg}");
    }

    public static bool IsSidePartOfGameplay(Side side)
    {
        switch (side)
        {
        case Side.Scp:
        case Side.Mtf:
        case Side.ChaosInsurgency:
            return true;
        default:
            return false;
        }
    }

    public void OnPlayerDied(DiedEventArgs ev)
    {
        // zombie suicide check
        if (ev.TargetOldRole == RoleTypeId.Scp0492
                && (ev.Attacker == ev.Player
                    || ev.Attacker == null))
            Report($"Hráč {ev.Player.DisplayNickname} se zabil jako SCP-049-2");

        // teamkill check
        if (ev.Attacker != null
                && ev.Attacker != ev.Player
                && ev.TargetOldRole.GetSide() == ev.Attacker.Role.Side
                && IsSidePartOfGameplay(ev.Attacker.Role.Side))
            Report($"Hráč {ev.Attacker.DisplayNickname} teamkillnul hráče {ev.Player.DisplayNickname}");

        // mic recording perms check
        if (ev.Attacker != null
                && ev.Attacker.Role.Type == RoleTypeId.Scp939
                && !ev.Player.AgreedToRecording)
            Report($"Hráč {ev.Player.DisplayNickname} nemá povolené nahrávání mikrofonu");
    }
}
}