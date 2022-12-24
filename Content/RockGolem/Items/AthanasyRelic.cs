using Entrogic.Content.RockGolem.Tiles;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.Items
{
    public class AthanasyRelic : ItemBase
	{
		public override void SetStaticDefaults() => SacrificeTotal = 1;

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<AthanasyRelicTile>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.buyPrice(0, 5);
		}
	}
}
