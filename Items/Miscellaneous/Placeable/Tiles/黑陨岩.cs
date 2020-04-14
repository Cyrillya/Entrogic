using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Placeable.Tiles
{
    public class 黑陨岩 :ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Obsidian);
            item.createTile = mod.TileType("黑陨岩");
        }
    }
}
