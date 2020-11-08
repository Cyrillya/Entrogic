using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class EffectRay : ModProjectile
    {
        private bool Died = false; // 标记射线是否有过一刻没有owner
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = projectile.height = 9;
            projectile.timeLeft = 20;
            projectile.alpha = 255;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile owner = Main.projectile[(int)projectile.ai[1]];
            if (owner.active || !Died) projectile.Center = Main.projectile[(int)projectile.ai[1]].Center + new Vector2(projectile.ai[0], 0f);
            if (!owner.active) Died = true;
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (projectile.timeLeft > 15)
            {
                projectile.alpha -= 255 / 5;

            }
            if (projectile.timeLeft < 10)
            {
                projectile.alpha += 255 / 10;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha);
        }

    }
}