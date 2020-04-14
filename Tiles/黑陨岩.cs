using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Tiles
{
	public class 黑陨岩 : ModTile
	{
		public override void SetDefaults()
		{
            Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
            mineResist = 2f;
            minPick = 54;
			dustType = MyDustId.DarkGrey;//Obsidian Dust
			drop = mod.ItemType("黑陨岩");
			AddMapEntry(new Color(100, 78, 128));
		}
        public override bool CanExplode(int i, int j)
        {
            return Main.hardMode || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee;
        }
    }
}