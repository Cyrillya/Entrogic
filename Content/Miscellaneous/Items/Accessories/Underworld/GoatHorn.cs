using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items.Accessories.Underworld
{
    public class GoatHorn : ItemBase
	{
		public override void SetDefaults() {
			Item.width = 66;
			Item.height = 64;
			Item.value = 10000;
			Item.rare = RarityLevelID.MiddleHM;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			DemonAccPlayer.Get(player).GoatHorn = true;
		}
	}
}
