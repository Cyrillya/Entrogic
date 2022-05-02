namespace Entrogic.Content.Projectiles.ContyElemental.Friendly
{
    public class CorrosiveArrow : ProjectileBase
    {
        public override string Texture => ResourceManager.Blank;
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode

            DisplayName.SetDefault("Corrosive");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污痕");
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.TinyEater);
            //Projectile.Size = new Vector2(12, 12);
            //Projectile.alpha = 255;
            //Projectile.ignoreWater = false;
            //Projectile.tileCollide = true;
            //Projectile.timeLeft = 450;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center, MyDustID.White, Projectile.velocity, 50, new Color(56, 114, 80), 1.5f);
            d.noGravity = true;
            d.velocity *= 0.3f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override bool PreDraw(ref Color lightColor) {
            return true;
        }
    }
}
