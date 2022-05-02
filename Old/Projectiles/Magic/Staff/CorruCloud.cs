using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic.Staff
{
    public class CorruCloud : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 4;
        }
        bool CheckMouse = false;
        Vector2 StartMouse = new Vector2(0f, 0f);
        public override void AI()
        {
            if (!CheckMouse)
            {
                StartMouse = Main.MouseWorld;
                CheckMouse = true;
            }
            Vector2 distanceToChansform = projectile.Center - StartMouse;
            float num415 = projectile.ai[0];
            float num416 = projectile.ai[1];
            if (num415 != 0f && num416 != 0f)
            {
                bool flag12 = false;
                bool flag13 = false;
                if ((projectile.velocity.X < 0f && projectile.Center.X < num415) || (projectile.velocity.X > 0f && projectile.Center.X > num415))
                {
                    flag12 = true;
                }
                if ((projectile.velocity.Y < 0f && projectile.Center.Y < num416) || (projectile.velocity.Y > 0f && projectile.Center.Y > num416))
                {
                    flag13 = true;
                }
                if (flag12 && flag13)
                {
                    projectile.Kill();
                }
            }
            if (distanceToChansform.Length() < 32) projectile.Kill();
            projectile.rotation += projectile.velocity.X * 0.02f;
            int num3 = projectile.frameCounter;
            projectile.frameCounter = num3 + 1;
            if (projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                num3 = projectile.frame;
                projectile.frame = num3 + 1;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                    return;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0.0f, 0.0f, ProjectileType<CorruCloudOver>(), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0.0f);
            }
        }
    }
}