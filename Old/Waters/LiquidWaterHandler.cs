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

namespace Entrogic.Waters
{
	public class LiquidWaterHandler : ModSystem
	{
		bool DrawingLiquid;
		public override void Load() {
			base.Load();
            //On.Terraria.Main.DrawWater += Main_DrawWater;
			On.Terraria.Graphics.TileBatch.InternalDraw += TileBatch_InternalDraw;
			On.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += LiquidRenderer_InternalDraw;
			On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid += TileDrawing_DrawPartialLiquid;
		}

        private void TileDrawing_DrawPartialLiquid(On.Terraria.GameContent.Drawing.TileDrawing.orig_DrawPartialLiquid orig, Terraria.GameContent.Drawing.TileDrawing self, Tile tileCache, Vector2 position, Rectangle liquidSize, int liquidType, Color aColor) {
			if (AEntrogicConfigClient.Instance.OldWaterEffect) {
				orig(self, tileCache, position, liquidSize, liquidType, aColor);
				return;
			}
			//Asset<Texture2D> BaseTexture = TextureAssets.Liquid[liquidType];
			//if (EntrogicWorld.LifeLiquidCheck(position.X + Main.screenPosition.X, position.Y + Main.screenPosition.Y)) {
            //    TextureAssets.Liquid[liquidType] = Entrogic.Instance.GetTexture("Waters/LifeWaterStyle_Block");
            //    aColor *= 1.2f;
			//}

            orig(self, tileCache, position, liquidSize, liquidType, aColor);
			// 直接替换原版的字段未免有点不安全，最终决定牺牲些许性能来Draw两次（感觉影响不大）
			if (EntrogicWorld.LifeLiquidCheck(position.X + Main.screenPosition.X, position.Y + Main.screenPosition.Y)) {
				aColor *= 1.2f;
				Asset<Texture2D> Liquid = Entrogic.Instance.GetTexture("Waters/LifeWaterStyle_Block");
				Asset<Texture2D> LiquidSlope = Entrogic.Instance.GetTexture("Waters/LifeWaterSlope");
				int num = tileCache.slope();
				if (!TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type] || num == 0) {
					Main.spriteBatch.Draw(Liquid.Value, position, liquidSize, aColor, 0f, default, 1f, SpriteEffects.None, 0f);
					return;
				}

				liquidSize.X += 18 * (num - 1);
				if (tileCache.slope() == 1)
					Main.spriteBatch.Draw(LiquidSlope.Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				else if (tileCache.slope() == 2)
					Main.spriteBatch.Draw(LiquidSlope.Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				else if (tileCache.slope() == 3)
					Main.spriteBatch.Draw(LiquidSlope.Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				else if (tileCache.slope() == 4)
					Main.spriteBatch.Draw(LiquidSlope.Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

            //TextureAssets.Liquid[liquidType] = BaseTexture;
        }

		/// <summary>
		/// 在Draw时不断从Mod获取贴图，很占用资源（Draw几次就获取几次）
		/// 因此在全局定义一个，Draw之前获取好，到时候直接调用即可
		/// </summary>
		Texture2D WaterTexture;
		private void LiquidRenderer_InternalDraw(On.Terraria.GameContent.Liquid.LiquidRenderer.orig_InternalDraw orig, LiquidRenderer self, SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw) {
			DrawingLiquid = true;
			WaterTexture = Entrogic.Instance.GetTexture("Waters/LifeWaterStyle").Value;
			orig(self, spriteBatch, drawOffset, waterStyle, globalAlpha, isBackgroundDraw);
			DrawingLiquid = false;
		}

		private void TileBatch_InternalDraw(On.Terraria.Graphics.TileBatch.orig_InternalDraw orig, TileBatch self, Texture2D texture, ref Vector4 destination, bool scaleDestination, ref Rectangle? sourceRectangle, ref VertexColors colors, ref Vector2 origin, SpriteEffects effects, float rotation) {
			if (AEntrogicConfigClient.Instance.OldWaterEffect) {
				orig(self, texture, ref destination, scaleDestination, ref sourceRectangle, ref colors, ref origin, effects, rotation);
				return;
			}
            if (EntrogicWorld.LifeLiquidCheck(destination.X + Main.screenPosition.X - 16 * 15, destination.Y + Main.screenPosition.Y - 16 * 15) && DrawingLiquid) {
				//texture = Entrogic.Instance.GetTexture("Waters/LifeWaterStyle").Value;
				texture = WaterTexture;
                float num = 1.2f;
                colors.BottomLeftColor *= num;
                colors.BottomRightColor *= num;
                colors.TopLeftColor *= num;
                colors.TopRightColor *= num;
            }
            orig(self, texture, ref destination, scaleDestination, ref sourceRectangle, ref colors, ref origin, effects, rotation);
		}
	}
}
