using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.Globals.PlayerLayers
{
    internal class HangingDrawLayer : PlayerDrawLayer
	{
		private readonly Asset<Texture2D> tex = ModContent.Request<Texture2D>(TextureManager.Images + "Hanging");

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
			HangingPlayer hangingPlayer = drawInfo.drawPlayer.GetModPlayer<HangingPlayer>();
			return hangingPlayer.Hanging;

			// If you'd like to reference another PlayerDrawLayer's visibility,
			// you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisibility on it
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

		protected override void Draw(ref PlayerDrawSet drawInfo) {
			var drawPlayer = drawInfo.drawPlayer;
			var hangingPlayer = drawPlayer.GetModPlayer<HangingPlayer>();

			var data = new DrawData(
				tex.Value,
				hangingPlayer.HangingRingPos * 16f - Main.screenPosition,
				null,
				Lighting.GetColor(hangingPlayer.HangingRingPos.ToPoint()),
				0f,
				Vector2.Zero,
				1f,
				SpriteEffects.None,
				0
			);
			drawInfo.DrawDataCache.Add(data);
		}
	}
}
