using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class 晨昏线 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("晨昏线");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 26;
            projectile.aiStyle = 5;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.alpha = 0;
            projectile.light = 1.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.localNPCHitCooldown = 15;
            aiType = ProjectileID.HellfireArrow;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (float rad = 0.0f; rad < 2 * 3.141f; rad += (float)Main.rand.Next(1, 3) / 10)
            {
                Vector2 vec = new Vector2(120f, 0f);
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * 14f;
                Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, Main.rand.Next(-10, 11), Main.rand.Next(-10, 11), 668, 29, 0f, base.projectile.owner, 0f, 0f);
            }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 14, 1f, 0f);
            base.projectile.position.X = base.projectile.position.X + (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y + (float)(base.projectile.height / 2);
            base.projectile.width = 20;
            base.projectile.height = 20;
            base.projectile.position.X = base.projectile.position.X - (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y - (float)(base.projectile.height / 2);
            for (int i = 0; i < 15; i++)
            {
                int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 31, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num].velocity *= 2f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 15; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 35, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 5f;
                num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 174, 0f, 0f, 90, default(Color), 1f);
                Main.dust[num2].velocity *= 2f;
            }
            for (int k = 0; k < 3; k++)
            {
                float scaleFactor = 0.33f;
                if (k == 1)
                {
                    scaleFactor = 0.66f;
                }
                if (k == 2)
                {
                    scaleFactor = 1f;
                }
                int num3 = Gore.NewGore(new Vector2(base.projectile.position.X + (float)(base.projectile.width / 2) - 24f, base.projectile.position.Y + (float)(base.projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore = Main.gore[num3];
                gore.velocity.X = gore.velocity.X + 1f;
                Gore gore2 = Main.gore[num3];
                gore2.velocity.Y = gore2.velocity.Y + 1f;
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

            int length = Math.Min(17, 2 + (int)projectile.oldVelocity.Length());

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

        public override void AI()
        {
            if (projectile.timeLeft < 297)
            {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height
                    , 174, 0f, 0f, 90, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
    }

}




