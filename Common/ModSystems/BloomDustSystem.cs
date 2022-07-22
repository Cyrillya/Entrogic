using Entrogic.Content.Dusts;

namespace Entrogic.Common.ModSystems
{
    public class BloomDustSystem : ModSystem
    {
        public override void Load() {
            if (Main.dedServ)
                return;

            On.Terraria.Main.SetDisplayMode += RefreshCutawayTarget;
            On.Terraria.Main.DrawInfernoRings += DrawBloom;
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;

            Main.RunOnMainThread(() => {
                bloomTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, default, default, default, RenderTargetUsage.PreserveContents);
                bloomTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, default, default, default, RenderTargetUsage.PreserveContents);
            });
        }

        private void DrawBloom(On.Terraria.Main.orig_DrawInfernoRings orig, Main self) {
            orig.Invoke(self);

            //Main.graphics.GraphicsDevice.SetRenderTarget(bloomTarget);
            //Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.BeginGameSpriteBatch();
            //DrawDusts(); // 在RenderTarget上绘制粒子
            //Main.spriteBatch.End();

            //UseBloom(Main.graphics.GraphicsDevice);

            //Main.spriteBatch.BeginGameSpriteBatch();
            //DrawDusts(); // 绘制一层粒子覆盖回来
            //Main.spriteBatch.End();
        }

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor) {
            UseBloom(Main.graphics.GraphicsDevice);

            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }

        private static void UseBloom(GraphicsDevice graphicsDevice) {
            var effect = ShaderManager.Bloom.Value;

            // 存游戏画面的Rt2D
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            graphicsDevice.SetRenderTarget(bloomTarget);
            graphicsDevice.Clear(Color.Transparent);
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Main.spriteBatch.BeginGameSpriteBatch(false);

            effect.CurrentTechnique.Passes["Bloom"].Apply();
            effect.Parameters["uMinBrightness"].SetValue(0f);
            DrawDusts(); // 在RenderTarget上绘制粒子

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom);
            effect.Parameters["uRange"].SetValue(1f);
            effect.Parameters["uIntensity"].SetValue(0.3f);

            effect.CurrentTechnique.Passes["GlurV"].Apply();
            graphicsDevice.SetRenderTarget(bloomTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Draw(bloomTarget, Vector2.Zero, Color.White);

            effect.CurrentTechnique.Passes["GlurH"].Apply();
            graphicsDevice.SetRenderTarget(bloomTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Draw(bloomTargetSwap, Vector2.Zero, Color.White);

            Main.spriteBatch.End();

            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.Draw(bloomTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }


        internal static RenderTarget2D bloomTarget;
        internal static RenderTarget2D bloomTargetSwap;

        public override void Unload() {
            bloomTarget = null;
            bloomTargetSwap = null;
        }

        private void RefreshCutawayTarget(On.Terraria.Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen) {
            if (!Main.gameInactive && (width != Main.screenWidth || height != Main.screenHeight)) {
                bloomTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, default, default, default, RenderTargetUsage.PreserveContents);
                bloomTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, default, default, default, RenderTargetUsage.PreserveContents);
            }

            orig(width, height, fullscreen);
        }

        internal static List<int> BloomDusts = new();

        public override void PostSetupContent() {
            if (Main.dedServ)
                return;

            BloomDusts = new() {
                ModContent.DustType<BubbleCopy>()
            };
        }

        private static void DrawDusts() {
            for (int i = 0; i < Main.maxDustToDraw; i++) {
                Dust dust = Main.dust[i];
                if (!dust.active || !BloomDusts.Contains(dust.type))
                    continue;
                float scale = dust.GetVisualScale();

                Color newColor = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);
                newColor = dust.GetAlpha(newColor);

                Main.spriteBatch.Draw(TextureAssets.Dust.Value, dust.position - Main.screenPosition, dust.frame, newColor, dust.GetVisualRotation(), new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
            }
        }
    }
}
