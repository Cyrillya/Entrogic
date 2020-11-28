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
        public static void AddRecipes()
        {
            ModRecipe hermers = new ModRecipe(Mod);
            hermers.AddIngredient(ItemID.Silk, 3);
            hermers.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            hermers.AddTile(TileID.WorkBenches);
            hermers.SetResult(ItemID.HermesBoots);
            hermers.AddRecipe();

            ModRecipe pyramidThings = new ModRecipe(Mod);
            pyramidThings.AddIngredient(ItemID.SandstoneBrick, 30);
            pyramidThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            pyramidThings.AddTile(TileID.WorkBenches);
            pyramidThings.SetResult(ItemID.FlyingCarpet);
            pyramidThings.AddRecipe();
            pyramidThings = new ModRecipe(Mod);
            pyramidThings.AddIngredient(ItemID.SandBlock, 50);
            pyramidThings.AddIngredient(ItemID.Bottle, 1);
            pyramidThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            pyramidThings.AddTile(TileID.WorkBenches);
            pyramidThings.SetResult(ItemID.SandstorminaBottle);
            pyramidThings.AddRecipe();

            ModRecipe skyThings = new ModRecipe(Mod);
            skyThings.AddIngredient(ItemID.Cloud, 10);
            skyThings.AddIngredient(ItemID.RainCloud, 10);
            skyThings.AddIngredient(ItemID.Umbrella, 1);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            skyThings.AddTile(TileID.WorkBenches);
            skyThings.SetResult(ItemID.ShinyRedBalloon);
            skyThings.AddRecipe();
            skyThings = new ModRecipe(Mod);
            skyThings.AddIngredient(ItemID.Umbrella, 1);
            skyThings.AddRecipeGroup("Entrogic:GoldBar", 5);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 5);
            skyThings.AddTile(TileID.WorkBenches);
            skyThings.SetResult(ItemID.LuckyHorseshoe);
            skyThings.AddRecipe();
            skyThings = new ModRecipe(Mod);
            skyThings.AddIngredient(ItemID.SkywareChest, 1);
            skyThings.AddIngredient(ModContent.ItemType<SoulOfPure>(), 20);
            skyThings.SetResult(ItemID.FloatingIslandFishingCrate);
            skyThings.needWater = true;
            skyThings.alchemy = true;
            skyThings.AddRecipe();

            ModRecipe umbrella = new ModRecipe(Mod);
            umbrella.AddIngredient(ItemID.Silk, 5);
            umbrella.AddTile(TileID.WorkBenches);
            umbrella.SetResult(ItemID.Umbrella);
            umbrella.AddRecipe();

            ModRecipe emblems = new ModRecipe(Mod);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.SetResult(ItemID.RangerEmblem);
            emblems.AddRecipe();
            emblems = new ModRecipe(Mod);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.SetResult(ItemID.SorcererEmblem);
            emblems.AddRecipe();
            emblems = new ModRecipe(Mod);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.SetResult(ItemID.SummonerEmblem);
            emblems.AddRecipe();
            emblems = new ModRecipe(Mod);
            emblems.AddRecipeGroup("Entrogic:FourEmblem");
            emblems.AddTile(TileID.TinkerersWorkbench);
            emblems.SetResult(ItemID.WarriorEmblem);
            emblems.AddRecipe();

            ModRecipe goldenNet = new ModRecipe(Mod);
            goldenNet.AddIngredient(ItemID.BugNet);
            goldenNet.AddRecipeGroup("Entrogic:GoldBar", 5);
            goldenNet.AddTile(TileID.TinkerersWorkbench);
            goldenNet.SetResult(ItemID.GoldenBugNet);
            goldenNet.AddRecipe();
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
