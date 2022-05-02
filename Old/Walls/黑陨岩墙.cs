
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Walls
{
	public class 黑陨岩墙 : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
            dustType = MyDustId.DarkGrey;//Obsidian Dust
            AddMapEntry(new Color(71, 55, 92));
        }
        public override bool CanExplode(int i, int j)
        {
            return Main.hardMode || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee;
        }
    }
}