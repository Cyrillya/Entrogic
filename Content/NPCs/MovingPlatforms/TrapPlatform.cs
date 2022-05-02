using System;
using System.Linq;
using Entrogic.Content.NPCs.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Content.NPCs.MovingPlatforms
{
	internal class TrapPlatform : MovingPlatform
	{
		public override void SafeSetDefaults() {
			NPC.width = 200;
			NPC.height = 20;
		}

		// ai0:初始方向（弧度制）
		// ai1:转向间隔
		// ai2:转向计时器
		// ai3:移动速度
		public override void SafeAI() {
			NPC.velocity = NPC.ai[3] * NPC.ai[0].ToRotationVector2();
			NPC.ai[2]--;
			if (NPC.ai[2] <= 0) {
				NPC.ai[0] += MathHelper.ToRadians(180);
				NPC.ai[2] = NPC.ai[1];
			}
			//if (NPC.collideX && collisionTimer <= 0) {
			//	NPC.ai[0] = -NPC.ai[0];
			//	collisionTimer = 5;
			//}
			//collisionTimer--;
		}
	}
}
