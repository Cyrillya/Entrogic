using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.GameContent.Liquid;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Utilities;
using ReLogic.Content;
using Terraria.ID;
using static Terraria.WaterfallManager;
using Terraria.IO;
using Entrogic.Dusts;
using Entrogic.Assets.Gores;

namespace Entrogic.Waters
{
    public class LiquidDustHandler : ModSystem
    {
		private UnifiedRandom _rand;
		private Gore[] _gore => Main.gore;
		public override void Load() {
            base.Load();
			_rand = new UnifiedRandom();
			On.Terraria.GameContent.Drawing.TileDrawing.EmitLiquidDrops += TileDrawing_EmitLiquidDrops;
        }

        private void TileDrawing_EmitLiquidDrops(On.Terraria.GameContent.Drawing.TileDrawing.orig_EmitLiquidDrops orig, Terraria.GameContent.Drawing.TileDrawing self, int j, int i, Tile tileCache, ushort typeCache) {
			if (!EntrogicWorld.LifeLiquidCheck(i * 16, j * 16)) {
				orig(self, j, i, tileCache, typeCache);
				return;
            }

			if (_rand.NextBool(600) || tileCache.liquid != 0)
				return;
			Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
			rectangle.X -= 34;
			rectangle.Width += 68;
			rectangle.Y -= 100;
			rectangle.Height = 400;
			for (int k = 0; k < _gore.Length; k++) {
				if (_gore[k].active && ((_gore[k].type >= 706 && _gore[k].type <= 717) || _gore[k].type == 943 || _gore[k].type == 1147 || (_gore[k].type >= 1160 && _gore[k].type <= 1162) || _gore[k].type == ModContent.GoreType<LifeDroplet>())) {
					Rectangle value = new Rectangle((int)_gore[k].position.X, (int)_gore[k].position.Y, 16, 16);
					if (rectangle.Intersects(value))
						return;
				}
			}
			int num2 = Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), ModContent.GoreType<LifeDroplet>());
			_gore[num2].velocity *= 0f;
		}

        public override void MidUpdateDustTime() {
            foreach (Dust dust in Main.dust) {
                if (EntrogicWorld.LifeLiquidCheck(dust.position.X, dust.position.Y) && IsVanillaWaterSplash(dust.type)) {
                    //dust.type = ModContent.DustType<LifeWaterSplash>();
                    dust.color.R = 255;
                    dust.color.G = 156;
                    dust.color.B = 0;
                }
            }
        }
        private static bool IsVanillaWaterSplash(int type) {
            int[] waterSplashes = { 33, 98, 99, 100, 101, 102, 103, 104, 105, 123, 288 };
            foreach (int i in waterSplashes) {
                if (type == i) return true;
            }
            return false;
        }
    }
}
