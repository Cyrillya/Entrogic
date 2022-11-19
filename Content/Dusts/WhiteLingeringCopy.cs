﻿namespace Entrogic.Content.Dusts
{
    public class WhiteLingeringCopy : ModDust
	{
		public override string Texture => null;

		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			UpdateType = MyDustID.WhiteLingering;

			int desiredVanillaDustTexture = MyDustID.WhiteLingering;
			int frameX = desiredVanillaDustTexture * 10 % 1000;
			int frameY = desiredVanillaDustTexture * 10 / 1000 * 30 + Main.rand.Next(3) * 10;
			dust.frame = new Rectangle(frameX, frameY, 8, 8);
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) => new(255, 255, 255, 0);
	}
}
