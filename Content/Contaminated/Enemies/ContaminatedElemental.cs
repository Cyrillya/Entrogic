using Entrogic.Content.Contaminated.Armors;
using Entrogic.Content.Contaminated.Hostile;
using Entrogic.Content.Contaminated.Items;
using Entrogic.Content.Contaminated.Weapons;
using Entrogic.Core.BaseTypes;
using Entrogic.Core.Global;
using Entrogic.Core.ItemDropRules;
using Entrogic.Helpers;
using Entrogic.Helpers.ID;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.NetModules;

namespace Entrogic.Content.Contaminated.Enemies
{
    [AutoloadBossHead]
    internal class ContaminatedElemental : NPCBase
    {
        public const string DownedMessageKey = "Mods.Entrogic.PollutionAccumulation";

        public override void Load() {
            base.Load();
            Translation.RegisterTranslation("PollutionAccumulation", GameCulture.CultureName.Chinese, "污染生物正在聚集...", "Contaminating organisms are gathering...");
        }

        enum NPCState
        {
            Start,

            Break,
            Move,
            Dash,
            Rotation,
            FindingWaters,

            BreakState2,
            MoveState2,
            MagicState2,
            RoundState2,

            PlayerDead
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Contaminated Elemental");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染之灵");
            Main.npcFrameCount[NPC.type] = 4;

            //Add this in for bosses that have a summon item, requires corresponding code in the item
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { //Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 40f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 80f,
                PortraitScale = 1.2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            //Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new() {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Confused,
                    BuffID.Bleeding,
                    BuffID.Poisoned
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            int indexHeavenlySlime = 0;
            for (int i = 0; i < NPCID.Sets.BossBestiaryPriority.Count; i++) {
                if (NPCID.Sets.BossBestiaryPriority[i] == 660) {
                    indexHeavenlySlime = i;
                    break;
                }
            }
            // 尽量把我们的Boss插入到史莱姆皇后图鉴后面，因为原版Boss图鉴是根据时期排序的
            if (indexHeavenlySlime != 0) {
                // 倒着插入，因为每次都是插入到史莱姆皇后后面
                NPCID.Sets.BossBestiaryPriority.Insert(indexHeavenlySlime, ModContent.NPCType<PolluShark>());
                NPCID.Sets.BossBestiaryPriority.Insert(indexHeavenlySlime, ModContent.NPCType<Migrant>());
                NPCID.Sets.BossBestiaryPriority.Insert(indexHeavenlySlime, NPC.type);
            }
            // 如果有事故找不到，没办法，放在最后面
            else {
                NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
                NPCID.Sets.BossBestiaryPriority.Add(ModContent.NPCType<Migrant>());
                NPCID.Sets.BossBestiaryPriority.Add(ModContent.NPCType<PolluShark>());
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean, //Sets the spawning conditions of this NPC that is listed in the bestiary.

                new FlavorTextBestiaryInfoElement("一只污染之灵") //Sets the description of this NPC that is listed in the bestiary.
            });
        }

        // private bool IsRevengenceMode => CrossModHandler.ModLoaded("CalamityMod") && CrossModHandler.IsCalamityModRevengenceMode

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.EyeofCthulhu);
            NPC.aiStyle = -1;
            NPC.lifeMax = /*IsRevengenceMode ? 11000 :*/ 7600;
            NPC.damage = 52;
            NPC.defense = 15;
            NPC.width = 96;
            NPC.height = 96;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.npcSlots = 15f;
            NPC.boss = NPC.lavaImmune = NPC.noGravity = NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.SpawnWithHigherTime(30);
            NPC.alpha = 0;
            NPC.scale = 0.6f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            float bossAdjustment = 1f;
            if (Main.masterMode)
                bossAdjustment = 0.85f;
            NPC.lifeMax = (int)(NPC.lifeMax * 0.73f * bossLifeScale * bossAdjustment);
        }

        private const float _scaleMin = 0.7f;
        private const float _scaleMax = 1.1f;
        private const int _waterMax = 1295;
        private readonly int[] damagedRecently = new int[240];
        internal float Water;
        private int _oldLife;
        private Vector2 _waterPos;
        public override void FindFrame(int frameHeight) {
            NPC.frameCounter += 0.15000000596046448;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int num = (int)NPC.frameCounter;
            NPC.frame.Y = num * frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            base.ModifyNPCLoot(npcLoot);

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ContaminatedElementalTreasureBag>()));
            npcLoot.Add(ItemDropRule.ByCondition(new CustomConditions.DownedContyElemental(), ModContent.ItemType<ContyElementalTrophy>(), 10));

            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
            // OnSuccess：当指定条件满足/指定物品成功掉落
            // OneFromOptions：从中选取其一掉落
            IItemDropRule itemDropRule = ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<ContaMelee>(),
                ModContent.ItemType<ContaRanged>(),
                ModContent.ItemType<ContaMagic>(),
                ModContent.ItemType<ContaSummoner>(),
                ModContent.ItemType<ContaArcane>());
            //notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ContyMask>(), 7)); // 1/7概率 面具
            notExpertRule.OnSuccess(new OneFromRulesRule(1, itemDropRule,
                ItemDropRule.Common(ModContent.ItemType<ContaBreastplate>()),
                ItemDropRule.Common(ModContent.ItemType<ContaGraves>())));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<ContyLongbow>(),
                ModContent.ItemType<ContyCurrent>(),
                ModContent.ItemType<SymbioticGelatinStaff>()));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SoulofContamination>(), 1, 20, 25));

            npcLoot.Add(notExpertRule);
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.GreaterHealingPotion;
        }

        private Vector2 recordedStartRoundPos;
        private bool IsState2 = false;
        public int GetState() => IsState2 ? 2 : 1;
        public void ScreenDarknessEffect() {
            if (Main.dedServ) return;
            Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
            if (NPC.active && NPC.Distance(mountedCenter) < 3000f) {
                float value = 0.95f;
                ScreenDarkness.frontColor = new Color(0, 0, 120) * 0.3f;
                float amount = IsState2 ? 0.06f : 0.04f;

                ScreenDarkness.screenObstruction = MathHelper.Lerp(ScreenDarkness.screenObstruction, value, amount);
            }
        }
        public override void AI() {
            ScreenDarknessEffect();
            NPC.wet = false;

            Player player = Main.player[NPC.target];
            bool TargetZoneBeach = player.ZoneBeach;
            bool UpgradeOutBeach = !TargetZoneBeach && (IsState2/* || EntrogicWorld.IsDownedPollutionElemental*/); // 不在海洋而且：是第二阶段或这个世界已经击败过一次，即true
            int damagedNearlySum = 0;
            NPC.alpha = Math.Min(255, NPC.alpha);
            NPC.alpha = Math.Max(0, NPC.alpha);
            if (NPC.alpha <= 0)
                NPC.dontTakeDamage = true;
            else NPC.dontTakeDamage = false;
            if (player.dead || !player.active || (!TargetZoneBeach && !UpgradeOutBeach))
                SwitchState((int)NPCState.PlayerDead);
            if (Water < _waterMax) {
                for (int i = 0; i < NPC.width / 16; i++)
                    for (int j = 0; j < NPC.height / 16; j++) {
                        if (WorldGen.InWorld(i + (int)NPC.position.X / 16, j + (int)NPC.position.Y / 16)) {
                            Tile tile = Main.tile[i + (int)NPC.position.X / 16, j + (int)NPC.position.Y / 16];
                            if (Main.rand.NextBool(2)) {
                                Water += 0.8f;
                                if ((NPCState)State == NPCState.FindingWaters) {
                                    Water += 1.2f;
                                }
                                if (Water >= _waterMax)
                                    Water = _waterMax;
                                if (NPC.life < NPC.lifeMax)
                                    NPC.life += 2;
                            }
                        }
                    }
            }
            if (Water < 0) {
                Water = 0;
            }
            NPC.scale = _scaleMin + (_scaleMax - _scaleMin) * ((float)Water / (float)_waterMax);
            if ((NPCState)State != NPCState.Start) {
                for (int i = damagedRecently.Length - 1; i >= 1; i--) {
                    damagedRecently[i] = damagedRecently[i - 1];
                    damagedNearlySum += damagedRecently[i];
                }
                damagedRecently[0] = _oldLife - NPC.life;
                damagedNearlySum += damagedRecently[0];
            }
            if (UpgradeOutBeach) // 如果是第二阶段离开海洋的话
                MovementAI(player, damagedNearlySum, UpgradeOutBeach);// AI多执行一遍（也就是速度提升一倍）
            MovementAI(player, damagedNearlySum, UpgradeOutBeach);
            _oldLife = NPC.life;
        }
        private void MovementAI(Player player, int damagedNearlySum, bool UpgradeOutBeach) {
            switch ((NPCState)State) {
                case NPCState.Start: {
                        _oldLife = NPC.life;
                        NPC.TargetClosest(true);
                        NPC.alpha = 0;
                        SwitchState((int)NPCState.Move);
                        break;
                    }
                case NPCState.Break: {
                        if ((float)NPC.life / (float)NPC.lifeMax <= 0.46f) {
                            SwitchState((int)NPCState.BreakState2);
                            Timer = 0;
                            NPC.ai[2] = NPC.ai[3] = 0;
                            IsState2 = true;
                            return;
                        }
                        Timer++;
                        if (Timer <= 15)
                            NPC.alpha -= 255 / 15 + 1;
                        NPC.dontTakeDamage = true;
                        if (Timer > (int)(80f - (1f - (float)NPC.life / (float)NPC.lifeMax) * 40f)) {
                            NPC.TargetClosest();
                            switch (Main.rand.Next(3)) {
                                case 0:
                                    SwitchState((int)NPCState.Move);
                                    break;
                                case 1:
                                    SwitchState((int)NPCState.Dash);
                                    break;
                                default:
                                    SwitchState((int)NPCState.Rotation);
                                    break;
                            }
                            if (damagedNearlySum >= 680 && FindWater(30, 30) && Main.rand.NextBool(2)) {
                                SwitchState((int)NPCState.FindingWaters);
                            }
                            if ((NPCState)State == NPCState.Dash) // 让冲刺BISS
                            {
                                switch (Main.rand.Next(2)) {
                                    case 0:
                                        SwitchState((int)NPCState.Move);
                                        break;
                                    default:
                                        SwitchState((int)NPCState.Rotation);
                                        break;
                                }
                            }
                            Timer = 0;
                        }
                        break;
                    }
                case NPCState.Move: {
                        Timer++;
                        if (Timer == 1) {
                            NPC.Center = player.Center + new Vector2(Main.rand.Next(24, 33) * 16 * (Main.rand.NextBool(2) ? 1f : -1f)
                                , Main.rand.Next(14, 21) * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            NPC.netUpdate = true;
                        }
                        if (Timer <= 15)
                            NPC.alpha += 255 / 15 + 1;
                        NPC.velocity = (player.Center - NPC.Center).ToRotation().ToRotationVector2() * (7.2f - (float)Water / (float)_waterMax * 2f);
                        int MigrantCount = 0;
                        foreach (NPC NPC in Main.npc) {
                            if (NPC.type == ModContent.NPCType<Migrant>()) {
                                MigrantCount++;
                            }
                        }
                        if (Timer % 95 == 0 && MigrantCount <= 10 && Main.netMode != NetmodeID.MultiplayerClient) {
                            NPC migrant = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Migrant>())];
                            migrant.scale = 1f + (float)Water / (float)_waterMax * 0.4f;
                            Water -= 32;
                            if (Main.netMode == NetmodeID.Server) {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, migrant.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        if (Timer % (int)(72f - (1f - (float)NPC.life / (float)NPC.lifeMax) * 50f) == 0) {
                            float shouldHeight = -12.4f;
                            if (player.Center.Y < NPC.Center.Y) {
                                shouldHeight -= (NPC.Center.Y - player.Center.Y) * 0.0625f;
                            }
                            if (shouldHeight < -20f)
                                shouldHeight = -20f;
                            Shooting(2, NPC.Center, player.Center + player.velocity * 85f, 0.0f, Main.rand.NextFloat(9.6f, 14.9f) * 3f, shouldHeight);
                            Water -= 8;
                        }
                        if (Timer >= 185) {
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.Dash: {
                        Timer++;
                        if (Timer == 1) {
                            NPC.Center = player.Center + new Vector2(30 * 16 * (Main.rand.NextBool(2) ? 1f : -1f)
                                , 20 * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            NPC.netUpdate = true;
                        }
                        if (Timer <= 15)
                            NPC.alpha += 255 / 15 + 1;
                        if (Timer == 25) {
                            NPC.direction = NPC.Center.X > player.Center.X ? -1 : 1;
                            NPC.spriteDirection = NPC.direction;
                            NPC.velocity.X = NPC.direction * 24;
                            SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.position);
                        }
                        if ((Timer - 25) % 5 == 0) {
                            Shooting(0, NPC.Center, player.Center, 0f, 20f);
                            Water -= 2;
                        }
                        if (Timer == 40 && Main.netMode != NetmodeID.MultiplayerClient) {
                            NPC migrant = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Migrant>())];
                            migrant.scale = 1f + (float)Water / (float)_waterMax * 0.4f;
                            Water -= 32;
                            if (Main.netMode == NetmodeID.Server) {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, migrant.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        if (Timer >= 65) {
                            Timer = 0;
                            NPC.velocity.X = 0f;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.Rotation: {
                        NPC.velocity = Vector2.Zero;
                        Timer++;
                        if (Timer == 1) {
                            NPC.Center = player.Center + new Vector2(0f, 26 * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            NPC.netUpdate = true;

                            int insideTiles = 0;
                            for (int i = 0; i < NPC.width / 16; i++)
                                for (int j = 0; j < NPC.height / 16; j++) {
                                    Tile tile = Main.tile[i + (int)NPC.position.X / 16, j + (int)NPC.position.Y / 16];
                                    if (Main.tileSolid[tile.TileType] && tile.HasUnactuatedTile && tile != null)
                                        insideTiles++;
                                }
                            if (insideTiles >= 12) {
                                NPC.Center = new Vector2(NPC.Center.X, NPC.Center.Y + -(NPC.Center.Y - player.Center.Y));
                            }
                        }
                        if (Timer <= 15)
                            NPC.alpha += 255 / 15 + 1;
                        NPC.ai[3] += 0.006f;
                        if (NPC.ai[3] >= 0.3f) {
                            if (NPC.ai[3] >= 0.55f) {
                                NPC.ai[3] = 0.55f;
                            }
                            NPC.frameCounter += NPC.ai[3];
                            // 发射
                            if (Timer % 3 == 0 && Main.rand.Next(1, 4) <= 2 && Main.netMode != NetmodeID.MultiplayerClient) {
                                if (Main.rand.NextBool(6)) {
                                    Water -= 6;
                                    Shooting(1, NPC.Center, NPC.Center + new Vector2(0f, -10f), Main.rand.NextFloat(0.0f, MathHelper.TwoPi), Main.rand.Next(9, 20));
                                }
                                Water -= 1;
                                Shooting(0, NPC.Center, player.Center, Main.rand.NextFloat(0.0f, MathHelper.TwoPi), Main.rand.Next(10, 16));
                            }
                        }
                        if (Timer >= 245) {
                            Timer = 0;
                            NPC.ai[3] = 0f;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.FindingWaters: {
                        Timer++;
                        NPC.velocity = Vector2.Zero;
                        if (Timer == 1) {
                            NPC.Center = _waterPos + new Vector2(50f, 50f);
                            NPC.netUpdate = true;
                            if (Water >= _waterMax)
                                Water = (int)(_waterMax * 0.7f);
                        }
                        if (Timer <= 15)
                            NPC.alpha += 255 / 15 + 1;
                        if (Timer >= 60 || Water >= _waterMax || NPC.life >= NPC.lifeMax) {
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.BreakState2: {
                        Timer++;
                        if (Timer > 10 && Main.netMode != NetmodeID.MultiplayerClient) {
                            switch (Main.rand.Next(2)) {
                                case 0:
                                    SwitchState((int)NPCState.MoveState2);
                                    break;
                                default:
                                    SwitchState((int)NPCState.RoundState2);
                                    break;
                            }
                            NPC.netUpdate = true;
                            Timer = 0;
                            NPC.ai[2] = NPC.ai[3] = 0;
                            IsState2 = true;
                        }
                        break;
                    }
                case NPCState.MoveState2: {
                        NPC.velocity = ModHelper.GetFromToVectorNormalized(NPC.Center, player.Center) * 8.2f;
                        Timer++;
                        if ((Timer - 30) % 50 == 0 && Timer > 60 && Main.netMode != NetmodeID.MultiplayerClient) {
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ShootingLine>(), 0, 0f, 255, 0f, NPC.whoAmI);
                            Main.projectile[proj].ai[1] = NPC.whoAmI;
                            if (Main.dedServ) {
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                            }
                        }
                        if (Timer == 250) {
                            if (Main.rand.NextBool(3)) {
                                SwitchState((int)NPCState.MagicState2);
                            }
                            else {
                                SwitchState((int)NPCState.RoundState2);
                            }
                            NPC.ai[2] = NPC.ai[3] = Timer = 0;
                            return;
                        }
                        break;
                    }
                case NPCState.MagicState2: {
                        // 距离小于50，且没有成功过就进入
                        Vector2 targetPos = player.Center + new Vector2(0f, 320f);
                        if (Vector2.Distance(NPC.Center, targetPos) > 50f && NPC.ai[2] != 1) {
                            NPC.velocity = ModHelper.GetFromToVectorNormalized(NPC.Center, targetPos) * 36f;
                            return;
                        }
                        else if (NPC.ai[2] != 1) {
                            NPC.ai[2] = 1;
                            Timer = 20;
                        }
                        if (NPC.ai[2] == 1) Timer++;
                        if (Timer >= 20) {
                            NPC.velocity = Vector2.Zero;
                            NPC.Center = targetPos;
                            NPC.netUpdate = true;
                        }
                        if (Timer <= 25) {
                            //NPC.alpha -= 255 / 15 + 1;
                            //NPC.alpha = Math.Max(0, NPC.alpha);
                        }
                        if (Timer % 40 == 0 && Timer >= 70 && Main.netMode != NetmodeID.MultiplayerClient) {
                            int offset = Timer % 80 == 40 ? 120 : 0;
                            for (float pos = -1600f; pos <= 1600f; pos += 320f) {
                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(pos + offset, 1000f), Vector2.Zero, ModContent.ProjectileType<ContaShark>(), (int)(GetSpikeDamage() * 1.2f), 3f, 255, 0f, 0f);
                                if (Main.dedServ) {
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                                }
                            }
                        }
                        if (Timer >= 200f) {
                            //NPC.alpha += 255 / 15 + 1;
                            //NPC.alpha = Math.Min(255, NPC.alpha);
                        }
                        if (Timer >= 215f) {
                            SwitchState((int)(NPCState.MoveState2));
                            NPC.ai[2] = NPC.ai[3] = Timer = 0;
                            return;
                        }
                        break;
                    }
                case NPCState.RoundState2: {
                        // 规定ai2只有0,1,2两种状态
                        // ai3为记录中的本回合的总时长
                        // Timer为进入转圈状态的总时长
                        NPC.ai[3]++;
                        // 距离小于36，且没有成功过就进入
                        Vector2 targetPos = player.Center + new Vector2(0f, -500f);
                        if (Vector2.Distance(NPC.Center, targetPos) > 36f && NPC.ai[2] < 1) {
                            NPC.velocity = ModHelper.GetFromToVectorNormalized(NPC.Center, targetPos) * 30f;
                            return;
                        }
                        else if (NPC.ai[2] < 1) {
                            recordedStartRoundPos = NPC.Center;
                            NPC.ai[2] = 1;
                        }
                        if (NPC.ai[2] >= 1) {
                            Timer++;
                            NPC.velocity = Vector2.Zero;
                            // 缓冲时间
                            if (Timer >= 30 && NPC.ai[2] != 2f) {
                                NPC.ai[2] = 2f;
                                Timer = 0;
                            }
                            if (NPC.ai[2] != 2f) return;
                            float speedMultipiler = 3.4f;
                            float radians = MathHelper.ToRadians(Timer * speedMultipiler);
                            NPC.velocity = new Vector2(16f * speedMultipiler) * radians.ToRotationVector2();
                            //Main.NewText($"Timer:{Timer}, Radians:{radians}, Velocity:{NPC.velocity}.");

                            // 射击（向与NPC移动方向垂直的圈内向量射击）（其实我也不知道我这注释写的什么）
                            Vector2 shootTarget = NPC.Center + NPC.velocity;
                            // 旋转90°，也就是垂直，是向圆心射击
                            Shooting(3, NPC.Center, shootTarget, 90f, 24f);
                            if (Main.expertMode && Timer % 5 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
                                Shooting(3, NPC.Center, shootTarget, 90f, 10f);
                            }

                            // for模拟一圈，用于显示粒子特效。因为属于特效所以服务器就别掺和了
                            if (!Main.dedServ) {
                                Vector2 _position = recordedStartRoundPos;
                                for (int _Timer = 0; _Timer <= 360; _Timer += UpgradeOutBeach ? 2 : 1) {
                                    float _radians = MathHelper.ToRadians(_Timer);
                                    Vector2 _velocity = new Vector2(16f) * _radians.ToRotationVector2();
                                    _position += _velocity;
                                    // 生成粒子
                                    Dust d = Dust.NewDustDirect(_position, 4, 4, MyDustID.LightBlueParticle);
                                    d.noGravity = true;
                                }
                            }

                            // 转了2.75圈
                            if (Timer >= 2.75 * (360 / speedMultipiler)) {
                                switch (Main.rand.Next(2)) {
                                    case 0:
                                        SwitchState((int)NPCState.MoveState2);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.MagicState2);
                                        break;
                                }
                                NPC.ai[2] = NPC.ai[3] = Timer = 0;
                                return;
                            }
                        }
                        else {
                            if (NPC.ai[3] >= 240) {
                                switch (Main.rand.Next(2)) {
                                    case 0:
                                        SwitchState((int)NPCState.MoveState2);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.MagicState2);
                                        break;
                                }
                                NPC.ai[2] = NPC.ai[3] = Timer = 0;
                                return;
                            }
                        }
                        break;
                    }
                case NPCState.PlayerDead: {
                        NPC.TargetClosest(true);
                        Timer++;
                        if (!player.dead && player.active && player.ZoneBeach) {
                            SwitchState((int)NPCState.Break);
                            Timer = 0;
                        }
                        else if (Timer >= 180) {
                            NPC.TargetClosest(true);
                            if (player.dead || !player.active || !player.ZoneBeach) {
                                if (Timer <= 195)
                                    NPC.alpha -= 255 / 15 + 1;
                                else NPC.active = false;
                            }
                            else {
                                SwitchState((int)NPCState.Break);
                                Timer = 0;
                            }
                        }
                        break;
                    }
            }
        }

        public int GetSpikeDamage() {
            int normalDamage = 50;
            int expertDamage = 80;
            int masterDamage = 100;
            if (IsState2) {
                normalDamage = 80;
                expertDamage = 110;
                masterDamage = 160;
            }
            return NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(normalDamage, expertDamage, masterDamage);
        }

        public void Shooting(int type, Vector2 position, Vector2 target, float rad, float speed, float StartY = -15f) {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            int SpikeDamage = GetSpikeDamage();
            switch (type) {
                case 0: {
                        int shot = Projectile.NewProjectile(NPC.GetSource_FromAI(), position, ((target - position).ToRotation() + rad).ToRotationVector2() * speed, ModContent.ProjectileType<ContaSpike>(), SpikeDamage, 0f, 255, type);
                        Projectile shots = Main.projectile[shot];
                        shots.scale = 1f + (float)Water / (float)_waterMax * 1.5f;
                        NPC.netUpdate = true;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shots.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                        }
                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                        break;
                    }
                case 1: {
                        target.Y += 32;
                        Vector2 sVelo = ((target - position).ToRotation() + rad).ToRotationVector2() * speed;
                        NPC shark = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolluShark>(), 0, 0, 0, sVelo.Y - 5f, sVelo.X)];
                        shark.scale = 1f + (float)Water / (float)_waterMax * 0.43f;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                        }
                        break;
                    }
                case 2: {
                        const float fallSpeed = 0.32f;
                        float min = -speed, max = speed;
                        float mid = (min + max) * 0.5f;
                        while (Math.Abs(max - min) > 0.001f) {
                            Vector2 vShark = NPC.Center;
                            mid = (min + max) * 0.5f;
                            float maxHeight = 0f;
                            float beyond = 0f;
                            float veloY = StartY;
                            for (int i = 0; i < 1000; i++) {
                                vShark.X += mid;
                                vShark.Y += veloY;
                                veloY += fallSpeed;
                                maxHeight = Math.Min(vShark.Y, maxHeight);
                                if (veloY > 1f && vShark.Y > target.Y) {
                                    beyond = vShark.X - target.X;
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
                        NPC shark = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolluShark>(), 0, 0, 0, StartY, mid)];
                        shark.scale = 1f + (float)Water / (float)_waterMax * 0.43f;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                        }
                        break;
                    }
                case 3:
                case 4: {
                        // 专家模式敌对会翻4倍，所以改成40即为40*4=160
                        int proj = ModContent.ProjectileType<ContaSpikeRound>();
                        if (proj == 4) {
                            type = ModContent.ProjectileType<ContaSpike>();
                        }
                        int shot = Projectile.NewProjectile(NPC.GetSource_FromAI(), position, ((target - position).ToRotation() + rad).ToRotationVector2() * speed, proj, SpikeDamage, 0f, 255, type);
                        if (speed >= 15f) {
                            Main.projectile[shot].timeLeft = 160;
                        }
                        NPC.netUpdate = true;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shot, -1f, 0f, 0f, 0, 0, 0);
                        }
                        break;
                    }
            }
        }

        public override void OnKill() {
            base.OnKill();
            if (!DownedHandler.DownedContyElemental) {
                // 第一次击败，放点水吧（物理）
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        int x = (int)(NPC.Center.ToWorldCoordinates().X + i);
                        int y = (int)(NPC.Center.ToWorldCoordinates().Y + j);
                        PlaceWater(x, y, byte.MaxValue);
                    }
                }

                Color textColor = Color.GreenYellow;
                if (Main.netMode == NetmodeID.SinglePlayer) {
                    Main.NewText(Language.GetTextValue(DownedMessageKey), textColor);
                }
                else if (Main.netMode == NetmodeID.Server) {
                    NetTextModule.SerializeServerMessage(NetworkText.FromKey(DownedMessageKey, new object[0]), textColor);
                }
                DownedHandler.DownedContyElemental = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            NPC.SimpleDrawShadow(spriteBatch, screenPos, drawColor, Math.Min(10, (int)(NPC.velocity.Length() * 0.7f)));

            return false;
        }

        internal bool FindWater(int areaX, int areaY) {
            for (int i = 0; i >= -areaX; i--) {
                for (int j = 0; j >= -areaY; j--) {
                    if (WorldGen.InWorld((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j)) {
                        Tile tileSafely = Framing.GetTileSafely((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j);
                        if (tileSafely.LiquidType == 0 && tileSafely.LiquidAmount > 50) {
                            _waterPos = new Vector2(NPC.Center.X + i * 16f, NPC.Center.Y + j * 16f);
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i <= areaX; i++) {
                for (int j = 0; j <= areaY; j++) {
                    if (WorldGen.InWorld((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j)) {
                        Tile tileSafely = Framing.GetTileSafely((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j);
                        if (tileSafely.LiquidType == 0 && tileSafely.LiquidAmount > 50) {
                            _waterPos = new Vector2(NPC.Center.X + i * 16f, NPC.Center.Y + j * 16f);
                            return true;
                        }
                    }
                }
            }
            _waterPos = Vector2.Zero;
            return false;
        }

        internal static bool PlaceWater(int x, int y, byte amount) {
            Tile tileSafely = Framing.GetTileSafely(x, y);
            if (WorldGen.InWorld(x, y)) {
                if (tileSafely.LiquidAmount < 230 && (!tileSafely.IsActuated || !Main.tileSolid[(int)tileSafely.TileType] || Main.tileSolidTop[(int)tileSafely.TileType]) && (tileSafely.LiquidAmount == 0 || tileSafely.LiquidType == 0)) {
                    tileSafely.LiquidType = 0;
                    tileSafely.LiquidAmount = amount;
                    WorldGen.SquareTileFrame(x, y, true);
                    if (Main.netMode != NetmodeID.SinglePlayer) {
                        NetMessage.sendWater(x, y);
                    }
                    return true;
                }
            }
            return false;
        }

        public override void HitEffect(int hitDirection, double damage) {
            Water -= Main.rand.Next(2, 6);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            return !NPC.dontTakeDamage;
        }
    }
}