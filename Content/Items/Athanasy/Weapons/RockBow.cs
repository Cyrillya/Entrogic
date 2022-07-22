using Entrogic.Content.Items.Ammos;
using Terraria.ID;

namespace Entrogic.Content.Items.Athanasy.Weapons
{
    public class RockBow : ItemBase
    {
		internal static int ProjectileType;

		public override void SetDefaults() {
			Item.DefaultToBow(20, 5f, true);
			Item.damage = 64;
			Item.width = 48;
			Item.height = 132;
			Item.noMelee = true;
			Item.channel = true;
			Item.knockBack = 4.25f;
			Item.rare = RarityLevelID.EarlyHM;
			Item.noUseGraphic = true;
			Item.useAmmo = ModContent.ItemType<RockArrowSpear>();
			Item.UseSound = SoundAssets.BowPulling;
			Item.reuseDelay = 20;
		}

		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ProjectileType] <= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 shootDirection = velocity.SafeNormalize(Vector2.UnitX * (float)player.direction);
			Projectile.NewProjectile(source, position, shootDirection, ProjectileType, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
