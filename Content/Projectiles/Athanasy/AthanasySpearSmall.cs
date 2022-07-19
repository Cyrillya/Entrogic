using Terraria.Audio;
using Terraria.Graphics.Shaders;

namespace Entrogic.Content.Projectiles.Athanasy
{
    public class AthanasySpearSmall : ProjectileBase
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = -1; // 自己有一套残影Update
            NPCs.Enemies.Athanasy.Athanasy.SmallSpearType = Type;
        }

        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.PoisonDart);
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.DamageType = DamageClass.Ranged;
            DrawOffsetX = -70;
            DrawOriginOffsetX = 34;
            DrawOriginOffsetY = -4;
        }

        public override bool ShouldUpdatePosition() {
            return true;
        }

        public ref float Timer => ref Projectile.ai[0];
        public float rotation;

        public override void AI() {
            Timer++;
            if (Timer == 1) {
                Projectile.netUpdate = true;
                rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = -Projectile.velocity;
                for (int i = 0; i < 8; i++) {
                    float radians = 6.28f / 8f * i;
                    var velocity = Vector2.One.RotatedBy(radians) * 1.4f;
                    var d = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonWater, velocity, 180);
                    d.fadeIn = 0.4f;
                    d.noGravity = true;
                }
            }

            if (Timer <= 5 && !Main.dedServ) {
                float factor = Utils.GetLerpValue(1, 5, Timer);
                Projectile.Opacity = MathHelper.Lerp(0f, 1f, factor);
            }

            if (Timer <= 55f) {
                if (Timer <= 20f) {
                    Projectile.velocity = -rotation.ToRotationVector2() * 0.1f;
                }
                else if (Timer <= 40f) {
                    // Timer区间: 20-40
                    float factor = Utils.GetLerpValue(20, 40, Timer);
                    factor *= factor;
                    Projectile.velocity = -rotation.ToRotationVector2() * MathHelper.Lerp(3f, 16f, factor);
                }
                else if (Timer <= 50f) {
                    float factor = Utils.GetLerpValue(40, 50, Timer);
                    factor *= factor;
                    Projectile.velocity = -rotation.ToRotationVector2() * MathHelper.Lerp(16f, 0.9f, factor);
                }
                else {
                    float factor = Utils.GetLerpValue(50, 55, Timer);
                    Projectile.velocity = -rotation.ToRotationVector2() * MathHelper.Lerp(0.9f, 12f, factor);

                    var d = Dust.NewDustDirect(Projectile.Center, 8, 8, DustID.Stone, Alpha: 140, Scale: Main.rand.NextFloat(1.5f, 3.2f));
                    d.fadeIn = 0.4f;
                    d.noGravity = true;
                }
            }
            else {
                Projectile.velocity = rotation.ToRotationVector2() * 18f;
            }

            Projectile.rotation = rotation;
            
            // 射出的时候才开始录残影
            if (Timer >= 50f && !Main.dedServ) {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--) {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldPos[0] = Projectile.position;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            var tex = TextureAssets.Projectile[Type];
            var t = tex.Value;
            int frameHeight = tex.Height() / Main.projFrames[Type];

            Color drawColor = Projectile.GetAlpha(lightColor);
            int drawOriginOffsetY = 0;
            int drawOffsetX = 0;
            float drawOriginOffsetX = (float)(tex.Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
            ProjectileLoader.DrawOffset(Projectile, ref drawOffsetX, ref drawOriginOffsetY, ref drawOriginOffsetX);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;

            var rect = new Rectangle(0, frameHeight * Projectile.frame, tex.Width(), frameHeight - 1);

            var origin = new Vector2(drawOriginOffsetX, Projectile.height / 2 + drawOriginOffsetY);
            var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            // 残影
            if (Timer > 50f) {
                for (int i = 0; i < Projectile.oldPos.Length; i++) {
                    var posShadow = new Vector2(Projectile.oldPos[i].X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.oldPos[i].Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);
                    float factor = Utils.GetLerpValue(0, Projectile.oldPos.Length, i); // 透明度插值
                    float opacity = MathHelper.Lerp(0.5f, 0f, factor);
                    Main.EntitySpriteDraw(t, posShadow, rect, drawColor * opacity, Projectile.rotation, origin, Projectile.scale, effects, 0);
                }
            }

            if (Timer <= 20) {
                Color color = new(90, 70, 255, 50);
                float factor = Utils.GetLerpValue(0f, 20f, Timer);
                color *= MathHelper.Lerp(0.7f, 0f, factor);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.Apply(ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex, Projectile, null);

                for (float i = 0f; i < 1f; i += 0.4f) {
                    float radians = i * MathHelper.TwoPi;

                    Main.EntitySpriteDraw(t, pos + new Vector2(0f, 8f).RotatedBy(radians), null, color, Projectile.rotation, origin, Projectile.scale, effects, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Main.EntitySpriteDraw(t, pos, rect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return true;
        }

        public override bool CanHitPlayer(Player target) {
            return base.CanHitPlayer(target) && Timer > 50f;
        }

        public override void Kill(int timeLeft) {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Projectile.velocity *= 0.2f;
            for (int i = 0; i < 20; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonBlue, Projectile.velocity.X, Projectile.velocity.Y, 40);
            }
        }
    }
}
