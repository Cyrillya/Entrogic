using Terraria.ModLoader;

namespace Entrogic.Content.Tiles.BaseTypes
{
    public abstract class TileBase : ModTile
    {
        public virtual int? MultiTileDropItem { get; }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            if (MultiTileDropItem.HasValue) {
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i << 4, j << 4), 16, 48, MultiTileDropItem.Value);
            }
        }
    }
}
