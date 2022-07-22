using Entrogic.Content.Dusts;

namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class CollapseLaser : ProjectileBase
    {
        public override void SetDefaults() {
            Projectile.Size = new(8);
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
        }

        public override void AI() {
            if (Projectile.timeLeft > 290)
                return;
            var d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BubbleCopy>(), Vector2.Zero, 180, default, 1.4f);
            d.fadeIn = 1.2f;
            d.noGravity = true;
        }
    }
}
