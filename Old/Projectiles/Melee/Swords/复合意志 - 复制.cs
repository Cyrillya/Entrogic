/*using System;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class 复合意志 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 98;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.alpha = 180;
        }
        public override void AI()
        {
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 20;
            }
            projectile.timeLeft = 2;
            if (!Main.player[projectile.owner].channel)
                projectile.Kill();
            projectile.ai[0]++;
            if (projectile.ai[0] >= 3)
            {
                projectile.frame++;
                projectile.ai[0] = 0;
            }
            if (projectile.frame >= 7)
                projectile.frame = 0;
            Main.player[projectile.owner].heldProj = projectile.whoAmI;
            projectile.Center = Main.player[projectile.owner].Center;
            Main.player[projectile.owner].itemTime = 6;
            Main.player[projectile.owner].itemAnimation = 6;
            if (Main.MouseWorld.X > projectile.Center.X)
                Main.player[projectile.owner].direction = 1;
            else
                Main.player[projectile.owner].direction = -1;
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 220 / 255f, 29 / 255f, 183 / 255f);
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(projectile.position + new Vector2(Main.rand.Next(18, 31) * Main.player[projectile.owner].direction, 10f), projectile.width, projectile.height - 10, MyDustId.TransparentPurple, 8.6f * Main.player[projectile.owner].direction, 0, 160, default(Color), 2f);
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
*/