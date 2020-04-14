using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace Entrogic.UI
{
    internal class UITextImage : UIImage
    {
        internal string Text;
        internal Color drawColor;
        internal Texture2D Texture;

        public UITextImage(Texture2D texture, string text, Color color) : base(texture)
        {
            Texture = texture;
            Text = text;
            drawColor = color;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Left.Pixels, Top.Pixels), null, drawColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            Vector2 size = Main.fontMouseText.MeasureString(Text) * 0.5f;
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, Text, Left.Pixels + Width.Pixels * 0.5f - size.X, Top.Pixels + Height.Pixels * 0.6f - size.Y, Color.White, Color.Black, Vector2.Zero, 1f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.
        }
    }
}
