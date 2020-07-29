using System;
using System.Runtime.InteropServices;

using Entrogic.Items.Equipables.Accessories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class Ooze : FSM_NPC
    {
        private int NowFrame;
        public override void AI()
        {
            npc.noTileCollide = false;
            npc.spriteDirection = npc.direction;
            Copied_AI_003_Fighters_Type_430();
        }
        public override void FindFrame(int frameHeight) // 显示不需要服务器同步，毕竟也就那几个动作循环
        {
			if (Main.dedServ)
				return;
            if (npc.ai[2] == 0f) // 移动模式
			{
				npc.frameCounter++;
				if (npc.frameCounter >= 6)
                {
                    NowFrame++;
                    npc.frameCounter = 0;
                }
				if (NowFrame >= 4) // 第四帧后面没有了，需要重置
                {
                    NowFrame = 0;
                }
                npc.frame.Y = NowFrame * frameHeight;
            }
            else // 攻击模式
			{
                npc.frameCounter++;
				if (npc.frameCounter >= 6)
				{
                    NowFrame++;
                    npc.frameCounter = 0;
				}
				if (NowFrame >= 6) // 第七帧后面没有了，需要重置
				{
					NowFrame = 0;
                }
                npc.frame.Y = NowFrame * frameHeight + 4 * frameHeight;
            }
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 11;
            // 学习NPC430挥手僵尸
            On.Terraria.NPC.GetMeleeCollisionData += NPC_GetMeleeCollisionData;
            On.Terraria.Player.UpdateEquips += Player_UpdateEquips;
        }

        private void Player_UpdateEquips(On.Terraria.Player.orig_UpdateEquips orig, Player self, int i)
        {
            orig(self, i);
            for (int l = 3; l < 10; l++)
            {
                Item currentItem = self.armor[l];
                if (currentItem.expertOnly && !Main.expertMode)
                {
                    return;
                }
                if (currentItem.type == ItemID.RoyalGel)
                {
                    self.npcTypeNoAggro[npc.type] = true;
                }
            }
        }

        private void NPC_GetMeleeCollisionData(On.Terraria.NPC.orig_GetMeleeCollisionData orig, Rectangle victimHitbox, int enemyIndex, ref int specialHitSetter, ref float damageMultiplier, ref Rectangle npcRect)
        {
            orig(victimHitbox, enemyIndex, ref specialHitSetter, ref damageMultiplier, ref npcRect);
            NPC npc = Main.npc[enemyIndex];
            if (npc.ai[2] > 18f)
            {
                int num = 30;
                if (npc.spriteDirection < 0)
                {
                    npcRect.X -= num;
                    npcRect.Width += num;
                }
                else
                {
                    npcRect.Width += num;
                }
                damageMultiplier *= 2f;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.width = 86;
            npc.height = 46;
            npc.damage = 20;
            npc.defense = 6;
            npc.lifeMax = 62;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 4, 0);
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            npc.noTileCollide = false;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(10))
            {
                Item.NewItem(npc.getRect(), ItemType<EarthElementalAffinityAgent>());
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (ModHelper.NormalSpawn(spawnInfo) && player.ZoneDirtLayerHeight)
            {
                return 0.05f;
            }
            return base.SpawnChance(spawnInfo);
        }

        private void Copied_AI_003_Fighters_Type_430()
        {
            if (Main.player[npc.target].position.Y + (float)Main.player[npc.target].height == npc.position.Y + (float)npc.height)
            {
                npc.directionY = -1;
            }
            bool flag = false;
            bool flag5 = false;
            bool flag6 = false;
            if (npc.velocity.X == 0f)
            {
                flag6 = true;
            }
            if (npc.justHit)
            {
                flag6 = false;
            }
            int num52 = 60;
            bool flag7 = false;
            bool flag8 = false;
            bool flag9 = false;
            bool flag10 = true;
            if (!flag9 && flag10)
            {
                if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
                {
                    flag7 = true;
                }
                if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num52 || flag7)
                {
                    npc.ai[3] += 1f;
                }
                else if ((double)Math.Abs(npc.velocity.X) > 0.9 && npc.ai[3] > 0f)
                {
                    npc.ai[3] -= 1f;
                }
                if (npc.ai[3] > (float)(num52 * 10))
                {
                    npc.ai[3] = 0f;
                }
                if (npc.justHit)
                {
                    npc.ai[3] = 0f;
                }
                if (npc.ai[3] == (float)num52)
                {
					npc.velocity.X = -npc.velocity.X;
					npc.direction = -npc.direction;
                    npc.netUpdate = true;
                }
                if (Main.player[npc.target].Hitbox.Intersects(npc.Hitbox))
                {
                    npc.ai[3] = 0f;
                }
                if (npc.ai[3] <= 2f || npc.ai[3] >= 300f)
                {
					npc.TargetClosest();
                }
            }
            if (npc.ai[2] == 0f)
            {
                npc.damage = npc.defDamage;
                float num95 = 1f;
                num95 *= 2f + (1f - npc.scale);
                if (npc.velocity.X < -num95 || npc.velocity.X > num95)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < num95 && npc.direction == 1)
                {
                    npc.velocity.X = npc.velocity.X + 0.07f;
                    if (npc.velocity.X > num95)
                    {
                        npc.velocity.X = num95;
                    }
                }
                else if (npc.velocity.X > -num95 && npc.direction == -1)
                {
                    npc.velocity.X = npc.velocity.X - 0.07f;
                    if (npc.velocity.X < -num95)
                    {
                        npc.velocity.X = -num95;
                    }
                }
                if (npc.velocity.Y == 0f && !Main.player[npc.target].dead && !Main.player[npc.target].npcTypeNoAggro[npc.type])
                {
                    Vector2 vector15 = npc.Center - Main.player[npc.target].Center;
					int num96 = 32 + (int)(npc.width * 0.5f); // NPC宽度外的2格都属于搜索范围
                    if (vector15.Length() < (float)num96 && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1)) // 如果他能打到玩家
                    {
                        npc.velocity.X = npc.velocity.X * 0.7f;
                        npc.ai[2] = 1f; // 将ai[2]设为非0数，即开启攻击模式
                        npc.frameCounter = 0;
                    }
                }
            }
            else // 攻击模式
            {
                npc.damage = (int)((double)npc.defDamage * 1.5);
                npc.ai[3] = 1f;
                npc.velocity.X = npc.velocity.X * 0.9f;
                if ((double)Math.Abs(npc.velocity.X) < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                npc.ai[2] += 1f;
                if (npc.ai[2] >= 20f || npc.velocity.Y != 0f)
                {
                    npc.ai[2] = 0f;
                    npc.frameCounter = 0;
                }
            }
            if (npc.velocity.Y == 0f || flag)
            {
                int num163 = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
                int num164 = (int)(npc.position.Y - 9f) / 16;
                int num165 = (int)npc.position.X / 16;
                int num166 = (int)(npc.position.X + (float)npc.width) / 16;
                int num167 = (int)(npc.position.X + 8f) / 16;
                int num168 = (int)(npc.position.X + (float)npc.width - 8f) / 16;
                bool flag20 = false;
                for (int num169 = num167; num169 <= num168; num169++)
                {
                    if (num169 >= num165 && num169 <= num166 && Main.tile[num169, num163] == null)
                    {
                        flag20 = true;
                    }
                    else
                    {
                        if (Main.tile[num169, num164] != null && Main.tile[num169, num164].nactive() && Main.tileSolid[(int)Main.tile[num169, num164].type])
                        {
                            flag5 = false;
                            break;
                        }
                        if (!flag20 && num169 >= num165 && num169 <= num166 && Main.tile[num169, num163].nactive() && Main.tileSolid[(int)Main.tile[num169, num163].type])
                        {
                            flag5 = true;
                        }
                    }
                }
                if (!flag5 && npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = 0f;
                }
                if (flag20)
                {
                    return;
                }
			}
			if (npc.velocity.Y >= 0f)
			{
				int num170 = 0;
				if (npc.velocity.X < 0f)
				{
					num170 = -1;
				}
				if (npc.velocity.X > 0f)
				{
					num170 = 1;
				}
				Vector2 position3 = npc.position;
				position3.X += npc.velocity.X;
				int num171 = (int)((position3.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num170)) / 16f);
				int num172 = (int)((position3.Y + (float)npc.height - 1f) / 16f);
				if (WorldGen.InWorld(num171, num172, 4))
				{
					if (Main.tile[num171, num172] == null)
					{
						Main.tile[num171, num172] = new Tile();
					}
					if (Main.tile[num171, num172 - 1] == null)
					{
						Main.tile[num171, num172 - 1] = new Tile();
					}
					if (Main.tile[num171, num172 - 2] == null)
					{
						Main.tile[num171, num172 - 2] = new Tile();
					}
					if (Main.tile[num171, num172 - 3] == null)
					{
						Main.tile[num171, num172 - 3] = new Tile();
					}
					if (Main.tile[num171, num172 + 1] == null)
					{
						Main.tile[num171, num172 + 1] = new Tile();
					}
					if (Main.tile[num171 - num170, num172 - 3] == null)
					{
						Main.tile[num171 - num170, num172 - 3] = new Tile();
					}
					if ((float)(num171 * 16) < position3.X + (float)npc.width && (float)(num171 * 16 + 16) > position3.X && ((Main.tile[num171, num172].nactive() && !Main.tile[num171, num172].topSlope() && !Main.tile[num171, num172 - 1].topSlope() && Main.tileSolid[(int)Main.tile[num171, num172].type] && !Main.tileSolidTop[(int)Main.tile[num171, num172].type]) || (Main.tile[num171, num172 - 1].halfBrick() && Main.tile[num171, num172 - 1].nactive())) && (!Main.tile[num171, num172 - 1].nactive() || !Main.tileSolid[(int)Main.tile[num171, num172 - 1].type] || Main.tileSolidTop[(int)Main.tile[num171, num172 - 1].type] || (Main.tile[num171, num172 - 1].halfBrick() && (!Main.tile[num171, num172 - 4].nactive() || !Main.tileSolid[(int)Main.tile[num171, num172 - 4].type] || Main.tileSolidTop[(int)Main.tile[num171, num172 - 4].type]))) && (!Main.tile[num171, num172 - 2].nactive() || !Main.tileSolid[(int)Main.tile[num171, num172 - 2].type] || Main.tileSolidTop[(int)Main.tile[num171, num172 - 2].type]) && (!Main.tile[num171, num172 - 3].nactive() || !Main.tileSolid[(int)Main.tile[num171, num172 - 3].type] || Main.tileSolidTop[(int)Main.tile[num171, num172 - 3].type]) && (!Main.tile[num171 - num170, num172 - 3].nactive() || !Main.tileSolid[(int)Main.tile[num171 - num170, num172 - 3].type]))
					{
						float num173 = (float)(num172 * 16);
						if (Main.tile[num171, num172].halfBrick())
						{
							num173 += 8f;
						}
						if (Main.tile[num171, num172 - 1].halfBrick())
						{
							num173 -= 8f;
						}
						if (num173 < position3.Y + (float)npc.height)
						{
							float num174 = position3.Y + (float)npc.height - num173;
							float num175 = 16.1f;
							if (num174 <= num175)
							{
								npc.gfxOffY += npc.position.Y + (float)npc.height - num173;
								npc.position.Y = num173 - (float)npc.height;
								if (num174 < 9f)
								{
									npc.stepSpeed = 2f;
								}
								else
								{
									npc.stepSpeed = 4f;
								}
							}
						}
					}
				}
			}
			if (flag5)
			{
				int num176 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
				int num177 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
				if (Main.tile[num176, num177] == null)
				{
					Main.tile[num176, num177] = new Tile();
				}
				if (Main.tile[num176, num177 - 1] == null)
				{
					Main.tile[num176, num177 - 1] = new Tile();
				}
				if (Main.tile[num176, num177 - 2] == null)
				{
					Main.tile[num176, num177 - 2] = new Tile();
				}
				if (Main.tile[num176, num177 - 3] == null)
				{
					Main.tile[num176, num177 - 3] = new Tile();
				}
				if (Main.tile[num176, num177 + 1] == null)
				{
					Main.tile[num176, num177 + 1] = new Tile();
				}
				if (Main.tile[num176 + npc.direction, num177 - 1] == null)
				{
					Main.tile[num176 + npc.direction, num177 - 1] = new Tile();
				}
				if (Main.tile[num176 + npc.direction, num177 + 1] == null)
				{
					Main.tile[num176 + npc.direction, num177 + 1] = new Tile();
				}
				if (Main.tile[num176 - npc.direction, num177 + 1] == null)
				{
					Main.tile[num176 - npc.direction, num177 + 1] = new Tile();
				}
				Main.tile[num176, num177 + 1].halfBrick();
				if (Main.tile[num176, num177 - 1].nactive() && (Main.tile[num176, num177 - 1].type == 10 || Main.tile[num176, num177 - 1].type == 388) && flag8)
				{
					npc.ai[2] += 1f;
					npc.ai[3] = 0f;
					if (npc.ai[2] >= 60f)
					{
						npc.velocity.X = 0.5f * (float)-(float)npc.direction;
						int num178 = 5;
						if (Main.tile[num176, num177 - 1].type == 388)
						{
							num178 = 2;
						}
                        npc.ai[1] += (float)num178;
						npc.ai[2] = 0f;
						bool flag23 = false;
						if (npc.ai[1] >= 10f)
						{
							flag23 = true;
							npc.ai[1] = 10f;
						}
						WorldGen.KillTile(num176, num177 - 1, true, false, false);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag23) && flag23 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (Main.tile[num176, num177 - 1].type == 10)
							{
								bool flag24 = WorldGen.OpenDoor(num176, num177 - 1, npc.direction);
								if (!flag24)
								{
									npc.ai[3] = (float)num52;
									npc.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag24)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, (float)num176, (float)(num177 - 1), (float)npc.direction, 0, 0, 0);
								}
							}
							if (Main.tile[num176, num177 - 1].type == 388)
							{
								npc.ai[3] = (float)num52;
								npc.netUpdate = true;
							}
						}
					}
				}
                // 跳跃功能为僵尸的1/3
                else
                {
                    int num179 = npc.spriteDirection;
                    if ((npc.velocity.X < 0f && num179 == -1) || (npc.velocity.X > 0f && num179 == 1))
                    {
                        if (npc.height >= 32 && Main.tile[num176, num177 - 2].nactive() && Main.tileSolid[(int)Main.tile[num176, num177 - 2].type])
                        {
                            if (Main.tile[num176, num177 - 3].nactive() && Main.tileSolid[(int)Main.tile[num176, num177 - 3].type])
                            {
                                npc.velocity.Y = -8f / 2.5f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7f / 2.5f;
                                npc.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num176, num177 - 1].nactive() && Main.tileSolid[(int)Main.tile[num176, num177 - 1].type])
                        {
                            npc.velocity.Y = -6f / 2.5f;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(num177 * 16) > 20f && Main.tile[num176, num177].nactive() && !Main.tile[num176, num177].topSlope() && Main.tileSolid[(int)Main.tile[num176, num177].type])
                        {
                            npc.velocity.Y = -5f / 2.5f;
                            npc.netUpdate = true;
                        }
                        else if (npc.directionY < 0 && (!Main.tile[num176, num177 + 1].nactive() || !Main.tileSolid[(int)Main.tile[num176, num177 + 1].type]) && (!Main.tile[num176 + npc.direction, num177 + 1].nactive() || !Main.tileSolid[(int)Main.tile[num176 + npc.direction, num177 + 1].type]))
                        {
                            npc.velocity.Y = -8f / 2.5f;
                            npc.velocity.X = npc.velocity.X * 1.5f;
                            npc.netUpdate = true;
                        }
                        else if (flag8)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                        }
                        if (npc.velocity.Y == 0f && flag6 && npc.ai[3] == 1f)
                        {
                            npc.velocity.Y = -5f / 2.5f;
                        }
                        if (npc.velocity.Y == 0f && Main.player[npc.target].Bottom.Y < npc.Top.Y && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < (float)(Main.player[npc.target].width * 3) && ModHelper.CanHit(npc, Main.player[npc.target]))
                        {
                            if (npc.velocity.Y == 0f)
                            {
                                int num182 = 6;
                                if (Main.player[npc.target].Bottom.Y > npc.Top.Y - (float)(num182 * 16))
                                {
                                    npc.velocity.Y = -7.9f / 2.5f;
                                }
                                else
                                {
                                    int num183 = (int)(npc.Center.X / 16f);
                                    int num184 = (int)(npc.Bottom.Y / 16f) - 1;
                                    for (int num185 = num184; num185 > num184 - num182; num185--)
                                    {
                                        if (Main.tile[num183, num185].nactive() && TileID.Sets.Platforms[(int)Main.tile[num183, num185].type])
                                        {
                                            npc.velocity.Y = -7.9f / 2.5f;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        // NPC下平台
                        npc.TryDownstairs();
                    }
                }
            }
			else if (flag8)
			{
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
            }
        }
	}
}
