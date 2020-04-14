using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染护胫 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/22 18:27:33
    /// </summary>
    [AutoloadEquip(EquipType.Legs)]
    public class 污染护胫 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            int critpoint = 6;
            player.magicCrit += critpoint;
            player.meleeCrit += critpoint;
            player.rangedCrit += critpoint;
            player.thrownCrit += critpoint;
            player.moveSpeed += 0.1f;
        }
        public override bool DrawLegs()
        {
            return false;
        }
    }
}
