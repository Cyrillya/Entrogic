using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;

namespace Entrogic
{
    public static partial class ModHelper
    {
        public static Rectangle GetTileCoordinates(Rectangle rect) => new(rect.X << 4, rect.Y << 4, rect.Width << 4, rect.Height << 4);

        public static Rectangle ToTileCoordinates(this Rectangle rect) => new(rect.X << 4, rect.Y << 4, rect.Width << 4, rect.Height << 4);

        public static Rectangle CreateFromVector2(Vector2 vec, float width, float height) => new((int)vec.X, (int)vec.Y, (int)width, (int)height);

        public static Rectangle CreateFromVector2(Vector2 vec, Vector2 size) => CreateFromVector2(vec, size.X, size.Y);

        public static Vector2 GetFromToVector(Vector2 v1, Vector2 v2) => v2 - v1;

        public static Vector2 GetFromToVectorNormalized(Vector2 v1, Vector2 v2) => Vector2.Normalize(GetFromToVector(v1, v2));

        public static float GetFromToRadians(Vector2 v1, Vector2 v2) => GetFromToVector(v1, v2).ToRotation();
        public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth) {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - borderWidth, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X + (int)size.X, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
        }

        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="rectangle1">矩形</param>
        /// <returns></returns>
        public static bool MouseInRectangle(Rectangle rectangle1) => rectangle1.Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));

        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height) => new Rectangle(X, Y, width, height).Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));
        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <param name="OFFXLeft">向左偏移长度。</param>
        /// <param name="OFFYTop">向上偏移长度。</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height, int OFFXLeft = 0, int OFFYTop = 0) {
            Vector2 mountedCenter = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            return new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)(X + Main.screenPosition.X - OFFXLeft), (int)(Y + Main.screenPosition.Y - OFFYTop), width, height));
        }

        /// <summary>
        /// 鼠标与某矩形重合后绘制鼠标旁的悬浮字
        /// </summary>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="Width">矩形宽</param>
        /// <param name="Hegith">矩形高</param>
        public static void DrawMouseTextOnRectangle(DynamicSpriteFont font, string text, int X, int Y, int Width, int Hegith) {
            Vector2 mountedCenter = Main.MouseScreen;
            if (new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)X, (int)Y, Width, Hegith))) {
                string name = text;
                Vector2 worldPos = new(mountedCenter.X + 15, mountedCenter.Y + 15);
                Vector2 size = font.MeasureString(name);
                Vector2 texPos = worldPos + new Vector2(-size.X * 0.5f, name.Length);
                Main.spriteBatch.DrawString(font, name, new Vector2(texPos.X, texPos.Y), Color.White);
            }
        }
    }
}
