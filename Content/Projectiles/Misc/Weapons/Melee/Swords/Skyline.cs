using Terraria.Audio;
using Terraria.ID;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class Skyline : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults() {
            Projectile.width = 14;
            Projectile.height = 26;
            Projectile.aiStyle = 5;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.light = 1.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Melee;
            AIType = ProjectileID.HellfireArrow;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            for (float rad = 0.0f; rad < 2 * 3.141f; rad += (float)Main.rand.Next(1, 3) / 10) {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Main.rand.Next(-10, 11), Main.rand.Next(-10, 11), ProjectileID.DD2FlameBurstTowerT3Shot, 29, 0f, Projectile.owner, 0f, 0f);
            }
        }
        public override void Kill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 14, 1f, 0f);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int i = 0; i < 15; i++) {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num].velocity *= 2f;
                if (Main.rand.Next(2) == 0) {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 15; j++) {
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 5f;
                num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.InfernoFork, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num2].velocity *= 2f;
            }
            for (int k = 0; k < 3; k++) {
                float scaleFactor = 0.33f;
                if (k == 1) {
                    scaleFactor = 0.66f;
                }
                if (k == 2) {
                    scaleFactor = 1f;
                }
                var g = Gore.NewGoreDirect(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                g.velocity *= scaleFactor;
                g.velocity += Vector2.One;
            }
        }
        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor);
            return false;
        }

        public override void AI() {
            if (Projectile.timeLeft < 297) {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height
                    , DustID.InfernoFork, 0f, 0f, 90, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
    }

}




