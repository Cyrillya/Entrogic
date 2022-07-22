using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Items.ContyElemental.Armors;
using Entrogic.Content.Items.ContyElemental.Weapons;
using Entrogic.Content.NPCs.Enemies.ContyElemental;
using Terraria.GameContent.ItemDropRules;
using Terraria.Utilities;

namespace Entrogic.Content.Items.ContyElemental
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