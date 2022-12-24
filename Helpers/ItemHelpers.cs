using Terraria.Enums;

namespace Entrogic.Helpers
{
    public static partial class ModHelper
	{
		public static void DefaultToWhip(this Item item, int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30) {
			item.autoReuse = false;
			item.useStyle = ItemUseStyleID.Swing;
			item.useAnimation = animationTotalTime;
			item.useTime = animationTotalTime;
			item.width = 18;
			item.height = 18;
			item.shoot = projectileId;
			item.UseSound = SoundID.Item152;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.damage = dmg;
			item.knockBack = kb;
			item.shootSpeed = shootspeed;
			item.DamageType = DamageClass.Summon;
		}
		public static void SetShopValues(this Item item, ItemRarityColor rarity, int coinValue) {
			item.rare = (int)rarity;
			item.value = coinValue;
		}
    }
}
