using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace Entrogic.UI
{
    internal class UITextImage : UIImage
    {
        internal string Text;
        internal Color drawColor;
        internal Asset<Texture2D> Texture;

        public UITextImage(Asset<Texture2D> texture, string text, Color color) : base(texture)
        {
            Texture = texture;
            Text = text;
            drawColor = color;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw((Texture2D)Texture, new Vector2(Left.Pixels, Top.Pixels), null, drawColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            Vector2 size = ((DynamicSpriteFont)FontAssets.MouseText).MeasureString(Text) * 0.5f;
            Utils.DrawBorderStringFourWay(spriteBatch, (DynamicSpriteFont)FontAssets.MouseText, Text, Left.Pixels + Width.Pixels * 0.5f - size.X, Top.Pixels + Height.Pixels * 0.6f - size.Y, Color.White, Color.Black, Vector2.Zero, 1f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.
        }
    }
}
