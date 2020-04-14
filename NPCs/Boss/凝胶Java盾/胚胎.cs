using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    [AutoloadBossHead]
    public class 胚胎 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter % 15 == 0)
                npc.frame.Y += frameHeight;
            if (npc.frame.Y > frameHeight * 5)
                npc.frame.Y = 0;
        }
        public override void SetDefaults()
        {
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.width = 148;
            npc.height = 170;
            npc.lifeMax = 1800;
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax = 2400;
                }
                if (Entrogic.IsCalamityModDeathMode)
                {
                    npc.lifeMax = 4800;
                }
            }
            npc.damage = 0;
            npc.knockBackResist = 0f;
            npc.npcSlots = 15;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.timeLeft = NPC.activeTime * 30;
            music = MusicID.Boss2;
            musicPriority = MusicPriority.BossLow;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.defense += 2;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = 0;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return base.DrawHealthBar(hbPosition, ref scale, ref position);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                npc.Transform(NPCType<嘉沃顿>());
            }
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Vector2 distance = new Vector2(Math.Abs(npc.Center.X - Main.player[npc.target].Center.X), Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y));
            distance /= 16;
            if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneRockLayerHeight || Math.Max((byte)distance.X, (byte)distance.Y) > 255)
            {
                distance = new Vector2(Math.Abs(npc.Center.X - Main.player[npc.target].Center.X), Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y));
                distance /= 16;
                npc.TargetClosest(true);
            }
            else
                npc.timeLeft = 10;

            if (npc.ai[0] == 0)
            {
                Vector2 r = new Vector2(Main.rand.Next(240, 428 + 1), Main.rand.Next(240, 428 + 1));
                r = r - new Vector2(Main.rand.Next(201), Main.rand.Next(201));
                if (Main.rand.NextBool(2)) r.X = -r.X;
                if (Main.rand.NextBool(2)) r.Y = -r.Y;
                string type = "";
                switch (Main.rand.Next(3))
                {
                    case 0:
                        type = "凝胶恶魔";
                        break;
                    case 1:
                        type = "凝胶鸟妖";
                        break;
                    default:
                        type = "凝胶蜜蜂";
                        break;
                }
                int gel = NPC.NewNPC((int)(npc.Center.X + r.X), (int)(npc.Center.Y + r.Y), mod.NPCType(type));
                npc.netUpdate = true;
            }
            else if (npc.ai[0] == 270)
            {
                npc.ai[0] = 0f;
                return;
            }
            npc.ai[0]++;

            npc.ai[1]++;
            if (npc.ai[1] == 1f)
            {
                short numPlayers = 0;
                foreach (Player player in Main.player)
                {
                    if (player.active)
                    {
                        numPlayers++;
                    }
                }
                npc.lifeMax += (int)(npc.lifeMax * 0.6f * numPlayers);
                npc.life = npc.lifeMax;
            }
            if (npc.ai[1] < 600)
                npc.dontTakeDamage = true;
            else
                npc.dontTakeDamage = false;

            if (npc.wet)
                npc.velocity.Y = -6.6f;
        }
    }
}
