using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Projectiles.Miscellaneous
{
    public class GraniteChestExplode : ModProjectile
    {
        public override string Texture => "Entrogic/Images/Block";
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Granite Chest");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "花岗岩箱");
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Grenade);
            projectile.timeLeft = 1;
        }
        public override void Kill(int timeLeft)
		{
            projectile.ProjectileExplode();
		}
    }
}
