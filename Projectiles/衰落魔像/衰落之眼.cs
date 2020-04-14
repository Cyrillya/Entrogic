using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.衰落魔像
{
    public class 衰落之眼 : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.MedusaHead; } }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.MedusaHeadRay);
            aiType = ProjectileID.MedusaHeadRay;
        }
        public override void AI()
        {
            Dust d = Main.dust[Dust.NewDust(projectile.position, projectile.width / 2, projectile.height / 2, DustID.Stone, 0, 0, 60, default(Color), 2f)];
            d.noGravity = true;
        }
    }
}
