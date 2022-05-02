﻿using Entrogic.Common.WorldGeneration;
using Entrogic.Content.Dusts;
using Entrogic.Content.NPCs.BaseTypes;
using Entrogic.Content.Projectiles.Athanasy;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;

namespace Entrogic.Content.NPCs.Enemies.Athanasy
{
    [AutoloadBossHead]
    internal class Athanasy : FSM_NPC
    {
        enum AIState
        {
            Despawn,
            Dash,
            WallSpear,
            SuperSmash,
            Smash,
            SpawnEffects,
            SpawnAnimation,
        }

        protected override void SwitchState(int state) {
            base.SwitchState(state);
            Timer = 0;
        }

        public override void Load() {
            base.Load();
            On.Terraria.NPC.UpdateNPC_UpdateGravity += NPC_UpdateNPC_UpdateGravity;
            On.Terraria.NPC.Collision_DecideFallThroughPlatforms += NPC_Collision_DecideFallThroughPlatforms;
            On.Terraria.Main.RenderTiles += Main_RenderTiles;
        }

        private void Main_RenderTiles(On.Terraria.Main.orig_RenderTiles orig, Main self) {
            orig.Invoke(self);
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //Main.spriteBatch.Draw(Main.instance.blackTarget, Vector2.Zero, Color.White);
            //Main.spriteBatch.End();
        }

        private bool NPC_Collision_DecideFallThroughPlatforms(On.Terraria.NPC.orig_Collision_DecideFallThroughPlatforms orig, NPC self) {
            if (self.type == Type) {
                if (State == (int)AIState.Smash) {
                    return Player.Bottom.Y - NPC.Bottom.Y > 32;
                }
                if (State == (int)AIState.SuperSmash) {
                    return RoomBottom.Y - NPC.Bottom.Y > 40;
                }
                return false;
            }
            return orig.Invoke(self);
        }

        private void NPC_UpdateNPC_UpdateGravity(On.Terraria.NPC.orig_UpdateNPC_UpdateGravity orig, NPC self, out float maxFallSpeed) {
            orig.Invoke(self, out maxFallSpeed);
            if (self.type == Type) {
                maxFallSpeed = SmashSpeedMax;
                if (State == (int)AIState.SuperSmash) {
                    maxFallSpeed = SmashSpeedMax * 2f;
                }
            }
        }

        private Vector2 savedPosition = Vector2.Zero;
        private Player Player => Main.player[NPC.target];
        private bool canHitPlayer = true;
        private const int DespawnDistance = 300;
        private static float SmashSpeedMax => Main.getGoodWorld ? 21.99f : 18f;
        private static float SmashSpeed => Main.getGoodWorld ? 2.6f : 1.5f;
        private static float DashSpeedMax => Main.getGoodWorld ? 28f : 22f;
        private static float DashSpeed => Main.getGoodWorld ? 0.1f : 0.03f;
        private static Vector2 RoomBottom => new Vector2(ImmortalGolemRoom.BossZone.Center.X, ImmortalGolemRoom.BossZone.Bottom - 8).ToWorldCoordinates(0, 0);

        public override void AI() {
            base.AI();

            switch (State) {
                case (int)AIState.Despawn: DespawnState(); break;
                case (int)AIState.Dash: DashState(); break;
                case (int)AIState.WallSpear: WallSpearState(); break;
                case (int)AIState.SuperSmash: SuperSmashState(); break;
                case (int)AIState.Smash: SmashState(); break;
            }
            NPC.spriteDirection = NPC.direction;
        }

        private void DespawnState() {
            NPC.TargetClosest(true);
            Timer++;
            if (!Player.dead && Player.active) {
                SwitchState((int)AIState.SuperSmash);
                Timer = 0;
            }
            else if (Timer >= 180) {
                NPC.TargetClosest(true);
                if (Player.dead || !Player.active || !Player.ZoneBeach) {
                    if (Timer <= 195)
                        NPC.alpha -= 255 / 15 + 1;
                    else NPC.active = false;
                }
                else {
                    SwitchState((int)AIState.Smash);
                    Timer = 0;
                }
            }
        }

        private void DashState() {
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPCAimedTarget targetData = NPC.GetTargetData();
            float stoppingTimer = 20;
            float dashingTimer = 90;
            float moveSpeed = 26f;
            if (Timer <= 40f) {
                // 来自光女
                NPC.ai[2] = -Math.Sign(NPC.Center.X - targetData.Center.X);
                NPC.noTileCollide = true;
                NPC.noGravity = true;

                if (Timer == 40f)
                    NPC.velocity *= 0.3f;

                Vector2 destination = (targetData.Invalid ? NPC.Bottom : targetData.Center + new Vector2(0f, targetData.Height / 2f)) + new Vector2(NPC.ai[2] * -550, 0);
                NPC.velocity = destination - NPC.Bottom;
                NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero) * moveSpeed;

                if (Vector2.Distance(NPC.Bottom, destination) <= 32) {
                    Timer = 40f;
                    savedPosition = NPC.Center; // 后面作NPC是否超过玩家的检查器
                    NPC.velocity *= 0.1f;
                }
            }
            else if (Timer <= 40f + dashingTimer) {
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                if (Timer == 40f + dashingTimer)
                    NPC.velocity *= 0.7f;

                NPC.velocity.X += DashSpeed * NPC.ai[2];

                if (NPC.velocity.X >= DashSpeedMax)
                    NPC.velocity.X = DashSpeedMax;

                if (NPC.velocity.X <= -DashSpeedMax)
                    NPC.velocity.X = -DashSpeedMax;
                NPC.velocity.Y *= 0.7f;
            }
            else {
                NPC.velocity *= 0.92f;
            }

            if (Timer >= 40f + dashingTimer + stoppingTimer) {
                SwitchState((int)AIState.Despawn);
                NPC.netUpdate = true;
            }
            Timer++;
        }

        private void WallSpearState() {
            int wallSpearDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            Vector2 center = NPC.Center;
            if (!Player.dead && Player.active && Math.Abs(NPC.Center.X - Player.Center.X) / 16f <= DespawnDistance) // 这里是如果离得太远或玩家死了的话就跑路了
                center = Player.Center;

            Timer++;
            if (Main.netMode != NetmodeID.MultiplayerClient && Timer % 60 == 0) {
                var centerTile = center.ToTileCoordinates();
                float spearSpeed = 12f;
                for (int i = 0; i <= 80; i++) {
                    for (int a = -1; a <= 1; a += 2) { // 左右的检测
                        var pos = new Point(centerTile.X + i * a, centerTile.Y);
                        var tile = Framing.GetTileSafely(pos);
                        if (tile != null && tile.IsActiveUnactuated && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) {
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.ToWorldCoordinates(), Vector2.UnitX * -Math.Sign(a) * spearSpeed, ModContent.ProjectileType<AthanasySpear>(), wallSpearDamage, 2f, ai0: spearSpeed);
                            Main.NewText(pos);
                            goto VerticalDetect;
                        }
                    }
                }
                VerticalDetect:
                for (int j = 0; j <= 60; j++) {
                    for (int a = -1; a <= 1; a += 2) { // 上下的检测
                        var pos = new Point(centerTile.X, centerTile.Y + j * a);
                        var tile = Framing.GetTileSafely(pos);
                        if (tile != null && tile.IsActiveUnactuated && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) {
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.ToWorldCoordinates(), Vector2.UnitY * -Math.Sign(a) * spearSpeed, ModContent.ProjectileType<AthanasySpear>(), wallSpearDamage, 2f, ai0: spearSpeed);
                            goto AfterDetect;
                        }
                    }
                }
                AfterDetect:;
            }
        }

        private void SuperSmashState() {
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
                            var d = Dust.NewDustDirect(NPC.Bottom - new Vector2(NPC.width / 2, 30f), NPC.width, 30, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, NPC.AI_121_QueenSlime_GetDustColor());
                            d.noGravity = true;
                            d.velocity.Y = -5f + Main.rand.NextFloat() * -3f;
                            d.velocity.X *= 7f;
                        }
                        PunchCameraModifier modifier4 = new PunchCameraModifier(NPC.Center, -Vector2.UnitY, 30f, 15f, 30, 2000f, "Entrogic: Athanasy");
                        Main.instance.CameraModifiers.Add(modifier4);
                    }
                    if (Timer <= 20 && Main.netMode != NetmodeID.MultiplayerClient) {// 借用一下鹿角怪的Proj
                        float d = RoomBottom.Distance(ImmortalGolemRoom.BossZone.BottomLeft().ToWorldCoordinates(0, 0)); // 计算出中心到两边的距离
                        float p = d / 26f; // 分成22分给不同的ai1使用（26是考虑了墙面不需要）

                        for (int l = -1; l <= 1; l += 2) { // 左右都有
                            for (int offset = 0; offset <= 180; offset += 60) { // 来多几层
                                var rubbleVector = -Vector2.UnitY;
                                rubbleVector = rubbleVector.RotatedByRandom(MathHelper.ToRadians(5));
                                int frame = 3 + Main.rand.Next(3);
                                var pos = new Vector2(NPC.Bottom.X + (p * Timer + Main.rand.Next((int)p + 1)) * l, NPC.Bottom.Y - 8 + offset);
                                Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos, rubbleVector * (16f + Main.rand.NextFloat() * 8f), ProjectileID.DeerclopsRangedProjectile, rubbleDamage, 0f, Main.myPlayer, 0f, frame);
                            }
                            var dPos = new Vector2(NPC.Bottom.X + (p * Timer + Main.rand.Next((int)p + 1)) * l, NPC.Bottom.Y - 8);
                            for (int i = 0; i < 15; i++) { // 粒子效果
                                Dust.NewDustDirect(dPos + new Vector2(-15, -48), 30, 60, ModContent.DustType<Sand>(), 0, Main.rand.NextFloat(-5f, -1f), Main.rand.Next(255), default, Main.rand.NextFloat(1.5f));
                            }
                        }
                    }
                    else {
                        SwitchState((int)AIState.Despawn);
                        NPC.ai[2] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                else if (Timer >= (float)smashStartTimer) { // 下砸中
                    for (int m = 0; m < 4; m++) {
                        Vector2 position = NPC.Bottom - new Vector2(Main.rand.NextFloatDirection() * 16f, Main.rand.Next(8));
                        int num22 = Dust.NewDust(position, 2, 2, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, GetDustColor(), 1.4f);
                        Main.dust[num22].position = position;
                        Main.dust[num22].noGravity = true;
                        Main.dust[num22].velocity.Y = NPC.velocity.Y * 0.9f;
                        Main.dust[num22].velocity.X = ((Main.rand.Next(2) == 0) ? (-10f) : 10f) + Main.rand.NextFloatDirection() * 3f;
                    }
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
                    if (NPC.velocity.Y >= SmashSpeedMax * 2.3f)
                        NPC.velocity.Y = SmashSpeedMax * 2.3f;
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

            float r = 70f;
            for (int k = 0; k < 4; k++) {
                Vector2 vector2 = NPC.Center + Main.rand.NextVector2CircularEdge(r, r);

                Vector2 v = (vector2 - NPC.Center).SafeNormalize(Vector2.Zero) * -8f;
                var d = Dust.NewDustDirect(vector2, 2, 2, DustID.Smoke, v.X, v.Y, (int)MathHelper.Lerp(120f, 190f, Main.rand.NextFloat()), GetDustColor(), 1.8f);
                d.position = vector2;
                d.noGravity = true;
                d.velocity = v;
                d.customData = this;
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

        private void SmashState() {
            int smashStartDistanceY = 420;
            int smashStartDistanceX = 8;
            int smashStartTimer = 15; // 下砸起始计时器
            int smashDamage = NPC.GetAttackDamage_ForProjectiles_MultiLerp_Exactly(40f, 90f, 140f);
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (NPC.ai[2] == 1f) { // ai[2]为1时是下砸
                NPC.noTileCollide = Player.Bottom.Y - NPC.Bottom.Y > 32;
                NPC.noGravity = false;

                if (Main.getGoodWorld) // GoodWorld是ftw种子
                    smashStartTimer = 0;

                // 雾气效果，尽量提前几帧出
                if (NPC.velocity.Y <= 8f && Timer >= (float)smashStartTimer + 5)
                    for (int l = 0; l < 30; l++) { // 粒子效果
                        Dust.NewDustDirect(NPC.Bottom - new Vector2(NPC.width * 0.8f, 20f), (int)(NPC.width * 1.6f), 50, ModContent.DustType<Sand>(), 0, Main.rand.NextFloat(-5f, -1f), Main.rand.Next(255), default, Main.rand.NextFloat(1f));
                    }
                if (NPC.velocity.Y == 0f) { // 下砸到地了
                    PunchCameraModifier modifier4 = new PunchCameraModifier(NPC.Center, -Vector2.UnitY, 15f, 10.8f, 20, 1300f, "Entrogic: Athanasy");
                    Main.instance.CameraModifiers.Add(modifier4);
                    SwitchState((int)AIState.Despawn);
                    Timer = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient) // 这部分打算参考死亡细胞王手的小下砸
                        Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Bottom, Vector2.Zero, 922, smashDamage, 0f, Main.myPlayer);
                }
                else if (Timer >= (float)smashStartTimer) { // 下砸中
                    for (int m = 0; m < 4; m++) {
                        Vector2 position = NPC.Bottom - new Vector2(Main.rand.NextFloatDirection() * 16f, Main.rand.Next(8));
                        int num22 = Dust.NewDust(position, 2, 2, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, 40, GetDustColor(), 1.4f);
                        Main.dust[num22].position = position;
                        Main.dust[num22].noGravity = true;
                        Main.dust[num22].velocity.Y = NPC.velocity.Y * 0.9f;
                        Main.dust[num22].velocity.X = ((Main.rand.Next(2) == 0) ? (-10f) : 10f) + Main.rand.NextFloatDirection() * 3f;
                    }
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

            Timer += 1f;

            float r = 70f;
            for (int k = 0; k < 4; k++) {
                Vector2 vector2 = NPC.Center + Main.rand.NextVector2CircularEdge(r, r);

                Vector2 v = (vector2 - NPC.Center).SafeNormalize(Vector2.Zero) * -8f;
                var d = Dust.NewDustDirect(vector2, 2, 2, DustID.Smoke, v.X, v.Y, (int)MathHelper.Lerp(120f, 190f, Main.rand.NextFloat()), GetDustColor(), 1.8f);
                d.position = vector2;
                d.noGravity = true;
                d.velocity = v;
                d.customData = this;
            }
            if (!(Timer >= 40f)) // 40之前是在原地不动，属于是一个准备阶段吧
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
            float speed = NPC.Distance(RoomBottom) <= 160 ? 12f : 18f;
            NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero) * speed;
        }

        private static Color GetDustColor() {
            Color c = Color.Lerp(new Color(210, 210, 210), new Color(160, 160, 160), Main.rand.NextFloat());
            return Color.Lerp(Color.Gray, c, Main.rand.NextFloat());
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            base.CanHitPlayer(target, ref cooldownSlot);
            return canHitPlayer;
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

                Vector2 halfSize = new Vector2(t.Width / 2, t.Height / Main.npcFrameCount[Type] / 2);
                var npcColor = NPC.GetNPCColorTintedByBuffs(drawColor);
                int num183 = 7;
                for (int num185 = 1; num185 < num183; num185 += 2) {
                    Color value93 = npcColor;
                    value93 = NPC.GetAlpha(value93);
                    value93 *= (float)(num183 - num185) / 15f;
                    Vector2 position19 = NPC.oldPos[num185] + NPC.Size / 2f - screenPos;
                    position19 -= new Vector2(t.Width, t.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    position19 += halfSize * NPC.scale + new Vector2(0f, extraDrawY + NPC.gfxOffY);
                    Main.EntitySpriteDraw(t, position19, NPC.frame, value93, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0);
                }

                // 绘制NPC
                Main.EntitySpriteDraw(t, pos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
                if (NPC.color != default(Color)) {
                    Main.EntitySpriteDraw(t, pos, NPC.frame, NPC.GetColor(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
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

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { //Influences how the NPC looks in the Bestiary
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon, //Sets the spawning conditions of this NPC that is listed in the bestiary.
                new FlavorTextBestiaryInfoElement("一只衰落魔像。") //Sets the description of this NPC that is listed in the bestiary.
            });
        }

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.Golem);
            NPC.aiStyle = -1;
            NPC.lifeMax = 5000;
            NPC.damage = 52;
            NPC.defense = 15;
            NPC.width = 80;
            NPC.height = 120;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.Opacity = 1f;
            NPC.scale = 1.3f;
            NPC.lavaImmune = true;
            NPC.hide = true; // 为了让它渲染在BehindNonSolidTiles层
            //music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheStormy");
            Music = MusicID.Boss2;
        }

        public override void DrawBehind(int index) {
            Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
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
