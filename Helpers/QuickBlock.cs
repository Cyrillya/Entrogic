using Terraria.ObjectData;

namespace Entrogic.Helpers
{
    internal static class QuickBlock
    {
        public static void QuickSet(this ModTile tile, int minPick, int dustType, SoundStyle soundType, Color mapColor, int drop, bool dirtMerge = false, bool stone = false, string mapName = "")
        {
            tile.MinPick = minPick;
            tile.DustType = dustType;
            tile.HitSound = soundType;
            tile.ItemDrop = drop;
            Main.tileMergeDirt[tile.Type] = dirtMerge;
            Main.tileStone[tile.Type] = stone;

            Main.tileSolid[tile.Type] = true;
            Main.tileLighted[tile.Type] = true;
            Main.tileBlockLight[tile.Type] = true;

            ModTranslation name = tile.CreateMapEntryName();
            name.SetDefault(mapName);
            tile.AddMapEntry(mapColor, name);
        }

        public static void QuickSetWall(this ModWall wall, int dustType, SoundStyle soundType, int drop, bool safe, Color mapColor)
        {
            wall.DustType = dustType;
            wall.HitSound = soundType;
            wall.ItemDrop = drop;
            Main.wallHouse[wall.Type] = safe;
            wall.AddMapEntry(mapColor);
        }

        public static void QuickSetFurniture(this ModTile tile, int width, int height, int yOffset, int dustType, SoundStyle soundType, bool tallBottom, Color mapColor, bool solidTop = false, bool solid = false, AnchorData bottomAnchor = default, AnchorData topAnchor = default, int[] anchorTiles = null)
        {
            Main.tileLavaDeath[tile.Type] = false;
            Main.tileFrameImportant[tile.Type] = true;
            Main.tileSolidTop[tile.Type] = solidTop;
            Main.tileSolid[tile.Type] = solid;

            TileObjectData.newTile.Width = width;
            TileObjectData.newTile.Height = height;
            TileObjectData.newTile.CoordinateHeights = new int[height];

            for (int k = 0; k < height; k++)
            {
                TileObjectData.newTile.CoordinateHeights[k] = 16;
            }

            if (tallBottom) TileObjectData.newTile.CoordinateHeights[height - 1] = 18;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(width - 1, height - 1);
            TileObjectData.newTile.DrawYOffset = yOffset;

            if (bottomAnchor != default)
                TileObjectData.newTile.AnchorBottom = bottomAnchor;

            if (topAnchor != default)
                TileObjectData.newTile.AnchorTop = topAnchor;

            if (anchorTiles != null)
                TileObjectData.newTile.AnchorAlternateTiles = anchorTiles;


            TileObjectData.addTile(tile.Type);

            tile.AddMapEntry(mapColor);
            tile.DustType = dustType;
            tile.HitSound = soundType;
        }
    }
}
