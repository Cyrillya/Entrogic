namespace Entrogic.Content.Projectiles.ContyElemental.Hostile
{
    public class ContamintedSpike : ProjectileBase
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Contaminated Spike");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染尖刺");
        }

        public override void SetDefaults() {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.Size = new Vector2(12, 12);
            Projectile.penetrate = 6;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 450;
            Projectile.scale = 1.6f;
        }

        public override void AI() {
            Projectile.velocity *= 1.3f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, 6);
            return false;
        }
    }
}
