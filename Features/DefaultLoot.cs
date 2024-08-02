using System;
using System.Collections.Generic;
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
            switch (ev.NewRole)
            {
                case PlayerRoles.RoleTypeId.ClassD:
                    foreach (ItemType x in GenerateDefaultLoot(ClassDTable, ClassDMaxItems))
                        ev.Items.Add(x);
                    break;
            }
        }

        private List<ItemType> GenerateDefaultLoot(ItemType[] table, ushort maxItems)
        {
            List<ItemType> result = new List<ItemType>();
            ushort itemCount = (ushort)rnd.Next(maxItems + 1);
            for (ushort i = 0; i < itemCount; i++)
                result.Add(table[rnd.Next(table.Length)]);
            return result;
        }
    }
}