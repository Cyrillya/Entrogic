using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.VoluGels
{
    /// <summary>
    /// 凝胶安卡 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/10 0:55:35
    /// </summary>
    public class 凝胶安卡 : ModItem
    {
        public override void SetDefaults()
        {
            item.Size = new Vector2(48);
            item.rare = RareID.LV2;
            item.expert = true;
            item.accessory = true;
            item.value = Item.sellPrice(0, 3, 20, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EntrogicPlayer>().CanReborn = true;
        }
    }
}
