namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class Tornado : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Type] = 7;

        public override void SetDefaults() {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0f;
            DrawOriginOffsetX = -8;
            DrawOriginOffsetY = -8;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            HitCounts++;
            if (HitCounts > 3 && Projectile.timeLeft > 30)
                Projectile.timeLeft = 30;
        }

        public ref float HitCounts => ref Projectile.ai[0];

        public override void AI() {
            if (Projectile.Opacity < 0.6f) {
                Projectile.Opacity += 0.05f;
            }

            if (Projectile.timeLeft <= 30) {
                float factor = Utils.GetLerpValue(0f, 30f, Projectile.timeLeft);
                Projectile.Opacity = MathHelper.SmoothStep(0f, 0.6f, factor);
            }

            if (++Projectile.frameCounter >= 5) {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            hitboxCenterFrac = new(0.3f, 0.3f);
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity;
            if (Projectile.timeLeft > 30)
                Projectile.timeLeft = 30;
            return false;
        }
    }
}
