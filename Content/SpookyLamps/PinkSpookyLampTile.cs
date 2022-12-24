using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.SpookyLamps
{
    internal class PinkSpookyLampTile : SpookyLamp
    {
        public override int? MultiTileDropItem => ModContent.ItemType<PinkSpookyLamp>();

        public override Color LightColor => new(0.25f, 0.32f, 0.5f);
    }
}
