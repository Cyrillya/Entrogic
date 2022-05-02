using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.AntaGolem.Projectiles
{
    public class 魔像飞弹 : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Projectiles/Enemies/Gravel"; } }

        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.Size = new Vector2(8, 8);
            projectile.scale = 1.5f;
            projectile.penetrate = 6;
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 300;
        }
        public int maxTimeLeft = 0;
        public float Timer = 0f;
        public override void AI()
        {
            if (maxTimeLeft == 0)
            {
                maxTimeLeft = projectile.timeLeft;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, projectile.position);
            }
            projectile.ai[1]++;
            if (projectile.ai[1] < (projectile.scale > 2f ? 6f : 4f))
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Stone, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), projectile.scale + 0.2f);
                Main.dust[num].noGravity = true;
                Dust dust = Main.dust[num];
                dust.velocity *= 0.4f;
            }
            else
                projectile.ai[1] = 0f;
        }
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (!target.stoned && Main.rand.Next(1, 6) <= 2)// 2/5
                target.AddBuff(BuffID.Stoned, Main.rand.Next(30, 90));
        }
    }
}
