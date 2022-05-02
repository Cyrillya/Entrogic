using Entrogic.Common.ModSystems;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Content.NPCs.BaseTypes
{
	public abstract class MovingPlatform : NPCBase
	{
        public virtual void SafeSetDefaults() {
		}

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("");
		}

		public sealed override void SetDefaults() {
			SafeSetDefaults();
			NPC.lifeMax = 1;
			NPC.immortal = true;
			NPC.dontTakeDamage = true;
			NPC.noGravity = true;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
		}

		public override bool CheckActive() {
			return false;
		}

		public virtual void SafeAI() {
		}

		public sealed override void AI() {
			SafeAI();
			foreach (Player player in from p in Main.player
									  where new Rectangle((int)p.position.X, (int)p.position.Y + (p.height - 2), p.width, 4).Intersects(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, 4)) && p.position.Y <= NPC.position.Y && p.velocity.Y <= 0.2f + NPC.velocity.Y
									  select p) {
				player.position += NPC.velocity;
			}
			foreach (Projectile projectile in from n in Main.projectile
											  where n.active && n.aiStyle == 7 && n.ai[0] != 1f && n.timeLeft < 35997 && n.Hitbox.Intersects(NPC.Hitbox)
											  select n) {
				projectile.ai[0] = 2f;
				projectile.netUpdate = true;
			}
		}

		public override bool? CanBeHitByProjectile(Projectile projectile) {
			return new bool?(false);
		}

		public override bool? CanBeHitByItem(Player player, Item item) {
			return new bool?(false);
		}
	}
}
