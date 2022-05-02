using Entrogic.Items.Materials;
using Entrogic.Items.Weapons.Summon.Whip;
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

            // 铜鞭 => 铜短剑
            Recipe copperShortsword = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.CopperShortsword);
            copperShortsword.AddIngredient(ModContent.ItemType<CopperSwordWhip>());
            copperShortsword.Register();

            // 赫尔墨斯之靴
            Recipe hermers = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.HermesBoots);
            hermers.AddIngredient(ItemID.Silk, 3);
            hermers.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            hermers.AddTile(TileID.WorkBenches);
            hermers.Register();

            // 金字塔独特产品
            Recipe pyramidThings = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.FlyingCarpet);
            pyramidThings.AddIngredient(ItemID.SandstoneBrick, 30);
            pyramidThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            pyramidThings.AddTile(TileID.WorkBenches);
            pyramidThings.Register();
            pyramidThings = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.SandstorminaBottle);
            pyramidThings.AddIngredient(ItemID.SandBlock, 50);
            pyramidThings.AddIngredient(ItemID.Bottle, 1);
            pyramidThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            pyramidThings.AddTile(TileID.WorkBenches);
            pyramidThings.Register();

            // 空岛独特产品
            Recipe skyThings = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.ShinyRedBalloon);
            skyThings.AddIngredient(ItemID.Cloud, 10);
            skyThings.AddIngredient(ItemID.RainCloud, 10);
            skyThings.AddIngredient(ItemID.Umbrella, 1);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            skyThings.AddTile(TileID.WorkBenches);
            skyThings.Register();
            skyThings = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.LuckyHorseshoe);
            skyThings.AddIngredient(ItemID.Umbrella, 1);
            skyThings.AddRecipeGroup("Entrogic:GoldBar", 5);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            skyThings.AddTile(TileID.WorkBenches);
            skyThings.Register();
            skyThings = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.FloatingIslandFishingCrate);
            skyThings.AddIngredient(ItemID.SkywareChest, 1);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 20);
            skyThings.AddCondition(Recipe.Condition.NearWater);
            skyThings.AddTile(TileID.AlchemyTable);
            skyThings.Register();

            // 雨伞
            Recipe umbrella = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.Umbrella);
            umbrella.AddIngredient(ItemID.Silk, 5);
            umbrella.AddTile(TileID.WorkBenches);
            umbrella.Register();

            // 四纹章
            Recipe emblems = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.RangerEmblem);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.Register();
            emblems = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.SorcererEmblem);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.Register();
            emblems = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.SummonerEmblem);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.Register();
            emblems = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.WarriorEmblem);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.Register();

            // 金虫网
            Recipe goldenNet = ModContent.GetInstance<Entrogic>().CreateRecipe(ItemID.GoldenBugNet);
            goldenNet.AddIngredient(ItemID.BugNet);
            goldenNet.AddRecipeGroup("Entrogic:GoldBar", 5);
            goldenNet.AddTile(TileID.TinkerersWorkbench);
            goldenNet.Register();
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
