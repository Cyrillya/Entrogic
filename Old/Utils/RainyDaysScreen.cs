using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic
{
    public class RainyDaysScreen : CustomSky
    {
        private bool isActive;
        private float intensity;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 0.5f)
            {
                intensity += 0.01f;
            }
            else if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            if (!Main.dayTime)
            {
                return inColor;
            }
            float light = 1.2f;
            return new Color(light - intensity, light - intensity, light - intensity);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                int color = 0;
<<<<<<< HEAD
                spriteBatch.Draw((Texture2D)TextureAssets.BlackTile, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(color, color, color, 60f) * intensity);
                Main.numClouds = 172;
                Main.windSpeedCurrent = 0.3f + 3f * (intensity * 2f);
=======
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(color, color, color, 60f) * intensity);
                Main.numClouds = 172;
                Main.windSpeed = 0.3f + 3f * (intensity * 2f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
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
