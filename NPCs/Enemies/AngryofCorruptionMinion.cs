using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.NPCs.Enemies
{
    public class AngryofCorruptionMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 26;
            npc.aiStyle = 49;
            animationType = 250;
            npc.value = 0f;
            npc.buffImmune[20] = true;
            npc.alpha = 128;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;

            npc.lifeMax = !NPC.downedGolemBoss ? 300 : !NPC.downedMoonlord ? 400 : 1000;
            npc.defense = !NPC.downedGolemBoss ? 12 : !NPC.downedMoonlord ? 20 : 42;
            npc.damage = !NPC.downedGolemBoss ? 25 : !NPC.downedMoonlord ? 50 : 80;
            npc.knockBackResist = !NPC.downedGolemBoss ? 0.3f : !NPC.downedMoonlord ? 0.2f : 0.15f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            int num5;
            if (npc.life > 0)
            {
                int num333 = 0;
                while ((double)num333 < damage / (double)npc.lifeMax * 100.0)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 14, 0f, 0f, npc.alpha, npc.color, 1f);
                    num5 = num333;
                    num333 = num5 + 1;
                }
            }
            else
            {
                for (int num334 = 0; num334 < 50; num334 = num5 + 1)
                {
                    int num335 = Dust.NewDust(npc.position, npc.width, npc.height, MyDustId.CorruptionParticle, Main.rand.Next(-15, 16) * 0.2f, Main.rand.Next(-15, 16) * 0.2f, 0, default(Color), 1.3f);
                    Dust dust = Main.dust[num335];
                    dust.velocity *= 2f;
                    num5 = num334;
                }
            }
        }
        public override void AI()
        {
            if (NPC.CountNPCS(NPCType<AngryofCorruption>()) < 1)
            {
                npc.life = 0;
                int num5;
                for (int num334 = 0; num334 < 50; num334 = num5 + 1)
                {
                    int num335 = Dust.NewDust(npc.position, npc.width, npc.height, MyDustId.CorruptionParticle, Main.rand.Next(-15, 16) * 0.2f, Main.rand.Next(-15, 16) * 0.2f, 0, default(Color), 1.3f);
                    Dust dust = Main.dust[num335];
                    dust.velocity *= 1.2f;
                    num5 = num334;
                }
            }
        }
    }
}