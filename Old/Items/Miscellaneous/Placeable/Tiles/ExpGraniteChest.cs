using Entrogic.Tiles;

using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Placeable.Tiles
{
    public class ExpGraniteChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
<<<<<<< HEAD:Items/Miscellaneous/Placeable/Tiles/ExpGraniteChest.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Miscellaneous/Placeable/Tiles/ExplodeGraniteChest.cs
            item.consumable = true;
            item.value = 500;
            item.createTile = TileType<ExplodeGraniteChest>();
        }
    }
}
