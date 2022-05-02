using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Items.ContyElemental.Armors;
using Entrogic.Content.Items.ContyElemental.Weapons;
using Entrogic.Content.NPCs.Enemies.ContyElemental;
using Terraria.Utilities;

namespace Entrogic.Content.Items.ContyElemental
{
    public class ContaminatedElementalTreasureBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<ContaminatedElemental>();

        public override bool PreHardmode => false;

        public override void BossBagLoot(IEntitySource source, Player player) {
            player.QuickSpawnItem(source, ModContent.ItemType<SoulofContamination>(), Main.rand.Next(20, 25)); // 污染之魂
            player.QuickSpawnItem(source, ModContent.ItemType<BottleofStorm>()); // 专家饰品

            var dropChooser = new WeightedRandom<int>();
            dropChooser.Add(ModContent.ItemType<ContyLongbow>());
            dropChooser.Add(ModContent.ItemType<ContyCurrent>());
            dropChooser.Add(ModContent.ItemType<SymbioticGelatinStaff>());
            int choice = dropChooser;
            player.QuickSpawnItem(source, choice);
            dropChooser.Clear();

            var headChooser = new WeightedRandom<int>();
            headChooser.Add(ModContent.ItemType<ContaMelee>());
            headChooser.Add(ModContent.ItemType<ContaRanged>());
            headChooser.Add(ModContent.ItemType<ContaMagic>());
            headChooser.Add(ModContent.ItemType<ContaSummoner>());
            headChooser.Add(ModContent.ItemType<ContaArcane>());

            dropChooser.Add(headChooser);
            dropChooser.Add(ModContent.ItemType<ContaBreastplate>());
            dropChooser.Add(ModContent.ItemType<ContaGraves>());
            choice = dropChooser;
            player.QuickSpawnItem(source, choice);
            dropChooser.Clear();
        }
    }
}