
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Ranged.Bows
{
    public class GodBeamFri : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 96;
            projectile.friendly = true;
            projectile.scale = 0.7f;
            projectile.timeLeft = 360;
            projectile.aiStyle = 1;
            aiType = 14;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (projectile.timeLeft < 353)
            {
                projectile.alpha = 45;
            }
            //projectile.velocity *= 0.98f;
            if (projectile.timeLeft < 60)
                projectile.alpha += 5;
            //projectile.velocity = 1;
            projectile.scale += 0.0016f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = Main.projectileTexture[projectile.type];
            int frameHeight = t.Height / Main.projFrames[projectile.type];
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            if (projectile.localAI[0] < 0) effects = effects | SpriteEffects.FlipVertically;
            Vector2 origin = new Vector2(t.Width / 2, frameHeight / 2);

            int length = Math.Min(10, 2 + (int)projectile.oldVelocity.Length());

            for (int i = length; i >= 0; i--)
            {
                Vector2 drawPos = projectile.Center - Main.screenPosition - projectile.oldVelocity * i * 0.5f;
                float trailOpacity = projectile.Opacity - 0.05f - (0.95f / length) * i;
                if (i != 0) trailOpacity /= 2f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t,
                        drawPos.ToPoint().ToVector2(),
                        new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight),
                        new Color(1f * colMod, 1f * colMod, 1f, 0.5f) * trailOpacity,
                        projectile.rotation,
                        origin,
                        projectile.scale * (1f + 0.02f * i),
                        effects,
                        0);
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                MyDustId.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
                d.noGravity = true;
            }
        }
    }
}
