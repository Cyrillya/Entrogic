namespace Entrogic.Content.DamageClasses
{
    public class ArcaneDamageClass : DamageClass
	{
		public override void SetStaticDefaults() {
			// Make weapons with this damage type have a tooltip of 'X arcane damage'.
			ClassName.SetDefault("arcane damage");
			ClassName.AddTranslation((int)GameCulture.CultureName.Chinese, "奥术伤害");
		}

		protected override float GetBenefitFrom(DamageClass damageClass) {
			// Make this damage class not benefit from any otherclass stat bonuses by default, but still benefit from universal/all-class bonuses.
			if (damageClass == Generic)
				return 1f;
			// 受30%的魔法加成
			if (damageClass == Magic)
				return .3f;

			return 0;
		}

		public override bool CountsAs(DamageClass damageClass) {
			// Make this damage class not benefit from any otherclass effects (e.g. Spectre bolts, Magma Stone) by default.
			// Note that unlike GetBenefitFrom, you do not need to account for universal bonuses in this method.
			return false;
		}
	}
}
