using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic
{
    public class ArcaneDamageClass : DamageClass
	{
		public override void SetupContent()
		{
			// Make weapons with this damage type have a tooltip of 'X example damage'.
			ClassName.SetDefault(Language.GetTextValue("Mods.Entrogic.Common.ArcaneDamage"));
		}
	}
}
