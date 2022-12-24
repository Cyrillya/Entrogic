using Entrogic.Content.RockGolem.Weapons;
using Entrogic.Core.BaseTypes;
using Entrogic.Core.Global.Player;
using Entrogic.Core.Global.Resource;
using Entrogic.Helpers;
using Terraria.Graphics.Shaders;

namespace Entrogic.Content.RockGolem.FriendlyProjectiles
{
    public class RockSpearBow : ProjectileBase
    {
        public override void SetStaticDefaults() {
			RockBow.ProjectileType = Type;
        }

        public override void SetDefaults() {
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		private Player Player => Main.player[Projectile.owner];

		private bool CanShoot => Player.channel && Player.HasAmmo(Player.HeldItem) && Player.active && !Player.dead && !Player.noItems && !Player.CCed;

		private const int ChargingTime = 40;
		private const int MaxSpears = 3; // 一边的+中间的
		private const int SpearDegree = 15;

		public ref float Timer => ref Projectile.ai[0];

		public int SpearCount { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }

        public override void ModifyDamageScaling(ref float damageScale) {
			// 速度越快伤害越高
			float factor = Utils.GetLerpValue(0f, 10f, Main.LocalPlayer.velocity.Length(), true);
			damageScale = MathHelper.Lerp(0.4f, 1f, factor * factor);
        }

        public override void AI() {
			if (!CanShoot) {
				if (SpearCount == 0) {
					SpearCount = 1;
				}
				UnleashSpears(Player.MountedCenter);
				Projectile.Kill();

				if (!Main.dedServ) {
					SoundEngine.PlaySound(SoundAssets.BowReleased, Projectile.Center);
					// 强行掐断拉弓
					var pullingSoundPlaying = SoundEngine.FindActiveSound(SoundAssets.BowPulling);
					if (pullingSoundPlaying is not null) {
						pullingSoundPlaying.Stop();
					}
				}
				return;
			}
			if (Main.myPlayer == Projectile.owner && Main.mapFullscreen) {
				Projectile.Kill();
				return;
			}

			Timer++;
			if (Timer >= ChargingTime && SpearCount < MaxSpears) {
				Timer = 0;
				SpearCount++;
				//SoundEngine.PlaySound(SoundID.Item149);
				if (SpearCount == MaxSpears)
					SoundEngine.PlaySound(SoundAssets.BowFilled);
			}

			var mountedCenter = Player.MountedCenter;
			var mouseWorld = Player.GetModPlayer<SyncedDataPlayer>().MouseWorld;
			var armPosition = Player.RotatedRelativePoint(mountedCenter, reverseRotation: true, addGfxOffY: true);

			Projectile.timeLeft = 2;
			Projectile.QuickDirectionalHeldProj(Player, mouseWorld, armPosition);
		}


		private void UnleashSpears(Vector2 mountedCenter) {
			if (Main.myPlayer != Projectile.owner)
				return;

			for (int i = -2; i <= 2; i++) {
				// 没蓄到的不绘制
				if (Math.Abs(i) > SpearCount - 1)
					continue;
				Player.PickAmmo(Player.HeldItem, out int arrowProj, out float speed, out int damage, out float knockBack, out int ammoItemID, i == 0);
				float radians = MathHelper.ToRadians(i * SpearDegree);
				float rotation = Projectile.rotation + radians;
				var normalizedVector = rotation.ToRotationVector2() * Player.direction;

				var position = mountedCenter + normalizedVector * 30f;
				var velocity = normalizedVector * speed;

				Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, ammoItemID),
					position, velocity, arrowProj, damage, knockBack, Main.myPlayer);
			}
		}

        public override bool PreDraw(ref Color lightColor) {
			var center = Projectile.Center;
			Projectile.width = 48;
			Projectile.height = 132;
			Projectile.Center = center;
			return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor) {
			var center = Projectile.Center;
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.Center = center;

			Player.PickAmmo(Player.HeldItem, out int arrowProj, out _, out _, out _, out _, true);

			var bowTexture = TextureAssets.Projectile[Type].Value;
			var spearTexture = TextureAssets.Projectile[arrowProj].Value;

			var positioningRotation = Projectile.velocity.ToRotation();
			var origin = spearTexture.Size() / 2f;
			var bowstringCenter = Projectile.Center - new Vector2(bowTexture.Width / 2f, 0f).RotatedBy(positioningRotation) - Main.screenPosition;
			var arrowCenter = bowstringCenter + new Vector2(origin.X, 0f).RotatedBy(positioningRotation);

			var spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			float factor = 1f - Utils.GetLerpValue(0f, ChargingTime, Timer, true);
			float opacity = Utils.GetLerpValue(0f, 16f, Timer, true);

			for (int i = -2; i <= 2; i++) {
				// 没蓄到的不绘制
				if (Math.Abs(i) > SpearCount)
					continue;
				float radians = MathHelper.ToRadians(i * SpearDegree) * Player.direction;
				var drawPosition = arrowCenter.RotatedBy(radians, bowstringCenter);
				float rotation = Projectile.rotation + radians;
				// 缓进效果
				if (Math.Abs(i) == SpearCount) {
					var color = lightColor * opacity;
					var toShootTargetVector = rotation.ToRotationVector2() * Player.direction;
					var finalPosition = drawPosition + toShootTargetVector * factor * 30f;

					Main.EntitySpriteDraw(spearTexture, finalPosition, null, color, rotation, origin, 1f, spriteEffects, 0);
				}
				else {
					Main.EntitySpriteDraw(spearTexture, drawPosition, null, lightColor, rotation, origin, 1f, spriteEffects, 0);
				}
			}

			if (opacity == 1f)
				return;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
			GameShaders.Armor.Apply(ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex, Projectile, null);

			// 已经填充好的有个闪光效果，放到这里是为了防止不断Begin End开销过大
			Color shineColor = new(255, 200, 100);
			Color finalColor = lightColor.MultiplyRGB(shineColor) * (1f - opacity);
			for (int i = -2; i <= 2; i++) {
				if (Math.Abs(i) != SpearCount - 1 && SpearCount != MaxSpears)
					continue;
				float radians = MathHelper.ToRadians(i * SpearDegree);
				var drawPosition = arrowCenter.RotatedBy(radians, bowstringCenter);
				float rotation = Projectile.rotation + radians;
				Main.EntitySpriteDraw(spearTexture, drawPosition, null, finalColor, rotation, origin, 1f, spriteEffects, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
		}
    }
}
