using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Tools
{
    public class StoneHammer : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 5;
            item.knockBack = 3;
            item.scale = 1.1f;
            item.crit -= 2;
            item.rare = ItemRarityID.Blue;
            item.useTurn = false;
            item.autoReuse = true;
            item.melee = true;
            item.value = Item.sellPrice(0, 0, 0, 50);
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 40;
            item.height = 40;
            item.maxStack = 1;
            item.useTime = 40;
            item.useAnimation = 20;
            item.hammer = 40;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddRecipeGroup("Wood", 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
