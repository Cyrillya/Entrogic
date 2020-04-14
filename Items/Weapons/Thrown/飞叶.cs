using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Thrown
{
    public class 飞叶 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“飞花摘叶皆可伤人”\n" +
                "早已失传的武功");
        }

        public override void SetDefaults()
        {
            item.damage = 5;       
            item.width = 28;         
            item.height = 22;          
            item.useTime = 12;          
            item.useAnimation = 12;     
            item.useStyle = 1;
            item.knockBack = 2;         
            item.value = Item.sellPrice(copper: 5);
            item.rare = 2;              
            item.UseSound = SoundID.Item1;      
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("叶片proj");
            item.shootSpeed = 15f;
            item.thrown = true;
            item.noUseGraphic = true;
            item.crit += 19;
            item.consumable = true;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood", 10);
            recipe.AddTile(TileID.LivingLoom);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 2);
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}
