using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染之冠 的摘要说明
    /// 创建人：Cyril
    /// 创建时间：2019/1/22 12:44:58
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class CrownofContamination : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) += 0.16f;
            player.minionKB += 0.07f;
        }
    }
}
