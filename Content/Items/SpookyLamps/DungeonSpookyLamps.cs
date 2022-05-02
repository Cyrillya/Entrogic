using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Tiles.Furnitures.SpookyLamps;

namespace Entrogic.Content.Items.SpookyLamps
{
    public class GreenSpookyLamp : SpookyLampItem
    {
        public override int CreateTile() => ModContent.TileType<GreenSpookyLampTile>();
    }
    public class PinkSpookyLamp : SpookyLampItem
    {
        public override int CreateTile() => ModContent.TileType<PinkSpookyLampTile>();
    }
    public class BlueSpookyLamp : SpookyLampItem
    {
        public override int CreateTile() => ModContent.TileType<BlueSpookyLampTile>();
    }
}
