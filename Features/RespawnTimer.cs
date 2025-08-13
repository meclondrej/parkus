using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Respawning;
using Respawning.Waves;

namespace parkus.Features
{
    public class RespawnTimer
    {
        private CancellationTokenSource timerCancellationTokenSource = null;
        private TimedWave ntfWave = null;
        private TimedWave chaosWave = null;

        private async Task Timer()
        {
            while (!timerCancellationTokenSource.Token.IsCancellationRequested)
            {
                await Task.Delay(1000, timerCancellationTokenSource.Token);
                string hint = GenerateTimerText();
                foreach (Player player in Player.List.Where(p => !p.IsAlive))
                {
                    player.ShowHint(hint, 1.2f);
                }
            }
        }

        private void RunTimer()
        {
            if (TimedWave.TryGetTimedWave<NtfSpawnWave>(out TimedWave ntfWave))
                this.ntfWave = ntfWave;
            if (TimedWave.TryGetTimedWave<ChaosSpawnWave>(out TimedWave chaosWave))
                this.chaosWave = chaosWave;
            if (timerCancellationTokenSource != null)
                return;
            timerCancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => Timer(), timerCancellationTokenSource.Token);
        }

        private void CancelTimer()
        {
            ntfWave = null;
            chaosWave = null;
            if (timerCancellationTokenSource == null)
                return;
            timerCancellationTokenSource.Cancel();
            timerCancellationTokenSource.Dispose();
            timerCancellationTokenSource = null;
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

        private string GenerateTimerText()
        {
            TimeSpan ntfTime = (ntfWave?.Timer.TimeLeft ?? TimeSpan.Zero) + TimeSpan.FromSeconds(18);
            if (ntfTime < TimeSpan.Zero)
                ntfTime = TimeSpan.Zero;
            TimeSpan chaosTime = (chaosWave?.Timer.TimeLeft ?? TimeSpan.Zero) + TimeSpan.FromSeconds(13);
            if (chaosTime < TimeSpan.Zero)
                chaosTime = TimeSpan.Zero;
            StringBuilder builder = new StringBuilder("Další spawn jako: {nextrole}\n<color=#3333FF>NTF</color>: {ntftime}\n<color=#33FF33>CHAOS</color>: {chaostime}");
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
            builder.Replace("{ntftime}", $"{ntfTime.TotalMinutes:00}:{ntfTime.Seconds:00}");
            builder.Replace("{chaostime}", $"{chaosTime.TotalMinutes:00}:{chaosTime.Seconds:00}");
            return builder.ToString();
        }
    }
}