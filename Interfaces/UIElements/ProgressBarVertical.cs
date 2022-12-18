using Terraria.UI;

namespace Entrogic.Interfaces.UIElements;

public class ProgressBarVertical : UIElement
{
    private readonly Func<float> _getPercent;
    private readonly Texture2D _emptyTexture;
    private readonly Texture2D _filledTexture;

    public ProgressBarVertical(Texture2D emptyTexture, Texture2D filledTexture, Func<float> getPercent) {
        _emptyTexture = emptyTexture;
        _filledTexture = filledTexture;
        _getPercent = getPercent;
        Width = new StyleDimension(_emptyTexture.Width, 0f);
        Height = new StyleDimension(_emptyTexture.Height, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        var dimensions = GetDimensions();
        var center = dimensions.Center();
        var origin = _filledTexture.Size() / 2f;

        spriteBatch.Draw(_emptyTexture, center, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

        float percent = _getPercent.Invoke(); // 获取百分比
        if (percent <= 0f) return; // 如果小于等于0则不绘制填充

        var rect = new Rectangle(0, 0, (int) (_filledTexture.Width * percent), _filledTexture.Height); // 绘制

        spriteBatch.Draw(_filledTexture, center, rect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
    }
}