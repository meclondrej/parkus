using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Respawning;
using Respawning.Waves;
using MEC;

namespace parkus.Features
{
    public class RespawnTimer
    {
        private CoroutineHandle timerHandle;

        private IEnumerator<float> Timer()
        {
            while (true)
            {
                string hint = GenerateTimerText();
                foreach (Player player in Player.List.ToList().Where(p => p.IsDead))
                        player.ShowHint(hint, 1.2f);
                yield return Timing.WaitForSeconds(1f);
            }
        }

        private void RunTimer()
        {
            if (timerHandle.IsRunning)
                return;
            timerHandle = Timing.RunCoroutine(Timer());
        }

        private void CancelTimer()
        {
            if (!timerHandle.IsRunning)
                return;
            Timing.KillCoroutines(timerHandle);
            timerHandle = default;
        }

        public void OnRoundStarted()
        {
            RunTimer();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            CancelTimer();
        }

        public void OnRestartingRound()
        {
            CancelTimer();
        }

        public void OnDisabled()
        {
            CancelTimer();
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            ev.Player.ShowHint(GenerateTimerText(), 1.0f);
        }

        private static string GetWaveRespawnText<T>() where T : TimeBasedWave
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

        private string GenerateTimerText()
        {
            StringBuilder builder = new StringBuilder("Další spawn jako: {nextrole}\n<color=#3333FF>NTF</color>: {ntftime}\n<color=#3333FF>NTF</color> MINI: {ntfminitime}\n<color=#33FF33>CHAOS</color>: {chaostime}\n<color=#33FF33>CHAOS</color> MINI: {chaosminitime}");
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
            return builder.ToString();
        }
    }
}
