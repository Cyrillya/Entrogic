using Entrogic.Content.Items.BaseTypes;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Content.Items.Symbiosis
{
    public class VolutioTreasureBag : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = RarityLevelID.Expert;
            Item.expert = true;
        }
        //public override int BossBagNPC => NPCType<Volutio>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0) player.QuickSpawnItem(ItemType<VolutioMask>());
            player.QuickSpawnItem(ItemType<GelOfLife>(), Main.rand.Next(12, 16 + 1));//12-16个
            player.QuickSpawnItem(ItemType<GelAnkh>());
        }
    }
}
