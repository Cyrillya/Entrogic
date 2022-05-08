namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class SmeltDaggerProjectile : BladeProjectile
    {
        public override void SelectTrailTextures(out Texture2D mainColor, out Texture2D mainShape, out Texture2D maskColor) {
            base.SelectTrailTextures(out _, out mainShape, out maskColor);
            mainColor = ResourceManager.Heatmap.Value;
        }

        public override void SetDefaults() {
            TrailColor = Color.Red;
            StartDegree = -120;
            FinalDegree = 100;
            ReadyDegree = 10;
            EndDegree = 10;
            ReadyTimePercent = 0.3f;
            FinalTimePercent = 0.3f;
            base.SetDefaults();
        }

        public override void PostSwingAI() {
            // 加点粒子
            Player player = Main.player[Projectile.owner];
            if (Stage == 1)
                for (float r = 0f; r <= 1f; r += 0.5f) { // 平滑角度
                    for (float i = 0.4f; i <= 1f; i += 0.2f) { // 向内延申粒子
                        if (Projectile.oldRot[0] == 114514 || Projectile.oldRot[1] == 114514) { // 无旋转角度
                            return;
                        }

                        int length = (int)(Projectile.Size.Length() * Projectile.scale * i);
                        float radiansPassed = Projectile.oldRot[0] - Projectile.oldRot[1];
                        float radians = radiansPassed * r + Projectile.oldRot[1];

                        var pos = Projectile.Center + radians.ToRotationVector2() * length;
                        var velocity = (radians + MathHelper.ToRadians(90f) * player.direction * player.gravDir).ToRotationVector2() * 18f;

                        int spawnRange = 20;
                        pos -= new Vector2(spawnRange) / 2f;

                        Dust d = Dust.NewDustDirect(pos, spawnRange, spawnRange, MyDustID.Fire, velocity.X, velocity.Y, 200, Scale: Main.rand.NextFloat(0.6f, 1.5f));
                        d.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);
                        d.noGravity = true;
                    }
                }
        }

        public override void GetBladeDrawStats(ref Color lightColor, out Color bladeColor, out BlendState blendState) {
            bladeColor = Color.OrangeRed;
            blendState = BlendState.Additive;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire3, 180);
    }
}
