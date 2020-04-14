using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic.Staff
{
	public class 可爱魔法流 : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("可爱魔法流");
            Main.projFrames[projectile.type] = 2;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f, 0, default(Color), 2f);
            }
        }

        public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.magic = true;
            projectile.aiStyle = 1;
            aiType = 14;
			projectile.penetrate = 1;
            projectile.light = 1f;
			projectile.timeLeft = 600;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            for (int i = 0; i < 10; i++)
            {
                int Cyril = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f, 0, default(Color), 2f);
                Main.dust[Cyril].noGravity = true;
                Main.dust[Cyril].fadeIn = 1f;
                Main.dust[Cyril].velocity *= 1.5f;
            }
        }

		public override void AI()
        {
            for (int j = 0; j < 5; j++)
            {
                Dust d = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1f)];
                d.noGravity = true;
                d.velocity *= 0.75f;
                d.fadeIn = 0.75f;
            }
        }
	}
}
