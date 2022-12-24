using Entrogic.Content.RockGolem.Weapons;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;
using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Content.RockGolem.Items
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