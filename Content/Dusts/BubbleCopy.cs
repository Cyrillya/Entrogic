using Entrogic.Helpers.ID;

namespace Entrogic.Content.Dusts
{
    public class BubbleCopy : ModDust
	{
		public override string Texture => null;

        public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			UpdateType = MyDustID.BlueWhiteBubble;

			int desiredVanillaDustTexture = MyDustID.BlueWhiteBubble;
			int frameX = desiredVanillaDustTexture * 10 % 1000;
			int frameY = desiredVanillaDustTexture * 10 / 1000 * 30 + Main.rand.Next(3) * 10;
			dust.frame = new Rectangle(frameX, frameY, 8, 8);
		}

        public override Color? GetAlpha(Dust dust, Color lightColor) => new(255, 255, 255, 0);
	}
}
