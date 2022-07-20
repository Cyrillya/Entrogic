using Entrogic.Content.Items.Athanasy.Weapons;
using Terraria.Utilities;

namespace Entrogic.Content.Items.Athanasy
{
    public class AthanasyTreasureBag : BossBag
    {
        public override void BossBagLoot(IEntitySource entitySource, Player player) {
            if (Main.rand.NextBool(7)) {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<AthanasyMask>());
            }

            var weightedRandom = new WeightedRandom<int>();
            weightedRandom.Add(ModContent.ItemType<RockFlail>());
            weightedRandom.Add(ModContent.ItemType<RockBow>());
            player.QuickSpawnItem(entitySource, weightedRandom.Get());
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Enemies.Athanasy.Athanasy>();

        public override int Rarity => RarityLevelID.EarlyHM;

        public override bool PreHardmode => base.PreHardmode;
    }
}