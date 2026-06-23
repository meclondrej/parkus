using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace parkus.Features
{
    public static class Scp3114Spawner
    {
        private static readonly Random random = new Random();
        private const int SPAWNABLE_SCP_ROLE_COUNT = 7;

        public static void OnRoundStarted()
        {
            IEnumerable<Player> scps = Player.List.Where(p => p.Role.Side == Side.Scp);
            int scpCount = scps.Count();
            if (scpCount < 2)
                return;

            double scp3114Chance = (double)scpCount / SPAWNABLE_SCP_ROLE_COUNT;
            if (random.NextDouble() > scp3114Chance)
                return;

            Player player = scps.ElementAt(random.Next(scpCount));
            player.Role.Set(RoleTypeId.Scp3114);
        }
    }
}
