using Entrogic.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Placeable.Furnitrue
{
    public class EvilSasukeItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.createTile = TileType<EvilSasuke>();
            item.placeStyle = 0;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Entrogic:RCAV", 10);
            recipe.AddRecipeGroup("Entrogic:DemBar", 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
