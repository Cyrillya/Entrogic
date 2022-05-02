using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Entrogic.Content.Items.ContyElemental;
using Entrogic.Content.Tiles.BaseTypes;

namespace Entrogic.Content.Tiles.ContyElemental
{
    public class ContyElementalTrophy_Tile : TileBase
    {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
            DustType = 7;
            TileID.Sets.DisableSmartCursor[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("{$MapObject.Trophy}");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override int? MultiTileDropItem => ItemType<ContyElementalTrophy>();
    }
}
