using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Entrogic.Items.Weapons.Card.Elements;

namespace Entrogic
{
    public class EntrogicProjectile : GlobalProjectile
    {
        public override void Kill(Projectile projectile, int timeLeft)
        {
            //if (ModLoader.GetMod("FallenStar49") != null)
            //{
            //    if (projectile.type == ModHelper.ProjectileType("ShootingStar") && Main.rand.NextFloat() < .003)
            //        Item.NewItem(projectile.Center, ItemType<AstralImpact>());
            //}
        }
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
    }
}
