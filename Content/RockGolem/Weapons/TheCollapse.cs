using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.RockGolem.Weapons
{
    public class TheCollapse : ItemBase
    {
        public override void SetDefaults() {
            Item.DefaultToMagicWeapon(ModContent.ProjectileType<RockGolem.FriendlyProjectiles.TheCollapse>(), 10, 2f, false);
            Item.damage = 64;
            Item.width = 82;
            Item.height = 60;
            Item.channel = true;
            Item.knockBack = 4.25f;
            Item.rare = RarityLevelID.EarlyHM;
            Item.noUseGraphic = true;
            Item.mana = 6;
            Item.UseSound = SoundID.Item72;
        }
    }
}
