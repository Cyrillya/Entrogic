using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Thrown
{
    public class Leafblade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("叶片");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 14;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 180;
            projectile.alpha = 0;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            aiType = ProjectileID.Bullet;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(36, 180);
            for (int i = 0; i < 7; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2, 0f, 0f, 90, default(Color), 0.75f);
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
            projectile.velocity.Y += 0.05f;
            if (projectile.timeLeft < 177)
            {
                for (int j = 0; j < 2; j++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 2, 0f, 0f, 90, default(Color), 0.75f);
                    dust.noGravity = true;
                }
            }
        }
    }

}




