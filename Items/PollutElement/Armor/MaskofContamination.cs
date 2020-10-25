using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染面具 的摘要说明
    /// 创建人：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/22 12:51:05
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class MaskofContamination : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 22;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.08f;
            player.GetCrit(DamageClass.Melee) += 12;
            player.GetDamage(DamageClass.Melee) += 0.1f;
        }
        public override bool DrawHead()
        {
            return false;
        }
    }
}
