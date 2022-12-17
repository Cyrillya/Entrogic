namespace Entrogic.Content.Projectiles.Weapons.Arcane;

public class CrystalProj : ProjectileBase
{
    public override void SetDefaults() {
        Projectile.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void AI() {
        Projectile.velocity.Y += 0.21f;
    }
}