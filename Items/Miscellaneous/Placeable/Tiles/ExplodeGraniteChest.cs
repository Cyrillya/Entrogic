using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Placeable.Tiles
{
    public class ExplodeGraniteChest : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 500;
            item.createTile = Entrogic.Instance.TileType("ExplodeGraniteChest");
        }
    }
}
