using Entrogic.Projectiles.Ranged.Bows;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class Dragonflow : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 23;
            item.ranged = true;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.rare = RareID.LV5;
            item.autoReuse = true;
            item.shoot = ProjectileType<DragonflowProj>();
            item.shootSpeed = 15;
            item.useAmmo = AmmoID.Arrow;
            item.width = 58;
            item.height = 22;
            item.crit = 24;
            item.UseSound = SoundID.Item39;
            item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 0f);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly) { type = item.shoot; damage += 17; }
            // 改自Player.cs中的海啸射击源码(1.4.0.5)
            int arrows = 3;
            Vector2 collideSpeed = new Vector2(speedX, speedY);
            collideSpeed.Normalize();
            collideSpeed *= 40f;
            bool flag4 = Collision.CanHit(position, 0, 0, position + collideSpeed, 0, 0);
            for (int i = 0; i < arrows; i++)
            {
                float angleUnit = MathHelper.ToRadians(Math.Abs(player.velocity.Y) >= 12f ? 40f : Math.Abs(player.velocity.Y) / 12f * 20f + 20f);
                float degree = (float)i - ((float)arrows - 1f) / 2f;
                Vector2 offsetVec = collideSpeed.RotatedBy((double)(angleUnit * degree), default(Vector2));
                if (!flag4)
                {
                    offsetVec -= collideSpeed;
                }
                int proj = Projectile.NewProjectile(position.X + offsetVec.X, position.Y + offsetVec.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].noDropItem = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddRecipe();
        }
    }
}
