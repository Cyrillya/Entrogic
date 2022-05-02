﻿using Entrogic.Content.Items.SpookyLamps;
using Entrogic.Content.Tiles.BaseTypes;

namespace Entrogic.Content.Tiles.Furnitures.SpookyLamps
{
    internal class GreenSpookyLampTile : SpookyLamp
    {
        public override int? MultiTileDropItem => ModContent.ItemType<GreenSpookyLamp>();

        public override Color LightColor => new(0.34f, 0.4f, 0.31f);
    }
}
