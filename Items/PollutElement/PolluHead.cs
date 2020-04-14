using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Terraria.DataStructures;

namespace Entrogic.Items.PollutElement
{
    /// <summary>
    /// 污染之灵头套01 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/18 12:00:55
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class PolluHead :ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = RareID.LV8;
            item.vanity = true;
            item.value = Item.sellPrice(0, 0, 20, 0);
        }
    }
    public class PolluHead1 : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }
    public class PolluHead2 : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }
    public class PolluHead3 : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }
    public class PolluHead4 : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }
}
