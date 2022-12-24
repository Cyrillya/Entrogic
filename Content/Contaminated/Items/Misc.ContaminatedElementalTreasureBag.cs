using Entrogic.Content.Contaminated.Armors;
using Entrogic.Content.Contaminated.Weapons;
using Entrogic.Core.BaseTypes;
using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Content.Contaminated.Items
{
    public class ContaminatedElementalTreasureBag : BossBag
    {
        public override bool PreHardmode => false;

        public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulofContamination>(), minimumDropped: 20, maximumDropped: 25));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BottleofStorm>()));
            itemLoot.Add(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<ContyLongbow>(),
                ModContent.ItemType<ContyCurrent>(),
                ModContent.ItemType<SymbioticGelatinStaff>()
            ));
            itemLoot.Add(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<ContaMelee>(),
                ModContent.ItemType<ContaRanged>(),
                ModContent.ItemType<ContaMagic>(),
                ModContent.ItemType<ContaSummoner>(),
                ModContent.ItemType<ContaArcane>()
            ));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ContaBreastplate>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ContaGraves>()));
        }
    }
}