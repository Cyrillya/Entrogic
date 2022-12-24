using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.SpookyLamps
{
    internal class BlueSpookyLampTile : SpookyLamp
    {
        public override int? MultiTileDropItem => ModContent.ItemType<BlueSpookyLamp>();

        public override Color LightColor => new(0.35f, 0.5f, 0.3f);
    }
}
