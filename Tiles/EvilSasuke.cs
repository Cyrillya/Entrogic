using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Entrogic.Items.Miscellaneous.Placeable.Furnitrue;

namespace Entrogic.Tiles
{
    public class EvilSasuke : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("邪神佐助");
            dustType = 7;
            AddMapEntry(new Color(200, 200, 200), name);
            disableSmartCursor = true;
        }
        
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<EvilSasukeItem>());
        }
    }
}