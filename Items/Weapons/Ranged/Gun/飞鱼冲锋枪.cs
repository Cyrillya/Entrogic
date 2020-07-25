using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 飞鱼冲锋枪 : ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“来自东亚弧形群岛的美食”");
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.width = 62;
            item.height = 32;
            item.useTime = 5;
            item.useAnimation = 5;
            
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 5f;
            item.useAmmo = AmmoID.Bullet;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 18);
            recipe.AddIngredient(ItemID.SoulofFright, 18);
            recipe.AddIngredient(ItemID.SoulofSight, 18);
            recipe.AddIngredient(null,"飞鱼手枪", 1);
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            recipe.AddIngredient(null, "碳钢枪械部件", 1);//碳钢枪械部件
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .66f;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet)
            {
                type = 242;
            }
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
                return true;
            }
        }
    }
}