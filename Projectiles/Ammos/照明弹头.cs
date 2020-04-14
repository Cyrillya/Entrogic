using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Ammos
{
    public class 照明弹头 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("照明弹头");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 400;
            projectile.alpha = 0;
            projectile.light = 1.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.Bullet;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300, false);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num].velocity *= 7f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 6f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[num].noGravity = true;
                }
            }
            for (int j = 0; j < 15; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 3f;
                Main.dust[num2].fadeIn = 10f + (float)Main.rand.Next(10) * 0.1f;
            }
        }
        public override void AI()
        {
            if (projectile.timeLeft < 398)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 0.5f);
                // 粒子特效不受重力
                dust.noGravity = false;
                dust.fadeIn = 0.75f;
            }
        }
    }

}




