using Entrogic.Items.Materials;

using System;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic
{
    public class RecipeManager
    {
        private static Entrogic Mod => Entrogic.Instance;

        public static void Load(Mod mod)
        {
            AddRecipeGroups();
            // 赫尔墨斯之靴
            //Mod.CreateRecipe(ItemID.HermesBoots)
            //    .AddIngredient(ItemID.Silk, 3)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();

            // 金字塔独特产品
            //Mod.CreateRecipe(ItemID.FlyingCarpet)
            //    .AddIngredient(ItemID.SandstoneBrick, 30)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();
            //Mod.CreateRecipe(ItemID.SandstorminaBottle)
            //    .AddIngredient(ItemID.SandBlock, 50)
            //    .AddIngredient(ItemID.Bottle, 1)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();

            //// 空岛独特产品
            //Mod.CreateRecipe(ItemID.ShinyRedBalloon)
            //    .AddIngredient(ItemID.Cloud, 10)
            //    .AddIngredient(ItemID.RainCloud, 10)
            //    .AddIngredient(ItemID.Umbrella, 1)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();
            //Mod.CreateRecipe(ItemID.LuckyHorseshoe)
            //    .AddIngredient(ItemID.Umbrella, 1)
            //    .AddRecipeGroup("Entrogic:GoldBar", 5)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();
            //Mod.CreateRecipe(ItemID.FloatingIslandFishingCrate)
            //    .AddIngredient(ItemID.SkywareChest, 1)
            //    .AddIngredient(ModContent.ItemType<SoulOfPure>(), 20)
            //    .AddTile(TileID.AlchemyTable)
            //    .AddCondition(Recipe.Condition.NearWater)
            //    .Register();

            //// 雨伞
            //Mod.CreateRecipe(ItemID.Umbrella)
            //    .AddIngredient(ItemID.Silk, 5)
            //    .AddTile(TileID.WorkBenches)
            //    .Register();

            //// 四纹章
            //Mod.CreateRecipe(ItemID.RangerEmblem)
            //    .AddIngredient(ItemID.StoneBlock)
            //    .AddRecipeGroup("Entrogic:FourEmblem")
            //    .AddTile(TileID.TinkerersWorkbench)
            //    .Register();
            //Mod.CreateRecipe(ItemID.SorcererEmblem)
            //    .AddIngredient(ItemID.StoneBlock)
            //    .AddRecipeGroup("Entrogic:FourEmblem")
            //    .AddTile(TileID.TinkerersWorkbench)
            //    .Register();
            //Mod.CreateRecipe(ItemID.SummonerEmblem)
            //    .AddIngredient(ItemID.StoneBlock)
            //    .AddRecipeGroup("Entrogic:FourEmblem")
            //    .AddTile(TileID.TinkerersWorkbench)
            //    .Register();
            //Mod.CreateRecipe(ItemID.WarriorEmblem)
            //    .AddIngredient(ItemID.StoneBlock)
            //    .AddRecipeGroup("Entrogic:FourEmblem")
            //    .AddTile(TileID.TinkerersWorkbench)
            //    .Register();

            //// 金虫网
            //Mod.CreateRecipe(ItemID.GoldenBugNet)
            //    .AddIngredient(ItemID.BugNet)
            //    .AddRecipeGroup("Entrogic:GoldBar", 5)
            //    .AddTile(TileID.TinkerersWorkbench)
            //    .Register();
        }

        public static void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.AdaTitBar"), new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("Entrogic:AdamantiteBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.RCAV"), new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("Entrogic:RCAV", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.CriDemBar"), new int[]
            {
                ItemID.CrimtaneBar,
                ItemID.DemoniteBar
            });
            RecipeGroup.RegisterGroup("Entrogic:DemBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.GolPlaBar"), new int[]
            {
                ItemID.PlatinumBar,
                ItemID.GoldBar
            });
            RecipeGroup.RegisterGroup("Entrogic:GoldBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("Mods.Entrogic.Emblems"), new int[]
            {
                ItemID.RangerEmblem,
                ItemID.SorcererEmblem,
                ItemID.SummonerEmblem,
                ItemID.WarriorEmblem
            });
            RecipeGroup.RegisterGroup("Entrogic:FourEmblem", group);
        }
    }
}
