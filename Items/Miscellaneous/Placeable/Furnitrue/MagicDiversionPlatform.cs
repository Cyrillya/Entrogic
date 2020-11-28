using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Tiles;

namespace Entrogic.Items.Miscellaneous.Placeable.Furnitrue
{
    /// <summary>
    /// 魔力导流台 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/9 21:32:40
    /// </summary>
    public class MagicDiversionPlatform : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
<<<<<<< HEAD:Items/Miscellaneous/Placeable/Furnitrue/MagicDiversionPlatform.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Miscellaneous/Placeable/Furnitrue/魔力导流台.cs
            item.consumable = true;
            item.rare = RareID.LV2;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.createTile = TileType<MagicDiversionPlatformTile>();
            item.placeStyle = 0;
        }
    }
}
