using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Tiles;

namespace Entrogic.Items.Miscellaneous.Placeable.Trophy
{
    /// <summary>
    /// VTrophy 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/12 21:22:39
    /// </summary>
    public class VTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{$Mods.Entrogic.NPCName.Volutio} " + "{$MapObject.Trophy}");
            DisplayName.AddTranslation(GameCulture.Chinese, "凝胶共生体纪念章");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.EyeofCthulhuTrophy);
            item.createTile = TileType<BossTrophy>();
            item.placeStyle = 2;
        }
    }
}
