using Entrogic.Core.Global.Resource;
using Entrogic.Core.Systems.BookSystem;
using Entrogic.Helpers;
using Entrogic.Interfaces.UIElements;

namespace Entrogic.Content.Books;

public class CardManual : ItemBook
{
    public override void SetDefaultsBook() {
        PageMax = 4;
    }

    public override void ModifyBookPageContent(Player player, int page, ref List<BookContent> contents) {
        contents.Clear();
        switch (page) {
            case 1:
                contents.Add(Text("1", BookContent.LeftPagePos));
                contents.Add(Text("2", BookContent.RightPagePos));
                contents.Add(Image("Cards", BookContent.RightPagePos + new Vector2(12, 190)));
                break;
            case 2:
                contents.Add(Text("3", BookContent.LeftPagePos));
                contents.Add(Image("Bag", BookContent.LeftPagePos + new Vector2(12, 170)));
                contents.Add(Text("4.1", BookContent.RightPagePos));
                contents.Add(Image("Gloves", BookContent.RightPagePos + new Vector2(12, 80)));
                contents.Add(Text("4.2", BookContent.RightPagePos + new Vector2(0, 180)));
                contents.Add(Image("Costs", BookContent.RightPagePos + new Vector2(20, 260)));
                break;
            case 3:
                contents.Add(Text("5", BookContent.LeftPagePos, 0.94f));
                contents.Add(Image("Buttons", BookContent.LeftPagePos + new Vector2(12, 200), 2.8f));
                contents.Add(Text("6.1", BookContent.RightPagePos, 0.9f));
                contents.Add(Image("Bar", BookContent.RightPagePos + new Vector2(-6, 95)));
                contents.Add(Text("6.2", BookContent.RightPagePos + new Vector2(0, 200), 0.9f));
                contents.Add(Image("EyeCard", BookContent.RightPagePos + new Vector2(100, 264)));
                break;
            case 4:
                contents.Add(Text("7", BookContent.LeftPagePos));
                contents.Add(Text("8", BookContent.RightPagePos));
                break;
        }
    }

    private static TextContent Text(string key, Vector2 pos, float scale = 1f) => new(ModHelper.GetText($"Common.CardManual.Text{key}"), pos, font: FontManager.BookFont, scale: scale);
    
    private static ImageContent Image(string key, Vector2 pos, float scale = 3f) => new(ModHelper.GetTexture($"BookUI/CardManual_{key}"), pos, scale);
}