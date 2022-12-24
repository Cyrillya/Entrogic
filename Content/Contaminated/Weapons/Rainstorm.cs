using Entrogic.Content.Contaminated.Friendly;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Contaminated.Weapons
{
    public class Rainstorm : ItemBase
    {
        public override void SetDefaults() {
            Item.damage = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 1f;
            Item.knockBack = 6f;
            Item.width = 64;
            Item.height = 64;
            Item.rare = RarityLevelID.MiddleHM;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<RainstormProj>();
            Item.value = Item.sellPrice(0, 2);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTurn = false;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}
