using Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Bullets;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class Phage : ItemBase
    {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;
        }

        public override void SetDefaults() {
            Item.damage = 36;
            Item.crit -= 9;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PhageProj>();
            Item.value = Item.sellPrice(0, 0, 80, 0);
        }
    }
}
