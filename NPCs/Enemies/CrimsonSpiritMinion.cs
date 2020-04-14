using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class CrimsonSpiritMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 26;
            npc.aiStyle = 56;
            animationType = NPCID.DungeonSpirit;
            npc.HitSound = SoundID.NPCHit36;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.knockBackResist = 0.2f;
            npc.value = 0f;
            npc.alpha = 128;
            npc.noTileCollide = true;
            npc.noGravity = true;
            if (!NPC.downedPlantBoss && Main.hardMode)
            {
                npc.damage = 70;
                npc.defense = 15;
                npc.lifeMax = 200;
            }
            else
            {
                npc.damage = 140;
                npc.defense = 30;
                npc.lifeMax = 400;
            }
        }
    }
}