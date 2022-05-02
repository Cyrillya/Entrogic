using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Items.Misc.Materials;
using Entrogic.Interfaces.UI.BookUI;

namespace Entrogic.Content.Items.Books
{
    public class TheGuide : ItemBook
    {
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaultsBook() {
			base.SetDefaultsBook();
            PageMax = 2;
		}

		public override void ModifyBookContent(Player player, int page, ref List<BookContent> contents) {
			base.ModifyBookContent(player, page, ref contents);
            contents.Clear();
            switch (page) {
                case 1: {
                        contents.Add(new BookContent(Language.GetTextValue("Mods.Entrogic.Common.TheGuide.PageText1"), BookContent.LeftPagePos));
                        contents.Add(new BookContent(Language.GetTextValue("Mods.Entrogic.Common.TheGuide.PageText2"), BookContent.RightPagePos));
                        break;
                    }
                case 2: {
                        contents.Add(new BookContent(Language.GetTextValue("Mods.Entrogic.Common.TheGuide.PageText3"), BookContent.LeftPagePos));
                        contents.Add(new BookContent(Language.GetTextValue("Mods.Entrogic.Common.TheGuide.PageText4"), BookContent.RightPagePos));
                        break;
                    }
            }
        }

		public override void AddRecipes() =>
            CreateRecipe()
                .AddRecipeGroup("Wood", 10)
                .AddIngredient(ModContent.ItemType<SoulOfPure>(), 5)
                .AddTile(TileID.Sawmill)
                .Register();
    }
}