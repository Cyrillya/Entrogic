using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;

namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染头饰 的摘要说明
    /// 创建人：Cyril
    /// 创建时间：2019/1/22 12:48:56
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class HeadgearofContamination : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 40;
            player.manaRegenBonus++;
            player.GetCrit(DamageClass.Magic) += 10;
            player.manaCost -= 0.2f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
        }
    }
}
