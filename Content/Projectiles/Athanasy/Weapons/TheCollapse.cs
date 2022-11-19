using Entrogic.Common.Globals.Players;

namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class TheCollapse : ProjectileBase
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 16;
		}

		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.width = 78;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
		}

		public enum AIState {
			Ready,
			Laser,
			Ending
        };

		public bool Stopped = false;

		public ref float ManaCostTimer => ref Projectile.localAI[0];

		public ref float Timer => ref Projectile.ai[0];

		public AIState State { get => (AIState)Projectile.ai[1]; set => Projectile.ai[1] = (float)value; }

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead || player.noItems || player.CCed) {
				Projectile.Kill();
				return;
			}
			if (Main.myPlayer == Projectile.owner && Main.mapFullscreen) {
				Projectile.Kill();
				return;
			}

			Vector2 mountedCenter = player.MountedCenter;
			var mouseWorld = player.GetModPlayer<SyncedDataPlayer>().MouseWorld;
			var armPosition = player.RotatedRelativePoint(mountedCenter, reverseRotation: true, addGfxOffY: true);
			// 调节位置到握把
			var vector = armPosition.DirectionTo(mouseWorld);
			var offset = vector * 30f;
			armPosition += offset;
			Projectile.timeLeft = 2;

			FindFrame();

			switch (State) {
				case AIState.Ready:
					Timer++;
					if (Timer >= 30) {
						State = AIState.Laser;
						Projectile.netUpdate = true;
						Timer = 0;
                    }
					else if (Stopped) {
						State = AIState.Ending;
						Projectile.netUpdate = true;
					}
					break;
				case AIState.Ending:
					Timer--;
					Projectile.timeLeft = (int)Timer;
					if (Timer <= 0) {
						Projectile.Kill();
                    }
					break;
				case AIState.Laser:
					var center = armPosition + offset;
					Timer++;
					Projectile.timeLeft = 30;
					if (Stopped) {
						State = AIState.Ending;
						Timer = 30;
						Projectile.netUpdate = true;
					}
					if (Timer % 36 == 24) {
						if (!player.CheckMana(player.HeldItem, pay: true)) {
							Stopped = true;
						}
						else {
							SoundEngine.PlaySound(player.HeldItem.UseSound);
							if (player.whoAmI == Main.myPlayer) {
								Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center,
									player.Center.DirectionTo(mouseWorld) * 6f, ModContent.ProjectileType<CollapseLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
							}
						}
						for (int i = 0; i < 20; i++) {
							var position = Utils.RandomVector2(Main.rand, -60f, 60f) + center;
							var velocity = center.DirectionTo(position) * Main.rand.NextFloat(2.3f, 5.4f);
							var d = Dust.NewDustPerfect(center, DustID.FireworksRGB, velocity, 100, Color.SkyBlue);
							d.noGravity = true;
						}
					}
					else if (Timer % 36 < 19) {
						var position = Utils.RandomVector2(Main.rand, -60f, 60f) + center;
						var velocity = position.DirectionTo(center) * Main.rand.NextFloat(2.3f, 5.4f);
						var d = Dust.NewDustPerfect(position, DustID.FireworksRGB, velocity, 100, Color.SkyBlue);
						d.noGravity = true;
                    }
					break;
			}

			if (!player.channel) {
				Stopped = true;
			}

			Projectile.QuickDirectionalHeldProj(player, mouseWorld, armPosition, 0.785f);
		}

		private void FindFrame() {
			// 0-5 准备/终止 (Timer: 0-30)
			// 6-15 发射中
			switch (State) {
				case AIState.Ready:
				case AIState.Ending:
					Projectile.frame = (int)(Timer / 5);
					break;
				case AIState.Laser:
					Projectile.frame = (int)(Timer / 4) % 9 + 6;
					break;
            }
        }

        public override bool? CanDamage() => false;
    }
}
