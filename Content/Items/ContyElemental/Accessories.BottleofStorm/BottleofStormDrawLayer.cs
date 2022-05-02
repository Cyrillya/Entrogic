namespace Entrogic.Content.Items.ContyElemental
{

    public class BottleofStormDrawLayer : PlayerDrawLayer
	{
		internal class DrawDataInfo
		{
			public Vector2 Position;
			public Rectangle? Frame;
			public float Rotation;
			public Texture2D Texture;
			public Vector2 Origin;
		}

		private static Asset<Texture2D> texture;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
            return drawInfo.drawPlayer.GetModPlayer<BottleofStormPlayer>().HasWings == true;

            // If you'd like to reference another PlayerDrawLayer's visibility,
            // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisibility on it
        }

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HairBack);

		internal static DrawDataInfo GetHeadDrawData(PlayerDrawSet drawInfo, Texture2D texture) {
			Player drawPlayer = drawInfo.drawPlayer;
			Vector2 pos = drawPlayer.headPosition + drawInfo.headVect + new Vector2(
				(int)(drawInfo.Position.X + drawPlayer.width / 2f - drawPlayer.bodyFrame.Width / 2f - Main.screenPosition.X),
				(int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y)
			);
			// 特定帧下，position要向上2个像素格（常规的直接在帧图上表现了，像这样单图就要特殊处理）
			if ((drawPlayer.bodyFrame.Y >= 7 * 56 && drawPlayer.bodyFrame.Y <= 9 * 56) || drawPlayer.bodyFrame.Y >= 14 * 56 && drawPlayer.bodyFrame.Y <= 16 * 56) {
				pos.Y -= 2;
            }

			return new DrawDataInfo {
				Position = pos,
				Frame = drawPlayer.bodyFrame,
				Origin = drawInfo.headVect,
				Rotation = drawPlayer.headRotation,
				Texture = texture
			};
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			if (Main.gameMenu || drawInfo.drawPlayer.grapCount > 0) {
				return;
            }

            if (texture == null) {
                texture = ModContent.Request<Texture2D>("Entrogic/Content/Items/ContyElemental/BottleofStorm_Wings_Effect");
            }

            var drawPlayer = drawInfo.drawPlayer;
			var stormPlayer = drawPlayer.GetModPlayer<BottleofStormPlayer>();
			// 选自Jim的翅膀的绘制代码
			if (!Main.gamePaused) {
				FindFrame(ref drawInfo);
            }
			if (drawInfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated()) {
				Vector2 drawPosition = drawInfo.Center - Main.screenPosition - Vector2.UnitX * drawInfo.drawPlayer.direction * 4f + Vector2.UnitY * 8;
				Rectangle frame = texture.Frame(1, 6, 0, stormPlayer.wingFrame);
				Color color = Lighting.GetColor(((drawPosition + Main.screenPosition) / 16f).ToPoint()) * (1 - drawInfo.shadow);
				frame.Width -= 2;
				frame.Height -= 2;
				DrawData data = new DrawData(texture.Value, drawPosition.Floor(), frame, color, drawInfo.drawPlayer.bodyRotation, frame.Size() / 2f, 1f, drawInfo.playerEffect, 0);
				data.shader = drawInfo.cWings;
				drawInfo.DrawDataCache.Add(data);
			}
		}
		private void FindFrame(ref PlayerDrawSet drawInfo) {
			var drawPlayer = drawInfo.drawPlayer;
			var stormPlayer = drawPlayer.GetModPlayer<BottleofStormPlayer>();
			if (drawPlayer.jump > 0) {
				stormPlayer.wingFrameCounter++;
				int num13 = 4;
				if (stormPlayer.wingFrameCounter >= num13 * 6)
					stormPlayer.wingFrameCounter = 0;

				stormPlayer.wingFrame = stormPlayer.wingFrameCounter / num13;
			}
			else if (drawPlayer.velocity.Y != 0f) {
				if (drawPlayer.controlJump) {
					stormPlayer.wingFrameCounter++;
					int num14 = 7;
					if (stormPlayer.wingFrameCounter >= num14 * 6)
						stormPlayer.wingFrameCounter = 0;

					stormPlayer.wingFrame = stormPlayer.wingFrameCounter / num14;
				}
				else {
					stormPlayer.wingFrameCounter++;
					int num15 = 5;
					if (stormPlayer.wingFrameCounter >= num15 * 6)
						stormPlayer.wingFrameCounter = 0;

					stormPlayer.wingFrame = stormPlayer.wingFrameCounter / num15;
				}
			}
			else {
				stormPlayer.wingFrameCounter++;
				int num16 = 4;
				if (stormPlayer.wingFrameCounter >= num16 * 6)
					stormPlayer.wingFrameCounter = 0;

				stormPlayer.wingFrame = stormPlayer.wingFrameCounter / num16;
			}
		}
    }
}
