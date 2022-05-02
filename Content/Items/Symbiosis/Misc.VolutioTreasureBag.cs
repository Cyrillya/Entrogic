using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.Symbiosis
{
    public class VolutioTreasureBag : BossBag
    {
        public override int BossBagNPC => NPCID.None; // 占位符

        public override bool PreHardmode => false;

        public override void BossBagLoot(IEntitySource source, Player player) {
            if (Main.rand.Next(7) == 0) player.QuickSpawnItem(source, ModContent.ItemType<VolutioMask>());
            player.QuickSpawnItem(source, ModContent.ItemType<GelOfLife>(), Main.rand.Next(12, 16 + 1));//12-16个
            player.QuickSpawnItem(source, ModContent.ItemType<GelAnkh>());
        }
    }
}
