using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class 疫医 : FSM_NPC
    {
        enum NPCState
        {
            Attack,
            Move
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 20;
        }
        public override void SetDefaults()
        {
            npc.width = 20;
            npc.height = 28;
            npc.damage = 5;
            npc.defense = 6;
            npc.lifeMax = 62;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 4, 0);
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            npc.noTileCollide = false;
            npc.scale = 2;
            aiType = -1;
        }
        public override void AI()
        {
            Vector2 vector134 = Main.player[npc.target].Center - npc.Center;
            Vector2 vector135 = Vector2.Normalize(vector134);
            float num1043 = vector134.Length();
            float num1044 = 550f;
            if (num1043 < num1044)
            {
                SwitchState((int)NPCState.Attack);
            }
            else
            {
                SwitchState((int)NPCState.Move);
            }
            npc.TargetClosest(true);
            switch ((NPCState)State)
            {
                case NPCState.Move:
                    {
                        /*if (npc.direction == -1) npc.spriteDirection = 1;
                        else if (npc.direction == 1) npc.spriteDirection = -1;*/
                        npc.aiStyle = 3;
                        npc.TargetClosest(false);
                        if (npc.collideX && npc.velocity.Y == 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y - 8.5f;
                        }
                        npc.velocity.X = Main.expertMode ? 1.6f : 1.15f * 0.9f * npc.direction;
                        npc.velocity.X = npc.velocity.X * npc.direction;
                        break;
                    }
                case NPCState.Attack:
                    {
                        npc.aiStyle = -1;
                        npc.velocity.X = 0f;
                        int num1041 = 438;
                        int num1042 = 30;
                        float scaleFactor7 = 7f;
                        npc.ai[1]++;
                        if (npc.ai[1] >= 60f)
                        {
                            if (npc.ai[1] == 60f) npc.ai[2] = 1f;
                            else if (npc.ai[1] == 63f) npc.ai[2] = 2f;
                            else if (npc.ai[1] == 66f) npc.ai[2] = 3f;
                            else if (npc.ai[1] == 69f) npc.ai[2] = 4f;
                            else if (npc.ai[1] == 72f) npc.ai[2] = 5f;
                            else if (npc.ai[1] >= 75f)
                            {
                                Vector2 center8 = Main.player[npc.target].Center;
                                Vector2 vector136 = npc.Center - Vector2.UnitY * 4f;
                                Vector2 vector137 = center8 - vector136;
                                vector137.X += Main.rand.Next(-50, 51);
                                vector137.Y += Main.rand.Next(-50, 51);
                                vector137.X *= Main.rand.Next(80, 121) * 0.01f;
                                vector137.Y *= Main.rand.Next(80, 121) * 0.01f;
                                vector137.Normalize();
                                if (float.IsNaN(vector137.X) || float.IsNaN(vector137.Y))
                                {
                                    vector137 = -Vector2.UnitY;
                                }
                                vector137 *= scaleFactor7;
                                Projectile.NewProjectile(vector136.X, vector136.Y, vector137.X, vector137.Y, num1041, num1042, 0f, Main.myPlayer, 0f, 0f);
                                npc.netUpdate = true;
                                npc.ai[2] = 0f;
                                npc.ai[1] = -60f;
                                SwitchState((int)NPCState.Move);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (!(npc.velocity.Y <= 0f && npc.velocity.Y >= -1f))
            {
                npc.frameCounter = 0.0;
                npc.frame.Y = 0;
                return;
            }
            if (npc.ai[2] > 0f)
            {
                npc.frame.Y = frameHeight * (int)npc.ai[2];
                npc.frameCounter = 0.0;
                return;
            }
            if (npc.frame.Y < frameHeight * 6)
            {
                npc.frame.Y = frameHeight * 6;
            }
            npc.frameCounter += Math.Abs(npc.velocity.X) * 2f;
            npc.frameCounter += npc.velocity.X;
            if (npc.frameCounter > 6.0)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y / frameHeight >= Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = frameHeight * 6;
                return;
            }
        }
    }
}
