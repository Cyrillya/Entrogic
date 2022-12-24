using Entrogic.Core.BaseTypes;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Content.Contaminated.Items
{
    public class ContyElementalTrophyTile : TileBase
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
