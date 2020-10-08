using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Entrogic.Projectiles.Magic.Staff
{
    public class Hellquartz : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.scale = 1f;
            projectile.alpha = 60;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() / 255f);
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            projectile.ai[0]++;
            if (projectile.ai[0] == 1) Effect();
            if (projectile.ai[0] >= 120 && !Main.dedServ && Main.myPlayer == projectile.owner && Main.player[Main.myPlayer].active)
            {
                projectile.Kill();
            }
            if (projectile.timeLeft < 580 && projectile.timeLeft % 3 == 0)
            {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(projectile.Center, 1, 1
                    , 174, 0f, 0f, 90, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] == 1)
            {
                projectile.ProjectileExplode();
                return;
            }
            projectile.velocity.Y = 8f;
            projectile.velocity.X = 0f;
            Vector2 radRand = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(10, 43));
            Projectile proj = Projectile.NewProjectileDirect(new Vector2(projectile.localAI[0], projectile.localAI[1] - 8f * 120f - 16f) + radRand, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 0f, 1f);
            proj.netUpdate = true;
            Effect();
        }
        public override bool? CanHitNPC(NPC target)
        {
            return projectile.ai[1] == 1;
        }
        public override bool CanHitPlayer(Player target)
        {
            return projectile.ai[1] == 1;
        }
        public override bool CanHitPvp(Player target)
        {
            return projectile.ai[1] == 1;
        }
        public override bool CanDamage()
        {
            return projectile.ai[1] == 1;
        }
        public void Effect()
        {
            float num = 16f;
            int num2 = 0;
            while ((float)num2 < num)
            {
                int num3 = MyDustId.OrangeFire1;
                if (num2 > 8) num3 = MyDustId.OrangeFx;
                Color color = default(Color);
                if (num2 <= 8) color = Color.Orange;
                Vector2 vector12 = Vector2.UnitX * 0f;
                vector12 += -Vector2.UnitY.RotatedBy((double)((float)num2 * (6.28318548f / num)), default) * new Vector2(1f, 4f);
                vector12 = vector12.RotatedBy((double)projectile.velocity.ToRotation(), default);
                int num5 = Dust.NewDust(projectile.Center, 0, 0, num3, 0f, 0f, 0, default, 1f);
                Main.dust[num5].scale = 1.5f;
                Main.dust[num5].noGravity = true;
                Main.dust[num5].position = projectile.Center + vector12;
                Main.dust[num5].velocity = projectile.velocity * 0f + vector12.SafeNormalize(Vector2.UnitY) * 1f;
                int num4 = num2;
                num2 = num4 + 1;
            }
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
                float trailOpacity = projectile.Opacity - 0.05f - 0.95f / length * i;
                if (i != 0) trailOpacity *= 0.8f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t,
                        drawPos.ToPoint().ToVector2(),
                        new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight),
                        //new Color(1f * colMod, 0.7f * colMod, 0.4f, 0.8f) * trailOpacity,
                        new Color(1f, 0.8f, 0.6f, 0.8f) * trailOpacity, // 不受环境影响
                        projectile.rotation,
                        origin,
                        projectile.scale * (1f + 0.02f * i),
                        effects,
                        0);
                }
            }
            return false;
        }
    }
}
