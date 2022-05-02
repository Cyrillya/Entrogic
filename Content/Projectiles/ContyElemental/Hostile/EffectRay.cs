namespace Entrogic.Content.Projectiles.ContyElemental.Hostile
{
    public class EffectRay : ProjectileBase
    {
        private bool Died = false; // 标记射线是否有过一刻没有owner
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = Projectile.height = 9;
            Projectile.timeLeft = 20;
            Projectile.alpha = 255;
        }
        public override bool? CanDamage() {
            return false;
        }
        public override void AI() {
            Projectile.alpha = Math.Min(255, Projectile.alpha);
            Projectile.alpha = Math.Max(0, Projectile.alpha);
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            //Main.NewText(Projectile.Center - owner.Center);
            if (owner.active || !Died) Projectile.Center = Main.projectile[(int)Projectile.ai[1]].Center + new Vector2(Projectile.ai[0], 0f);
            if (!owner.active) Died = true;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Projectile.timeLeft > 15)
            {
                Projectile.alpha -= 255 / 5;
            }
            if (Projectile.timeLeft < 10)
            {
                Projectile.alpha += 255 / 10;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

    }
}