﻿using Entrogic.Content.Items.SpookyLamps;
using Entrogic.Content.Tiles.BaseTypes;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Content.Tiles.Furnitures.SpookyLamps
{
    internal class BlueSpookyLampTile : SpookyLamp
	{
		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<BlueSpookyLamp>());
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			Tile tile = Main.tile[i, j];
			if (tile.frameX == 0) {
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 0.35f;
				g = 0.5f;
				b = 0.3f;
			}
		}
	}
}
