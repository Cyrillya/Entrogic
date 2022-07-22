using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Content.Items.Symbiosis
{
    public class VolutioTreasureBag : BossBag
    {
        public override bool PreHardmode => false;

        public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VolutioMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GelOfLife>(), minimumDropped: 12, maximumDropped: 16));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GelAnkh>()));
        }
    }
}
