namespace Entrogic.Core.Global.Resource;

internal class FontManager : ModSystem
{
    internal static Asset<DynamicSpriteFont> BookFont;

    public override void Load() {
        if (!Main.dedServ) {
            BookFont = ModContent.Request<DynamicSpriteFont>("Entrogic/Assets/BookFont");
        }
    }
}
