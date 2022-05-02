using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Projectiles.Melee.Stabs
{
    public class ExtraCopperShortsword : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.CopperShortswordStab);
            projectile.aiStyle = -1;
			projectile.hide = false;
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.ai[0] += 1f;
			projectile.Opacity = Utils.GetLerpValue(0f, 7f, projectile.ai[0], true) * Utils.GetLerpValue(32f, 12f, projectile.ai[0], true);
			projectile.Center = player.RotatedRelativePoint(player.MountedCenter, false, false) + projectile.velocity * 0.7f * (projectile.ai[0] - 1f);
			projectile.spriteDirection = ((Vector2.Dot(projectile.velocity, Vector2.UnitX) >= 0f) ? 1 : -1);
			if (projectile.ai[0] >= 32f)
			{
				projectile.Kill();
				return;
			}
            player.heldProj = projectile.whoAmI;
        }
    }
}
