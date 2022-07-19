using Entrogic.Content.Projectiles.Ammos;

namespace Entrogic.Content.Items.Ammos
{
    public class RockArrowSpear : ItemBase
    {
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.WoodenArrow);
            Item.ammo = Type;
            Item.width = 18;
            Item.height = 68;
            Item.shoot = ModContent.ProjectileType<RockSpearFriendly>();
        }
    }
}
