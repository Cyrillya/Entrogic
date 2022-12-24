using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items.Accessories.Underworld
{
    public class BrokenTail : ItemBase
	{
		public override void SetDefaults() {
			Item.width = 62;
			Item.height = 58;
			Item.value = 10000;
			Item.rare = RarityLevelID.MiddleHM;
			Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual) {
			DemonAccPlayer.Get(player).BrokenTail = true;
        }
    }
}
