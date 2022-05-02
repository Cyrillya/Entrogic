using Entrogic.Common.ModSystems;
using Entrogic.Content.Items.Elementals;
using Entrogic.Content.NPCs.BaseTypes;
using Entrogic.Content.Projectiles.Elementals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.NPCs.Enemies.Elementals
{
    public class EarthElemental : FSM_NPC
    {
        private Rectangle _frame = new Rectangle();
        private short _frameCounter;
        private short _drawOffsetY;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Earth Elemental");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "土元素");
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { //Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 5f),
                Scale = 0.9f,
                PortraitScale = 0.9f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[1] { //Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
            });
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement( //Sets the description of this NPC that is listed in the bestiary.
                "这个巨大的生物游走在大地上，保护大地不受破坏，消灭任何踏足于此的外来者。"
            ));
        }

        enum NPCState
        {
            Normal,
            Attack,
            Teleport,
            Dead,
            PlayerDead
        }

        public override void AI() {
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = -NPC.direction;
            if (State != (int)NPCState.Dead && State != (int)NPCState.PlayerDead) {
                NPC.velocity = (player.Center - NPC.Center).ToRotation().ToRotationVector2() * 1.8f;
            }
            if (!player.active || player.dead) {
                NPC.TargetClosest();
                if (!player.active || player.dead) {
                    SwitchState((int)NPCState.PlayerDead);
                }
            }
            switch ((NPCState)State) {
                // 正常状态下
                case NPCState.Normal: {
                        NPC.TargetClosest();
                        Timer++;
                        if (Timer >= 60 && player.active) {
                            if (NPC.Distance(player.Center) >= 40f * 16f) {
                                SwitchState((int)NPCState.Teleport);
                            }
                            else if (Collision.CanHit(NPC.Center, NPC.width, NPC.height, player.Center, player.width, player.height)) {
                                SwitchState((int)NPCState.Attack);
                            }
                        }
                        break;
                    }
                // 攻击状态下
                case NPCState.Attack: {
                        // 帧数=计时器/4
                        Timer++;
                        // 第九帧攻击 (也就是此阶段第32帧)
                        if (Timer == 32 && Main.netMode != NetmodeID.MultiplayerClient) {
                            // 来点二分法
                            Vector2 shootPos = new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 2f - 30f);
                            // min, max和mid都表示射击角度
                            float min = -MathHelper.Pi, max = MathHelper.Pi;
                            float mid = (min + max) * 0.5f;
                            while (Math.Abs(max - min) > 0.005f) {
                                Vector2 dirtProj = shootPos;
                                Vector2 plrMovement = player.Center;
                                mid = (min + max) * 0.5f;
                                float beyond = 0f;
                                Vector2 velo = mid.ToRotationVector2() * 15f;
                                for (int i = 0; i < 1000; i++) {
                                    dirtProj += velo;
                                    plrMovement += player.velocity;
                                    if (NPC.Distance(dirtProj) > NPC.Distance(plrMovement)) {
                                        beyond = (dirtProj - NPC.Center).ToRotation() - (plrMovement - NPC.Center).ToRotation();
                                        break;
                                    }
                                }
                                if (beyond > 0f) {
                                    max = mid;
                                }
                                else {
                                    min = mid;
                                }
                            }
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), shootPos, mid.ToRotationVector2() * 15f, ModContent.ProjectileType<EarthElemental_Proj>(), NPC.damage / 2, 1f, Main.myPlayer);
                            NPC.netUpdate = true;
                        }
                        // 第12帧结束 (4*11=44)
                        if (Timer >= 44) {
                            SwitchState((int)NPCState.Normal);
                        }
                        break;
                    }
                // 传送状态下
                case NPCState.Teleport: {
                        // 随机传送到以玩家为中心的六十格的矩形内任意松软类物块
                        Timer++;
                        // 帧数=计时器/4
                        // 第16帧传送 (4*15=60)
                        if (Timer == 60) {
                            NPC.TargetClosest();
                            if (player.active) {
                                List<Point> findTile = new List<Point>();
                                for (int i = -30; i <= 30; i++) {
                                    for (int j = -30; j <= 30; j++) {
                                        Point pos = new Point(i + (int)(player.Center.X / 16f), j + (int)(player.Center.Y / 16f));
                                        if (WorldGen.InWorld(pos.X, pos.Y) && Main.tile[pos.X, pos.Y].IsActive && TileID.Sets.CanBeDugByShovel[Main.tile[pos.X, pos.Y].type]) {
                                            bool rsafe = true;
                                            for (int k = -1; k <= 1; k++) {
                                                for (int l = -1; l >= -3; l--) {
                                                    Point posSafe = new Point(k + pos.X, l + pos.Y);
                                                    if (WorldGen.InWorld(posSafe.X, posSafe.Y) && Main.tile[posSafe.X, posSafe.Y].IsActive && Main.tileSolid[Main.tile[posSafe.X, posSafe.Y].type]) {
                                                        rsafe = false;
                                                    }
                                                }
                                            }
                                            if (rsafe) {
                                                findTile.Add(pos);
                                            }
                                        }
                                    }
                                }
                                if (findTile.Count > 0) {
                                    int random = Main.rand.Next(0, findTile.Count);
                                    Point randomTile = findTile[random];
                                    NPC.position = randomTile.ToWorldCoordinates(8 - NPC.width * 0.5f, -NPC.height);
                                }
                            }
                        }
                        // 传送之后倒放帧图，倒放完了返回普通模式
                        if (Timer == 120) {
                            SwitchState((int)NPCState.Normal);
                        }
                        break;
                    }
                // 死亡状态下
                case NPCState.Dead: {
                        _drawOffsetY = 0;
                        NPC.height = 138;
                        NPC.velocity.X *= 0.88f;
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                        // 不断下降直到碰到物块
                        // 第9帧最后落下动画 (6*8=48)
                        if (Timer <= 48) {
                            Timer++;
                        }
                        else if (NPC.position.Y == NPC.oldPosition.Y) {
                            // 帧数=计时器/6
                            Timer++;
                        }
                        // 第13帧最后动画 (6*12=72)
                        if (Timer >= 72) {
                            Timer = 72;
                            NPC.ai[2]++;
                            NPC.alpha = (int)NPC.ai[2] * 3;
                            if (NPC.ai[2] >= 90) {
                                NPC.life = 0;
                                NPC.HitEffect(0, 0);
                                NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
                            }
                        }
                        NPC.netUpdate = true;
                        break;
                    }
                // 玩家死亡状态下
                case NPCState.PlayerDead: {
                        NPC.noGravity = false;
                        NPC.noTileCollide = true;
                        NPC.TargetClosest();
                        if (player.active && !player.dead) {
                            SwitchState((int)NPCState.Normal);
                        }
                        Timer++;
                        if (Timer >= 130) {
                            NPC.StrikeNPCNoInteraction(9999, 0.0f, 0);
                        }
                        break;
                    }
            }
        }

        private void FindFrame(int frameHeight, int frameWidth) {
            switch ((NPCState)State) {
                // 正常状态下
                case NPCState.Normal: {
                        _frame.X = 0;
                        _frameCounter++;
                        if (_frameCounter >= 6) {
                            _frame.Y += frameHeight;
                            _frameCounter = 0;
                        }
                        if (_frame.Y >= frameHeight * 8) {
                            _frame.Y = 0;
                        }
                        break;
                    }
                // 攻击状态下
                case NPCState.Attack: {
                        _frame.X = frameWidth;
                        _frame.Y = (int)Timer / 4 * frameHeight;
                        break;
                    }
                // 传送状态下
                case NPCState.Teleport: {
                        _frame.X = frameWidth * 2;
                        if (Timer <= 60) {
                            _frame.Y = (int)Timer / 4 * frameHeight;
                        }
                        else {
                            _frame.Y = (15 - ((int)Timer - 60) / 4) * frameHeight;
                        }
                        break;
                    }
                // 死亡状态下
                case NPCState.Dead: {
                        _frame.X = frameWidth * 3;
                        _frame.Y = (int)Timer / 6 * frameHeight;
                        break;
                    }
                // 玩家死亡状态下
                case NPCState.PlayerDead: {
                        _frame.X = 0;
                        _frameCounter++;
                        if (_frameCounter >= 6) {
                            _frame.Y += frameHeight;
                            _frameCounter = 0;
                        }
                        if (_frame.Y >= frameHeight * 8) {
                            _frame.Y = 0;
                        }
                        break;
                    }
            }
        }

        // 给图鉴的
        public override void FindFrame(int frameHeight) {
            base.FindFrame(frameHeight);
            if (!NPC.IsABestiaryIconDummy) return;

            NPC.frame.X = 0;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6) {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type]) {
                NPC.frame.Y = 0;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            base.ModifyNPCLoot(npcLoot);

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EarthElementalAffinityAgent>(), 4));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            if (ModHelper.NormalSpawn(spawnInfo) && player.ZoneOverworldHeight) {
                return 0.001f;
            }
            return base.SpawnChance(spawnInfo);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
            //((Texture2D)Terraria.GameContent.TextureAssets.Npc[NPC.type]) = ModContent.Request<Texture2D>("NPCs/Enemies/EarthElemental_" + tex);
            Texture2D tex = ModContent.Request<Texture2D>($"{Texture}_Ingame").Value;
            int NPCFrameHeightX = tex.Width / 4;
            int NPCFrameHeightY = tex.Height / 16;
            _frame.Width = NPCFrameHeightX;
            _frame.Height = NPCFrameHeightY;
            FindFrame(NPCFrameHeightY, NPCFrameHeightX);
            _frame.Width = NPCFrameHeightX;
            Main.EntitySpriteDraw(tex, NPC.Center - screenPos + new Vector2(0f, _drawOffsetY), new Rectangle?(_frame), drawColor, NPC.rotation, new Vector2(NPCFrameHeightX, NPCFrameHeightY) / 2f, 1f, NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            //SpriteEffects effects = SpriteEffects.None;
            //if (NPC.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            //Main.EntitySpriteDraw(tex, NPC.position, _frame, NPC.GetAlpha(drawColor), NPC.rotation, Vector2.Zero, new Vector2(NPC.scale), effects, 0f);
            return false;
        }

        // We use CheckDead to delay death providing time for our death drama to happen. The logic here is a bit complicated, if you are curious, please step through AI and CheckDead in Visual Studio to see how CheckDead prevents death the first time but allows it after the death drama has finished.
        public override bool CheckDead() {
            if (State != (int)NPCState.Dead) {
                SwitchState((int)NPCState.Dead);
                NPC.damage = 0;
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                return false;
            }
            return true;
        }

        protected override void SwitchState(int state) {
            _frameCounter = 0;
            Timer = 0;
            base.SwitchState(state);
        }

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.GraniteGolem);
            NPC.width = 80;
            NPC.height = 96;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            _drawOffsetY = 20;

            NPC.lifeMax = !Main.hardMode ? 120 : !NPC.downedGolemBoss ? 280 : !NPC.downedMoonlord ? 360 : 500;
            NPC.defense = !Main.hardMode ? 6 : !NPC.downedGolemBoss ? 16 : !NPC.downedMoonlord ? 22 : 28;
            NPC.damage = !Main.hardMode ? 18 : !NPC.downedGolemBoss ? 43 : !NPC.downedMoonlord ? 66 : 81;
            NPC.knockBackResist = 0f;

            var num = !Main.hardMode ? 33 : !NPC.downedGolemBoss ? 73 : !NPC.downedMoonlord ? 50 : 27;
            var num2 = !NPC.downedGolemBoss ? 0 : !NPC.downedMoonlord ? 2 : 7;
            NPC.value = Item.buyPrice(0, num2, num, 0);
        }
    }
}
