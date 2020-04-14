using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;

namespace Entrogic.Items.VoluGels
{
    /// <summary>
    /// 瓦卢提奥宝藏袋 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/11 17:55:47
    /// </summary>
    public class 瓦卢提奥宝藏袋 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("宝藏袋");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
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
        public override int BossBagNPC => mod.NPCType("嘉沃顿");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0) player.QuickSpawnItem(mod.ItemType("瓦卢提奥头套"));
            player.QuickSpawnItem(mod.ItemType("GelOfLife"), Main.rand.Next(12, 16 + 1));//12-16个
            player.QuickSpawnItem(mod.ItemType("凝胶安卡"));
        }
    }
}
