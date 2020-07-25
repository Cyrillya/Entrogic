using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Entrogic.Projectiles.Magic.Books
{
    public class 污染水滴 : ModProjectile
    {
        public override string Texture => "Entrogic/Projectiles/Magic/Books/污染水流";
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.magic = true;
            projectile.damage = 20;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.scale = 3f;
        }
        public override void AI()
        {
            // 发出蓝光
            Lighting.AddLight(projectile.position, 49 / 255f, 94 / 255f, 227 / 255f);

            // 线性粒子效果
            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 217, 0f, 0f, 100);
                d.position = projectile.Center - projectile.velocity * i / 3f;
                d.velocity *= 1f;
                d.noGravity = true;
                d.scale = (float)Main.rand.Next(90, 110) * 0.014f;
            }

            // 获取目标NPC
            NPC target = Main.npc[(int)projectile.ai[0]];
            // 如果敌对npc是活着的
            if (target.active)
            {
                // 计算朝向目标的向量
                Vector2 targetVec = target.Center - projectile.Center;
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为20的向量
                targetVec *= 6f;
                // 朝向npc的单位向量*20 + 3.33%偏移量
                projectile.velocity = (projectile.velocity * 30f + targetVec) / 31f;
            }
        }
    }
}
