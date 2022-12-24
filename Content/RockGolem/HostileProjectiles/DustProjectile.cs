using Entrogic.Content.Dusts;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.HostileProjectiles
{
    public class DustProjectile : ProjectileBase
    {
        public override void SetStaticDefaults() {
            RockGolem.Enemies.Athanasy.DustProjectileType = Type;
        }

        public override void SetDefaults() {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
        }

        public ref float Timer => ref Projectile.ai[0];
        public ref float Rotation => ref Projectile.ai[1];

        public override void OnSpawn(IEntitySource source) {
            Rotation = Projectile.velocity.ToRotation();
        }

        public override void AI() {
            Timer++;
            if (Timer < 160)
                Projectile.velocity *= 0.94f;

            if (Timer == 160) {
                Projectile.velocity = -Rotation.ToRotationVector2() * 8f;
            }

            if (Main.netMode != NetmodeID.Server) {
                for (float lerp = 0f; lerp <= 1f; lerp += 0.33f) {
                    var d = Dust.NewDustPerfect(Projectile.position - Projectile.velocity * lerp, ModContent.DustType<BubbleCopy>(), Projectile.velocity * 0.03f, 180, default, 1.5f);
                    d.fadeIn = 1f;
                    d.noGravity = true;
                }
            }

            if (Timer >= 190)
                Projectile.Kill();
        }
    }
}
