
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Arcane
{
    public class UndeadGrudge : ArcaneProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void ArcaneDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 380;
            projectile.alpha = 10;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            aiType = -1;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Lighting.AddLight(projectile.position, 0.0f, 0.0f, 0.7f);
            if (projectile.timeLeft == 380 && Main.player[projectile.owner].active)
            {
                projectile.position = Main.player[projectile.owner].position;
                projectile.alpha = 10;
            }
            else if (projectile.timeLeft < 380)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.GreyPebble, 0, 0, 100, Color.LightBlue, 1.6f);
                dust.noGravity = true;
            }
            else
            {
                projectile.alpha = 255;
                return;
            }
            NPC target = null;
            NPC bossTarget = null;
            float distanceMax = 1200f;
            float distanceMaxBoss = 1220f;
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
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = 1;
            if (bossTarget != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = bossTarget.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为10的向量
                targetVec *= 10f;
                // 朝向npc的单位向量*10 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
            }
            else if (target != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = target.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为10的向量
                targetVec *= 10f;
                // 朝向npc的单位向量*10 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
            }

            projectile.frameCounter++;
            if (projectile.frame > 3)
                projectile.frameCounter = 0;
            projectile.frame = projectile.frameCounter / 6 + 1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type == NPCID.TargetDummy)
                return;
            int healAmount = damage / 4;
            Projectile.NewProjectile(projectile.Center, Vector2.One, ProjectileType<UndeadHeal>(), 0, 0f, projectile.owner, healAmount);
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item);
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.GreyPebble, 0, 0, 100, Color.LightBlue, 1.8f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.0f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LightBlue;
        }
    }
    public class UndeadHeal : ArcaneProjectile
    {
        public override string Texture => "Entrogic/Images/Block";
        public override void ArcaneDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 300;
            projectile.alpha = 10;
            projectile.tileCollide = false;
            aiType = -1;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Lighting.AddLight(projectile.position, 0.0f, 0.5f, 0.0f);
            if (projectile.timeLeft < 380)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.GreyPebble, 0f, 0f, 100, Color.MediumPurple, 1.1f);
                dust.noGravity = true;
            }
            if (player != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = player.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为20的向量
                targetVec *= 20f;
                // 朝向Plr单位向量*20 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
                if (player.getRect().Intersects(projectile.getRect()))
                {
                    projectile.Kill();
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.HealEffect((int)projectile.ai[0]);
            player.statLife += (int)projectile.ai[0];
        }
    }
}
