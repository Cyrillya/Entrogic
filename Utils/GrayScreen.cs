using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic
{
    public class GrayScreen : CustomSky
    {
        private bool isActive;
        private float intensity;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 1f)
            {
                intensity += 0.015f;
            }
            else if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            float light = Main.dayTime ? 1.5f : 1.2f;
            return new Color(light - intensity, light - intensity, light - intensity);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                int color = Main.dayTime ? 20 : 15;
                spriteBatch.Draw((Texture2D)TextureAssets.BlackTile, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(color, color, color) * intensity);

                if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
                {
                    for (var index = 0; index < Main.numStars; ++index)
                    {
                        Star star = Main.star[index];
                        Texture2D tex = (Texture2D)TextureAssets.Star[Main.star[index].type];
                        var num11 = star.position.X * ((float)Main.screenWidth / 800f);
                        var num12 = star.position.Y * ((float)Main.screenHeight / 600f);
                        int alpha = Main.dayTime ? 95 : 130;
                        spriteBatch.Draw(tex, new Vector2(num11, num12), null, new Color(alpha, alpha, alpha, alpha) * intensity, 0f, Vector2.Zero, star.scale * star.twinkle, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 180f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0f;
        }
    }
}