using Entrogic.Content.Dusts;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Entrogic.Content.Projectiles.Weapons.Ranged.Bullets
{
    public class SnowStormy : ModProjectile
    {
        public override void Load() {
            On.Terraria.Graphics.RainbowRodDrawer.Draw += RainbowRodDrawer_Draw;
        }

        private static VertexStrip _vertexStrip = new();
        private void RainbowRodDrawer_Draw(On.Terraria.Graphics.RainbowRodDrawer.orig_Draw orig, ref RainbowRodDrawer self, Projectile proj) {
            if (proj.type == Type) {
                MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
                miscShaderData.UseSaturation(-2.8f);
                miscShaderData.UseOpacity(3f);
                miscShaderData.Apply(null);
                _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false);
                _vertexStrip.DrawTrail();
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }
            else orig(ref self, proj);
        }

        private Color StripColors(float progressOnStrip) => Color.White;

        private float StripWidth(float progressOnStrip) {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.localNPCHitCooldown = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 3;
        }

        public override void AI() {
            Projectile.rotation = Utils.ToRotation(Projectile.velocity);
            Projectile.ai[0]++;
            if (Projectile.ai[0] <= 20f) Projectile.velocity *= 1.03f;
        }

        public override void PostDraw(Color lightColor) {
            default(RainbowRodDrawer).Draw(Projectile/*, Color.White, 2.8f, 40f*/);
            base.PostDraw(lightColor);
        }

        // 来自彩虹法杖Kill代码
        public override void Kill(int timeLeft) {
            //范围伤害
            Projectile.Resize(80, 80);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            // 粒子
            Projectile.Resize(16, 16);
            for (int l = 0; l < Projectile.oldPos.Length; l++) {
                Vector2 trailPosition = Projectile.oldPos[l];
                if (trailPosition == Vector2.Zero)
                    break;

                Color dustColor = Color.White;
                int dustAmount = Main.rand.Next(1, 4);
                float lerp = Utils.GetLerpValue(Projectile.oldPos.Length, 0f, l, clamped: true);
                float fade = MathHelper.Lerp(0.3f, 1f, lerp);
                if (l >= Projectile.oldPos.Length * 0.3f)
                    dustAmount--;

                if (l >= Projectile.oldPos.Length * 0.65f)
                    dustAmount -= 2;

                if (l >= Projectile.oldPos.Length * 0.85f)
                    dustAmount -= 3;

                Vector2 dustVelocity = trailPosition.DirectionTo(Projectile.Center).SafeNormalize(Vector2.Zero);
                Vector2 positionOffset = Projectile.Size / 2f;
                for (float i = 0f; i < dustAmount; i++) {
                    var d = Dust.NewDustDirect(Vector2.Lerp(positionOffset, Vector2.Zero, lerp) + trailPosition, Projectile.width, Projectile.height, ModContent.DustType<WhiteLingeringCopy>(), 0f, 0f, 0, dustColor);
                    d.velocity *= Main.rand.NextFloat() * 0.8f;
                    d.noGravity = true;
                    d.scale = MathHelper.Lerp(0.5f, 1.5f, lerp) + Main.rand.NextFloat() * 1.2f;
                    d.fadeIn = Main.rand.NextFloat() * 1.2f * fade;
                    d.velocity += dustVelocity * 6f;
                    d.scale *= fade;
                    if (d.dustIndex != 6000) {
                        d = Dust.CloneDust(d);
                        d.scale /= 2f;
                        d.fadeIn *= 0.85f;
                        d.color = new Color(255, 255, 255, 255);
                    }
                }
            }

            for (float r = 0f; r < 0.5f; r += 0.25f) {
                ParticleOrchestraSettings settings = new ParticleOrchestraSettings {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Vector2.UnitX.RotatedBy(r * ((float)Math.PI * 2f)) * 16f
                };

                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, settings, Projectile.owner);
            }

            for (int m = 0; m < 14; m++) {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, MyDustID.WhiteParticle, 0f, 0f, 100, Color.White, 1.7f);
                d.noGravity = true;
                d.velocity *= 3f;
            }
        }
    }
}
