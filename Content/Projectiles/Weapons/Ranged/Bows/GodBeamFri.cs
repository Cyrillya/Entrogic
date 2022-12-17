namespace Entrogic.Content.Projectiles.Weapons.Ranged.Bows
{
    public class GodBeamFri : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.scale = 0.7f;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = 1;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            AIType = 14;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 353)
            {
                Projectile.alpha = 45;
            }
            //Projectile.velocity *= 0.98f;
            if (Projectile.timeLeft < 60)
                Projectile.alpha += 5;
            //Projectile.velocity = 1;
            Projectile.scale += 0.0016f;
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, Math.Min(10, 2 + (int)Projectile.oldVelocity.Length()), 0.8f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                MyDustID.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
                d.noGravity = true;
            }
        }
    }
}
