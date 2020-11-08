using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Entrogic.NPCs.Boss.凝胶Java盾;

namespace Entrogic.Items.VoluGels
{
    /// <summary>
    /// 瓦卢提奥宝藏袋 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/11 17:55:47
    /// </summary>
    public class VolutioTreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.expert = true;
        }
        public override int BossBagNPC => NPCType<Volutio>();

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
