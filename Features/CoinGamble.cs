using System;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using UnityEngine;
using Exiled.API.Enums;
using PlayerRoles;

namespace parkus.Features
{
public class CoinGamble
{
    private static readonly System.Random rnd = new System.Random();

    private struct ItemChance
    {
        public ItemChance(ItemType item, ushort luck_required)
        {
            this.item = item;
            this.luck_required = luck_required;
        }

        public ItemType item;
        public ushort luck_required;
    }

    private static readonly ItemChance[] itemChances = {
        new ItemChance(ItemType.KeycardO5, 98),
        new ItemChance(ItemType.MicroHID, 96),
        new ItemChance(ItemType.ParticleDisruptor, 94),
        new ItemChance(ItemType.Jailbird, 90),
        new ItemChance(ItemType.GunFRMG0, 85),
        new ItemChance(ItemType.GunE11SR, 80),
        new ItemChance(ItemType.ArmorHeavy, 70),
        new ItemChance(ItemType.GrenadeHE, 60),
        new ItemChance(ItemType.SCP500, 50),
        new ItemChance(ItemType.Adrenaline, 40),
        new ItemChance(ItemType.Medkit, 20),
        new ItemChance(ItemType.KeycardScientist, 0),
    };

    private struct CoinEffect
    {
        public CoinEffect(ushort luck_required, Action<Player> action)
        {
            this.luck_required = luck_required;
            this.action = action;
        }

        public Action<Player> action;
        public ushort luck_required;
    }

    private static readonly CoinEffect[] coinEffects = {

        // ========== GOOD EFFECTS ==========

        // gives the player a 300 HP boost
        new CoinEffect(90, player => {
            player.Heal(300, true);
        }),

        // gives the player an item
        new CoinEffect(50, player => {
            ushort luck = (ushort)rnd.Next(101);
            foreach (ItemChance ic in itemChances)
                if (luck >= ic.luck_required)
                {
                    Pickup.CreateAndSpawn(ic.item, player.Position, null);
                    break;
                }
        }),

        // ========== BAD EFFECTS ==========

        // spawns a grenade with a 3 second fuse
        new CoinEffect(40, player => {
            ExplosiveGrenade he = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            he.FuseTime = 3;
            he.SpawnActive(player.Position + Vector3.up, player);
            player.ShowHint("bacha na nohy!", 3);
        }),

        // flashes the player
        new CoinEffect(20, player => {
            player.EnableEffect(EffectType.Flashed, 10, true);
        }),

        // turns the player into SCP-3114
        new CoinEffect(10, player => {
            player.DropItems();
            player.DisableAllEffects();
            player.Role.Set(RoleTypeId.Scp3114, SpawnReason.ItemUsage, RoleSpawnFlags.UseSpawnpoint);
        }),

        // kills the player
        new CoinEffect(0, player => {
            player.Kill("chcipnul na ligmu");
        }),
    };

    public void OnFlippingCoin(FlippingCoinEventArgs ev)
    {
        ushort luck = (ushort)rnd.Next(101);
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