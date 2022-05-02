using Entrogic.Content.Tiles.Furnitures.HangingRope;

namespace Entrogic.Common.Globals.Players
{
    internal class HangingPlayer : ModPlayer
	{
		internal bool Hanging;
		internal int HangingTimer;
		internal Vector2 HangingPos;
		internal Vector2 HangingRingPos;
		internal Point HangingRopeTile;

		public void StartHanging(int x, int y) {
			Player.StopVanityActions();
			Player.RemoveAllGrapplingHooks();
			Player.RemoveAllFishingBobbers();
			if (Player.mount.Active)
				Player.mount.Dismount(Player);

			Player.position = new Vector2(x + 0.4f, y + 2.5f) * 16f;
			Player.velocity = Vector2.Zero;
			Player.gravDir = 1f;
			Hanging = true;
			HangingPos = new Vector2(x, y);
			HangingRingPos = new Vector2(x, y + 2f);
			if (Main.myPlayer == Player.whoAmI) ; // 联机同步以后补上
				//NetMessage.SendData(13, -1, -1, null, player.whoAmI);
		}

		public override void UpdateDead() {
			base.UpdateDead();
			Hanging = false;
			HangingTimer = 0;
		}

        public override bool CanUseItem(Item item) {
            return !Hanging;
        }

        public override bool PreItemCheck() {
			if (Hanging) {
				var t = Framing.GetTileSafely(HangingPos.ToPoint());
				if (t == null || !t.IsActive || t.type != ModContent.TileType<HangingRope_Tile>()) {
					UpdateDead();
					return base.PreItemCheck();
				}

				Player.controlJump = false;
				Player.controlDown = false;
				Player.controlLeft = false;
				Player.controlRight = false;
				Player.controlUp = false;
				Player.controlUseItem = false;
				Player.controlUseTile = false;
				Player.controlThrow = false;
				Player.gravDir = 1f;
				Player.velocity = Vector2.Zero;
				Player.StopVanityActions();
				Player.RemoveAllGrapplingHooks();
				Player.RemoveAllFishingBobbers();
				if (Player.mount.Active)
					Player.mount.Dismount(Player);

				HangingTimer++;
				if (HangingTimer > 1)
					Player.position = Player.oldPosition;

				Player.lifeRegen = 0;
				Player.lifeRegenCount = 0;
				if (HangingTimer % 5 == 0) {
					Player.statLife -= 5;
					CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), CombatText.LifeRegen, 5, dramatic: false, dot: true);
					if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer) {
						Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name}将自己绞死了。"), 10.0, 0);
					}
					if (Player.creativeGodMode) {
						Main.NewText("[c/FF0000:你试图在无敌模式绞死自己，可惜这并不可能]");
						UpdateDead();
					}
				}

				Player.bodyFrame = new Rectangle(0, 0, 40, 56);
				Player.legFrame = new Rectangle(0, 0, 40, 56);
				Player.eyeHelper.BlinkBecausePlayerGotHurt();
			}
			return base.PreItemCheck();
		}
	}
}
