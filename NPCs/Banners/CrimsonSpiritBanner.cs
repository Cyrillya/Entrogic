using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.NPCs.Banners
{
    public class CrimsonSpiritBanner : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.createTile = TileType<MonsterBanner>();
<<<<<<< HEAD
            item.placeStyle = 0;
=======
            item.placeStyle = 1;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
    }
}