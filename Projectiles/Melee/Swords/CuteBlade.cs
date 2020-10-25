using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Microsoft.Xna.Framework;

namespace Entrogic.Projectiles.Melee.Swords
{
    /// <summary>
    /// 可爱剑光 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/8 22:59:28
    /// </summary>
    public class CuteBlade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 23;
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Arkhalis);
            projectile.damage = 51;
            projectile.aiStyle = -1;
            projectile.knockBack = 6f;
            projectile.DamageType = DamageClass.Melee;
            projectile.alpha = 70;
            projectile.Size = new Vector2(90f, 84f);
        }
        public override void AI()
        {
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 12;
            }
            projectile.timeLeft = 2;
            if (!Main.player[projectile.owner].channel)
                projectile.Kill();
            projectile.ai[0]++;
            if (projectile.ai[0] >= 1)
            {
                projectile.frame++;
                projectile.ai[0] = 0;
            }
            if (projectile.frame >= 23)
                projectile.frame = 0;
            Main.player[projectile.owner].heldProj = projectile.whoAmI;
            projectile.Center = Main.player[projectile.owner].Center;
            Main.player[projectile.owner].itemTime = 6;
            Main.player[projectile.owner].itemAnimation = 6;
            if (Main.MouseWorld.X > projectile.Center.X)
                Main.player[projectile.owner].direction = 1;
            else
                Main.player[projectile.owner].direction = -1;
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(projectile.position + new Vector2(Main.rand.Next(18, 31) * Main.player[projectile.owner].direction, 10f), projectile.width, projectile.height - 10, MyDustId.LightCyanParticle1, 8.6f * Main.player[projectile.owner].direction, 0, 160, default(Color), 1.2f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
