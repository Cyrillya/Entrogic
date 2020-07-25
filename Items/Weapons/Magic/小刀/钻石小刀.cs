//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using static Terraria.ModLoader.ModContent;

//namespace Entrogic.Items.Weapons.Magic.小刀
//{
//    public class 钻石小刀 : ModItem
//    {
//        public override void SetStaticDefaults()
//        {
//            Tooltip.SetDefault("“优雅而致命”\n" +
//                "会心一击Ⅱ\n" +
//                "精准Ⅱ");
//        }

//        public override void SetDefaults()
//        {
//            item.damage = 27;
//            item.width = 30;         
//            item.height = 32;
//            item.useTime = 45;
//            item.useAnimation = 45;
//            item.useStyle = 1;
//            item.knockBack = 3;         
//            item.value = Item.buyPrice(silver: 25);
//            item.rare = 1;              
//            item.UseSound = SoundID.Item1;      
//            item.autoReuse = false;
//            item.shoot = ModContent.ProjectileType<钻石小刀>();
//            item.shootSpeed = 14.8f;
//            item.magic = true;
//            item.noUseGraphic = true;
//            item.crit += 31;
//        }
//        public override void AddRecipes()
//        {
//            ModRecipe recipe = new ModRecipe(mod);
//            recipe.AddIngredient(ItemID.Diamond, 10);
//            recipe.AddRecipeGroup("IronBar", 3);
//            recipe.AddIngredient(ModContent.ItemType<SoulOfPure>(), 6);
//            recipe.AddTile(TileID.Anvils);
//            recipe.SetResult(this);
//            recipe.AddRecipe();
//        }
//    }
//}