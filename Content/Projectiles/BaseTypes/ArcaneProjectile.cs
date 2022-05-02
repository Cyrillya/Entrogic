using Entrogic.Content.DamageClasses;

namespace Entrogic
{
    public abstract class ArcaneProjectile : ProjectileBase
    {
        public sealed override void SetDefaults() {
            ArcaneDefaults();
            Projectile.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
        }

        public virtual void ArcaneDefaults() { }
    }
}
