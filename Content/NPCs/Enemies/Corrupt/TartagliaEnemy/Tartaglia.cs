using Entrogic.Content.Projectiles.Enemies;
using Terraria.WorldBuilding;

namespace Entrogic.Content.NPCs.Enemies.Corrupt.TartagliaEnemy
{
    public class Tartaglia : ModNPC
    {
        internal Asset<Texture2D> LegsTexture;
        internal Asset<Texture2D> BodyTexture;
        internal Dictionary<string, Asset<Texture2D>> WeaponsTexture = new();
        internal Rectangle handFrame = new(0, 0, 40, 56);
        internal Vector2 bodyOffset = new();
        internal Rectangle bodyFrame = new(0, 0, 40, 56);
        internal Rectangle shoulderFrame = new(0, 0, 40, 56);
        internal Rectangle secondHandFrame = new(0, 0, 40, 56);
        internal bool useExtraHand = false;

        public override bool? CanFallThroughPlatforms() {
            if (NPC.HasPlayerTarget && Player.Exists() && Player.Bottom.Y - NPC.Bottom.Y >= 30)
                return true;
            return base.CanFallThroughPlatforms();
        }

        public override void SetStaticDefaults() {
            Main.npcFrameCount[Type] = 20;
        }

        // 在血腐地刷出
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (TileID.Sets.Corrupt[spawnInfo.SpawnTileType]) {
                return 0.006f;
            }
            return 0f;
        }

        public override void SetDefaults() {
            base.SetDefaults();
            NPC.CloneDefaults(NPCID.PossessedArmor);
            NPC.aiStyle = -1;
            NPC.lifeMax = 120;
            NPC.defense = 10;
            NPC.damage = 85;
            NPC.knockBackResist = 0.4f;
            WeaponAniTimer = -999;
        }

        enum Weapons
        {
            Crossbow,
            Cannon,
            Potion,
            Glowstick
        }


        internal int RocketSoundDelay;
        internal const int RocketMaxTime = 60;
        internal ref float Timer => ref NPC.ai[0];
        internal ref float RocketTimer => ref NPC.ai[1];
        internal ref float ShootTimer => ref NPC.ai[2];
        internal ref float ShootRotation => ref NPC.ai[3];
        internal ref float PathfindingTimer => ref NPC.localAI[0];
        internal ref float WeaponAniTimer => ref NPC.localAI[1];
        internal ref float WeaponType => ref NPC.localAI[2];
        internal ref float TeleportTimer => ref NPC.localAI[3];
        internal List<PathVertex> path = new();

        private Player Player => Main.player[NPC.target];

        public override void SendExtraAI(BinaryWriter writer) {
            base.SendExtraAI(writer);
            writer.Write(PathfindingTimer);
            writer.Write(WeaponAniTimer);
            writer.Write(WeaponType);
            writer.Write(TeleportTimer);

            writer.Write(path.Count);
            for (int i = 0; i <= path.Count - 1; i++) {
                writer.Write(path[i].x);
                writer.Write(path[i].y);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            base.ReceiveExtraAI(reader);
            PathfindingTimer = reader.ReadSingle();
            WeaponAniTimer = reader.ReadSingle();
            WeaponType = reader.ReadInt32();
            TeleportTimer = reader.ReadInt32();

            path.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i <= count - 1; i++) {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                PathVertex p = new(x, y);
                path.Add(p);
            }
        }

        public override void AI() {
            // 初始潜行状态
            if (WeaponAniTimer == -999) {
                NPC.Opacity = .2f;
                NPC.velocity.X *= 0.92f;
                // 检测到玩家，发炮并开始移动
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    foreach (var plr in from p in Main.player where p.active && !p.dead && NPC.Distance(p.Center) <= 600 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, p.position, p.width, p.height) select p) {
                        Vector2 toPlayer = plr.Center - NPC.Center;
                        int damage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(90, 150, 260);
                        ShootRotation = toPlayer.ToRotation();

                        var cannon = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Normalize(toPlayer) * 4f, ModContent.ProjectileType<LavaCannonHostile>(), damage, 0f, Main.myPlayer);
                        cannon.timeLeft = (int)(toPlayer.Length() / 4f);

                        ShootTimer = 60;
                        WeaponAniTimer = 35;
                        WeaponType = (int)Weapons.Cannon;
                        NPC.target = plr.whoAmI;
                        NPC.netUpdate = true;
                        break;
                    }
                }
                return;
            }

            NPC.Opacity = 1f;
            if (WeaponAniTimer > 0)
                WeaponAniTimer--;

            var targetCenter = Player.Center;
            var targetPosition = Player.position;

            // 在玩家死亡点上方跳三下并且丢出三个荧光棒
            Despawning:
            if (Timer == 114514) {
                NPC.velocity.X = 0f;
                if (TeleportTimer == 0) {
                    SoundEngine.PlaySound(SoundID.Item3, NPC.Center);
                    WeaponAniTimer = 21;
                    WeaponType = (int)Weapons.Potion;
                    // 喝药水的流的粒子
                    for (int i = 0; i < 12; i++) {
                        var drinkOffset = new Vector2(7 * NPC.spriteDirection - 3, -10);
                        var d = Dust.NewDustDirect(NPC.Center + drinkOffset, 6, 6, DustID.FoodPiece, newColor: new Color(169, 83, 170));
                        d.velocity.X *= 0.1f;
                        d.velocity.Y *= 0.5f;
                    }
                    Teleport(Player.Bottom);
                }
                TeleportTimer++;
                if (TeleportTimer % 60 == 0) {
                    Jump(5);
                    WeaponAniTimer = 15;
                    WeaponType = (int)Weapons.Glowstick;
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-1, 1), -7), ProjectileID.Glowstick, 0, 0f, Main.myPlayer);
                    }
                }
                if (TeleportTimer == 210) {
                    // 装作瞬移实际上是直接消失的离开
                    SoundEngine.PlaySound(SoundID.Item3, NPC.Center);
                    WeaponAniTimer = 21;
                    WeaponType = (int)Weapons.Potion;
                    // 喝药水的流的粒子
                    for (int i = 0; i < 12; i++) {
                        var drinkOffset = new Vector2(7 * NPC.spriteDirection - 3, -10);
                        var d = Dust.NewDustDirect(NPC.Center + drinkOffset, 6, 6, DustID.FoodPiece, newColor: new Color(169, 83, 170));
                        d.velocity.X *= 0.1f;
                        d.velocity.Y *= 0.5f;
                    }
                }
                if (TeleportTimer == 215) {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                    for (int i = 0; i < 30; i++) {
                        var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.FoodPiece, 0, 0, 200, new Color(169, 83, 170), 1.5f);
                        d.noGravity = true;
                        d.velocity.X *= 1.5f;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        NPC.active = false;
                        if (Main.netMode == NetmodeID.Server) {
                            NPC.netSkip = -1;
                            NPC.life = 0;
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                        }
                    }
                }
                return;
            }
            if (NPC.target < 0 || NPC.target >= Main.maxPlayers || Player.dead || !Player.active) {
                NPC.TargetClosest();
                // 玩家至少曾经有过
                if (Player.dead || !Player.active) {
                    Timer = 114514;
                    TeleportTimer = 0;
                    goto Despawning;
                }
                // 装作瞬移实际上是直接消失的离开，
                if (NPC.target < 0 || NPC.target >= Main.maxPlayers) {
                    Timer = 114514;
                    TeleportTimer = 209;
                    goto Despawning;
                }
            }
            if (Player.Bottom.Y == NPC.Bottom.Y)
                NPC.directionY = -1;

            // 在玩家一定范围内且可以射箭打到玩家
            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetPosition, Player.width, Player.height)) {
                TeleportTimer = 0;
                // 发射毒箭
                ShootTimer++;
                if (Main.netMode != NetmodeID.MultiplayerClient && ShootTimer % 300 < 60 && ShootTimer % 6 == 0) {
                    Vector2 toPlayer = targetCenter + Player.velocity * NPC.Distance(Player.Center) / 15f + new Vector2(0, (int)-Math.Abs(targetCenter.X - NPC.Center.X) >> 3) - NPC.Center;
                    toPlayer.Normalize();
                    toPlayer = (toPlayer.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f))).ToRotationVector2();
                    int damage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(15, 24, 35);
                    ShootRotation = toPlayer.ToRotation();
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, toPlayer * 15f, ModContent.ProjectileType<PoisonedArrowHostile>(), damage, 0f, Main.myPlayer);
                    NPC.netUpdate = true;
                    // 设置武器
                    WeaponAniTimer = 7;
                    WeaponType = (int)Weapons.Crossbow;
                }
                // 硬核保持距离
                if (NPC.Distance(Player.Center) <= 260) {
                    NPC.velocity.X *= 0.92f;
                    NPC.direction = NPC.spriteDirection = Math.Sign(targetCenter.X - NPC.Center.X);
                    return;
                }
            }
            // 打不到而且也没有可用路径
            else if (path.Count == 0) {
                // 增加瞬移计时器
                TeleportTimer++;
                // 喝下药水，5帧后瞬移
                if (TeleportTimer >= 240) {
                    SoundEngine.PlaySound(SoundID.Item3, NPC.Center);
                    TeleportTimer = 0;
                    WeaponAniTimer = 21;
                    WeaponType = (int)Weapons.Potion;
                    // 喝药水的流的粒子
                    for (int i = 0; i < 12; i++) {
                        var drinkOffset = new Vector2(7 * NPC.spriteDirection - 3, -10);
                        var d = Dust.NewDustDirect(NPC.Center + drinkOffset, 6, 6, DustID.FoodPiece, newColor: new Color(169, 83, 170));
                        d.velocity.X *= 0.1f;
                        d.velocity.Y *= 0.5f;
                    }
                }
            }
            if (WeaponType == (int)Weapons.Potion && WeaponAniTimer == 16) {
                NPC.velocity *= 0f;
                NPC.position += NPC.netOffset;
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                Teleport(null);
                ShootTimer = 60; // 不要立马开枪
            }

            float MaxXSpeed = 7f; // 最大速度
            const float AccelerateSpeed = 0.086f; // 加速度
            const float AccelerateSpeedRotating = 0.8f; // 转向加速度

            // 向左
            void AcceLeft() {
                NPC.velocity.X -= AccelerateSpeed;
                if (NPC.velocity.X <= -MaxXSpeed) {
                    NPC.velocity.X = -MaxXSpeed;
                }
                if (NPC.velocity.X >= 1f) {
                    NPC.velocity.X -= AccelerateSpeedRotating;
                }
            }
            // 向右
            void AcceRight() {
                NPC.velocity.X += AccelerateSpeed;
                if (NPC.velocity.X >= MaxXSpeed) {
                    NPC.velocity.X = MaxXSpeed;
                }
                if (NPC.velocity.X <= -1f) {
                    NPC.velocity.X += AccelerateSpeedRotating;
                }
            }

            // 寻路移动
            // 可以打到，直接跑
            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetPosition, Player.width, Player.height)) {
                // 玩家在右边
                if (targetCenter.X - NPC.Center.X > 0) {
                    AcceRight();
                }
                // 玩家在左边
                else {
                    AcceLeft();
                }
            }
            // 不能打到，用A*寻路
            else {
                Point npcTile = NPC.Center.ToTileCoordinates();
                Point plrTile = Player.Center.ToTileCoordinates();

                while (path.Count > 0) {
                    Point next = new(path[^1].x, path[^1].y);
                    targetCenter = next.ToWorldCoordinates();
                    targetPosition = targetCenter - new Vector2(20f, 28f);
                    if (NPC.Distance(targetCenter) <= 40) { // 能删就继续找下一个点
                        path.RemoveAt(path.Count - 1);
                    }
                    else break;
                }
                // 玩家在右边
                if (targetCenter.X - NPC.Center.X > 0) {
                    AcceRight();
                }
                // 玩家在左边
                else {
                    AcceLeft();
                }
                // 60帧更新一次路径
                if (PathfindingTimer % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient && NPC.Distance(Player.Center) <= 16 * 60) {
                    path = AStarPathfinding.Pathfinding(npcTile, plrTile, 600);
                    NPC.netUpdate = true;
                }
                PathfindingTimer++;
            }
            NPC.direction = NPC.spriteDirection = Math.Sign(targetCenter.X - NPC.Center.X);

            // 走上方块代码
            // 如果站在地上(y碰撞),脚左右有方块就开启stepUp
            int TopX = (int)(NPC.Center.X + 8) >> 4;
            int NextTopX = (int)(NPC.Center.X + NPC.velocity.X + NPC.direction * 8) >> 4;
            int LegY = (int)(NPC.Bottom.Y - 8) >> 4;
            if (NPC.collideY && NPC.velocity.Y == 0) { // 一旦在地面就试图StepUp
                NPC.stepSpeed = 2f;
                Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, true, 1);
                RocketTimer = -1; // 重置火箭时间
            }

            void Jump(int height) {
                switch (height) {
                    case 1:
                    case 2:
                    case 3:
                        NPC.velocity.Y = -5.8f;
                        break;
                    case 4:
                        NPC.velocity.Y = -6.6f;
                        break;
                    case 5:
                        NPC.velocity.Y = -7.2f;
                        break;
                    case 6:
                        NPC.velocity.Y = -7.9f;
                        break;
                    case 7:
                        NPC.velocity.Y = -8.6f;
                        break;
                    case 8:
                        NPC.velocity.Y = -9.0f;
                        break;
                    case 9:
                        NPC.velocity.Y = -9.5f;
                        break;
                }
            }
            int WallHeightCount(int amount) {
                amount -= 1; // 不记录为0
                int tileWallHigh = 0;
                for (int j = -amount; j <= 1; j++) {
                    Tile t = Framing.GetTileSafely((int)(NPC.Center.X + NPC.direction * 16) >> 4, LegY + j);
                    if (Main.tileSolid[t.TileType] && t.HasUnactuatedTile && !t.IsActuated) {
                        tileWallHigh++;
                    }
                }
                return tileWallHigh;
            }

            // 跳跃
            if (NPC.velocity.Y == 0f) {
                if (NPC.collideX && NPC.velocity.X == 0f) {
                    Timer++;
                    if (Timer >= 15f) {
                        Jump(5);
                        Timer = 0;
                    }
                }
                // 如果能直线看到玩家，就向玩家去
                if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetCenter - new Vector2(20, 28), Player.width, Player.height)) {
                    int toPlayerYTile = (int)(targetCenter.Y - NPC.Center.Y) >> 4;
                    int distanceXTile = Math.Abs((int)(targetCenter.X - NPC.Center.X) >> 4);
                    if (toPlayerYTile < -2 && distanceXTile <= 6) { // 玩家在上面两格以上，离得近
                        Jump(Math.Min(9, -toPlayerYTile)); // 跳最高9格高度
                    }
                }
                // 否则爬墙
                else {
                    // 看看面前的墙有多高，往前向上寻找9格高看有多少墙
                    int tileWallHigh = WallHeightCount(9);
                    Jump(tileWallHigh - 1);
                }
            }

            // 火箭靴，跳起来摸不到/跳不过再用（速度>=0往下掉，且没有接触地面）还要注意重置了火箭时间，随机游荡时也不用
            const float MaxRocketSpeed = 4f;
            const float RocketAccelerateSpeed = 0.5f; // 加速速度设置大点因为还要抵消重力影响
            if (NPC.velocity.Y >= 0 && !NPC.collideY && RocketTimer == -1) {
                RocketTimer = RocketMaxTime; // 激活火箭靴
            }
            // 激活火箭靴后用火箭靴，玩家在下方且可以打到就不用了
            if (RocketTimer > 0 && (targetCenter.Y + 28 - NPC.Bottom.Y <= 20f || !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetCenter - new Vector2(20, 28), Player.width, Player.height))) {
                // 火箭靴声音和粒子
                if (RocketSoundDelay <= 0) {
                    RocketSoundDelay = 15;
                    SoundEngine.PlaySound(SoundID.Item24, NPC.Bottom); // 另一种是Item13
                }
                else {
                    RocketSoundDelay = 30;
                }
                RocketBootVisuals();

                int tileWallHigh = WallHeightCount(2);
                if (tileWallHigh >= 1) { // 一格或两格墙
                    NPC.velocity.Y -= RocketAccelerateSpeed; // 火箭加速
                }
                else { // 没有墙
                    int toPlayerY = (int)(targetCenter.Y - NPC.Center.Y);
                    int distanceXTile = Math.Abs((int)(targetCenter.X - NPC.Center.X) >> 4);
                    if (toPlayerY < 16 && distanceXTile <= 16) { // 玩家在怪物以上或与怪物Y距离相差小于1格，离得近
                        NPC.velocity.Y -= RocketAccelerateSpeed; // 火箭加速
                    }
                    // 能看到玩家的话再长点距离
                    else if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetPosition, Player.width, Player.height) && toPlayerY < 80 && distanceXTile <= 50) {
                        NPC.velocity.Y -= RocketAccelerateSpeed; // 火箭加速
                    }
                }
                if (NPC.velocity.Y > MaxRocketSpeed) {
                    NPC.velocity.Y = MaxRocketSpeed;
                }
                // 火箭靴可用时间减少
                RocketTimer--;
            }
        }

        private void RocketBootVisuals() {
            for (int i = -1; i <= 1; i += 2) {
                int num2 = (i == 0) ? 2 : (-2);
                Rectangle r = new((int)NPC.Center.X + 8 * i, (int)NPC.Bottom.Y - 6, 8, 8);
                r.X -= 6;

                int type = 16;
                float scale = 1.5f;
                int alpha = 20;
                float num3 = 1f;
                Vector2 vector = new((float)(-num2) - NPC.velocity.X * 0.3f, 2f - NPC.velocity.Y * 0.3f);
                Dust dust = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, type, 0f, 0f, alpha, default(Color), scale);
                dust.velocity += vector;
                dust.velocity *= num3;
                dust.velocity *= 0.1f;
            }
        }

        private void Teleport(Vector2? pos) {
            var NPCOldPos = NPC.Center;
            if (pos == null) {
                // 原版混沌精的代码
                var PlrPosition = (Player.position / 16).ToPoint();
                int range = 20;
                int num194 = 0;
                bool flag26 = false;
                if (Math.Abs(NPC.position.X - Player.position.X) + Math.Abs(NPC.position.Y - Player.position.Y) > 2000f) {
                    num194 = 100;
                    flag26 = true;
                }

                while (!flag26 && num194 < 100) {
                    num194++;
                    int x = Main.rand.Next(PlrPosition.X - range, PlrPosition.X + range);
                    for (int y = Main.rand.Next(PlrPosition.Y - range, PlrPosition.Y + range); y < PlrPosition.Y + range; y++) {
                        if (Main.tile[x, y].HasUnactuatedTile) {
                            if (Main.tile[x, y - 1].LiquidType == LiquidID.Lava)
                                continue;

                            if (Main.tileSolid[Main.tile[x, y].TileType] && Main.tile[x, y].HasUnactuatedTile && !Collision.SolidTiles(x - 1, x + 1, y - 4, y - 1)) {
                                NPC.Bottom = new Vector2(x * 16, y * 16);
                                NPC.netUpdate = true;
                            }
                        }
                    }
                }
            }
            else {
                NPC.Bottom = pos.Value;
                NPC.netUpdate = true;
            }

            NPC.position += NPC.netOffset;

            var teleportTarget = NPC.Center - NPCOldPos;
            float num55 = NPC.Distance(NPCOldPos);
            num55 = 2f / num55;
            teleportTarget.X *= num55;
            teleportTarget.Y *= num55;
            for (int i = 0; i < 30; i++) {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.FoodPiece, teleportTarget.X, teleportTarget.Y, 200, new Color(169, 83, 170), 1.5f);
                d.noGravity = true;
                d.velocity.X *= 1.5f;
            }

            for (int i = 0; i < 30; i++) {
                var d = Dust.NewDustDirect(NPCOldPos - NPC.Size / 2f, NPC.width, NPC.height, DustID.FoodPiece, 0f - teleportTarget.X, 0f - teleportTarget.Y, 200, new Color(169, 83, 170), 2f);
                d.noGravity = true;
                d.velocity.X *= 1.5f;
            }

            NPC.position -= NPC.netOffset;
        }

        public override void FindFrame(int frameHeight) {
            base.FindFrame(frameHeight);
            if (NPC.velocity.Y == 0f) {
                // 旧版帧数控制，同时也是头和腿的帧
                NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.15f;
                NPC.frame.Y = (int)(NPC.frameCounter % 14 + 6) * frameHeight;
                if (Math.Abs(NPC.velocity.X) <= 0.28f) {
                    NPC.frame.Y = 0;
                }
            }
            else {
                NPC.frame.Y = frameHeight * 5;
                NPC.frameCounter = 6;
            }
            // 管理手臂帧
            ManageHandFrame();
            // 管理盔甲绘制偏移
            ManageBodyOffset();
            // 管理盔甲帧
            ManageBodyFrame();
            shoulderFrame = new Rectangle(bodyFrame.X, bodyFrame.Y + 56, 40, 56);
            secondHandFrame = new Rectangle(handFrame.X, handFrame.Y + 56 * 2, 40, 56);
        }

        private void ManageHandFrame() {
            if (useExtraHand) {
                handFrame.X = 40 * 2;
                return;
            }
            handFrame.Y = 0;
            int frame = NPC.frame.Y / 56 + 1;
            if (frame <= 5) {
                handFrame.X = 40 * frame + 40;
                return;
            }
            handFrame.Y = 56;
            switch (frame) {
                case 6:
                    handFrame.X = 40 * 2;
                    break;
                case 7:
                case 12:
                case 13:
                case 14:
                case 19:
                case 20:
                    handFrame.X = 40 * 3;
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    handFrame.X = 40 * 4;
                    break;
                case 15:
                case 18:
                    handFrame.X = 40 * 5;
                    break;
                case 16:
                case 17:
                    handFrame.X = 40 * 5;
                    break;
            }
        }

        private void ManageBodyOffset() {
            int frame = NPC.frame.Y / 56 + 1;
            switch (frame) {
                case 8:
                case 9:
                case 10:
                case 15:
                case 16:
                case 17:
                    bodyOffset.Y = -2;
                    break;
            }
        }

        private void ManageBodyFrame() {
            int frame = NPC.frame.Y / 56 + 1;
            bodyFrame.Y = 0;
            switch (frame) {
                case 6:
                    bodyFrame.Y = 56 * 2;
                    break;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            if (WeaponAniTimer > 0 && WeaponType != (int)Weapons.Potion) { // 搁着发射呢
                // 搞个转换，这时正的往左边射，负的往右边射，正下是0
                float easyRotation = MathHelper.WrapAngle(ShootRotation - 1.57f);
                NPC.spriteDirection = -Math.Sign(easyRotation);
            }
            if (LegsTexture == null) {
                LegsTexture = ModContent.Request<Texture2D>(Texture + "_Legs");
            }
            if (BodyTexture == null) {
                BodyTexture = ModContent.Request<Texture2D>(Texture + "_Body");
            }
            SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 offset = new(0, 4 + NPC.gfxOffY);
            Vector2 finalBodyOffset = offset + bodyOffset + new Vector2(0, 2);
            Vector2 origin = new Vector2(40, 56 + (56 - NPC.height)) * 0.5f;
            bool drawNormalHand = DrawHand();
            Vector2 drawPosition = (NPC.Center - screenPos).Floor();
            // 原版顺序：腿->副手->头->身
            // 腿
            Main.EntitySpriteDraw(LegsTexture.Value, drawPosition + offset, new Rectangle?(NPC.frame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            // 副手
            Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(secondHandFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            // 头
            Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, drawPosition + offset, new Rectangle?(shoulderFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            if (drawNormalHand) {
                // 身（盔甲）
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(bodyFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                // 身（肩甲）
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(shoulderFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                // 武器
                DrawWeapons(ref handFrame, screenPos, drawColor);
                // 身（手甲）
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(handFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }
            else {
                // 身（盔甲）
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(bodyFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                // 武器
                DrawWeapons(ref handFrame, screenPos, drawColor);
                // 身（额外手）
                float rotation = MathHelper.Lerp(-1.8f, -0.6f, Math.Max(0, WeaponAniTimer - 12) / 9f);
                var offsetHand = new Vector2(-5, -7);
                var originHand = new Vector2(14, 28);
                if (NPC.spriteDirection == -1) { // 朝向向左的调整
                    rotation = MathHelper.Lerp(1.8f, 0.6f, Math.Max(0, WeaponAniTimer - 12) / 9f);
                    offsetHand = new Vector2(5, -6);
                    originHand = new Vector2(26, 28);
                }
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset + offsetHand, new Rectangle?(new Rectangle(40 * 7, 0, 40, 56)), drawColor * NPC.Opacity, rotation, originHand, NPC.scale, spriteEffects, 0);
                // 身（肩甲）
                Main.EntitySpriteDraw(BodyTexture.Value, drawPosition + finalBodyOffset, new Rectangle?(shoulderFrame), drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            return false;
        }

        // 返回值是是否绘制手臂
        private bool DrawHand() {
            if (WeaponAniTimer == -999 || WeaponAniTimer == 0) {
                return true; // 潜行状态不绘制武器
            }
            if (WeaponType == (int)Weapons.Potion) {
                return false;
            }
            if (WeaponType == (int)Weapons.Glowstick) {
                return true;
            }
            return true;
        }

        private void DrawWeapons(ref Rectangle handFrame, Vector2 screenPos, Color drawColor) {
            if (WeaponAniTimer == -999 || WeaponAniTimer == 0) {
                return; // 潜行状态不绘制武器
            }

            SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 offset = new(0, 4 + NPC.gfxOffY);
            Vector2 origin = new Vector2(40, 56 + (56 - NPC.height)) * 0.5f;
            Vector2 drawPosition = (NPC.Center - screenPos).Floor();
            Vector2 finalBodyOffset = offset + bodyOffset + new Vector2(0, 2);

            // 荧光棒时间。拿着荧光棒
            if (Timer == 114514 && WeaponType != (int)Weapons.Potion) {
                handFrame.X = 40 * 5;
                handFrame.Y = 0;
                var glowstickOffset = new Vector2(12 * NPC.spriteDirection, -4);
                var finalPos = glowstickOffset + NPC.Center - screenPos;
                Main.instance.LoadItem(ItemID.Glowstick);
                Main.EntitySpriteDraw(TextureAssets.Item[ItemID.Glowstick].Value, finalPos, null, new Color(250, 250, 250, 0), 0f, TextureAssets.Item[ItemID.Glowstick].Size() / 2f, NPC.scale, spriteEffects, 0);
            }
            if (WeaponType == (int)Weapons.Potion) {
                if (!WeaponsTexture.ContainsKey("Potion")) {
                    WeaponsTexture.Add("Potion", ModContent.Request<Texture2D>(Texture + "_PotionofDiscord"));
                }

                float rotation = MathHelper.Lerp(-1.8f, -0.6f, Math.Max(0, WeaponAniTimer - 12) / 9f);
                rotation += 1.57f;
                var offsetHand = new Vector2(-5, -7);
                var palmPosition = drawPosition + finalBodyOffset + offsetHand + rotation.ToRotationVector2() * 16;
                var potionOffset = new Vector2(5, 2);
                var finalPositon = palmPosition + potionOffset;
                rotation -= 1f; // 调整药水使瓶口对嘴
                if (NPC.spriteDirection == -1) { // 朝向向左的调整
                    rotation = MathHelper.Lerp(1.8f, 0.6f, Math.Max(0, WeaponAniTimer - 12) / 9f);
                    rotation -= 4.71f;
                    offsetHand = new Vector2(5, -6);
                    palmPosition = drawPosition + finalBodyOffset + offsetHand + rotation.ToRotationVector2() * 16;
                    potionOffset = new Vector2(-8, 1);
                    finalPositon = palmPosition + potionOffset;
                    rotation -= 2.1f; // 调整药水使瓶口对嘴
                }
                var originPotion = WeaponsTexture["Potion"].Size() / 2f;

                Main.EntitySpriteDraw(WeaponsTexture["Potion"].Value, finalPositon, null, drawColor * NPC.Opacity, rotation, originPotion, NPC.scale, spriteEffects, 0);
                return;
            }
            if (WeaponType == (int)Weapons.Glowstick) {
                return;
            }
            // 搞个转换，这时正的往左边射，负的往右边射，正下是0
            float easyRotation = MathHelper.WrapAngle(ShootRotation - 1.57f);
            if (WeaponAniTimer > 0) {
                handFrame.Y = 0;
                if (Math.Abs(easyRotation) <= 1.0) { // 往下
                    handFrame.X = 40 * 6;
                }
                else if (Math.Abs(easyRotation) <= 2.1) { // 稍微中间
                    handFrame.X = 40 * 5;
                }
                else { // 往上
                    handFrame.X = 40 * 4;
                }

                if (!WeaponsTexture.ContainsKey("Crossbow")) {
                    WeaponsTexture.Add("Crossbow", ModContent.Request<Texture2D>(Texture + "_RepeatingCrossbow"));
                }
                if (!WeaponsTexture.ContainsKey("Cannon")) {
                    WeaponsTexture.Add("Cannon", ModContent.Request<Texture2D>(Texture + "_LavaCannon"));
                }

                Asset<Texture2D> tex = DecideWeaponTexture();
                float adjust = easyRotation > 0 ? 0.4f : 0.6f;
                SpriteEffects weaponEffects = easyRotation < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

                Main.EntitySpriteDraw(tex.Value, drawPosition + finalBodyOffset, null, drawColor, ShootRotation, new Vector2(tex.Width() * 0.1f, tex.Height() * adjust), NPC.scale, weaponEffects, 0);
            }
        }

        private Asset<Texture2D> DecideWeaponTexture() {
            switch (WeaponType) {
                case (int)Weapons.Crossbow:
                    return WeaponsTexture["Crossbow"];
                case (int)Weapons.Cannon:
                    return WeaponsTexture["Cannon"];
                default:
                    return WeaponsTexture["Crossbow"];
            }
        }
    }
}
