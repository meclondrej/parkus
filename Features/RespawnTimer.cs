using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace parkus.Features
{
    public class RespawnTimer
    {
        private CancellationTokenSource timerCancellationTokenSource = null;

        private async Task Timer()
        {
            int sync_anchor = Respawn.TimeUntilSpawnWave.Seconds;
            while (Respawn.TimeUntilSpawnWave.Seconds == sync_anchor)
                await Task.Delay(5);
            while (!timerCancellationTokenSource.Token.IsCancellationRequested)
            {
                await Task.Delay(1000, timerCancellationTokenSource.Token);
                string hint = GenerateTimerText();
                foreach (Player player in Player.List.Where(p => !p.IsAlive))
                {
                    player.ShowHint(hint, 1.0f);
                }
            }
        }

        private void RunTimer()
        {
            if (timerCancellationTokenSource != null)
                return;
            timerCancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => Timer(), timerCancellationTokenSource.Token);
        }

        private void CancelTimer()
        {
            if (timerCancellationTokenSource == null)
                return;
            timerCancellationTokenSource.Cancel();
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
            StringBuilder builder = new StringBuilder("Další spawn");
            switch (Respawn.NextKnownTeam)
            {
                case Respawning.SpawnableTeamType.NineTailedFox:
                    builder.Append(" jako NTF");
                    break;
                case Respawning.SpawnableTeamType.ChaosInsurgency:
                    builder.Append(" jako Chaos");
                    break;
            }
            builder.Append(" za {time}");
            TimeSpan remainingTime = Respawn.TimeUntilSpawnWave;
            builder.Replace("{time}", $"{remainingTime.Minutes.ToString("00")}:{(remainingTime.Seconds % 60).ToString("00")}");
            builder.Append($"\nNTF/CI tickety: {Respawn.NtfTickets.ToString("0.00")}/{Respawn.ChaosTickets.ToString("0.00")}");
            return builder.ToString();
        }
    }
}