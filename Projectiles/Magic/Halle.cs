using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic
{
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
