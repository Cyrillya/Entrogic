using Terraria.ID;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Thrown
{
    public class Leafblade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("叶片");     //The English name of the Projectile
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(36, 180);
            for (int i = 0; i < 7; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 90, default(Color), 0.75f);
                Main.dust[num].velocity *= 0.75f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.25f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            if (Projectile.timeLeft < 177)
            {
                for (int j = 0; j < 2; j++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, 0f, 0f, 90, default(Color), 0.75f);
                    dust.noGravity = true;
                }
            }
        }
    }

}




