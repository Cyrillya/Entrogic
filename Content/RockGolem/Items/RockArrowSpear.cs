using Entrogic.Content.RockGolem.FriendlyProjectiles;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.Items
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
