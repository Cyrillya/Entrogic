namespace Entrogic.Interfaces.UIElements;

public abstract class BookContent
{
    internal float Scale;
    internal Vector2 Position;
    internal Color Color;

    internal static readonly Vector2 LeftPagePos = new(38, 46);
    internal static readonly Vector2 RightPagePos = new(338, 46);
    internal BookContent(Vector2 pos, float scale = 1f, Color? color = null) {
        Position = pos;
        Scale = scale;
        Color = color ?? Color.White;
    }
}

public class TextContent : BookContent
{
    internal string Text;
    internal int TextWarpWidth;
    internal Asset<DynamicSpriteFont> Font;

    internal TextContent(string text, Vector2 pos, float scale = 1f, Color? color = null, int textWarpWidth = 224, Asset<DynamicSpriteFont> font = null) : base(pos, scale, color) {
        Text = text;
        TextWarpWidth = 230;
        Font = font;
        Color = color ?? Color.Black;
    }
}

public class ImageContent : BookContent
{
    internal Asset<Texture2D> Texture;

    internal ImageContent(Asset<Texture2D> texture, Vector2 pos, float scale = 1f, Color? color = null) : base(pos, scale, color) {
        Texture = texture;
    }
}