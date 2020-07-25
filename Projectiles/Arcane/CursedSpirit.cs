using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Projectiles.Arcane

{
    public class CursedSpirit : ArcaneProjectile
    {
        public override void ArcaneDefaults()
        {
            projectile.width = 10;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;

            projectile.timeLeft = 480;
            projectile.alpha = 200;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = -1;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item);
            for (int i = 0; i < 16; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 272, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.75f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockBack, bool crit)
        {
            Player player = Main.player[projectile.owner];
            target.AddBuff(153, 180);
            player.AddBuff(153, 60);
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.timeLeft < 477)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.position, 173, null, 50, default(Color), 2f);
                    dust.noGravity = true;
                }
            }
            NPC target = null;
            NPC bossTarget = null;
            float distanceMax = 1500f;
            float distanceMaxBoss = 1600f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
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
                // 朝向npc的单位向量*20 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
            }
            else if (target != null)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = target.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为20的向量
                targetVec *= 20f;
                // 朝向npc的单位向量*20 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
            }
        }
    }
}