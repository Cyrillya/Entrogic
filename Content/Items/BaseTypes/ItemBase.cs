using Terraria.ModLoader;

namespace Entrogic.Content.Items.BaseTypes
{
	public abstract class ItemBase : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 1;
		}
	}
}
