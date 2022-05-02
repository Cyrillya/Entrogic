using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;


namespace Entrogic.NPCs.Boss.凝胶Java盾.Projectiles
{
    /// <summary>
    /// 凝胶镰刀 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/6 16:58:40
    /// </summary>
    public class GelSickle : ModProjectile
    {
        public Color gelcolor = new Color(255, 98, 71, 137);
        int cloneProj = 44;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = Main.projFrames[cloneProj];
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(cloneProj);
            projectile.aiStyle = -1;
            projectile.alpha = 30;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, projectile.position);
            }
            projectile.rotation += (float)projectile.direction * 0.8f;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 30f)
            {
                if (projectile.ai[0] < 100f)
                {
                    projectile.velocity *= 1.06f;
                }
                else
                {
                    projectile.ai[0] = 200f;
                }
            }
            int num3;
            for (int num258 = 0; num258 < 2; num258 = num3 + 1)
            {
                int num259 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.GreyPebble, 0, 0, 30, gelcolor, 1f);
                Main.dust[num259].noGravity = true;
                num3 = num258;
            }
            return;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), Main.rand.Next(240, 361));
        }
    }
}
