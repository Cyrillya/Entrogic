using Terraria.ID;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Bullets
{
    public class HighPressureWaterBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("High Pressure Water Bullet");     //The English name of the Projectile
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "高压水弹");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            Projectile.velocity *= 15f;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.alpha = 4;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(103, 240, false);

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft < 91)
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water_Snow, 0f, 0f, 50, default(Color), 0.85f);
                    dust.noGravity = true;
                    dust.scale = (float)Main.rand.Next(90, 110) * 0.014f;
                    dust.position = Projectile.Center - Projectile.velocity * i / 3f;
                    dust.velocity *= 0.45f;
                    dust.fadeIn = 0.1f;
                    Projectile.velocity.Y += 0.05f;
                }
        }
    }
}




