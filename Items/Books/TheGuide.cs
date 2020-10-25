using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Entrogic.UI;
using Entrogic.UI.Books;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Materials;
using Terraria.Localization;

namespace Entrogic.Items.Books
{
    public class TheGuide : ModBook
    {
        public override void SetBookInformations()
        {
            MaxPage = 2;
            for (int i = 1; i <= MaxPage * 2f; i++)
                textScale[i] = 0.7f;
            for (int i = 1; i <= MaxPage * 2f; i++)
                lineDistance[i] = -4f;
            textScale[1] = 0.68f;
            lineDistance[1] = -8f;
            PageText[1] = Language.GetTextValue("Mods.Entrogic.Common.PageText1.TheGuide");
            PageText[2] = Language.GetTextValue("Mods.Entrogic.Common.PageText2.TheGuide");
            PageText[3] = Language.GetTextValue("Mods.Entrogic.Common.PageText3.TheGuide");
            PageText[4] = Language.GetTextValue("Mods.Entrogic.Common.PageText4.TheGuide");
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood")
                .AddIngredient(ItemType<SoulOfPure>(), 5)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}