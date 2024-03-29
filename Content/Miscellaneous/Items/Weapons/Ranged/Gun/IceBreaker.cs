﻿using Entrogic.Content.Miscellaneous.Items.Materials;
using Entrogic.Content.Miscellaneous.Projectiles.Ranged.Bullets;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Ranged.Gun
{
    public class IceBreaker : ItemBase
    {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.Flamethrower);
            Item.useAmmo = ItemID.IceBlock;
            Item.shoot = ModContent.ProjectileType<FrostFire>();
            Item.shootSpeed = 8f;
            Item.damage = 38;
            Item.crit += 24;
            Item.rare = RarityLevelID.EarlyHM;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            position += velocity.ToRotation().ToRotationVector2() * 5.2f * 16f;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .10f && player.itemAnimation == player.itemAnimationMax;

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SnowballCannon)
                .AddIngredient(ItemID.FrostCore)
                .AddIngredient(ItemID.SoulofFright, 8)
                .AddIngredient(ModContent.ItemType<CuteWidget>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override Vector2? HoldoutOffset() => new(-6.0f, 0.0f);
    }
}
