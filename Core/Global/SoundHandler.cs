using Microsoft.Xna.Framework.Audio;

namespace Entrogic.Core.Global
{
	public class SoundHandler : ModSystem
	{
		public override void Load() {
			base.Load();

			On.Terraria.Audio.SoundEngine.PlaySound_int_int_int_int_float_float += SoundEngine_PlaySound_int_int_int_int_float_float;
		}

		private SoundEffectInstance SoundEngine_PlaySound_int_int_int_int_float_float(On.Terraria.Audio.SoundEngine.orig_PlaySound_int_int_int_int_float_float orig, int type, int x, int y, int Style, float volumeScale, float pitchOffset) {
			Vector2 oriScreenPos = Main.screenPosition;
			if (x != -1 && y != -1) {
				Main.screenPosition = Main.LocalPlayer.Center - Main.ScreenSize.ToVector2() * 0.5f;
			}
			var playSound = orig(type, x, y, Style, volumeScale, pitchOffset);
			Main.screenPosition = oriScreenPos;
			return playSound;
		}
	}
}
