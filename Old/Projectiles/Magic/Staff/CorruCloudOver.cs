using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic.Staff
{
    public class CorruCloudOver : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.width = 54;
            projectile.height = 28;
            projectile.aiStyle = 45;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 6;
        }
        public override void AI()
        {
            int num3 = projectile.frameCounter;
            projectile.frameCounter = num3 + 1;
            if (projectile.frameCounter > 8)
            {
                projectile.frameCounter = 0;
                num3 = projectile.frame;
                projectile.frame = num3 + 1;
                if (projectile.frame > 5)
                {
                    projectile.frame = 0;
                }
            }

            projectile.ai[1] += 1f;
            if (projectile.ai[1] >= 10800f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
            else
            {
                projectile.ai[0] += 1f;
                if (projectile.ai[0] > 8f)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer)
                    {
                        int num419 = (int)(projectile.position.X + 14f + (float)Main.rand.Next(projectile.width - 28));
                        int num420 = (int)(projectile.position.Y + (float)projectile.height + 4f);
                        Projectile.NewProjectile((float)num419, (float)num420, 0f, 5f, ProjectileType<CorruCloudRain>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
                    }
                }
            }
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= 10f)
            {
                projectile.localAI[0] = 0f;
                int num421 = 0;
                int num422 = 0;
                float num423 = 0f;
                int num424 = projectile.type;
                for (int num425 = 0; num425 < 1000; num425 = num3 + 1)
                {
                    if (Main.projectile[num425].active && Main.projectile[num425].owner == projectile.owner && Main.projectile[num425].type == num424 && Main.projectile[num425].ai[1] < 3600f)
                    {
                        num3 = num421;
                        num421 = num3 + 1;
                        if (Main.projectile[num425].ai[1] > num423)
                        {
                            num422 = num425;
                            num423 = Main.projectile[num425].ai[1];
                        }
                    }
                    num3 = num425;
                }
                if (num421 > 3)
                {
                    Main.projectile[num422].netUpdate = true;
                    Main.projectile[num422].ai[1] = 36000f;
                    return;
                }
            }
        }
    }
}