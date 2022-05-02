using Entrogic.Content.NPCs.BaseTypes;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Common.ModSystems
{
    public class MovingPlatformHandler : ModSystem
	{
		public override void Load() {
			base.Load();
			On.Terraria.Player.Update_NPCCollision += Player_Update_NPCCollision;
		}

		private void Player_Update_NPCCollision(On.Terraria.Player.orig_Update_NPCCollision orig, Player self) {
			bool controlDown = self.controlDown;
			if (controlDown) {
				self.GetModPlayer<MovingPlatformPlayer>().Timer = 5;
			}
			bool flag = self.controlDown || self.GetModPlayer<MovingPlatformPlayer>().Timer > 0 || self.GoingDownWithGrapple;
			if (flag) {
				orig.Invoke(self);
			}
			else {
				foreach (NPC npc in from n in Main.npc
									where n.active && n.ModNPC != null && n.ModNPC is MovingPlatform && new Rectangle((int)self.position.X, (int)self.position.Y + self.height, self.width, 1).Intersects(new Rectangle((int)n.position.X, (int)n.position.Y, n.width, 8 + ((self.velocity.Y > 0f) ? ((int)self.velocity.Y) : 0))) && self.position.Y <= n.position.Y
									select n) {
					if (!self.justJumped && self.velocity.Y >= 0f) {
						self.gfxOffY = npc.gfxOffY;
						self.velocity.Y = 0f;
						self.fallStart = (int)(self.position.Y / 16f);
						self.position.Y = npc.position.Y - (float)self.height + 4f;
						orig.Invoke(self);
					}
				}
				orig.Invoke(self);
			}
		}
	}
	public class MovingPlatformPlayer : ModPlayer
	{
		public int Timer { get; set; }
		private Vector2 _platformJustStand = new Vector2();
		private bool _standing = false;
		private int _screenMoveTimer;
		private const int SCREEN_TIMER_MAX = 15;

		public override void PreUpdate() {
			if (Timer > 0) {
				Timer--;
			}
		}

		public override void ModifyScreenPosition() {
			base.ModifyScreenPosition();
			_standing = false;
			// 站在移动平台上时，屏幕中心会逐渐移至平台处，这一段都是平滑相机
			foreach (NPC npc in from n in Main.npc
								where n.active && n.ModNPC != null && n.ModNPC is MovingPlatform && new Rectangle((int)Player.position.X, (int)Player.position.Y + (Player.height - 2), Player.width, 4).Intersects(new Rectangle((int)n.position.X, (int)n.position.Y, n.width, 4)) && Player.position.Y <= n.position.Y && Player.velocity.Y <= 0.2f + n.velocity.Y
								select n) {
					_platformJustStand = npc.Center;
					_standing = true;
			}
			if (_platformJustStand != Vector2.Zero) {
				var playerScreenize = Player.Center - Main.ScreenSize.ToVector2() / 2f;
				var npcScreenize = _platformJustStand - Main.ScreenSize.ToVector2() / 2f;
				if (_standing) { if (_screenMoveTimer < SCREEN_TIMER_MAX) _screenMoveTimer++; }
				else if (_screenMoveTimer > 0) _screenMoveTimer--;
				float factor = (float)_screenMoveTimer / (float)SCREEN_TIMER_MAX;
				Main.screenPosition = Utils.Floor(playerScreenize - factor * (playerScreenize - npcScreenize));
			}
		}
	}
}
