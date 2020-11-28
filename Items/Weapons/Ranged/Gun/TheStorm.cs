using Entrogic.Projectiles.Ranged.Bows;
using Entrogic.Projectiles.Ranged.Bullets;
using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class TheStorm : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 23;
            item.ranged = true;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4;
            item.rare = RareID.LV5;
            item.autoReuse = true;
            item.shoot = ProjectileType<SnowStormy>();
            item.shootSpeed = 15f;
            item.useAmmo = AmmoID.Snowball;
            item.width = 58;
            item.height = 22;
            item.crit = 24;
            item.UseSound = SoundID.Item11;
            item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 0f);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.SnowBallFriendly) { type = item.shoot; damage += 17; }
            // 改自Player.cs中的海啸射击源码(1.4.0.5)
            int arrows = 2;
            Vector2 collideSpeed = new Vector2(speedX, speedY);
            collideSpeed.Normalize();
            collideSpeed *= 40f;
            bool flag4 = Collision.CanHit(position, 0, 0, position + collideSpeed, 0, 0);
            for (int i = 0; i < arrows; i++)
            {
                float angleUnit = MathHelper.ToRadians(20f);
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
            recipe.AddIngredient(ItemID.SnowBlock, 30);
            recipe.AddIngredient(ItemID.SoulofLight, 8);
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddRecipe();
        }
    }
}
