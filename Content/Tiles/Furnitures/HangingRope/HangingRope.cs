using Entrogic.Common.Globals.Players;
using Entrogic.Content.Tiles.BaseTypes;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace Entrogic.Content.Tiles.Furnitures.HangingRope
{
    public class HangingRope : QuickTileItem
    {
        public HangingRope() : base("Hanging Rope", "A good rope to hang yourself", (int)ItemRarityColor.Green2, 1, "Entrogic/Content/Items/Misc/") { }

        public override int TileType() => ModContent.TileType<HangingRope_Tile>();

        public override void SafeSetDefaults() {
            Item.maxStack = 99;
            Item.value = Item.buyPrice(0, 20, 0, 0);

            base.SafeSetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Rope, 12)
                .Register();
        }
    }
    internal class HangingRope_Tile : TileBase
    {
        public override string HighlightTexture => $"{Texture}_Highlight";

        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(13, 88, 130), Language.GetText("MapObject.Rope"));
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<HangingRope>());

        public override bool RightClick(int i, int j) {
            Player player = Main.LocalPlayer;

            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX / 18 % 2;
            int topY = j - tile.TileFrameY / 18 % 4;
            player.GetModPlayer<HangingPlayer>().StartHanging(topX, topY);

            return true;
        }

        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemID.Rope;
        }
    }
}
