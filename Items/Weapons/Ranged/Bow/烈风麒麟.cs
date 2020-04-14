using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class 烈风麒麟 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("不朽圣火的力量，好好利用吧");
        }

        public override void SetDefaults()
        {
            item.damage = 89;
            item.ranged = true;
            item.width = 32;
            item.crit += 26;
            item.height = 64;
            item.useTime = 25;
            item.useAnimation = 25;
            
            item.useStyle = 5;

            item.noMelee = true;
            item.knockBack = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 10;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            
            item.shoot = 10;
            item.shootSpeed = 16f;

            item.useAmmo = AmmoID.Arrow;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(2624, 1);
            recipe.AddIngredient(3854, 1);
            recipe.AddIngredient(3456, 20);
            recipe.AddIngredient(3458, 20);
            recipe.AddIngredient(3467, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 706, 250, knockBack, player.whoAmI);
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = mod.ProjectileType("凤凰箭");
            }

            float numberProjectiles = 3 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(3);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // 如果只有1个弹丸，请注意除以0。
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }

            return false;
        }
    }
}