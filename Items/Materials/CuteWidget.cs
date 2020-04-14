using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Materials
{
	public class CuteWidget : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.value = 10000;
			item.rare = 6;
		}
    }
}
