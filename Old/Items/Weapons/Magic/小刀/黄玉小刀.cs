//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using static Terraria.ModLoader.ModContent;

//namespace Entrogic.Items.Weapons.Magic.小刀
//{
//    public class 黄玉小刀 : ModItem
//    {
//        public override void SetStaticDefaults()
//        {
//            Tooltip.SetDefault("“看上去很像琥珀，真的很像”\n" +
//                "忠诚Ⅰ\n" +
//                "伪制品Ⅰ");
//        }

//        public override void SetDefaults()
//        {
//            item.damage = 19;
//            item.width = 30;         
//            item.height = 32;
//            item.useTime = 45;
//            item.useAnimation = 45;
//            item.useStyle = 1;
//            item.knockBack = 3;         
//            item.value = Item.buyPrice(silver: 3);
//            item.rare = 1;              
//            item.UseSound = SoundID.Item1;      
//            item.autoReuse = false;
//            item.shoot = ModContent.ProjectileType<黄玉小刀>();
//            item.shootSpeed = 13.9f;
//            item.DamageType = DamageClass.Magic;
//            item.noUseGraphic = true;
//            item.crit += 19;
//            item.mana = 3;
//        }
//        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
//        {
//            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1));
//            speedX = perturbedSpeed.X;
//            speedY = perturbedSpeed.Y;
//            return true;
//        }
//        public override void AddRecipes()
//        {
//            ModRecipe recipe = new ModRecipe(mod);
//            recipe.AddIngredient(ItemID.Topaz, 10);
//            recipe.AddRecipeGroup("IronBar", 3);
//            recipe.AddIngredient(ModContent.ItemType<SoulOfPure>(), 6);
//            recipe.AddTile(TileID.Anvils);
//            recipe.SetResult(this);
//            recipe.AddRecipe();
//        }
//    }
//}
////在？你看你马呢？
////源码小偷gck