using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class 熔炼匕首 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 24;
        }

        public override void SetDefaults()
        {
            projectile.width = 76;
            projectile.height = 76;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.scale = 1.2f;
            projectile.alpha = 128;
        }
        public override void AI()
        {
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 12;
            }
            projectile.timeLeft = 2;
            if (!Main.player[projectile.owner].channel)
                projectile.Kill();
            projectile.ai[0]++;
            if (projectile.ai[0] >= 1)
            {
                projectile.frame++;
                projectile.ai[0] = 0;
            }
            if (projectile.frame >= 24)
                projectile.frame = 0;
            Main.player[projectile.owner].heldProj = projectile.whoAmI;
            projectile.Center = Main.player[projectile.owner].Center + projectile.Size * ((projectile.scale - 1f) * 0.5f);
            Main.player[projectile.owner].itemTime = 6;
            Main.player[projectile.owner].itemAnimation = 6;
            if (Main.MouseWorld.X > projectile.Center.X)
                Main.player[projectile.owner].direction = 1;
            else
                Main.player[projectile.owner].direction = -1;
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(projectile.position + new Vector2(Main.rand.Next(18, 31) * Main.player[projectile.owner].direction, 10f), projectile.width, projectile.height - 10, 6, 8.6f * Main.player[projectile.owner].direction, 0, 160, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
            }
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 254 / 255f, 133 / 255f, 70 / 255f);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
