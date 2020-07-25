using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace Entrogic.UI
{
    public class UIDelayImageY : UIImage
    {
        internal int maxValue = 10;
        internal int minValue = 0;
        internal int delayValue = 10;
        internal int Alpha;
        internal Texture2D Texture;

        public UIDelayImageY(Texture2D texture, int alpha) : base(texture)
        {
            Texture = texture;
            Alpha = alpha;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch); // 不使用原绘制方案
            float drawY = Height.Pixels / (maxValue - minValue);
            spriteBatch.Draw(Texture, new Vector2(Left.Pixels, Top.Pixels + (int)(Height.Pixels - drawY * delayValue)), new Rectangle(0, (int)(Height.Pixels - drawY * delayValue), (int)Width.Pixels, (int)(drawY * delayValue)), new Color(Alpha, Alpha, Alpha, Alpha), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
    public class UIDelayImageX : UIImage
    {
        internal int maxValue = 10;
        internal int minValue = 0;
        internal int delayValue = 10;
        internal int Alpha;
        internal Texture2D Texture;

        public UIDelayImageX(Texture2D texture, int alpha) : base(texture)
        {
            Texture = texture;
            Alpha = alpha;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch); // 不使用原绘制方案
            float drawX = Width.Pixels / (maxValue - minValue);
            spriteBatch.Draw(Texture, new Vector2(Left.Pixels, Top.Pixels), new Rectangle(0, 0, (int)(drawX * delayValue), (int)Height.Pixels), new Color(Alpha, Alpha, Alpha, Alpha), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
