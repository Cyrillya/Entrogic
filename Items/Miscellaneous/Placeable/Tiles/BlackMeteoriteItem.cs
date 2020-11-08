using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Tiles;

namespace Entrogic.Items.Miscellaneous.Placeable.Tiles
{
    public class BlackMeteoriteItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Obsidian);
            item.createTile = TileType<BlackMeteorite>();
        }
    }
}
