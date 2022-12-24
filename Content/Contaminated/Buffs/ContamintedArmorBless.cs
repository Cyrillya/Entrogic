using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Buffs
{
    public class ContamintedArmorBless : BuffBase
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ocean Blessing");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "海洋祝福");
			Description.SetDefault("Grants several major improvements of stats.");
			Description.AddTranslation((int)GameCulture.CultureName.Chinese, "属性大大增强。");
		}

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 16;
			player.moveSpeed += .20f;
			player.endurance += .30f;
			player.lifeRegen += 10;
			player.manaRegen += 10;
			player.GetDamage(DamageClass.Generic) += 2f;
			player.GetCritChance(DamageClass.Generic) += 50;
		}
	}
}
