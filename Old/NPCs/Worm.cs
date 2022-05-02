using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.IO;

namespace Entrogic.NPCs
{
    // Ported from my tAPI mod because I'm lazy
    // This abstract class can be used for non splitting worm type NPC.(我搞了个splitting)
    public abstract class Worm : ModNPC
    {
        /* ai[0] = “我”的追随者
		 * ai[1] = “我”追随的部位
		 * ai[2] = 生成时还差n个部件到尾巴
		 * ai[3] = 头部
		 */
        public bool head;
        public bool tail;
        public int minLength;
        public int maxLength;
        public int headType;
        public int bodyType;
        public int tailType;
        public bool flies = false;
        public bool directional = false;
        public float speed;
        public float turnSpeed;
        public bool splitting = false;

        public override void AI()
        {
            if (npc.localAI[1] == 0f)
            {
                npc.localAI[1] = 1f;
                Init();
            }
            if (npc.ai[3] > 0f && !splitting)
            {
                npc.realLife = (int)npc.ai[3];
            }
            if (!head && npc.timeLeft < 300)
            {
                npc.timeLeft = 300;
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            if (Main.player[npc.target].dead && npc.timeLeft > 300)
            {
                npc.timeLeft = 300;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!tail && npc.ai[0] == 0f)
                {
                    if (head)
                    {
                        npc.ai[3] = (float)npc.whoAmI;
                        if (!splitting)
                        {
                            npc.realLife = npc.whoAmI;
                        }
                        npc.ai[2] = (float)Main.rand.Next(minLength, maxLength + 1);
                        npc.ai[0] = (float)NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), bodyType, npc.whoAmI);
                    }
                    else if (npc.ai[2] > 0f)
                    {
                        npc.ai[0] = (float)NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type, npc.whoAmI);
                    }
                    else
                    {
                        npc.ai[0] = (float)NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), tailType, npc.whoAmI);
                    }
                    Main.npc[(int)npc.ai[0]].ai[3] = npc.ai[3];
                    if (!splitting)
                    {
                        Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
                    }
                    Main.npc[(int)npc.ai[0]].ai[1] = (float)npc.whoAmI;
                    Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] - 1f;
                    npc.netUpdate = true;
                }
                if (!splitting)
                {
                    if (!head && (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].type != headType && Main.npc[(int)npc.ai[1]].type != bodyType))
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }
                    if (!tail && (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].type != bodyType && Main.npc[(int)npc.ai[0]].type != tailType))
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }
                }
                else
                {
                    // ai[1] 我前面的
                    // ai[0] 我后面的
                    if (Main.npc[(int)npc.ai[1]].type == tailType) // 我前面的是尾巴？？
                    {
                        // 啊我吓死了
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }
                    if (Main.npc[(int)npc.ai[0]].type == headType) // 我后面的是头？？
                    {
                        // 啊我吓死了
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }
                    if (!Main.npc[(int)npc.ai[1]].active) // 我前面的没了
                    {
                        if (!tail) // 我8是尾巴
                        {
                            // 我一个大转身变成头
                            int whoAmI = npc.whoAmI;
                            float num25 = (float)npc.life / (float)npc.lifeMax;
                            float num26 = npc.ai[0];
                            float num27 = npc.ai[2];
                            npc.SetDefaultsKeepPlayerInteraction(headType); // 换替身，走了走了
                            npc.life = (int)((float)npc.lifeMax * num25);
                            npc.ai[0] = num26; // 我后面的还是我后面的，嘿嘿！
                            npc.TargetClosest(true);
                            npc.netUpdate = true; // 服务器给👴整个变身
                            npc.whoAmI = whoAmI; // 我还是我，嘿嘿！
                            npc.ai[2] = num27;
                            head = true; // 我现在是头了
                            npc.ai[3] = (float)npc.whoAmI;
                            for (int i = 1; i <= (int)npc.ai[2]; i++) // 遍历从我到尾巴
                            {
                                int foundNum = (int)npc.whoAmI + i; // 假定我的编号＋i就是我要找的人
                                if (foundNum < Main.npc.Length)
                                {
                                    NPC targetFound = Main.npc[foundNum];
                                    if (targetFound.active)
                                    {
                                        targetFound.ai[3] = (float)npc.whoAmI; // 后面的，你们的头现在是我了！
                                    }
                                }
                            }
                        }
                        else // 我去，我是尾巴？？
                        {
                            // 啊我死了
                            npc.life = 0;
                            npc.HitEffect(0, 10.0);
                            npc.active = false;
                            // 假定我编号一定比头大，则我到头的距离=我的编号-头的编号
                            // 这个理应在上面做好，所以我不要了
                            //int headNum = npc.whoAmI - (int)npc.ai[3];
                            //for (int i = -1; i <= -headNum; i++) // 遍历从我到头
                            //{
                            //    int foundNum = (int)npc.whoAmI + i; // 假定我的编号＋i就是我要找的人
                            //    if (foundNum >= 0)
                            //    {
                            //        NPC targetFound = Main.npc[foundNum];
                            //        if (targetFound.active)
                            //        {
                            //            targetFound.ai[2] -= 2; // 前面的，你们到尾巴的距离-2！
                            //        }
                            //    }
                            //}
                        }
                    }
                    if (!Main.npc[(int)npc.ai[0]].active) // 我后面的没了
                    {
                        if (!head) // 我8是头
                        {
                            // 我一个大转身变成尾巴
                            int whoAmI = npc.whoAmI;
                            float num25 = (float)npc.life / (float)npc.lifeMax;
                            float num26 = npc.ai[1];
                            npc.SetDefaultsKeepPlayerInteraction(tailType); // 换替身，走了走了
                            npc.life = (int)((float)npc.lifeMax * num25);
                            npc.ai[1] = num26; // 我前面的还是我前面的，嘿嘿！
                            npc.TargetClosest(true);
                            npc.netUpdate = true; // 服务器给👴整个变身
                            npc.whoAmI = whoAmI; // 我还是我，嘿嘿！
                            npc.ai[2] = 0;
                            tail = true; // 我现在是尾巴了
                            //假定我编号一定比头大，则我到头的距离=我的编号-头的编号
                            int headNum = npc.whoAmI - (int)npc.ai[3];
                            for (int i = -1; i <= -headNum; i++) // 遍历从我到头
                            {
                                int foundNum = (int)npc.whoAmI + i; // 假定我的编号＋i就是我要找的人
                                if (foundNum >= 0)
                                {
                                    NPC targetFound = Main.npc[foundNum];
                                    if (targetFound.active)
                                    {
                                        targetFound.ai[2] = Math.Abs(i); // 前面的，你们尾巴成我了，到尾巴的距离也变了！
                                    }
                                }
                            }
                        }
                        else // 我去，我是头？？
                        {
                            // 啊我死了
                            npc.life = 0;
                            npc.HitEffect(0, 10.0);
                            npc.active = false;
                            // 这个理应在上面做好，所以我不要了
                            //for (int i = 1; i <= (int)npc.ai[2]; i++) // 遍历从我到尾巴
                            //{
                            //    int foundNum = (int)npc.whoAmI + i; // 假定我的编号＋i就是我要找的人
                            //    if (foundNum < Main.npc.Length)
                            //    {
                            //        NPC targetFound = Main.npc[foundNum];
                            //        if (targetFound.active)
                            //        {
                            //            targetFound.ai[3] = (float)npc.whoAmI + 2; // 后面的，你们的头现在是我后面的后面了！
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                if (!npc.active && Main.netMode == NetmodeID.Server)
                {
<<<<<<< HEAD
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
=======
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                }
            }
            int num180 = (int)(npc.position.X / 16f) - 1;
            int num181 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
            int num182 = (int)(npc.position.Y / 16f) - 1;
            int num183 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
            if (num180 < 0)
            {
                num180 = 0;
            }
            if (num181 > Main.maxTilesX)
            {
                num181 = Main.maxTilesX;
            }
            if (num182 < 0)
            {
                num182 = 0;
            }
            if (num183 > Main.maxTilesY)
            {
                num183 = Main.maxTilesY;
            }
            bool flag18 = flies;
            if (!flag18)
            {
                for (int num184 = num180; num184 < num181; num184++)
                {
                    for (int num185 = num182; num185 < num183; num185++)
                    {
                        if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].nactive() && (Main.tileSolid[(int)Main.tile[num184, num185].type] || Main.tileSolidTop[(int)Main.tile[num184, num185].type] && Main.tile[num184, num185].frameY == 0) || Main.tile[num184, num185].liquid > 64))
                        {
                            Vector2 vector17;
                            vector17.X = (float)(num184 * 16);
                            vector17.Y = (float)(num185 * 16);
                            if (npc.position.X + (float)npc.width > vector17.X && npc.position.X < vector17.X + 16f && npc.position.Y + (float)npc.height > vector17.Y && npc.position.Y < vector17.Y + 16f)
                            {
                                flag18 = true;
                                if (Main.rand.NextBool(100) && npc.behindTiles && Main.tile[num184, num185].nactive())
                                {
                                    WorldGen.KillTile(num184, num185, true, true, false);
                                }
                                if (Main.netMode != NetmodeID.MultiplayerClient && Main.tile[num184, num185].type == 2)
                                {
                                    ushort arg_BFCA_0 = Main.tile[num184, num185 - 1].type;
                                }
                            }
                        }
                    }
                }
            }
            if (!flag18 && head)
            {
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int num186 = 1000;
                bool flag19 = true;
                for (int num187 = 0; num187 < 255; num187++)
                {
                    if (Main.player[num187].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[num187].position.X - num186, (int)Main.player[num187].position.Y - num186, num186 * 2, num186 * 2);
                        if (rectangle.Intersects(rectangle2))
                        {
                            flag19 = false;
                            break;
                        }
                    }
                }
                if (flag19)
                {
                    flag18 = true;
                }
            }
            if (directional)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.spriteDirection = 1;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.spriteDirection = -1;
                }
            }
            float num188 = speed;
            float num189 = turnSpeed;
            Vector2 vector18 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num191 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
            float num192 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
            num191 = (float)((int)(num191 / 16f) * 16);
            num192 = (float)((int)(num192 / 16f) * 16);
            vector18.X = (float)((int)(vector18.X / 16f) * 16);
            vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)Math.Sqrt((double)(num191 * num191 + num192 * num192));
            if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
            {
                try
                {
                    vector18 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    num191 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector18.X;
                    num192 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector18.Y;
                }
                catch
                {
                }
                npc.rotation = (float)Math.Atan2((double)num192, (double)num191) + 1.57f;
                num193 = (float)Math.Sqrt((double)(num191 * num191 + num192 * num192));
                int num194 = npc.width;
                num193 = (num193 - (float)num194) / num193;
                num191 *= num193;
                num192 *= num193;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + num191;
                npc.position.Y = npc.position.Y + num192;
                if (directional)
                {
                    if (num191 < 0f)
                    {
                        npc.spriteDirection = 1;
                    }
                    if (num191 > 0f)
                    {
                        npc.spriteDirection = -1;
                    }
                }
            }
            else
            {
                if (!flag18)
                {
                    npc.TargetClosest(true);
                    npc.velocity.Y = npc.velocity.Y + 0.11f;
                    if (npc.velocity.Y > num188)
                    {
                        npc.velocity.Y = num188;
                    }
                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num188 * 0.4)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X - num189 * 1.1f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X + num189 * 1.1f;
                        }
                    }
                    else if (npc.velocity.Y == num188)
                    {
                        if (npc.velocity.X < num191)
                        {
                            npc.velocity.X = npc.velocity.X + num189;
                        }
                        else if (npc.velocity.X > num191)
                        {
                            npc.velocity.X = npc.velocity.X - num189;
                        }
                    }
                    else if (npc.velocity.Y > 4f)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num189 * 0.9f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X - num189 * 0.9f;
                        }
                    }
                }
                else
                {
                    if (!flies && npc.behindTiles && npc.soundDelay == 0)
                    {
                        float num195 = num193 / 40f;
                        if (num195 < 10f)
                        {
                            num195 = 10f;
                        }
                        if (num195 > 20f)
                        {
                            num195 = 20f;
                        }
                        npc.soundDelay = (int)num195;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, npc.position, 1);
                    }
                    num193 = (float)Math.Sqrt((double)(num191 * num191 + num192 * num192));
                    float num196 = Math.Abs(num191);
                    float num197 = Math.Abs(num192);
                    float num198 = num188 / num193;
                    num191 *= num198;
                    num192 *= num198;
                    if (ShouldRun())
                    {
                        bool flag20 = true;
                        for (int num199 = 0; num199 < 255; num199++)
                        {
                            if (Main.player[num199].active && !Main.player[num199].dead && Main.player[num199].ZoneCorrupt)
                            {
                                flag20 = false;
                            }
                        }
                        if (flag20)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient && (double)(npc.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
                            {
                                npc.active = false;
                                int num200 = (int)npc.ai[0];
                                while (num200 > 0 && num200 < 200 && Main.npc[num200].active && Main.npc[num200].aiStyle == npc.aiStyle)
                                {
                                    int num201 = (int)Main.npc[num200].ai[0];
                                    Main.npc[num200].active = false;
                                    npc.life = 0;
                                    if (Main.netMode == NetmodeID.Server)
                                    {
                                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num200, 0f, 0f, 0f, 0, 0, 0);
                                    }
                                    num200 = num201;
                                }
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
                            num191 = 0f;
                            num192 = num188;
                        }
                    }
                    bool flag21 = false;
                    if (npc.type == NPCID.WyvernHead)
                    {
                        if ((npc.velocity.X > 0f && num191 < 0f || npc.velocity.X < 0f && num191 > 0f || npc.velocity.Y > 0f && num192 < 0f || npc.velocity.Y < 0f && num192 > 0f) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num189 / 2f && num193 < 300f)
                        {
                            flag21 = true;
                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num188)
                            {
                                npc.velocity *= 1.1f;
                            }
                        }
                        if (npc.position.Y > Main.player[npc.target].position.Y || (double)(Main.player[npc.target].position.Y / 16f) > Main.worldSurface || Main.player[npc.target].dead)
                        {
                            flag21 = true;
                            if (Math.Abs(npc.velocity.X) < num188 / 2f)
                            {
                                if (npc.velocity.X == 0f)
                                {
                                    npc.velocity.X = npc.velocity.X - (float)npc.direction;
                                }
                                npc.velocity.X = npc.velocity.X * 1.1f;
                            }
                            else
                            {
                                if (npc.velocity.Y > -num188)
                                {
                                    npc.velocity.Y = npc.velocity.Y - num189;
                                }
                            }
                        }
                    }
                    if (!flag21)
                    {
                        if (npc.velocity.X > 0f && num191 > 0f || npc.velocity.X < 0f && num191 < 0f || npc.velocity.Y > 0f && num192 > 0f || npc.velocity.Y < 0f && num192 < 0f)
                        {
                            if (npc.velocity.X < num191)
                            {
                                npc.velocity.X = npc.velocity.X + num189;
                            }
                            else
                            {
                                if (npc.velocity.X > num191)
                                {
                                    npc.velocity.X = npc.velocity.X - num189;
                                }
                            }
                            if (npc.velocity.Y < num192)
                            {
                                npc.velocity.Y = npc.velocity.Y + num189;
                            }
                            else
                            {
                                if (npc.velocity.Y > num192)
                                {
                                    npc.velocity.Y = npc.velocity.Y - num189;
                                }
                            }
                            if ((double)Math.Abs(num192) < (double)num188 * 0.2 && (npc.velocity.X > 0f && num191 < 0f || npc.velocity.X < 0f && num191 > 0f))
                            {
                                if (npc.velocity.Y > 0f)
                                {
                                    npc.velocity.Y = npc.velocity.Y + num189 * 2f;
                                }
                                else
                                {
                                    npc.velocity.Y = npc.velocity.Y - num189 * 2f;
                                }
                            }
                            if ((double)Math.Abs(num191) < (double)num188 * 0.2 && (npc.velocity.Y > 0f && num192 < 0f || npc.velocity.Y < 0f && num192 > 0f))
                            {
                                if (npc.velocity.X > 0f)
                                {
                                    npc.velocity.X = npc.velocity.X + num189 * 2f;
                                }
                                else
                                {
                                    npc.velocity.X = npc.velocity.X - num189 * 2f;
                                }
                            }
                        }
                        else
                        {
                            if (num196 > num197)
                            {
                                if (npc.velocity.X < num191)
                                {
                                    npc.velocity.X = npc.velocity.X + num189 * 1.1f;
                                }
                                else if (npc.velocity.X > num191)
                                {
                                    npc.velocity.X = npc.velocity.X - num189 * 1.1f;
                                }
                                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num188 * 0.5)
                                {
                                    if (npc.velocity.Y > 0f)
                                    {
                                        npc.velocity.Y = npc.velocity.Y + num189;
                                    }
                                    else
                                    {
                                        npc.velocity.Y = npc.velocity.Y - num189;
                                    }
                                }
                            }
                            else
                            {
                                if (npc.velocity.Y < num192)
                                {
                                    npc.velocity.Y = npc.velocity.Y + num189 * 1.1f;
                                }
                                else if (npc.velocity.Y > num192)
                                {
                                    npc.velocity.Y = npc.velocity.Y - num189 * 1.1f;
                                }
                                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num188 * 0.5)
                                {
                                    if (npc.velocity.X > 0f)
                                    {
                                        npc.velocity.X = npc.velocity.X + num189;
                                    }
                                    else
                                    {
                                        npc.velocity.X = npc.velocity.X - num189;
                                    }
                                }
                            }
                        }
                    }
                }
                npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
                if (head)
                {
                    if (flag18)
                    {
                        if (npc.localAI[0] != 1f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.localAI[0] = 1f;
                    }
                    else
                    {
                        if (npc.localAI[0] != 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.localAI[0] = 0f;
                    }
                    if ((npc.velocity.X > 0f && npc.oldVelocity.X < 0f || npc.velocity.X < 0f && npc.oldVelocity.X > 0f || npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f || npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f) && !npc.justHit)
                    {
                        npc.netUpdate = true;
                        return;
                    }
                }
            }
            if (splitting)
            {
                npc.timeLeft = Main.npc[(int)npc.ai[3]].timeLeft;
                npc.realLife = -1;
            }
            CustomBehavior();
        }

        public override void NPCLoot()
        {
            if (splitting)
            {
                bool flag = true;
                for (int j = 0; j < 200; j++)
                {
                    if (j != npc.whoAmI && Main.npc[j].active)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    SplittingLooting();
                    DeadLooting();
                }
                else
                {
                    DeadLooting();
                }
            }
        }

        public virtual void Init()
        {
        }

        public virtual bool ShouldRun()
        {
            return false;
        }

        public virtual void CustomBehavior()
        {
        }

        public virtual void DeadLooting()
        {
        }

        public virtual void SplittingLooting()
        {
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return !splitting ? (head ? (bool?)null : false) : (bool?)null;
        }
        //internal class TestSplittingWormHead : TestSplittingWorm
        //{
        //    public override string Texture => "Terraria/NPC_" + NPCID.DiggerHead;

        //    public override void SetDefaults()
        //    {
        //        // Head is 10 defence, body 20, tail 30.
        //        npc.CloneDefaults(NPCID.DiggerHead);
        //        npc.lifeMax = 100;
        //        npc.aiStyle = -1;
        //        npc.color = Color.Aqua;
        //        npc.scale = 1.5f;
        //    }

        //    public override void Init()
        //    {
        //        base.Init();
        //        head = true;
        //    }

        //    private int attackCounter;
        //    public override void SendExtraAI(BinaryWriter writer)
        //    {
        //        writer.Write(attackCounter);
        //    }

        //    public override void ReceiveExtraAI(BinaryReader reader)
        //    {
        //        attackCounter = reader.ReadInt32();
        //    }

        //    public override void CustomBehavior()
        //    {
        //        //if (Main.netMode != 1)
        //        //{
        //        //    if (attackCounter > 0)
        //        //    {
        //        //        attackCounter--;
        //        //    }

        //        //    Player target = Main.player[npc.target];
        //        //    if (attackCounter <= 0 && Vector2.Distance(npc.Center, target.Center) < 200 && Collision.CanHit(npc.Center, 1, 1, target.Center, 1, 1))
        //        //    {
        //        //        Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX);
        //        //        direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

        //        //        int projectile = Projectile.NewProjectile(npc.Center, direction * 1, ProjectileID.ShadowBeamHostile, 5, 0, Main.myPlayer);
        //        //        Main.projectile[projectile].timeLeft = 300;
        //        //        attackCounter = 500;
        //        //        npc.netUpdate = true;
        //        //    }
        //        //}
        //    }
        //}

        //internal class TestSplittingWormBody : TestSplittingWorm
        //{
        //    public override string Texture => "Terraria/NPC_" + NPCID.DiggerBody;

        //    public override void SetDefaults()
        //    {
        //        npc.CloneDefaults(NPCID.DiggerBody);
        //        npc.lifeMax = 200;
        //        npc.aiStyle = -1;
        //        npc.color = Color.Aqua;
        //        npc.scale = 1.5f;
        //    }
        //}

        //internal class TestSplittingWormTail : TestSplittingWorm
        //{
        //    public override string Texture => "Terraria/NPC_" + NPCID.DiggerTail;

        //    public override void SetDefaults()
        //    {
        //        npc.CloneDefaults(NPCID.DiggerTail);
        //        npc.lifeMax = 300;
        //        npc.aiStyle = -1;
        //        npc.color = Color.Aqua;
        //        npc.scale = 1.5f;
        //    }

        //    public override void Init()
        //    {
        //        base.Init();
        //        tail = true;
        //    }
        //}

        //// I made this 2nd base class to limit code repetition.
        //public abstract class TestSplittingWorm : Worm
        //{
        //    public override void SetStaticDefaults()
        //    {
        //        DisplayName.SetDefault("Test Splitting Worm");
        //    }
        //    public override void Init()
        //    {
        //        minLength = 20;
        //        maxLength = 30;
        //        tailType = NPCType<TestSplittingWormTail>();
        //        bodyType = NPCType<TestSplittingWormBody>();
        //        headType = NPCType<TestSplittingWormHead>();
        //        speed = 12.5f;
        //        turnSpeed = 0.095f;
        //        splitting = true;
        //    }
        //}
    }
}
