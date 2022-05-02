using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic
{
<<<<<<< HEAD:Projectiles/Magic/Halle.cs
=======
    /// <summary>
    /// 哈雷 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/19 16:30:43
    /// </summary>
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Projectiles/Magic/哈雷.cs
    public class Halle : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BulletHighVelocity);
            projectile.DamageType = DamageClass.Magic;
            projectile.penetrate = 5;
            projectile.extraUpdates = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            aiType = ProjectileID.BulletHighVelocity;
        }
    }
}
