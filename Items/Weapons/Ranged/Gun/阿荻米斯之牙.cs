using System;

using Entrogic.Items.Materials;
using Entrogic.Projectiles.Arcane;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 阿荻米斯之牙 : ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("汇聚了泰拉大陆上最顶尖的技术（迫真）\n" +
                "“还处于试验阶段”");
        }

        public override void SetDefaults()
        {
            item.damage = 34;
            item.DamageType = DamageClass.Ranged;
            item.width = 64;
            item.height = 36;
            item.useTime = 7;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.Shoot;

            item.noMelee = true;
            item.knockBack = 4;
            item.value = 140000;
            item.rare = 12;
            item.UseSound = SoundID.Item40;
            item.autoReuse = true;     
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 6f;
            item.reuseDelay = 20;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .15f;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(5) == 0) 
            {
                int numberProjectiles = 2 + Main.rand.Next(2); // 4 or 5 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(9));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ArcaneMissle>(), damage, knockBack, player.whoAmI);
                }
            }
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
                return true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Megashark, 1)//巨兽鲨
                .AddRecipeGroup("Entrogic:AdamantiteBar", 12)
                .AddIngredient(ItemType<碳钢枪械部件>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        private void AddRecipeGroup(string v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}