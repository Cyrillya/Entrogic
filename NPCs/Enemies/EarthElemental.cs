using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Entrogic.Projectiles.Enemies;
using Entrogic.Items.Equipables.Accessories;

namespace Entrogic.NPCs.Enemies
{
    public class EarthElemental : FSM_NPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 16;
        }

        enum NPCState
        {
            Normal,
            Attack,
            Teleport,
            Dead,
            PlayerDead
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.spriteDirection = -npc.direction;
            if (State != (int)NPCState.Dead && State != (int)NPCState.PlayerDead)
            {
                npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * 1.8f;
            }
            if (!player.active || player.dead)
            {
                npc.TargetClosest();
                if (!player.active || player.dead)
                {
                    SwitchState((int)NPCState.PlayerDead);
                }
            }
            switch ((NPCState)State)
            {
                // 正常状态下
                case NPCState.Normal:
                    {
                        npc.TargetClosest();
                        Timer++;
                        if (Timer >= 60 && player.active)
                        {
                            if (npc.Distance(player.Center) >= 40f * 16f)
                            {
                                SwitchState((int)NPCState.Teleport);
                            }
                            else if (Collision.CanHit(npc.Center, npc.width, npc.height, player.Center, player.width, player.height))
                            {
                                SwitchState((int)NPCState.Attack);
                            }
                        }
                        break;
                    }
                // 攻击状态下
                case NPCState.Attack:
                    {
                        // 帧数=计时器/4
                        Timer++;
                        // 第九帧攻击 (也就是此阶段第32帧)
                        if (Timer == 32 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            // 来点二分法
                            Vector2 shootPos = new Vector2(npc.Center.X, npc.Center.Y + npc.height / 2f - 30f);
                            // min, max和mid都表示射击角度
                            float min = -MathHelper.Pi, max = MathHelper.Pi;
                            float mid = (min + max) * 0.5f;
                            while (Math.Abs(max - min) > 0.005f)
                            {
                                Vector2 dirtProj = shootPos;
                                Vector2 plrMovement = player.Center;
                                mid = (min + max) * 0.5f;
                                float beyond = 0f;
                                Vector2 velo = mid.ToRotationVector2() * 15f;
                                for (int i = 0; i < 1000; i++)
                                {
                                    dirtProj += velo;
                                    plrMovement += player.velocity;
                                    if (npc.Distance(dirtProj) > npc.Distance(plrMovement))
                                    {
                                        beyond = (dirtProj - npc.Center).ToRotation() - (plrMovement - npc.Center).ToRotation();
                                        break;
                                    }
                                }
                                if (beyond > 0f)
                                {
                                    max = mid;
                                }
                                else
                                {
                                    min = mid;
                                }
                            }
                            Projectile.NewProjectile(shootPos, mid.ToRotationVector2() * 15f, ProjectileType<DirtHostile>(), npc.damage / 2, 1f, Main.myPlayer);
                            npc.netUpdate = true;
                        }
                        // 第12帧结束 (4*11=44)
                        if (Timer >= 44)
                        {
                            SwitchState((int)NPCState.Normal);
                        }
                        break;
                    }
                // 传送状态下
                case NPCState.Teleport:
                    {
                        // 随机传送到以玩家为中心的六十格的矩形内任意松软类物块
                        Timer++;
                        // 帧数=计时器/4
                        // 第16帧传送 (4*15=60)
                        if (Timer == 60)
                        {
                            npc.TargetClosest();
                            if (player.active)
                            {
                                List<Point> findTile = new List<Point>();
                                for (int i = -30; i <= 30; i++)
                                {
                                    for (int j = -30; j <= 30; j++)
                                    {
                                        Point pos = new Point(i + (int)(player.Center.X / 16f), j + (int)(player.Center.Y / 16f));
                                        if (WorldGen.InWorld(pos.X, pos.Y) && Entrogic.TileDirt(Main.tile[pos.X, pos.Y]))
                                        {
                                            bool rsafe = true;
                                            for (int k = -1; k <= 1; k++)
                                            {
                                                for (int l = -1; l >= -3; l--)
                                                {
                                                    Point posSafe = new Point(k + pos.X, l + pos.Y);
                                                    if (WorldGen.InWorld(posSafe.X, posSafe.Y) && Main.tile[posSafe.X, posSafe.Y].active() && Main.tileSolid[Main.tile[posSafe.X, posSafe.Y].type])
                                                    {
                                                        rsafe = false;
                                                    }
                                                }
                                            }
                                            if (rsafe)
                                            {
                                                findTile.Add(pos);
                                            }
                                        }
                                    }
                                }
                                if (findTile.Count > 0)
                                {
                                    int random = Main.rand.Next(0, findTile.Count);
                                    Point randomTile = findTile[random];
                                    npc.position = randomTile.ToWorldCoordinates(8 - npc.width * 0.5f, -npc.height);
                                }
                            }
                        }
                        // 传送之后倒放帧图，倒放完了返回普通模式
                        if (Timer == 120)
                        {
                            SwitchState((int)NPCState.Normal);
                        }
                        break;
                    }
                // 死亡状态下
                case NPCState.Dead:
                    {
                        drawOffsetY = 0;
                        npc.height = 138;
                        npc.velocity.X *= 0.88f;
                        npc.noGravity = false;
                        npc.noTileCollide = false;
                        // 不断下降直到碰到物块
                        // 第9帧最后落下动画 (6*8=48)
                        if (Timer <= 48)
                        {
                            Timer++;
                        }
                        else if (npc.position.Y == npc.oldPosition.Y)
                        {
                            // 帧数=计时器/6
                            Timer++;
                        }
                        // 第13帧最后动画 (6*12=72)
                        if (Timer >= 72)
                        {
                            Timer = 72;
                            npc.ai[2]++;
                            npc.alpha = (int)npc.ai[2] * 3;
                            if (npc.ai[2] >= 90)
                            {
                                npc.life = 0;
                                npc.HitEffect(0, 0);
                                npc.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
                            }
                        }
                        npc.netUpdate = true;
                        break;
                    }
                // 玩家死亡状态下
                case NPCState.PlayerDead:
                    {
                        npc.noGravity = false;
                        npc.noTileCollide = true;
                        npc.TargetClosest();
                        if (player.active && !player.dead)
                        {
                            SwitchState((int)NPCState.Normal);
                        }
                        Timer++;
                        if (Timer >= 130)
                        {
                            npc.life = 0;
                            npc.active = false;
                            npc.netUpdate = true;
                        }
                        break;
                    }
            }
        }

        private void FindFrame(int frameHeight, int frameWidth)
        {
            switch ((NPCState)State)
            {
                // 正常状态下
                case NPCState.Normal:
                    {
                        npc.frame.X = 0;
                        npc.frameCounter++;
                        if (npc.frameCounter >= 6)
                        {
                            npc.frame.Y += frameHeight;
                            npc.frameCounter = 0;
                        }
                        if (npc.frame.Y >= frameHeight * 8)
                        {
                            npc.frame.Y = 0;
                        }
                        break;
                    }
                // 攻击状态下
                case NPCState.Attack:
                    {
                        npc.frame.X = frameWidth;
                        npc.frame.Y = Timer / 4 * frameHeight;
                        break;
                    }
                // 传送状态下
                case NPCState.Teleport:
                    {
                        npc.frame.X = frameWidth * 2;
                        if (Timer <= 60) {
                            npc.frame.Y = Timer / 4 * frameHeight;
                        }
                        else
                        {
                            npc.frame.Y = (15 - (Timer - 60) / 4) * frameHeight;
                        }
                        break;
                    }
                // 死亡状态下
                case NPCState.Dead:
                    {
                        npc.frame.X = frameWidth * 3;
                        npc.frame.Y = Timer / 6 * frameHeight;
                        break;
                    }
                // 玩家死亡状态下
                case NPCState.PlayerDead:
                    {
                        npc.frame.X = 0;
                        npc.frameCounter++;
                        if (npc.frameCounter >= 6)
                        {
                            npc.frame.Y += frameHeight;
                            npc.frameCounter = 0;
                        }
                        if (npc.frame.Y >= frameHeight * 8)
                        {
                            npc.frame.Y = 0;
                        }
                        break;
                    }
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(4)) {
                Item.NewItem(npc.getRect(), ItemType<EarthElementalAffinityAgent>());
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (ModHelper.NormalSpawn(spawnInfo) && player.ZoneOverworldHeight)
            {
                return 0.005f;
            }
            return base.SpawnChance(spawnInfo);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Main.npcTexture[npc.type] = Entrogic.Instance.GetTexture("NPCs/Enemies/EarthElemental_" + tex);
            Texture2D tex = Entrogic.Instance.GetTexture("NPCs/Enemies/EarthElemental");
            int npcFrameCountX = 4;
            FindFrame(tex.Height / Main.npcFrameCount[npc.type], tex.Width / npcFrameCountX);
            npc.frame.Width = tex.Width / npcFrameCountX;
            spriteBatch.Draw(tex, npc.position - Main.screenPosition + new Vector2(-npc.frame.Width / npcFrameCountX, drawOffsetY), new Rectangle?(npc.frame), drawColor, npc.rotation, Vector2.Zero, 1f, npc.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            //SpriteEffects effects = SpriteEffects.None;
            //if (npc.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            //spriteBatch.Draw(tex, npc.position, npc.frame, npc.GetAlpha(drawColor), npc.rotation, Vector2.Zero, new Vector2(npc.scale), effects, 0f);
            return false;
        }

        // We use CheckDead to delay death providing time for our death drama to happen. The logic here is a bit complicated, if you are curious, please step through AI and CheckDead in Visual Studio to see how CheckDead prevents death the first time but allows it after the death drama has finished.
        public override bool CheckDead()
        {
            if (State != (int)NPCState.Dead)
            {
                SwitchState((int)NPCState.Dead);
                npc.damage = 0;
                npc.life = npc.lifeMax;
                npc.dontTakeDamage = true;
                npc.netUpdate = true;
                return false;
            }
            return true;
        }

        protected override void SwitchState(int state)
        {
            npc.frameCounter = 0;
            Timer = 0;
            base.SwitchState(state);
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.GraniteGolem);
            npc.width = 80;
            npc.height = 96;
            npc.aiStyle = -1;
            npc.scale = 1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            drawOffsetY = 20;

            npc.lifeMax = !Main.hardMode ? 120 : !NPC.downedGolemBoss ? 280 : !NPC.downedMoonlord ? 360 : 500;
            npc.defense = !Main.hardMode ? 6 : !NPC.downedGolemBoss ? 16 : !NPC.downedMoonlord ? 22 : 28;
            npc.damage = !Main.hardMode ? 18 : !NPC.downedGolemBoss ? 43 : !NPC.downedMoonlord ? 66 : 81;
            npc.knockBackResist = 0f;

            var num = !Main.hardMode ? 33 : !NPC.downedGolemBoss ? 73 : !NPC.downedMoonlord ? 50 : 27;
            var num2 = !NPC.downedGolemBoss ? 0 : !NPC.downedMoonlord ? 2 : 7;
            npc.value = Item.buyPrice(0, num2, num, 0);
        }
    }
}
