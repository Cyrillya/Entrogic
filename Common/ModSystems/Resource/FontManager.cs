namespace Entrogic.Common.ModSystems;

internal class FontManager : ModSystem
{
    internal static Asset<DynamicSpriteFont> BookFont;

    public override void Load() {
        if (!Main.dedServ) {
            BookFont = ModContent.Request<DynamicSpriteFont>("Entrogic/Assets/BookFont");
        }
    }
}
