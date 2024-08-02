using System;
using System.Collections.Generic;
using Exiled.API.Features.Pools;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace parkus.Features
{
    public class DefaultLoot
    {
        private static readonly Random rnd = new Random();

        private static readonly ItemType[] ClassDTable = {
            ItemType.Coin,
            ItemType.Medkit,
            ItemType.Adrenaline,
            ItemType.SCP500,
            ItemType.KeycardJanitor,
            ItemType.KeycardScientist,
            ItemType.GunCOM15,
        };
        private const ushort ClassDMaxItems = 2;

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Reason == Exiled.API.Enums.SpawnReason.ForceClass)
                return;
            Log.Debug("hit through reason checker");
            switch (ev.NewRole)
            {
                case PlayerRoles.RoleTypeId.ClassD:
                    Log.Debug("hit through dboy switch case");
                    foreach (ItemType x in GenerateDefaultLoot(ClassDTable, ClassDMaxItems))
                        ev.Items.Add(x);
                    Log.Debug($"final len: {ev.Items.Count}");
                    break;
            }
        }

        private List<ItemType> GenerateDefaultLoot(ItemType[] table, ushort maxItems)
        {
            List<ItemType> result = new List<ItemType>();
            ushort itemCount = (ushort)rnd.Next(maxItems + 1);
            for (ushort i = 0; i < itemCount; i++)
                result.Add(table[rnd.Next(table.Length)]);
            Log.Debug($"defloot len: {result.Count}");
            return result;
        }
    }
}