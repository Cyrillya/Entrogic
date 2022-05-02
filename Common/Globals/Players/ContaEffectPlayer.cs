using Entrogic.Common.ModSystems;
using Entrogic.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Common.Globals.Players
{
    public class ContaEffectPlayer : ModPlayer
	{
		//public ModSoundStyle offSound;

		private Asset<Texture2D> effectTexture;
		public bool enable;
		public bool protecting;
		public int protectLeft;
		public double protectCooldown;
		private float flameRingScale;
		private float flameRingRot;
		private float flameRingDeathTimer; // 这个以秒为单位

        public override void Load() {
            base.Load();

			//offSound = new ModSoundStyle(nameof(Entrogic), "Assets/Sounds/Player/ArmorConta", volume: 0.9f, pitchVariance: -0.2f);

			On.Terraria.Main.DrawInfernoRings += Main_DrawInfernoRings;
            On.Terraria.Player.Hurt += Player_Hurt;
        }

		public void ContaminatedDodge() {
			Player.SetImmuneTimeForAllTypes(100);

			protectLeft--;
			if (protectLeft <= 0) {
				protectCooldown = 40 * 60;
				protecting = false;

				flameRingRot = 0f;
				flameRingScale = 0f;
				flameRingDeathTimer = 0.8f;
				Player.SetImmuneTimeForAllTypes(160);
				Player.AddBuff(ModContent.BuffType<ContaBuff>(), 60 * 12);
				//SoundEngine.PlaySound(offSound);
			}
            else {
				Player.AddBuff(BuffID.Obstructed, 40);
			}

			if (Player.whoAmI != Main.myPlayer)
				return;

			if (Main.netMode != NetmodeID.SinglePlayer)
				ModNetHandler.Dodge.SendContaminatedDodge(-1, -1, (byte)Player.whoAmI);
		}

		private double Player_Hurt(On.Terraria.Player.orig_Hurt orig, Player self, PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter) {
			if (!self.creativeGodMode && !self.immune && self.whoAmI == Main.myPlayer && self.GetModPlayer<ContaEffectPlayer>().protecting) {
				self.GetModPlayer<ContaEffectPlayer>().ContaminatedDodge();
				return 0.0;
			}
            else {
				return orig(self, damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter);
            }
        }

        private void Main_DrawInfernoRings(On.Terraria.Main.orig_DrawInfernoRings orig, Main self) {
			for (int i = 0; i < 255; i++) {
				if (!Main.player[i].active || Main.player[i].outOfRange || (Main.player[i]?.GetModPlayer<ContaEffectPlayer>().protecting == false && Main.player[i]?.GetModPlayer<ContaEffectPlayer>().flameRingDeathTimer < 0) || Main.player[i].dead) {
					continue;
				}

				if (effectTexture == null) {
                    effectTexture = ResourceManager.Miscellaneous["ContaEffect"];
                }

				ContaEffectPlayer flamePlayer = Main.player[i].GetModPlayer<ContaEffectPlayer>();

				flamePlayer.flameRingDeathTimer -= (float)Main.gameTimeCache.ElapsedGameTime.TotalSeconds;
				float scaleMuilt = flamePlayer.flameRingDeathTimer;
				if (flamePlayer.protecting) scaleMuilt = 0.8f;


				float num2 = 0.1f;
                float num3 = 0.9f;
				if (!Main.gamePaused)
					flamePlayer.flameRingScale += 0.004f;

                float num;
                if (flamePlayer.flameRingScale < 1f) {
                    num = flamePlayer.flameRingScale;
                }
                else {
					flamePlayer.flameRingScale = 0.8f;
                    num = flamePlayer.flameRingScale;
                }

                if (!Main.gamePaused)
					flamePlayer.flameRingRot += 0.05f;

				if (flamePlayer.flameRingRot > (float)Math.PI * 2f)
					flamePlayer.flameRingRot -= (float)Math.PI * 2f;

				if (flamePlayer.flameRingRot < (float)Math.PI * -2f)
					flamePlayer.flameRingRot += (float)Math.PI * 2f;

				for (int j = 0; j < 3; j++) {
					float num4 = num + num2 * (float)j;
					if (num4 > 1f)
						num4 -= num2 * 2f;

					float num5 = MathHelper.Lerp(0.8f, 0f, Math.Abs(num4 - num3) * 10f);

					Main.spriteBatch.Draw(effectTexture.Value, Main.player[i].Center - Main.screenPosition, new Rectangle(0, 400 * j, 400, 400), new Color(num5, num5, num5, num5) * (scaleMuilt * 1.5f), Main.player[i].GetModPlayer<ContaEffectPlayer>().flameRingRot + (float)Math.PI / 3f * (float)j, new Vector2(200f, 200f), num4 * scaleMuilt, SpriteEffects.None, 0f);
				}

				Main.player[i].moveSpeed += .10f;
				Main.player[i].GetDamage(DamageClass.Generic) += .10f;
			}
			orig(self);
		}

        public override void ResetEffects() {
            base.ResetEffects();
            enable = false;
        }

        public override void UpdateDead() {
            base.UpdateDead();
            enable = false;
			protectCooldown = 0;
			flameRingScale = 0f;
			flameRingRot = 0f;
			flameRingDeathTimer = 0f;
		}

        public override void PostUpdate() {
            base.PostUpdate();
			protectCooldown--;
			if (enable && protectCooldown <= 0) {
				if (!protecting) {
					protecting = true;
					protectLeft = 3;
                }
			}
            else {
				protecting = false;
			}
        }
	}
}
