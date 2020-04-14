using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic
{
    public class MagicStormScreen : CustomSky
    {
        private bool isActive;
        private float intensity;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 0.12f)
            {
                intensity += 0.005f;
            }
            else if (!isActive && intensity > 0f)
            {
                intensity -= 0.005f;
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            float light = 0.1f;
            return new Color(light - intensity - 0.3f, light - intensity - 0.3f, light - intensity + 0.48f);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                int color = 0;
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(color, color, color + 20f, 60f) * intensity);

                if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
                {
                    for (var index = 0; index < Main.numStars - 2; index += 2)
                    {
                        Star star = Main.star[index];
                        Texture2D tex = Main.starTexture[Main.star[index].type];
                        var num11 = MathHelper.Max(star.position.X * ((float)Main.screenWidth / 800f), 10);
                        var num12 = MathHelper.Max(star.position.Y * ((float)Main.screenHeight / 600f), 10);
                        // Main.caveParallax 范围为0.8-1.0
                        num11 = (Main.screenPosition.X * 0.05f + Main.screenWidth) % num11;
                        num12 = (Main.screenPosition.Y * 0.05f + Main.screenHeight) % num12;
                        int alpha = 120;
                        spriteBatch.Draw(tex, new Vector2(num11, num12), null, new Color(alpha - 40, alpha - 40, alpha + 130, alpha + 40) * intensity, 0f, Vector2.Zero, star.scale * star.twinkle * 1.2f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 30f;
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
