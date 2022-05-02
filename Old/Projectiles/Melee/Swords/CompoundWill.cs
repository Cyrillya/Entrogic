
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class CompoundWill : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 98;
            projectile.friendly = true;
            projectile.timeLeft = 200;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.extraUpdates = 10;
        }
        public override void AI()
        {
            projectile.alpha = 10;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            Player player = Main.player[projectile.owner];
            if (!player.dead)
            {
                player.direction = (int)projectile.ai[0];
                projectile.position = player.position - new Vector2(0f, player.height/2) + projectile.velocity * 1.4f;
            }
            else
                projectile.Kill();
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool CanHitPvp(Player target)
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
