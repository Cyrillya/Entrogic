using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Arcane
{
    public class ArcaneMissle : ArcaneProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("奥术飞弹");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void ArcaneDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.alpha = 10;
            projectile.light = 3f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = -1;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10);
            for (int i = 0; i < 22; i++)
            {
                int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 272, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
                }
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

            int length = Math.Min(15, 2 + (int)projectile.oldVelocity.Length());

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
            Player player = Main.player[projectile.owner];
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 21, 0f, 0f, 100, default(Color), 2f);
            dust.noGravity = true;
            NPC target = null;
            NPC bossTarget = null;
            float distanceMax = 800f;
            float distanceMaxBoss = 1000f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != NPCID.TargetDummy)
                {
                    // 计算与玩家的距离
                    float currentDistance = Vector2.Distance(npc.Center, Main.MouseWorld);
                    if (!npc.boss)
                    {
                        if (currentDistance < distanceMax)
                        {
                            distanceMax = currentDistance;
                            target = npc;
                        }
                    }
                    else
                    {
                        if (currentDistance < distanceMaxBoss)
                        {
                            distanceMaxBoss = currentDistance;
                            bossTarget = npc;
                        }
                    }
                }
            }
            if (bossTarget != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = bossTarget.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为20的向量
                targetVec *= 20f;
                // 朝向npc的单位向量*20 + 一些(?)偏移量
                projectile.velocity = (projectile.velocity * 20f + targetVec) / 21f;
            }
            else if (target != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = target.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为20的向量
                targetVec *= 20f;
                // 朝向npc的单位向量*20 + 一些(?)偏移量
                projectile.velocity = (projectile.velocity * 20f + targetVec) / 21f;
            }
        }
    }
}