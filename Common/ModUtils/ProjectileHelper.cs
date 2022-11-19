using Terraria.Audio;
using Terraria.ID;

namespace Entrogic
{
    public static partial class ModHelper
    {
        public enum TrailColorMode
        {
            NormalColor,
            OpacityMixed
        }
        public enum TrailGlowMaskMode
        {
            None,
            OpacityMixed,
            VanillaColor
        }
        public static void DrawShadow(this Projectile Projectile, Color lightColor, int length = 17, float opacityMulitplier = 0.5f, Rectangle? frame = null, TrailColorMode trailColorMode = TrailColorMode.OpacityMixed, TrailGlowMaskMode glowMaskMode = TrailGlowMaskMode.None) {
            var tex = TextureAssets.Projectile[Projectile.type];
            var t = tex.Value;
            bool glowTextureExists = false;
            int frameHeight = tex.Height() / Main.projFrames[Projectile.type];

            Color alpha13 = Projectile.GetAlpha(lightColor);
            int num136 = 0;
            int num137 = 0;
            float num138 = (float)(tex.Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
            ProjectileLoader.DrawOffset(Projectile, ref num137, ref num136, ref num138);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;

            var rect = new Rectangle(0, frameHeight * Projectile.frame, tex.Width(), frameHeight - 1);
            if (frame != null) rect = frame.Value;

            var origin = new Vector2(num138, Projectile.height / 2 + num136);
            var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + num138 + (float)num137, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            Main.EntitySpriteDraw(t, pos, rect, alpha13, Projectile.rotation, origin, Projectile.scale, effects, 0);

            var modProjectile = Projectile.ModProjectile;

            if (modProjectile != null && ModContent.RequestIfExists<Texture2D>(modProjectile.GlowTexture, out var glowTexture, AssetRequestMode.ImmediateLoad)) { // todo: preload this!
                glowTextureExists = true;
                Main.EntitySpriteDraw(glowTexture.Value, pos, rect, new Color(250, 250, 250, Projectile.alpha), Projectile.rotation, origin, Projectile.scale, effects, 0);
            }
            if (glowMaskMode == TrailGlowMaskMode.None) glowTextureExists = false;

            // 残影
            for (int i = 0; i <= length; i++) {
                pos -= Projectile.oldVelocity * 0.5f;
                float trailOpacity = Projectile.Opacity - 0.05f - 0.95f / length * i;
                if (i != 0) trailOpacity *= opacityMulitplier;
                if (trailOpacity > 0f) {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    if (trailColorMode == TrailColorMode.NormalColor) {
                        colMod = 1f;
                    }
                    var colorSprite = (new Color(1f * colMod, 1f * colMod, 1f, 0.5f) * trailOpacity).MultiplyRGBA(lightColor);
                    Main.EntitySpriteDraw(t, pos, rect, colorSprite, Projectile.rotation, origin, Projectile.scale * (1f + 0.02f * i), effects, 0);

                    if (glowTextureExists) {
                        colMod = 0.4f + 0.6f * trailOpacity;
                        if (glowMaskMode == TrailGlowMaskMode.VanillaColor) {
                            colMod = 1f;
                        }
                        var colorGlow = new Color(1f * colMod, 1f * colMod, 1f, Projectile.Opacity) * trailOpacity;
                        Main.EntitySpriteDraw(t, pos, rect, colorGlow, Projectile.rotation, origin, Projectile.scale * (1f + 0.02f * i), effects, 0);
                    }
                }
            }
        }

        public static void QuickDirectionalHeldProj(this Projectile projectile, Player player, Vector2 mouseWorld, Vector2 armPosition, float rotationOffset = 0f) {
            projectile.velocity = armPosition.DirectionTo(mouseWorld);
            projectile.spriteDirection = projectile.direction = Math.Sign(mouseWorld.X - player.MountedCenter.X);
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.spriteDirection == -1)
                projectile.rotation = 3.14f + projectile.rotation;
            projectile.rotation += rotationOffset * projectile.direction;
            projectile.Center = armPosition + projectile.velocity * 20f;

            player.ChangeDir(projectile.direction);
            projectile.QuickHeldProjBasic(player);
        }

        public static void QuickHeldProjBasic(this Projectile projectile, Player player) {
            player.heldProj = projectile.whoAmI;
            player.SetDummyItemTime(projectile.timeLeft);
            player.itemRotation = MathHelper.WrapAngle(projectile.rotation);
        }

        public static void ProjectileExplode(this Projectile projectile, IEntitySource source, float statRangeX = 22f, float statRangeY = 22f) {
            SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            projectile.position.X += (float)(projectile.width / 2);
            projectile.position.Y += (float)(projectile.height / 2);
            projectile.width = (int)(statRangeX * projectile.scale);
            projectile.height = (int)(statRangeY * projectile.scale);
            projectile.position.X -= (float)(projectile.width / 2);
            projectile.position.Y -= (float)(projectile.height / 2);
            for (int i = 0; i < 20; i++) {
                var d = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
                d.velocity *= 1.4f;
            }
            for (int i = 0; i < 10; i++) {
                var d = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2.5f);
                d.noGravity = true;
                d.velocity *= 5f;
                d = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
                d.velocity *= 3f;
            }
            for (int i = -1; i <= 1; i += 2) {
                for (int j = -1; j <= 1; j += 2) {
                    var g = Gore.NewGoreDirect(source, new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                    g.velocity *= 0.4f;
                    g.velocity.X += i;
                    g.velocity.Y += j;
                }
            }
        }
    }
}
