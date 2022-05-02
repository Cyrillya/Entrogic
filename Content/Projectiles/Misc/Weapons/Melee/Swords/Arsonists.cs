using Terraria.ID;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class Arsonists : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI() {
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() / 255f);
            Projectile.rotation += 0.2f;
            if (Projectile.timeLeft < 297) {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height
                    , DustID.InfernoFork, 0f, 0f, 90, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
        public override void Kill(int timeLeft) {
            Projectile.ProjectileExplode();
        }
        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }

}




