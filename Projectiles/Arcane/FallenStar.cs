using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Arcane
{
    /// <summary>
    /// 陨星 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/2/1 20:57:03
    /// </summary>
    public class FallenStar : ArcaneProjectile
    {
        public override void ArcaneDefaults()
        {
            projectile.Size = new Vector2(8, 8);
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.alpha = 50;
            projectile.timeLeft = 300;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 159, 0f, 0f, 100, Color.White, 1f);
            d.noGravity = true;
            Lighting.AddLight(projectile.Center, (float)Color.Gold.R * 0.005f, (float)Color.Gold.G * 0.005f, (float)Color.Gold.B * 0.005f);
            if (projectile.ai[1] == 1)
            {
                float num470 = projectile.Center.X;
                float num471 = projectile.Center.Y;
                float num472 = 600f;
                bool flag17 = false;
                for (int num473 = 0; num473 < 200; num473++)
                {
                    if (Main.npc[num473].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[num473].Center, 1, 1))
                    {
                        float num474 = Main.npc[num473].position.X + (float)(Main.npc[num473].width / 2);
                        float num475 = Main.npc[num473].position.Y + (float)(Main.npc[num473].height / 2);
                        float num476 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num474) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num475);
                        if (num476 < num472)
                        {
                            num472 = num476;
                            num470 = num474;
                            num471 = num475;
                            flag17 = true;
                        }
                    }
                }
                if (flag17)
                {
                    float num477 = 18f;
                    Vector2 vector35 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num478 = num470 - vector35.X;
                    float num479 = num471 - vector35.Y;
                    float num480 = (float)Math.Sqrt((double)(num478 * num478 + num479 * num479));
                    num480 = num477 / num480;
                    num478 *= num480;
                    num479 *= num480;
                    projectile.velocity = (projectile.velocity * 20f + new Vector2(num478, num479)) / 21f;
                }
                return;
            }
            if (projectile.position.Y + projectile.Size.Y >= projectile.ai[0])
            {
                projectile.tileCollide = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, projectile.position);
            for (int index = 0; index < 5; index++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 1f);
            }
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 1f);
            }
            for (int i = 0; i < 4; i++)
            {
                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
            }
            for (int i = 0; i < 3; i++)
            {
                int type = Utils.SelectRandom<int>(Main.rand, new int[]
                {
                    6,
                    259,
                    158
                });
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, type, (float)(-1 * projectile.direction), -2.5f, 0, default(Color), 1f);
                Main.dust[d].alpha = 200;
                Main.dust[d].velocity *= 2.2f;
                Main.dust[d].scale += Utils.NextFloat(Main.rand);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = (Texture2D)Terraria.GameContent.TextureAssets.Projectile[projectile.type];
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
    }
}
