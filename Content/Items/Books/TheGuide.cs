using Entrogic.Content.Items.Misc.Materials;
using Entrogic.Interfaces.UIElements;

namespace Entrogic.Content.Items.Books;

public class TheGuide : ItemBook
{
    public override void SetDefaultsBook() {
        PageMax = 2;
    }

    public override void ModifyBookPageContent(Player player, int page, ref List<BookContent> contents) {
        contents.Clear();
        switch (page) {
            case 1:
                contents.Add(new TextContent(ModHelper.GetText("Common.TheGuide.PageText1"), BookContent.LeftPagePos, font: FontManager.BookFont));
                contents.Add(new TextContent(ModHelper.GetText("Common.TheGuide.PageText2"), BookContent.RightPagePos, font: FontManager.BookFont));
                break;
            case 2:
                contents.Add(new TextContent(ModHelper.GetText("Common.TheGuide.PageText3"), BookContent.LeftPagePos, font: FontManager.BookFont));
                contents.Add(new TextContent(ModHelper.GetText("Common.TheGuide.PageText4"), BookContent.RightPagePos, font: FontManager.BookFont));
                break;
        }
    }

    public override void AddRecipes() =>
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 10)
            .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
            .AddTile(TileID.Sawmill)
            .Register();
}