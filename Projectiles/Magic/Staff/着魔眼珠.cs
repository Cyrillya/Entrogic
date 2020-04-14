using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic.Staff
{
	public class 着魔眼珠 : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("着魔眼珠");
            Main.projFrames[projectile.type] = 2;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, 0f, 0f, 0, default(Color), 2f);
            }
        }

        public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.magic = true;
            projectile.aiStyle = 1;
            aiType = 14;
			projectile.penetrate = 4;
			projectile.timeLeft = 720;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 10f)
			{
				projectile.ai[0] = 10f;
				int num = this.HomeOnTarget();
				if (num != -1)
				{
					NPC npc = Main.npc[num];
					Vector2 value = projectile.DirectionTo(npc.Center) * 60f;
					projectile.velocity = Vector2.Lerp(projectile.velocity, value, 0.05f);
				}
			}
		}

		private int HomeOnTarget()
		{
			int num = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy(projectile, false))
				{
					bool wet = npc.wet;
					float num2 = projectile.Distance(npc.Center);
					if (num2 <= 1000f && (num == -1 || projectile.Distance(Main.npc[num].Center) > num2))
					{
						num = i;
                        projectile.frameCounter++;
                        if (projectile.frameCounter > 30)
                        {
                            projectile.frame++;
                            projectile.frameCounter = 0;
                        }
                        if (projectile.frame >= 2)
                        {
                            projectile.frame = 0;
                        }

                    }
                }
			}
			return num;
		}
	}
}
