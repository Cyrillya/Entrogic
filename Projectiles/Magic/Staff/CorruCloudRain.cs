using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic.Staff
{
    public class CorruCloudRain : ModProjectile
    {
        public override void SetDefaults()
        {
                projectile.ignoreWater = true;
                projectile.width = 4;
                projectile.height = 40;
                projectile.aiStyle = 45;
                projectile.friendly = true;
                projectile.penetrate = -1;
                projectile.timeLeft = 600;
                projectile.scale = 1.1f;
                projectile.extraUpdates = 1;
                projectile.damage = 52;
        }
    }
}