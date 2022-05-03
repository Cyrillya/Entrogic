namespace Entrogic.Content.DamageClasses
{
    public class ArcaneDamageClass : DamageClass
    {
        public static string ClassOnlyNameKey;
        public static string DisplayClassName => Language.GetTextValue(ClassOnlyNameKey);

        public override void SetStaticDefaults() {
            ClassName.SetDefault("arcane damage");
            ClassName.AddTranslation((int)GameCulture.CultureName.Chinese, "奥术伤害");
            ClassOnlyNameKey = Translation.RegisterTranslation("ClassOnlyName", GameCulture.CultureName.Chinese, "奥术", "arcane");
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
            // Make this damage class not benefit from any otherclass stat bonuses by default, but still benefit from universal/all-class bonuses.
            if (damageClass == Generic)
                return StatInheritanceData.Full;

            // 攻击、暴击和击退受30%的魔法加成
            if (damageClass == Magic)
                return new StatInheritanceData(
                    damageInheritance: .3f,
                    critChanceInheritance: .3f,
                    knockbackInheritance: .3f
                );

            return StatInheritanceData.None;
        }
    }
}
