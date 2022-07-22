using Entrogic.Common.WorldGeneration;
using Entrogic.Content.Dusts;
using Entrogic.Content.NPCs.BaseTypes;
using Entrogic.Content.Projectiles.Athanasy;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;

namespace Entrogic.Content.NPCs.Enemies.Athanasy
{
    [AutoloadBossHead]
    internal class Athanasy : NPCBase
    {
        enum AIState
        {
            Despawn,
            Waiting,
            First_DecideAttack,
            First_Spear,
            First_WallSpear,
            First_StoneHand,
            Second_ChangeState,
            Second_SuperSmash,
            Second_DecideAttack,
            Second_StoneHand,
            Second_Dash,
            Second_Smash,
            Second_Spear,
            Second_WallSpear,
            Second_PlatformHand,
            Second_StoneHandSpeed
        }

        protected override void SwitchState(int state) {
            base.SwitchState(state);
            Timer = 0;
        }

        public override void Load() {
            base.Load();
            On.Terraria.NPC.UpdateNPC_UpdateGravity += NPC_UpdateNPC_UpdateGravity;
        }

        public override bool? CanFallThroughPlatforms() {
            if (State == (int)AIState.Second_Smash) {
                return Player.Bottom.Y - NPC.Bottom.Y > 32;
            }
            if (State == (int)AIState.Second_SuperSmash) {
                return RoomBottom.Y - NPC.Bottom.Y > 40;
            }
            return false;
        }

        private void NPC_UpdateNPC_UpdateGravity(On.Terraria.NPC.orig_UpdateNPC_UpdateGravity orig, NPC self, out float maxFallSpeed) {
            orig.Invoke(self, out maxFallSpeed);
            if (self.type == Type && self.ModNPC is Athanasy) {
                maxFallSpeed = SmashSpeedMax;
                if ((self.ModNPC as Athanasy).State == (int)AIState.Second_SuperSmash) {
                    maxFallSpeed = SmashSpeedMax * 3.4f;
                }
            }
        }

        private readonly CoroutineRunner AIRunner = new();
        internal static int SmallSpearType;
        internal static int WallSpearType;
        internal static int DebrisType;
        internal static int DustProjectileType;
        internal static int StoneHandType;
        internal int Stage = 0;

        private Player Player => Main.player[NPC.target];
        private bool HitPlayer = true;
        private const int DespawnDistance = 300;
        private static float SmashSpeedMax => Main.getGoodWorld ? 29.99f : 26f;
        private static float SmashSpeed => Main.getGoodWorld ? 2.6f : 1.5f;
        private static Vector2 RoomBottom => new Vector2(ImmortalGolemRoom.BossZone.Center.X, ImmortalGolemRoom.BossZone.Bottom - 8).ToWorldCoordinates(0, 0);

        private byte AttackIndex;

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(AttackIndex);
            writer.Write(Stage != 0);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            AttackIndex = reader.ReadByte();
            bool secondStage = reader.ReadBoolean();
            if (secondStage)
                Stage = 1;
            else
                Stage = 0;
        }

        public override void AI() {
            switch (State) {
                case (int)AIState.Despawn: DespawnState(); break;
                case (int)AIState.Waiting: WaitingState(); break;
                case (int)AIState.First_DecideAttack: First_DecideAttack(); break;
                case (int)AIState.First_Spear: First_SpearState(); break;
                case (int)AIState.First_WallSpear: First_WallSpearState(); break;
                case (int)AIState.First_StoneHand: First_StoneHandState(); break;
                case (int)AIState.Second_ChangeState: Second_ChangeState(); break;
                case (int)AIState.Second_SuperSmash: Second_SuperSmashState(); break;
                case (int)AIState.Second_DecideAttack: Second_DecideAttackState(); break;
                case (int)AIState.Second_StoneHand: Second_StoneHandState(); break;
                case (int)AIState.Second_Dash: Second_DashState(); break;
                case (int)AIState.Second_Smash: Second_SmashState(); break;
                case (int)AIState.Second_Spear: Second_SpearState(); break;
                case (int)AIState.Second_WallSpear: Second_WallSpearState(); break;
                case (int)AIState.Second_PlatformHand: Second_PlatformHandState(); break;
                case (int)AIState.Second_StoneHandSpeed: Second_StoneHandSpeedState(); break;
            }
            NPC.spriteDirection = NPC.direction;

            if (Stage == 0) {
                NPC.localAI[0]++;

                if (Main.netMode != NetmodeID.Server) {
                    int particleCounts = Main.expertMode ? 5 : 4;
                    float distanceFromCenter = 80f;
                    if (Main.getGoodWorld) {
                        distanceFromCenter = 150f;
                        particleCounts += 3;
                    }
                    float particleGap = 360f / particleCounts;
                    for (int i = 1; i <= particleCounts; i++) {
                        Vector2 baseVector = Vector2.One.RotatedBy(MathHelper.ToRadians(i * particleGap + NPC.localAI[0]));
                        Vector2 velocity = Vector2.Zero;
                        Vector2 spawnPosition = baseVector * distanceFromCenter + NPC.Center;
                        var d = Dust.NewDustPerfect(spawnPosition, MyDustID.BlueWhiteBubble, velocity, 180, default, 1f);
                        d.fadeIn = 1.4f;
                        d.noGravity = true;
                    }
                }

                NPC.velocity = Vector2.Zero;
                NPC.noTileCollide = true;
                NPC.noGravity = true;
            }
            AIRunner.Update(1);
        }

        private void DespawnState() {
            NPC.TargetClosest(true);
            Timer++;
            if (!Player.dead && Player.active) {
                SwitchState((int)AIState.First_DecideAttack);
                if (Stage == 1)
                    SwitchState((int)AIState.Second_DecideAttack);
            }
            else if (Timer >= 180) {
                NPC.TargetClosest(true);
                NPC.EncourageDespawn(10);
                if (Player.dead || !Player.active || !Player.ZoneBeach) {
                    if (Timer <= 195)
                        NPC.alpha -= 255 / 15 + 1;
                    else NPC.active = false;
                }
                else {
                    SwitchState((int)AIState.First_DecideAttack);
                    if (Stage == 1)
                        SwitchState((int)AIState.Second_DecideAttack);
                }
            }
        }

        private void WaitingState() {
            Timer++;
            int waitTime = Main.expertMode ? 60 : 80;
            if (Main.getGoodWorld)
                waitTime -= 20;
            if (Timer >= waitTime) {
                SwitchState((int)AIState.First_DecideAttack);
                if (Stage == 1)
                    SwitchState((int)AIState.Second_DecideAttack);
                NPC.netUpdate = true;
            }
        }

        #region 第一阶段

        private void First_DecideAttack() {
            NPC.TargetClosest();

            if (!Player.active || Player.dead) {
                SwitchState((int)AIState.Despawn);
                return;
            }

            if (NPC.life <= NPC.lifeMax * (Main.expertMode ? 0.5f : 0.4f)) {
                SwitchState((int)AIState.Second_ChangeState);
                Stage = 1;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                return;
            }

            // s: 对玩家石矛
            // d: 对玩家石矛(少)
            // y: 对玩家石矛(预判)
            // l: 左墙石矛
            // r: 右墙石矛
            // h: 玩家石手
            // w: 等待1秒
            List<char> StageOneOrder = new() {
                's', 's', 'w', 'y', 'l', 'w',
                'h', 's', 'r', 'w', 'l', 'w',
                'y', 's', 'd', 'h',
                'r', 'r', 'w', 'w', 'l',
                'w', 'w', 'w', 'd'
            };

            if (!StageOneOrder.IndexInRange(AttackIndex))
                AttackIndex = 0;
            //Main.NewText(AttackIndex + " " + StageOneOrder[AttackIndex]);
            switch (StageOneOrder[AttackIndex]) {
                case 's':
                    SwitchState((int)AIState.First_Spear);
                    break;
                case 'd':
                    NPC.ai[2] = 1f;
                    SwitchState((int)AIState.First_Spear);
                    break;
                case 'y':
                    NPC.ai[2] = 114514f;
                    SwitchState((int)AIState.First_Spear);
                    break;
                case 'l':
                    NPC.ai[2] = 1;
                    SwitchState((int)AIState.First_WallSpear);
                    break;
                case 'r':
                    NPC.ai[2] = -1;
                    SwitchState((int)AIState.First_WallSpear);
                    break;
                case 'h':
                    SwitchState((int)AIState.First_StoneHand);
                    break;
                case 'w':
                    SwitchState((int)AIState.Waiting);
                    break;
            }
            AttackIndex++;
        }

        private void First_SpearState() {
            Timer++;
            int spearDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            int spearReduce = NPC.ai[2] != 114514f ? (int)NPC.ai[2] : 0;

            Vector2 target = Player.Center;
            if (NPC.ai[2] == 114514f)
                target += Player.velocity * 60f;
            Vector2 baseVector = NPC.DirectionTo(target);
            float distanceFromCenter = 160f;

            if (Timer == 10 && Main.netMode != NetmodeID.MultiplayerClient) {
                float rotationAngle = MathHelper.ToRadians(25f);
                int spears = Main.expertMode ? 3 : 2; // 单向矛数，实际矛数为 spears*2+1
                if (Main.getGoodWorld) {
                    rotationAngle += MathHelper.ToRadians(10f);
                    spears += 1;
                }
                for (int i = -spears; i <= spears; i++) {
                    if (Math.Abs(i) > spears - spearReduce) // 减少矛数量
                        continue;
                    Vector2 rotatedVector = baseVector.RotatedBy(rotationAngle / spears * i);
                    Vector2 finalSpawnPosition = rotatedVector * distanceFromCenter + NPC.Center;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), finalSpawnPosition, rotatedVector, SmallSpearType, spearDamage, 0f);
                }
            }
            if (Timer == 20) {
                SwitchState((int)AIState.First_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        private void First_WallSpearState() {
            if (Timer == 0) {
                NPC.netUpdate = true;
                NPC.ai[2] = Main.rand.Next(9) * NPC.ai[2]; // 随机化生成，正为左墙负为右墙
            }

            Timer++;
            int spearDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            bool left = NPC.ai[2] > 0;
            int top = ImmortalGolemRoom.BossZone.Top + 10;
            int bottom = ImmortalGolemRoom.BossZone.Bottom - 10;
            int distance = bottom - top;
            
            if (Timer % 9 == Math.Abs(NPC.ai[2]) && Main.netMode != NetmodeID.MultiplayerClient) {
                Vector2 finalSpawnPosition = new(ImmortalGolemRoom.BossZone.Left + 9, Timer + top);
                if (!left)
                    finalSpawnPosition.X = ImmortalGolemRoom.BossZone.Right - 9;
                finalSpawnPosition = finalSpawnPosition.ToWorldCoordinates();
                Projectile.NewProjectile(NPC.GetSource_FromAI(), finalSpawnPosition, new(left ? 1 : -1, 0), WallSpearType, spearDamage, 0f);
            }

            if (Timer >= distance) {
                SwitchState((int)AIState.First_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        private void First_StoneHandState() {
            Timer++;
            if (!Player.active || Player.dead) {
                SwitchState((int)AIState.Despawn);
                return;
            }

            int handTimer = Main.expertMode ? 80 : 100;
            if (Main.getGoodWorld)
                handTimer = 50;
            if (Timer % handTimer == 0) {
                Point16 center = Player.Center.ToTileCoordinates16();
                for (int i = 0; i < 30; i++) {
                    Point tilePosition = center.ToPoint();
                    tilePosition.Y += i;
                    var tile = Framing.GetTileSafely(tilePosition);
                    if (IsSaveTile(tile)) {
                        var tileUp = Framing.GetTileSafely(new Point(tilePosition.X, tilePosition.Y - 1));
                        var tileLeft = Framing.GetTileSafely(new Point(tilePosition.X - 1, tilePosition.Y));
                        var tileRight = Framing.GetTileSafely(new Point(tilePosition.X + 1, tilePosition.Y));
                        if (IsSaveTile(tileLeft) && IsSaveTile(tileRight) && !WorldGen.SolidTile(tileUp)) {
                            var spawnPosition = tilePosition.ToWorldCoordinates(autoAddY: 2f).ToPoint();
                            NPC.NewNPC(NPC.GetSource_FromAI("Stage1"), spawnPosition.X, spawnPosition.Y, StoneHandType, ai1: 80f, ai2: NPC.Center.X, ai3: NPC.Center.Y, Target: NPC.target);
                            break;
                        }
                    }
                }
            }
            if (Timer == 240) {
                SwitchState((int)AIState.First_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        #endregion

        #region 第二阶段

        private void Second_ChangeState() {
            NPC.dontTakeDamage = true;
            NPC.localAI[0]++;
            Timer++;
            if (Main.netMode != NetmodeID.Server) {
                if (Timer <= 120) {
                    float factor = Utils.GetLerpValue(0, 120, Timer);
                    factor *= factor;

                    int particleCounts = Main.expertMode ? 5 : 4;
                    float distanceFromCenter = MathHelper.Lerp(80f, 0f, factor);
                    if (Main.getGoodWorld) {
                        distanceFromCenter = MathHelper.Lerp(150f, 0f, factor);
                        particleCounts += 3;
                    }
                    float particleGap = 360f / particleCounts;
                    for (int i = 1; i <= particleCounts; i++) {
                        Vector2 baseVector = Vector2.One.RotatedBy(MathHelper.ToRadians(i * particleGap + NPC.localAI[0]));
                        Vector2 velocity = Vector2.Zero;
                        Vector2 spawnPosition = baseVector * distanceFromCenter + NPC.Center;
                        var d = Dust.NewDustPerfect(spawnPosition, MyDustID.BlueWhiteBubble, velocity, 180, default, 1f);
                        d.fadeIn = 1.4f;
                        d.noGravity = true;
                    }
                }
            }

            if (Timer < 40)
                return;

            if (Main.netMode != NetmodeID.MultiplayerClient && Timer < 1000f) {
                if (Timer == 200f) {
                    // 协程 发射粒子特效射弹
                    AIRunner.Run(ProjectileEffectQuarter(0f));
                    AIRunner.Run(ProjectileEffectQuarter(1.57f));
                    AIRunner.Run(ProjectileEffectQuarter(3.14f));
                    AIRunner.Run(ProjectileEffectQuarter(4.71f));
                }
                if (Timer > 200f && AIRunner.Count <= 0) {
                    NPC.netUpdate = true; // 同步给其他的客户端
                    Timer = 1000; // 下一阶段
                    AthanasySystem.TileRunner.Run(AthanasySystem.ActuateSlopedPlatforms(true));
                }
            }
            if (Timer == 1155f) {
                NPC.dontTakeDamage = false;
                SoundEngine.PlaySound(SoundID.Item4, NPC.Center);
                for (int i = 0; i < 80; i++) {
                    var velocity = Main.rand.NextVector2Circular(20f, 20f);
                    var d = Dust.NewDustPerfect(NPC.Center, DustID.FireworksRGB, velocity, 180, Color.SkyBlue, 1f);
                    d.fadeIn = 1.4f;
                    d.noGravity = true;
                }
                // 测试用代码
                //SwitchState((int)AIState.First_DecideAttack);
                //Stage = 0;
                //NPC.life = (int)(NPC.lifeMax * 0.8f);
                //return;
                SwitchState((int)AIState.Second_SuperSmash);
                NPC.localAI[0] = 0;
            }
        }

        IEnumerator ProjectileEffectQuarter(float startRadians) {
            for (float r = 0; r < 1.57f; r += 1.57f / 4f) {
                float radians = r + startRadians;
                var velocity = Vector2.One.RotatedBy(radians) * 10f;
                Projectile.NewProjectile(NPC.GetSource_FromAI("EffectProjectile"),
                                         NPC.Center,
                                         velocity,
                                         DustProjectileType,
                                         0, 0f, Main.myPlayer,
                                         ai0: Timer - 200f);
                yield return 6;
            }
            yield return 0;
        }

        private void Second_SuperSmashState() {
            // 超级下砸，是从场地上砸到场地下方
            int smashStartDistanceY = 16 * 80;
            int smashStartDistanceX = 8;
            int smashStartTimer = 25; // 下砸起始计时器
            int rubbleDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (NPC.ai[2] == 1f) { // ai[2]为1时是下砸
                NPC.noTileCollide = RoomBottom.Y - NPC.Bottom.Y > 48;
                NPC.noGravity = false;

                if (Main.getGoodWorld) // GoodWorld是ftw种子
                    smashStartTimer = 10;

                if (NPC.velocity.Y == 0f) { // 下砸到地了
                    if (Timer > 21) {
                        Timer = 0; // 重设，给射弹用，顺便是下砸特效
                        SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                        for (int l = 0; l < 20; l++) { // 粒子效果
                            var d = Dust.NewDustDirect(NPC.Bottom - new Vector2(NPC.width / 2, 30f), NPC.width, 30, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, GetDustColor());
                            d.noGravity = true;
                            d.velocity.Y = -5f + Main.rand.NextFloat() * -3f;
                            d.velocity.X *= 7f;
                        }
                        PunchCameraModifier modifier4 = new(NPC.Center, -Vector2.UnitY, 30f, 15f, 30, 2000f, "Entrogic: Athanasy");
                        Main.instance.CameraModifiers.Add(modifier4);
                    }
                    if (Timer <= 20) {// 借用一下鹿角怪的Proj
                        float d = RoomBottom.Distance(ImmortalGolemRoom.BossZone.BottomLeft().ToWorldCoordinates(0, 0)); // 计算出中心到两边的距离
                        float p = d / 26f; // 分成22分给不同的ai1使用（26是考虑了墙面不需要）
                        for (int l = -1; l <= 1; l += 2) { // 左右都有
                            if (Main.netMode != NetmodeID.MultiplayerClient) {
                                for (int offset = 0; offset <= 120; offset += 60) { // 来多几层
                                    var rubbleVector = -Vector2.UnitY;
                                    rubbleVector = rubbleVector.RotatedByRandom(MathHelper.ToRadians(5));
                                    
                                    int frame = 0;
                                    switch (ImmortalGolemRoom.BrickType) {
                                        case TileID.BlueDungeonBrick:
                                            frame = 0;
                                            break;
                                        case TileID.GreenDungeonBrick:
                                            frame = 3;
                                            break;
                                        case TileID.PinkDungeonBrick:
                                            frame = 6;
                                            break;
                                    }
                                    frame += Main.rand.Next(3);

                                    var pos = new Vector2(NPC.Bottom.X + (p * Timer + Main.rand.Next((int)p + 1)) * l, NPC.Bottom.Y - 8 + offset);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, rubbleVector * (16f + Main.rand.NextFloat() * 8f), DebrisType, rubbleDamage, 0f, Main.myPlayer, 0f, frame);
                                }
                            }
                            if (Main.netMode != NetmodeID.Server) {
                                NPC.position += NPC.netOffset;
                                var dPos = new Vector2(NPC.Bottom.X + (p * Timer + Main.rand.Next((int)p + 1)) * l, NPC.Bottom.Y - 8);
                                for (int i = 0; i < 10; i++) { // 粒子效果
                                    Dust.NewDustDirect(dPos + new Vector2(-15, -48), 30, 60, ModContent.DustType<Sand>(), 0, Main.rand.NextFloat(-5f, -1f), Main.rand.Next(255), default, Main.rand.NextFloat(1.5f));
                                }
                                NPC.position -= NPC.netOffset;
                            }
                        }
                    }
                    else {
                        NPC.ai[2] = 0f;
                        NPC.netUpdate = true;
                        SwitchState((int)AIState.Second_DecideAttack);
                    }
                }
                else if (Timer >= smashStartTimer && Main.netMode != NetmodeID.Server) { // 下砸中
                    NPC.position += NPC.netOffset;
                    for (int m = 0; m < 4; m++) {
                        Vector2 position = NPC.Bottom - new Vector2(Main.rand.NextFloatDirection() * 16f, Main.rand.Next(8));
                        var d = Dust.NewDustDirect(position, 2, 2, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, GetDustColor(), 1.4f);
                        d.position = position;
                        d.noGravity = true;
                        d.velocity.Y = NPC.velocity.Y * 0.9f;
                        d.velocity.X = ((Main.rand.NextBool(2)) ? (-10f) : 10f) + Main.rand.NextFloatDirection() * 3f;
                    }
                    NPC.position -= NPC.netOffset;
                }

                NPC.velocity.X *= 0.7f;
                float oldAI1 = Timer;
                if (++Timer >= smashStartTimer) { // 下砸过程
                    if (oldAI1 < smashStartTimer)
                        NPC.netUpdate = true;

                    NPC.velocity.Y += SmashSpeed * 1.2f;

                    if (NPC.velocity.Y == 0f)
                        NPC.velocity.Y = 0.01f;

                    if (NPC.Distance(RoomBottom) <= SmashSpeedMax * 8f) {
                        NPC.velocity.Y = SmashSpeedMax * 1.4f;
                        return;
                    }
                }
                else {
                    NPC.velocity.Y *= 0.001f;
                }

                return;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Timer == 0f) {
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }

            Timer++;

            if (Main.netMode != NetmodeID.Server) {
                float r = 70f;
                NPC.position += NPC.netOffset;
                for (int k = 0; k < 4; k++) {
                    Vector2 vector2 = NPC.Center + Main.rand.NextVector2CircularEdge(r, r);
                    Vector2 v = (vector2 - NPC.Center).SafeNormalize(Vector2.Zero) * -8f;
                    var d = Dust.NewDustDirect(vector2, 2, 2, DustID.Smoke, v.X, v.Y, (int)MathHelper.Lerp(120f, 190f, Main.rand.NextFloat()), GetDustColor(), 1.8f);
                    d.position = vector2;
                    d.noGravity = true;
                    d.velocity = v;
                    d.customData = this;
                }
                NPC.position -= NPC.netOffset;
            }
            if (!(Timer >= 30)) // 30之前是在原地不动，属于是一个准备阶段吧
                return;

            // 触发下砸
            if (Math.Abs(NPC.Center.X - RoomBottom.X) / 16f <= smashStartDistanceX && RoomBottom.Y - NPC.Center.Y >= smashStartDistanceY - 80) {
                Timer = 60f;
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Timer = 0f;
                    NPC.ai[2] = 1f;
                    NPC.velocity.Y = -0.8f;
                    NPC.netUpdate = true;
                }
                return;
            }

            var target = RoomBottom;
            target.Y -= smashStartDistanceY;
            NPC.velocity = target - NPC.Center;
            float speed = NPC.Distance(RoomBottom) <= 240 ? 20f : 24f;
            NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero) * speed;
        }

        private void Second_DecideAttackState() {
            NPC.TargetClosest();
            NPC.defense = NPC.defDefense;

            if (!Player.active || Player.dead) {
                SwitchState((int)AIState.Despawn);
                return;
            }

            // wal: 墙石矛，后面数字代表模式 (正左负右, 1上2下3全部)
            // dir: 对玩家石矛，长度4为有预判，为3无预判
            // sma: 下砸一套的动作
            // das(L/R): 从左/右边瞬移冲刺，不填哪一边随机
            // hnd: 石手
            // pfh(X): 平台石手，数字为长度
            // spd: 快速石手
            // wat: 等待1秒
            // act/dea(1,2,3)/(P) 虚化/反虚化，数字对应层数，为P虚化玩家脚下一层
            List<string> StageTwoOrder = new() {
                "wal-3", "wat", "dasL", "act3", "sma", "dir", "dire",
                "wal1", "act1", "dea2", "wal-2", "dea1",
                "dea3", "wat", "wat", "hnd", "wat", "dir", "dir", "dire", "pfh2", "wat", "sma", "sma",
                "sma", "dasL", "dire", "dire", "dasR", "act1", "act2", "sma", "wal3", "wat", "wat",
                "dea1", "dea2", "dasR", "sma",
                "dir", "actP", "pfh3", "das", "dir",
                "deaP", "spd"
            };

            if (!StageTwoOrder.IndexInRange(AttackIndex))
                AttackIndex = 0;
            //Main.NewText(AttackIndex + " " + StageOneOrder[AttackIndex]);
            string command = StageTwoOrder[AttackIndex];
            if (command.StartsWith("wal")) {
                NPC.ai[2] = 0;
                NPC.ai[3] = int.Parse(command[3..]);
                SwitchState((int)AIState.Second_WallSpear);
            }
            if (command.StartsWith("dir")) {
                if (command.Length >= 4)
                    NPC.ai[2] = 114514f;
                SwitchState((int)AIState.Second_Spear);
            }
            if (command == "sma") {
                NPC.defense = 0;
                SwitchState((int)AIState.Second_Smash);
            }
            if (command.StartsWith("das")) {
                if (command == "dasL")
                    NPC.ai[2] = -1;
                else if (command == "dasR")
                    NPC.ai[2] = 1;
                else
                    NPC.ai[2] = Main.rand.NextBool() ? 1 : -1;
                NPC.defense = 0;
                SwitchState((int)AIState.Second_Dash);
            }
            if (command == "hnd") {
                NPC.defense = NPC.defDefense + 10;
                SwitchState((int)AIState.Second_StoneHand);
            }
            if (command.StartsWith("pfh")) {
                NPC.ai[3] = int.Parse(command[3..]);
                SwitchState((int)AIState.Second_PlatformHand);
            }
            if (command == "spd") {
                SwitchState((int)AIState.Second_StoneHandSpeed);
            }
            if (command == "wat") {
                SwitchState((int)AIState.Waiting);
            }
            if (command.StartsWith("act")) {
                if (command == "actP") {
                    int distance = Player.Top.ToTileCoordinates16().Y - (ImmortalGolemRoom.BossZone.Top + 10);
                    if (distance <= 22)
                        AthanasySystem.ActuatePlatforms(true, 1);
                    else if (distance <= 42)
                        AthanasySystem.ActuatePlatforms(true, 2);
                    else if (distance <= 62)
                        AthanasySystem.ActuatePlatforms(true, 3);
                }
                else {
                    AthanasySystem.ActuatePlatforms(true, int.Parse(command[3..]));
                }
            }
            if (command.StartsWith("dea")) {
                if (command == "deaP") {
                    int distance = Player.Top.ToTileCoordinates16().Y - (ImmortalGolemRoom.BossZone.Top + 10);
                    if (distance <= 22)
                        AthanasySystem.ActuatePlatforms(false, 1);
                    else if (distance <= 42)
                        AthanasySystem.ActuatePlatforms(false, 2);
                    else if (distance <= 62)
                        AthanasySystem.ActuatePlatforms(false, 3);
                }
                else {
                    AthanasySystem.ActuatePlatforms(false, int.Parse(command[3..]));
                }
            }
            AttackIndex++;
        }

        private void Second_StoneHandState() {
            Timer++;

            Rectangle rect = ImmortalGolemRoom.BossZone;
            rect.Offset(9, 9);
            rect = rect.OffsetSize(-18, -18);

            NPC.noTileCollide = true;
            NPC.noGravity = true;

            if (Timer == 1) {
                for (int i = 0; i < NPC.oldPos.Length; i++) {
                    NPC.oldPos[i] = Vector2.Zero;
                }
                NPC.Center = Player.Center + new Vector2(0, -400);
                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;

                foreach (var player in Main.player) {
                    if (rect.ToWorldCoordinates().Contains(player.Center.ToPoint())) {
                        player.AddBuff(BuffID.Obstructed, 100);
                    }
                    break;
                }
            }
            // 为啥要分开呢？我还是不知道
            if (Timer == 2 && Main.netMode != NetmodeID.Server) {
                NPC.position += NPC.netOffset;
                for (float r = 0; r <= 6.28f; r += 6.28f / 50f) {
                    Vector2 velocity = Vector2.UnitY.RotatedBy(r) * 10f;
                    var d = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BubbleCopy>(), velocity, 180, default, 1.9f);
                    d.fadeIn = 1.2f;
                    d.noGravity = true;
                }
                NPC.position -= NPC.netOffset;
            }
            if (Timer <= 40f && NPC.ai[2] < 30f) {
                for (int i = 0; i < 50; i++) {
                    Point tilePosition = Main.rand.NextVector2FromRectangle(rect).ToPoint();
                    var tile = Framing.GetTileSafely(tilePosition);
                    var tileUp = Framing.GetTileSafely(new Point(tilePosition.X, tilePosition.Y - 1));
                    var tileLeft = Framing.GetTileSafely(new Point(tilePosition.X - 1, tilePosition.Y));
                    var tileRight = Framing.GetTileSafely(new Point(tilePosition.X + 1, tilePosition.Y));
                    // 平整物块表面
                    if (IsSaveTile(tile) && IsSaveTile(tileLeft) && IsSaveTile(tileRight) && !WorldGen.SolidTile(tileUp)) {
                        var spawnPosition = tilePosition.ToWorldCoordinates(autoAddY: 2f).ToPoint();
                        NPC.ai[2]++;

                        var n = NPC.NewNPCDirect(NPC.GetSource_FromAI(), spawnPosition.X, spawnPosition.Y, StoneHandType, ai1: Timer, ai2: -1, target: NPC.target);
                        foreach (var player in Main.player) {
                            if (rect.ToWorldCoordinates().Contains(player.Center.ToPoint()) && player.Hitbox.Intersects(n.Hitbox)) {
                                n.active = false;
                                n.life = 0;
                            }
                            break;
                        }
                    }
                }
            }
            if (Timer == 160) {
                SwitchState((int)AIState.Second_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        private static bool IsSaveTile(Tile t) => (WorldGen.SolidTile(t) || Main.tileSolidTop[t.TileType]) && t.HasUnactuatedTile;

        private void Second_DashState() {
            Timer++;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (Timer == 1) {
                for (int i = 0; i < NPC.oldPos.Length; i++) {
                    NPC.oldPos[i] = Vector2.Zero;
                }
                NPC.Center = Player.Center + new Vector2(600, 0) * NPC.ai[2];
                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;
                NPC.direction = Math.Sign(Player.Center.X - NPC.Center.X);
            }
            // 为啥要分开呢？我也不知道
            if (Timer == 2 && Main.netMode != NetmodeID.Server) {
                NPC.position += NPC.netOffset;
                for (float r = 0; r <= 6.28f; r += 6.28f / 50f) {
                    Vector2 velocity = Vector2.UnitY.RotatedBy(r) * 10f;
                    var d = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BubbleCopy>(), velocity, 180, default, 1.6f);
                    d.fadeIn = 1.2f;
                    d.noGravity = true;
                }
                NPC.position -= NPC.netOffset;
            }
            if (Timer <= 23) {
                NPC.velocity.X -= NPC.direction * 0.8f;
                NPC.oldPos[0] = Vector2.Zero; // 不要残影捏
            }

            if (Timer == 50)
                NPC.velocity.X = NPC.direction * 32f;
            if (Timer >= 24 && Timer < 50) {
                float factor = Utils.GetLerpValue(24, 50, Timer);
                float speed = Utils.MultiLerp(factor * factor, 2f, 17f, 25f, 27f, 32f);
                NPC.velocity.X = NPC.direction * speed;
            }

            if (Timer >= 72) {
                NPC.velocity.X *= 0f;
                SwitchState((int)AIState.Second_DecideAttack);
                NPC.ai[2] = 0;
            }
        }

        private void Second_SmashState() {
            HitPlayer = true;

            int smashStartDistanceY = 420;
            int smashStartDistanceX = 8;
            int smashStartTimer = 15; // 下砸起始计时器
            //int smashDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (NPC.ai[2] == 1f) { // ai[2]为1时是下砸
                NPC.noTileCollide = Player.Bottom.Y - NPC.Bottom.Y > 32;
                NPC.noGravity = false;

                if (Main.getGoodWorld) // GoodWorld是ftw种子
                    smashStartTimer = 0;

                // 雾气效果，尽量提前几帧出
                if (NPC.velocity.Y <= 8f && Timer >= (float)smashStartTimer + 5)
                    for (int l = 0; l < 30; l++) // 粒子效果
                        Dust.NewDustDirect(NPC.Bottom - new Vector2(NPC.width * 0.8f, 20f), (int)(NPC.width * 1.6f), 50, ModContent.DustType<Sand>(), 0, Main.rand.NextFloat(-5f, -1f), Main.rand.Next(255), default, Main.rand.NextFloat(1f));

                if (NPC.velocity.Y == 0f) { // 下砸到地了
                    PunchCameraModifier modifier4 = new(NPC.Center, -Vector2.UnitY, 12f, 10.8f, 20, 1300f, "Entrogic: Athanasy");
                    Main.instance.CameraModifiers.Add(modifier4);
                    SwitchState((int)AIState.Second_DecideAttack);
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    //    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom, Vector2.Zero, ProjectileID.QueenSlimeSmash, smashDamage, 0f, Main.myPlayer);
                }
                else if (Timer >= smashStartTimer) { // 下砸中
                    for (int m = 0; m < 4; m++) {
                        Vector2 position = NPC.Bottom - new Vector2(Main.rand.NextFloatDirection() * 16f, Main.rand.Next(8));
                        var d = Dust.NewDustDirect(position, 2, 2, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, GetDustColor(), 1.4f);
                        d.position = position;
                        d.noGravity = true;
                        d.velocity.Y = NPC.velocity.Y * 0.9f;
                        d.velocity.X = ((Main.rand.NextBool(2)) ? (-10f) : 10f) + Main.rand.NextFloatDirection() * 3f;
                    }
                }

                // 向下的圆形粒子
                if (Timer == 6 && Main.netMode != NetmodeID.Server) {
                    NPC.position += NPC.netOffset;
                    // 别问为什么+unit，本代码依赖Bug运行
                    float unit = 3.14f / 20f;
                    for (float r = 0f; r <= 3.14f + unit; r += unit) {
                        var velocity = r.ToRotationVector2() * 10f;
                        var spawnCenter = NPC.Center;
                        spawnCenter.Y += 50f;
                        var d = Dust.NewDustPerfect(spawnCenter, ModContent.DustType<BubbleCopy>(), velocity, 180, default, 2.2f);
                        d.fadeIn = 1.2f;
                        d.noGravity = true;
                    }
                    NPC.position -= NPC.netOffset;
                }

                NPC.velocity.X *= 0.7f;
                float oldTimer = Timer;
                Timer += 1f;
                if (Timer >= smashStartTimer) { // 下砸过程
                    if (oldTimer < smashStartTimer)
                        NPC.netUpdate = true;

                    NPC.velocity.Y += SmashSpeed;

                    if (NPC.velocity.Y == 0f)
                        NPC.velocity.Y = 0.01f;

                    if (NPC.velocity.Y >= SmashSpeedMax)
                        NPC.velocity.Y = SmashSpeedMax;
                }
                else {
                    NPC.velocity.Y *= 0.8f;
                }
                if (!NPC.noTileCollide) {
                    // 卡在两帧位置中间多加一个碰撞判定，这样就不会下到玩家下一层平台那里去了
                    bool throughPlatform = Player.Bottom.Y - NPC.Bottom.Y > 32;
                    NPC.velocity = Collision.TileCollision(NPC.position + NPC.velocity * 0.5f, NPC.velocity, NPC.width, NPC.height, throughPlatform, throughPlatform);
                }

                return;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Timer == 0f) {
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }

            Timer++;

            float radius = 70f;
            for (int k = 0; k < 4; k++) {
                Vector2 vector2 = NPC.Center + Main.rand.NextVector2CircularEdge(radius, radius);

                Vector2 v = (vector2 - NPC.Center).SafeNormalize(Vector2.Zero) * -8f;
                var d = Dust.NewDustDirect(vector2, 2, 2, DustID.Smoke, v.X, v.Y, (int)MathHelper.Lerp(120f, 190f, Main.rand.NextFloat()), GetDustColor(), 1.8f);
                d.position = vector2;
                d.noGravity = true;
                d.velocity = v;
                d.customData = this;
            }
            if (!(Timer >= 10f)) // 10之前是在原地不动，属于是一个准备阶段吧
                return;

            // 开始下砸
            if (Math.Abs(NPC.Center.X - Player.Center.X) / 16f <= smashStartDistanceX && Player.Center.Y - NPC.Center.Y >= smashStartDistanceY - 80) {
                Timer = 60f;
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    Timer = 0f;
                    NPC.ai[2] = 1f;
                    NPC.velocity.Y = -3f;
                    NPC.netUpdate = true;
                }
                return;
            }

            // 下面的是飞到玩家上方代码
            Vector2 center = NPC.Center;
            if (!Player.dead && Player.active && Math.Abs(NPC.Center.X - Player.Center.X) / 16f <= DespawnDistance) // 这里是如果离得太远或玩家死了的话就跑路了
                center = Player.Center;

            center.Y -= smashStartDistanceY;
            NPC.velocity = center - NPC.Center;
            float speed = NPC.Distance(RoomBottom) <= 160 ? 12f : 36f;
            NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero) * speed;

            HitPlayer = false;
        }

        private void Second_SpearState() {
            Timer++;
            if (Timer == 1 && Main.netMode != NetmodeID.Server) {
                NPC.position += NPC.netOffset;
                for (float r = 0; r <= 6.28f; r += 6.28f / 50f) {
                    Vector2 velocity = Vector2.UnitY.RotatedBy(r) * 10f;
                    var d = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BubbleCopy>(), velocity, 180, default, 1.6f);
                    d.fadeIn = 1.2f;
                    d.noGravity = true;
                }
                NPC.position -= NPC.netOffset;
            }

            int spearDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(80f, 150f, 190f);
            NPC.ai[2] = 114514;
            Vector2 target = Player.Center;
            if (NPC.ai[2] == 114514f) // 预判
                target += Player.velocity * (60f + NPC.Distance(target) / 20f);

            var toPlayerVector = NPC.DirectionTo(target);

            if (Timer == 10 && Main.netMode != NetmodeID.MultiplayerClient) {
                float distanceFromCenter = 120f;
                int spears = Main.expertMode ? 12 : 10; // 矛数
                if (Main.getGoodWorld) { // ftw
                    spears += 4;
                    distanceFromCenter += 20f;
                }

                for (float r = 0; r < 6.28f; r += 6.28f / spears) {
                    Vector2 rotatedVector = toPlayerVector.RotatedBy(r);
                    Vector2 finalSpawnPosition = rotatedVector * distanceFromCenter + NPC.Center;
                    var p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), finalSpawnPosition, toPlayerVector, SmallSpearType, spearDamage, 0f);
                    p.tileCollide = false;
                }
            }
            if (Timer == 20) {
                SwitchState((int)AIState.Second_DecideAttack);
                NPC.netUpdate = true;
            }
        }

        private void Second_WallSpearState() {
            if (Timer == 0) {
                NPC.netUpdate = true;
                NPC.ai[2] = Main.rand.Next(11); // 随机化生成
            }
            if (Timer == 1 && Main.netMode != NetmodeID.Server) {
                NPC.position += NPC.netOffset;
                for (float r = 0; r <= 6.28f; r += 6.28f / 50f) {
                    Vector2 velocity = Vector2.UnitY.RotatedBy(r) * 16f;
                    var d = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BubbleCopy>(), velocity, 180, default, 2.6f);
                    d.fadeIn = 1.2f;
                    d.noGravity = true;
                }
                NPC.position -= NPC.netOffset;
            }
            // 正左负右, 1上2下3全部
            bool left = NPC.ai[3] > 0;
            bool up = Math.Abs(NPC.ai[3]) == 1;
            bool allWall = Math.Abs(NPC.ai[3]) == 3;

            Timer++;

            int spearDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(80f, 150f, 190f);

            int top = ImmortalGolemRoom.BossZone.Top + 10;
            int bottom = ImmortalGolemRoom.BossZone.Bottom - 10;
            if (!allWall) {
                if (up)
                    bottom -= (bottom - top) / 2;
                else
                    top += (bottom - top) / 2;
            }
            int distance = bottom - top;

            if (Timer % 11 == NPC.ai[2] && Main.netMode != NetmodeID.MultiplayerClient) {
                Vector2 finalSpawnPosition = new(ImmortalGolemRoom.BossZone.Left + 9, Timer + top);
                if (!left)
                    finalSpawnPosition.X = ImmortalGolemRoom.BossZone.Right - 9;
                finalSpawnPosition = finalSpawnPosition.ToWorldCoordinates();
                Projectile.NewProjectile(NPC.GetSource_FromAI(), finalSpawnPosition, new(left ? 1 : -1, 0), WallSpearType, spearDamage, 0f);
            }

            if (Timer > distance) {
                SwitchState((int)AIState.Second_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        private void Second_PlatformHandState() {
            Timer++;
            if (!Player.active || Player.dead) {
                SwitchState((int)AIState.Despawn);
                return;
            }

            Point16 center = Player.Center.ToTileCoordinates16();
            for (int i = 0; i < 30; i++) {
                Point tilePosition = center.ToPoint();
                tilePosition.Y += i;
                var tile = Framing.GetTileSafely(tilePosition);
                if (IsSaveTile(tile)) {
                    var tileUp = Framing.GetTileSafely(new Point(tilePosition.X, tilePosition.Y - 1));
                    var tileLeft = Framing.GetTileSafely(new Point(tilePosition.X - 1, tilePosition.Y));
                    var tileRight = Framing.GetTileSafely(new Point(tilePosition.X + 1, tilePosition.Y));
                    if (IsSaveTile(tileLeft) && IsSaveTile(tileRight) && !WorldGen.SolidTile(tileUp)) {
                        var spawnPosition = tilePosition.ToWorldCoordinates(autoAddY: 2f).ToPoint();
                        NPC.NewNPC(NPC.GetSource_FromAI(), spawnPosition.X, spawnPosition.Y, StoneHandType, ai1: 80f, ai2: NPC.Center.X, ai3: NPC.Center.Y, Target: NPC.target);
                        NPC.ai[2] = 0;
                        GenerateHand(tilePosition.X - 2, tilePosition.Y, -1);
                        NPC.ai[2] = 0;
                        GenerateHand(tilePosition.X + 2, tilePosition.Y, 1);
                        break;
                    }
                }
            }

            SwitchState((int)AIState.Second_DecideAttack);
            NPC.netUpdate = true;
            NPC.ai[2] = 0f;
            NPC.ai[3] = 0f;
        }

        private void Second_StoneHandSpeedState() {
            Timer++;
            if (!Player.active || Player.dead) {
                SwitchState((int)AIState.Despawn);
                return;
            }

            int handTimer = Main.expertMode ? 10 : 30;
            if (Timer % handTimer == 0) {
                Point16 center = Player.Center.ToTileCoordinates16();
                for (int i = 0; i < 30; i++) {
                    Point tilePosition = center.ToPoint();
                    tilePosition.Y += i;
                    var tile = Framing.GetTileSafely(tilePosition);
                    if (IsSaveTile(tile)) {
                        var tileUp = Framing.GetTileSafely(new Point(tilePosition.X, tilePosition.Y - 1));
                        var tileLeft = Framing.GetTileSafely(new Point(tilePosition.X - 1, tilePosition.Y));
                        var tileRight = Framing.GetTileSafely(new Point(tilePosition.X + 1, tilePosition.Y));
                        if (IsSaveTile(tileLeft) && IsSaveTile(tileRight) && !WorldGen.SolidTile(tileUp)) {
                            var spawnPosition = tilePosition.ToWorldCoordinates(autoAddY: 2f).ToPoint();
                            NPC.NewNPC(NPC.GetSource_FromAI("Stage2"), spawnPosition.X, spawnPosition.Y, StoneHandType, ai1: 40f, ai2: NPC.Center.X, ai3: NPC.Center.Y, Target: NPC.target);
                            break;
                        }
                    }
                }
            }
            if (Timer == 160) {
                SwitchState((int)AIState.Second_DecideAttack);
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }
        }

        internal void GenerateHand(int i, int j, int direction) {
            if (!WorldGen.InWorld(i, j) || NPC.ai[2] >= NPC.ai[3])
                return;
            if (IsSaveTile(Main.tile[i, j])) {
                var spawnPosition = new Point(i, j).ToWorldCoordinates(autoAddY: 2f).ToPoint();
                NPC.ai[2]++;
                NPC.NewNPC(NPC.GetSource_FromAI(), spawnPosition.X, spawnPosition.Y, StoneHandType, ai1: 80f, ai2: NPC.Center.X, ai3: NPC.Center.Y, Target: NPC.target);
                GenerateHand(i + direction * 2, j, direction);
            }
        }

        #endregion

        private static Color GetDustColor() {
            Color c = Color.Lerp(new Color(210, 210, 210), new Color(160, 160, 160), Main.rand.NextFloat());
            return Color.Lerp(Color.Gray, c, Main.rand.NextFloat());
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            base.CanHitPlayer(target, ref cooldownSlot);
            return HitPlayer;
        }

        public void DrawBorder(Texture2D texture, Vector2 drawPos, Vector2 origin, SpriteEffects spriteEffects) {
            float time = Main.GlobalTimeWrappedHourly;

            time %= 4f;
            time /= 2f;

            if (time >= 1f) {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            Color color1 = new(90, 70, 255, 50);
            Color color2 = new(140, 120, 255, 77);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            GameShaders.Armor.Apply(ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex, NPC, null);

            for (float i = 0f; i < 1f; i += 0.25f) {
                float radians = (i + time) * MathHelper.TwoPi;

                Main.EntitySpriteDraw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, null, color1, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f) {
                float radians = (i + time) * MathHelper.TwoPi;

                Main.EntitySpriteDraw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, null, color2, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            if (!NPC.IsABestiaryIconDummy) {
                drawColor = NPC.GetAlpha(Lighting.GetColor(NPC.Center.ToTileCoordinates()));

                Texture2D t = TextureAssets.Npc[NPC.type].Value;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (NPC.spriteDirection == 1) {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                float extraDrawY = Main.NPCAddHeight(NPC);
                var origin = new Vector2(t.Width / 2, t.Height / Main.npcFrameCount[NPC.type] / 2);
                var pos = new Vector2(NPC.position.X - screenPos.X + NPC.width / 2 - (float)t.Width * NPC.scale / 2f + origin.X * NPC.scale,
                    NPC.position.Y - screenPos.Y + NPC.height - t.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + extraDrawY + origin.Y * NPC.scale + NPC.gfxOffY);

                Vector2 halfSize = new(t.Width / 2, t.Height / Main.npcFrameCount[Type] / 2);
                int shadowCounts = 7;
                for (int i = 1; i < shadowCounts; i += 2) {
                    Color shadowColor = NPC.GetAlpha(drawColor);
                    shadowColor *= (float)(shadowCounts - i) / 15f;
                    Vector2 position19 = NPC.oldPos[i] + NPC.Size / 2f - screenPos;
                    position19 -= new Vector2(t.Width, t.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    position19 += halfSize * NPC.scale + new Vector2(0f, extraDrawY + NPC.gfxOffY);
                    Main.EntitySpriteDraw(t, position19, NPC.frame, shadowColor, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0);
                }

                DrawBorder(t, pos, origin, spriteEffects);

                // 绘制NPC
                var npcColor = NPC.GetNPCColorTintedByBuffs(drawColor);
                Main.EntitySpriteDraw(t, pos, NPC.frame, NPC.GetAlpha(npcColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                if (NPC.color != default) {
                    Main.EntitySpriteDraw(t, pos, NPC.frame, NPC.GetColor(npcColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                }

                return false;
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Athanasy, The Immortal Golem");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "安塔尼希·衰落魔像");
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.TrailCacheLength[Type] = 7;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { //Influences how the NPC looks in the Bestiary
                Position = new Vector2(0f, 40f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 80f,
                PortraitScale = 1.2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            int indexQueenBee = 0;
            for (int i = 0; i < NPCID.Sets.BossBestiaryPriority.Count; i++) {
                if (NPCID.Sets.BossBestiaryPriority[i] == NPCID.QueenBee) {
                    indexQueenBee = i;
                    break;
                }
            }
            // 尽量把我们的Boss插入到蜂后图鉴后面，因为原版Boss图鉴是根据时期排序的
            if (indexQueenBee != 0) {
                // 倒着插入，因为每次都是插入到蜂后后面
                NPCID.Sets.BossBestiaryPriority.Insert(indexQueenBee, NPC.type);
            }
            // 如果有事故找不到，没办法，放在最后面
            else {
                NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("Mods.Entrogic.Bestiary.Athanasy")
            });
        }

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.Golem);
            NPC.aiStyle = -1;
            NPC.lifeMax = 5000;
            NPC.damage = 52;
            NPC.defense = 25;
            NPC.width = 80;
            NPC.height = 120;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.lavaImmune = true;
            NPC.behindTiles = false;
            NPC.hide = false;
            NPC.SpawnWithHigherTime(30);
            NPC.npcSlots = 5f;
            NPC.Opacity = 1f;
            NPC.hide = true; // 我们用自己的DrawNPC绘制
            //music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheStormy");
            Music = MusicID.Boss2;
            if (Main.getGoodWorld)
                NPC.scale = 2f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            float bossAdjustment = 1f;
            if (Main.masterMode)
                bossAdjustment = 0.85f;
            NPC.lifeMax = (int)(NPC.lifeMax * 0.73f * bossLifeScale * bossAdjustment);
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.HealingPotion;
        }
    }
}
