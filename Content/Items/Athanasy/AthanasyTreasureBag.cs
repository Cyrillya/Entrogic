using Entrogic.Content.Items.Athanasy.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.Utilities;

namespace Entrogic.Content.Items.Athanasy
{
    public class AthanasyTreasureBag : BossBag
    {
        public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AthanasyMask>(), 7));
            itemLoot.Add(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<RockBow>(),
                ModContent.ItemType<RockFlail>()
            ));
        }

        public override int Rarity => RarityLevelID.EarlyHM;

        public override bool PreHardmode => base.PreHardmode;
    }
}