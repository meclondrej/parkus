using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using Respawning;
using Respawning.Waves;

namespace parkus.Features
{
    public static class RespawnTimer
    {
        private static CoroutineHandle timerHandle;

        private static IEnumerator<float> Timer()
        {
            while (true)
            {
                string hint = GenerateTimerText();
                foreach (Player player in Player.List.ToList().Where(p => p.IsDead))
                    player.ShowHint(hint, 1.2f);
                yield return Timing.WaitForSeconds(1f);
            }
        }

        private static void RunTimer()
        {
            if (timerHandle.IsRunning)
                return;
            timerHandle = Timing.RunCoroutine(Timer());
        }

        private static void CancelTimer()
        {
            if (!timerHandle.IsRunning)
                return;
            Timing.KillCoroutines(timerHandle);
            timerHandle = default;
        }

        public static void OnRoundStarted()
        {
            RunTimer();
        }

        public static void OnRoundEnded()
        {
            CancelTimer();
        }

        public static void OnRestartingRound()
        {
            CancelTimer();
        }

        public static void OnDisabled()
        {
            CancelTimer();
        }

        public static void OnPlayerDied(DiedEventArgs ev)
        {
            ev.Player.ShowHint(GenerateTimerText(), 1.0f);
        }

        private static string GetWaveRespawnText<T>()
            where T : TimeBasedWave
        {
            if (!TimedWave.TryGetTimedWave<T>(out TimedWave wave))
                return "--:--";
            TimeSpan span = wave.Timer.TimeLeft;
            if (wave.Faction == Faction.FoundationStaff)
                span += TimeSpan.FromSeconds(18);
            if (wave.Faction == Faction.FoundationEnemy)
                span += TimeSpan.FromSeconds(13);
            if (span < TimeSpan.Zero)
                return "--:--";
            return $"{span.TotalMinutes:00}:{span.Seconds:00}";
        }

        private static string GetWarheadText()
        {
            if (Warhead.IsLocked)
                return "zamčená";
            StringBuilder str = new StringBuilder();
            WarheadStatus warheadStatus = Warhead.Status;
            int time = 0;
            switch (warheadStatus)
            {
                case WarheadStatus.NotArmed:
                    str.Append("<color=#00FF00>zajištěná</color>");
                    time = (int)Warhead.RealDetonationTimer;
                    break;
                case WarheadStatus.Armed:
                    str.Append("<color=#FF8C00>odjištěná</color>");
                    time = (int)Warhead.RealDetonationTimer;
                    break;
                case WarheadStatus.InProgress:
                    str.Append("<color=#FF0000>aktivní</color>");
                    time = (int)Warhead.DetonationTimer;
                    break;
                case WarheadStatus.Detonated:
                    str.Append("odpálená");
                    time = (int)Warhead.DetonationTimer;
                    break;
            }
            str.Append($" {time / 60:00}:{time % 60:00}");
            return str.ToString();
        }

        private const string TimerTextTemplate =
            "Další spawn jako: {nextrole}\n"
            + "<color=#3333FF>NTF</color>: {ntftime}\n"
            + "<color=#3333FF>NTF</color> MINI: {ntfminitime}\n"
            + "<color=#33FF33>CHAOS</color>: {chaostime}\n"
            + "<color=#33FF33>CHAOS</color> MINI: {chaosminitime}\n"
            + "Alpha warhead: {warheadstate}\n"
            + "Class-D uteklo: {classdescape}\n"
            + "Scientistů uteklo: {scientistescape}\n";

        private static string GenerateTimerText()
        {
            StringBuilder builder = new StringBuilder(TimerTextTemplate);
            switch (WaveManager._nextWave?.TargetFaction)
            {
                case Faction.FoundationStaff: // NTF
                    builder.Replace("{nextrole}", "<color=#3333FF>NTF</color>");
                    break;
                case Faction.FoundationEnemy: // Chaos
                    builder.Replace("{nextrole}", "<color=#33FF33>Chaos</color>");
                    break;
                default:
                    builder.Replace("{nextrole}", "???");
                    break;
            }
            builder.Replace("{ntftime}", GetWaveRespawnText<NtfSpawnWave>());
            builder.Replace("{ntfminitime}", GetWaveRespawnText<NtfMiniWave>());
            builder.Replace("{chaostime}", GetWaveRespawnText<ChaosSpawnWave>());
            builder.Replace("{chaosminitime}", GetWaveRespawnText<ChaosMiniWave>());
            builder.Replace("{warheadstate}", GetWarheadText());
            builder.Replace("{classdescape}", Round.EscapedDClasses.ToString());
            builder.Replace("{scientistescape}", Round.EscapedScientists.ToString());
            return builder.ToString();
        }
    }
}
