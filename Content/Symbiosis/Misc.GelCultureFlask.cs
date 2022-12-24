using Entrogic.Content.Miscellaneous.Items.Materials;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Symbiosis
{
    public class GelCultureFlask : ItemBase
    {
        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = RarityLevelID.EarlyPHM;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Gel, 20)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
                .AddRecipeGroup("IronBar")
                .AddTile(TileID.WorkBenches)
                .Register();

        }

        public override bool CanUseItem(Player player) {
            if (Main.myPlayer == player.whoAmI)
                NPC.SpawnBoss((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 113, player.whoAmI);
            return true;
        }
    }
}
