using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace parkus.Features
{
    public static class CoinGamble
    {
        private static readonly System.Random random = new System.Random();

        private struct CoinEffect
        {
            public CoinEffect(ushort luck_required, Action<Player> action)
            {
                this.luck_required = luck_required;
                this.action = action;
            }

            public Action<Player> action;
            public int luck_required;
        }

        private static void GiveAndEquip(Player player, ItemType itemType)
        {
            player.CurrentItem = player.AddItem(itemType);
        }

        private const float PICKUP_VELOCITY = 1.5f;
        private const float PICKUP_SPAWN_DISTANCE = 0.3f;
        private const float PICKUP_HEIGHT_FROM_PLAYER_FEET = .5f;

        private static void ThrowRadial(Player player, IEnumerable<ItemType> itemTypes)
        {
            Vector3 centerPosition =
                player.Position + new Vector3(0, PICKUP_HEIGHT_FROM_PLAYER_FEET, 0);
            int itemCount = itemTypes.Count();
            float anglePerItem = 2 * Mathf.PI / itemCount;
            float itemAngle = 0;
            foreach (ItemType itemType in itemTypes)
            {
                Vector3 itemDirection = new Vector3(
                    Mathf.Cos(itemAngle),
                    0f,
                    Mathf.Sin(itemAngle)
                ).normalized;
                Pickup pickup = Pickup.CreateAndSpawn(
                    itemType,
                    centerPosition + itemDirection * PICKUP_SPAWN_DISTANCE,
                    Quaternion.LookRotation(itemDirection, Vector3.up),
                    player
                );
                pickup.Rigidbody.AddForce(
                    player.Velocity + itemDirection * PICKUP_VELOCITY,
                    ForceMode.VelocityChange
                );
                itemAngle += anglePerItem;
            }
        }

        private static ItemType PickItemType(IEnumerable<ItemType> itemTypes)
        {
            return itemTypes.ElementAt(random.Next(itemTypes.Count()));
        }

        private static readonly ItemType[] goodScpItems =
        {
            ItemType.SCP1344,
            ItemType.SCP1576,
            ItemType.SCP500,
            ItemType.SCP268,
            ItemType.SCP1853,
        };

        private static readonly ItemType[] badScpItems =
        {
            ItemType.SCP207,
            ItemType.AntiSCP207,
            ItemType.SCP244a,
            ItemType.SCP244b,
            ItemType.SCP2176,
            ItemType.SCP018,
        };

        private static readonly ItemType[] scpGuns =
        {
            ItemType.GunSCP127,
            ItemType.GunCom45,
            ItemType.GunA7,
        };

        private static readonly ItemType[] specialGuns =
        {
            ItemType.Jailbird,
            ItemType.MicroHID,
            ItemType.ParticleDisruptor,
        };

        private static readonly ItemType[] allGuns =
        {
            ItemType.GunCOM15,
            ItemType.GunE11SR,
            ItemType.GunCrossvec,
            ItemType.GunFSP9,
            ItemType.GunLogicer,
            ItemType.GunCOM18,
            ItemType.GunRevolver,
            ItemType.GunAK,
            ItemType.GunShotgun,
            ItemType.GunCom45,
            ItemType.GunFRMG0,
            ItemType.GunA7,
            ItemType.GunSCP127,
        };

        private static readonly ItemType[] basicLoadout =
        {
            ItemType.ArmorLight,
            ItemType.GunRevolver,
            ItemType.Ammo44cal,
            ItemType.Ammo44cal,
            ItemType.Painkillers,
            ItemType.KeycardGuard,
        };

        private static readonly CoinEffect[] coinEffects =
        {
            new CoinEffect(99, player => GiveAndEquip(player, ItemType.KeycardO5)),
            new CoinEffect(
                96,
                player =>
                    ThrowRadial(
                        player,
                        new ItemType[]
                        {
                            PickItemType(scpGuns),
                            PickItemType(specialGuns),
                            ItemType.ArmorLight,
                        }
                    )
            ),
            new CoinEffect(
                91,
                player =>
                    ThrowRadial(
                        player,
                        new ItemType[] { PickItemType(allGuns), ItemType.ArmorCombat }
                    )
            ),
            new CoinEffect(84, player => GiveAndEquip(player, PickItemType(goodScpItems))),
            new CoinEffect(69, player => GiveAndEquip(player, PickItemType(badScpItems))),
            new CoinEffect(49, player => ThrowRadial(player, basicLoadout)),
            new CoinEffect(39, player => { }),
            new CoinEffect(
                20,
                player =>
                {
                    ExplosiveGrenade he = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    he.FuseTime = 3f;
                    Pickup pickup = he.SpawnActive(player.Position + Vector3.up);
                    pickup.Rigidbody.AddForce(player.Velocity, ForceMode.VelocityChange);
                    player.ShowHint("bacha na nohy!", 3);
                }
            ),
            new CoinEffect(
                5,
                player =>
                {
                    FlashGrenade flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash);
                    flash.FuseTime = 1f;
                    Pickup pickup = flash.SpawnActive(player.Position + Vector3.up);
                    pickup.Rigidbody.AddForce(player.Velocity, ForceMode.VelocityChange);
                    player.ShowHint("bacha na oči!", 1);
                }
            ),
            new CoinEffect(0, player => player.Kill("chcipnul na ligmu")),
        };

        public static void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            int luck = random.Next(100);
            ev.Player.RemoveHeldItem();
            foreach (CoinEffect e in coinEffects)
                if (luck >= e.luck_required)
                {
                    e.action(ev.Player);
                    break;
                }
        }
    }
}
