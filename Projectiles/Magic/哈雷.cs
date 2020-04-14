using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic
{
    /// <summary>
    /// 哈雷 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/19 16:30:43
    /// </summary>
    public class 哈雷 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BulletHighVelocity);
            projectile.magic = true;
            projectile.ranged = false;
            projectile.penetrate = 5;
            projectile.extraUpdates = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            aiType = ProjectileID.BulletHighVelocity;
        }
    }
}
