using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

using Microsoft.Xna.Framework.Graphics;
using Entrogic.Items.PollutElement;
using Entrogic.Items.Materials;
using Entrogic.Items.PollutElement.Armor;
using Entrogic.Items.Miscellaneous.Placeable.Trophy;
using Terraria.Utilities;
using Terraria.GameContent.NetModules;

namespace Entrogic.NPCs.Boss.PollutElement
{
    [AutoloadBossHead]
    public class PollutionElemental : FSM_NPC
    {
        enum NPCState
        {
            Start,

            Break,
            Move,
            Dash,
            Rotat,
            FindingWaters,

            Break_State2,
            Move_State2,
            Magic_State2,
            Round_State2,

            PlayerDead
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 8900;
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax = 11000;
                }
            }
            npc.damage = 82;
            npc.defense = 15;
            npc.width = 96;
            npc.height = 96;
            npc.value = Item.buyPrice(0, 14, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = npc.lavaImmune = npc.noGravity = npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Bleeding] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.timeLeft = NPC.activeTime * 30;
            music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheStormy");
            musicPriority = MusicPriority.BossMedium;
            bossBag = ItemType<ContaminatedElementalTreasureBag>();
            npc.alpha = 255;
            npc.scale = 0.6f;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return Main.rand.NextBool(2);
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return Main.rand.NextBool(2);
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.73f * bossLifeScale);
        }
        public const float scaleLess = 0.7f;
        public const float scaleMax = 1.3f;
        public const int waterMax = 2295;
        public int water = 0;
        public int[] damagedNearly = new int[240];
        public int oldLife = 0;
        public Vector2 waterPos = Vector2.Zero;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15000000596046448;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int num = (int)npc.frameCounter;
            npc.frame.Y = num * frameHeight;
        }
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                Item.NewItem(npc.getRect(), ItemType<SoulofContamination>(), Main.rand.Next(17, 22));

                var dropChooser = new WeightedRandom<int>();
                dropChooser.Add(ItemType<ContaminatedLongbow>());
                dropChooser.Add(ItemType<ContaminatedCurrent>());
                dropChooser.Add(ItemType<WaterElementalStaff>());
                int choice = dropChooser;
                Item.NewItem(npc.getRect(), choice);
                dropChooser.Clear();

                if (Main.rand.NextFloat() < .666) // 2/3掉落
                {
                    dropChooser.Add(ItemType<HelmetofContamination>());
                    dropChooser.Add(ItemType<HeadgearofContamination>());
                    dropChooser.Add(ItemType<CrownofContamination>());
                    dropChooser.Add(ItemType<MaskofContamination>());
                    choice = dropChooser;
                    Item.NewItem(npc.getRect(), choice);
                }
                if (Main.rand.NextFloat() < .666) // 2/3掉落
                {
                    Item.NewItem(npc.getRect(), ItemType<BreastplateofContamination>());
                }
                if (Main.rand.NextFloat() < .666) // 2/3掉落
                {
                    Item.NewItem(npc.getRect(), ItemType<GreavesofContamination>());
                }
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        private int tester;
        private Vector2 recordedStartRoundPos;
        private bool IsState2 = false;
        public int GetState() => IsState2 ? 2 : 1;
        public override void AI()
        {
            Player player = Main.player[npc.target];
            bool TargetZoneBeach = player.ZoneBeach;
            bool UpgradeOutBeach = !TargetZoneBeach && (IsState2 || EntrogicWorld.IsDownedPollutionElemental); // 不在海洋而且：是第二阶段或这个世界已经击败过一次，即true
            int damagedNearlySum = 0;
            npc.alpha = Math.Min(255, npc.alpha);
            npc.alpha = Math.Max(0, npc.alpha);
            if (npc.alpha <= 0)
                npc.dontTakeDamage = true;
            else npc.dontTakeDamage = false;
            if (player.dead || !player.active || (!TargetZoneBeach && !UpgradeOutBeach))
                SwitchState((int)NPCState.PlayerDead);
            if (water < waterMax)
            {
                for (int i = 0; i < npc.width / 16; i++)
                    for (int j = 0; j < npc.height / 16; j++)
                    {
                        if (WorldGen.InWorld(i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16))
                        {
                            Tile tile = Main.tile[i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16];
                            if (Sponge(i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16) && Main.rand.NextBool(2))
                            {
                                water++;
                                if (water >= waterMax)
                                    water = waterMax;
                                if (npc.life < npc.lifeMax)
                                    npc.life += 2;
                            }
                        }
                    }
            }
            if (water < 0)
            {
                water = 0;
            }
            npc.scale = scaleLess + (scaleMax - scaleLess) * ((float)water / (float)waterMax);
            if ((NPCState)State != NPCState.Start)
            {
                for (int i = damagedNearly.Length - 1; i >= 1; i--)
                {
                    damagedNearly[i] = damagedNearly[i - 1];
                    damagedNearlySum += damagedNearly[i];
                }
                damagedNearly[0] = oldLife - npc.life;
                damagedNearlySum += damagedNearly[0];
            }
            if (UpgradeOutBeach) // 如果是第二阶段离开海洋的话
                MovementAI(player, damagedNearlySum, UpgradeOutBeach);// AI多执行一遍（也就是速度提升一倍）
            MovementAI(player, damagedNearlySum, UpgradeOutBeach);
            oldLife = npc.life;
        }
        private void MovementAI(Player player, int damagedNearlySum, bool UpgradeOutBeach)
        {
            switch ((NPCState)State)
            {
                case NPCState.Start:
                    {
                        oldLife = npc.life;
                        npc.TargetClosest(true);
                        npc.alpha = 0;
                        SwitchState((int)NPCState.Move);
                        break;
                    }
                case NPCState.Break:
                    {
                        if ((float)npc.life / (float)npc.lifeMax <= 0.46f)
                        {
                            SwitchState((int)NPCState.Break_State2);
                            Timer = 0;
                            npc.ai[2] = npc.ai[3] = 0;
                            IsState2 = true;
                            return;
                        }
                        Timer++;
                        if (Timer <= 15)
                            npc.alpha -= 255 / 15 + 1;
                        npc.dontTakeDamage = true;
                        if (Timer > (int)(80f - (1f - (float)npc.life / (float)npc.lifeMax) * 40f))
                        {
                            npc.TargetClosest();
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    SwitchState((int)NPCState.Move);
                                    break;
                                case 1:
                                    SwitchState((int)NPCState.Dash);
                                    break;
                                default:
                                    SwitchState((int)NPCState.Rotat);
                                    break;
                            }
                            if (damagedNearlySum >= 680 && FindWater(60, 60) && Main.rand.NextBool(2))
                            {
                                SwitchState((int)NPCState.FindingWaters);
                            }
                            if ((NPCState)State == NPCState.Dash) // 冲刺Biss
                            {
                                switch (Main.rand.Next(2))
                                {
                                    case 0:
                                        SwitchState((int)NPCState.Move);
                                        break;
                                    default:
                                        SwitchState((int)NPCState.Rotat);
                                        break;
                                }
                            }
                            Timer = 0;
                        }
                        break;
                    }
                case NPCState.Move:
                    {
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2(Main.rand.Next(24, 33) * 16 * (Main.rand.NextBool(2) ? 1f : -1f)
                                , Main.rand.Next(14, 21) * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            npc.netUpdate = true;
                        }
                        if (Timer <= 15)
                            npc.alpha += 255 / 15 + 1;
                        npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * (7.2f - (float)water / (float)waterMax * 2f);
                        int MigrantCount = 0;
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.type == NPCType<Migrant>())
                            {
                                MigrantCount++;
                            }
                        }
                        if (Timer % 85 == 0 && MigrantCount <= 20 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC migrant = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<Migrant>())];
                            migrant.scale = 1f + (float)water / (float)waterMax * 0.4f;
                            water -= 32;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, migrant.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        if (Timer % (int)(72f - (1f - (float)npc.life / (float)npc.lifeMax) * 50f) == 0)
                        {
                            float shouldHeight = -12.4f;
                            if (player.Center.Y < npc.Center.Y)
                            {
                                shouldHeight -= (npc.Center.Y - player.Center.Y) * 0.0625f;
                            }
                            if (shouldHeight < -20f)
                                shouldHeight = -20f;
                            Shooting(2, npc.Center, player.Center + player.velocity * 85f, 0.0f, Main.rand.NextFloat(9.6f, 14.9f) * 3f, shouldHeight);
                            water -= 8;
                        }
                        if (Timer >= 185)
                        {
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.Dash:
                    {
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2(30 * 16 * (Main.rand.NextBool(2) ? 1f : -1f)
                                , 20 * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            npc.netUpdate = true;
                        }
                        if (Timer <= 15)
                            npc.alpha += 255 / 15 + 1;
                        if (Timer == 25)
                        {
                            npc.direction = npc.Center.X > player.Center.X ? -1 : 1;
                            npc.spriteDirection = npc.direction;
                            npc.velocity.X = npc.direction * 24;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, -1, 1f, 0f);
                        }
                        if ((Timer - 25) % 5 == 0)
                        {
                            Shooting(0, npc.Center, player.Center, 0f, 20f);
                            water -= 2;
                        }
                        if (Timer == 40 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC migrant = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<Migrant>())];
                            migrant.scale = 1f + (float)water / (float)waterMax * 0.4f;
                            water -= 32;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, migrant.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        if (Timer >= 65)
                        {
                            Timer = 0;
                            npc.velocity.X = 0f;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.Rotat:
                    {
                        npc.velocity = Vector2.Zero;
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2(0f, 26 * 16 * (Main.rand.NextBool(2) ? 1f : -1f));
                            npc.netUpdate = true;

                            int insideTiles = 0;
                            for (int i = 0; i < npc.width / 16; i++)
                                for (int j = 0; j < npc.height / 16; j++)
                                {
                                    Tile tile = Main.tile[i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16];
                                    if (Main.tileSolid[tile.type] && tile.active() && tile != null)
                                        insideTiles++;
                                }
                            if (insideTiles >= 12)
                            {
                                npc.Center = new Vector2(npc.Center.X, npc.Center.Y + -(npc.Center.Y - player.Center.Y));
                            }
                        }
                        if (Timer <= 15)
                            npc.alpha += 255 / 15 + 1;
                        npc.ai[3] += 0.006f;
                        if (npc.ai[3] >= 0.3f)
                        {
                            if (npc.ai[3] >= 0.55f)
                            {
                                npc.ai[3] = 0.55f;
                            }
                            npc.frameCounter += npc.ai[3];
                            // 发射
                            if (Timer % 2 == 0 && Main.rand.Next(1, 4) <= 2 && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (Main.rand.NextBool(6))
                                {
                                    water -= 6;
                                    Shooting(1, npc.Center, npc.Center + new Vector2(0f, -10f), Main.rand.NextFloat(0.0f, MathHelper.TwoPi), Main.rand.Next(9, 20));
                                }
                                water -= 1;
                                Shooting(0, npc.Center, player.Center, Main.rand.NextFloat(0.0f, MathHelper.TwoPi), Main.rand.Next(10, 16));
                            }
                        }
                        if (Timer >= 245)
                        {
                            Timer = 0;
                            npc.ai[3] = 0f;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.FindingWaters:
                    {
                        Timer++;
                        npc.velocity = Vector2.Zero;
                        if (Timer == 1)
                        {
                            npc.Center = waterPos + new Vector2(50f, 50f);
                            npc.netUpdate = true;
                            if (water >= waterMax)
                                water = (int)(waterMax * 0.7f);
                        }
                        if (Timer <= 15)
                            npc.alpha += 255 / 15 + 1;
                        if (Timer >= 150 || water >= waterMax || npc.life >= npc.lifeMax)
                        {
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                case NPCState.Break_State2:
                    {
                        Timer++;
                        if (Timer > 10 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    SwitchState((int)NPCState.Move_State2);
                                    break;
                                default:
                                    SwitchState((int)NPCState.Round_State2);
                                    break;
                            }
                            npc.netUpdate = true;
                            Timer = 0;
                            npc.ai[2] = npc.ai[3] = 0;
                            IsState2 = true;
                        }
                        break;
                    }
                case NPCState.Move_State2:
                    {
                        npc.velocity = ModHelper.GetFromToVector(npc.Center, player.Center) * 8.2f;
                        Timer++;
                        if ((Timer - 30) % 50 == 0 && Timer > 60 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileType<ShootingLine>(), 0, 0f, 255, 0f, npc.whoAmI);
                            Main.projectile[proj].ai[1] = npc.whoAmI;
                            if (Main.dedServ)
                            {
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                            }
                        }
                        if (Timer == 250)
                        {
                            if (Main.rand.NextBool(3))
                            {
                                SwitchState((int)NPCState.Magic_State2);
                            }
                            else
                            {
                                SwitchState((int)NPCState.Round_State2);
                            }
                            npc.ai[2] = npc.ai[3] = Timer = 0;
                            return;
                        }
                        break;
                    }
                case NPCState.Magic_State2:
                    {
                        // 距离小于50，且没有成功过就进入
                        Vector2 targetPos = player.Center + new Vector2(0f, 320f);
                        if (Vector2.Distance(npc.Center, targetPos) > 50f && npc.ai[2] != 1)
                        {
                            npc.velocity = ModHelper.GetFromToVector(npc.Center, targetPos) * 36f;
                            return;
                        }
                        else if (npc.ai[2] != 1)
                        {
                            npc.ai[2] = 1;
                            Timer = 20;
                        }
                        if (npc.ai[2] == 1) Timer++;
                        if (Timer >= 20)
                        {
                            npc.velocity = Vector2.Zero;
                            npc.Center = targetPos;
                            npc.netUpdate = true;
                        }
                        if (Timer <= 25)
                        {
                            //npc.alpha -= 255 / 15 + 1;
                            //npc.alpha = Math.Max(0, npc.alpha);
                        }
                        if (Timer % 40 == 0 && Timer >= 70 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int offset = Timer % 80 == 40 ? 80 : 0;
                            for (float pos = -1600f; pos <= 1600f; pos += 320f)
                            {
                                int proj = Projectile.NewProjectile(player.Center - new Vector2(pos + offset, 1000f), Vector2.Zero, ProjectileType<ContimatedShark>(), 60, 3f, 255, 0f, 0f);
                                if (Main.dedServ)
                                {
                                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                                }
                            }
                        }
                        if (Timer >= 200f)
                        {
                            //npc.alpha += 255 / 15 + 1;
                            //npc.alpha = Math.Min(255, npc.alpha);
                        }
                        if (Timer >= 215f)
                        {
                            SwitchState((int)(NPCState.Move_State2));
                            npc.ai[2] = npc.ai[3] = Timer = 0;
                            return;
                        }
                        break;
                    }
                case NPCState.Round_State2:
                    {
                        // 规定ai2只有0,1,2两种状态
                        // ai3为记录中的本回合的总时长
                        // Timer为进入转圈状态的总时长
                        npc.ai[3]++;
                        // 距离小于36，且没有成功过就进入
                        Vector2 targetPos = player.Center + new Vector2(0f, -500f);
                        if (Vector2.Distance(npc.Center, targetPos) > 36f && npc.ai[2] < 1)
                        {
                            npc.velocity = ModHelper.GetFromToVector(npc.Center, targetPos) * 30f;
                            return;
                        }
                        else if (npc.ai[2] < 1)
                        {
                            recordedStartRoundPos = npc.Center;
                            npc.ai[2] = 1;
                        }
                        if (npc.ai[2] >= 1)
                        {
                            Timer++;
                            npc.velocity = Vector2.Zero;
                            // 缓冲时间
                            if (Timer >= 30 && npc.ai[2] != 2f)
                            {
                                npc.ai[2] = 2f;
                                Timer = 0;
                            }
                            if (npc.ai[2] != 2f) return;
                            float speedMultipiler = 3.4f;
                            float radians = MathHelper.ToRadians(Timer * speedMultipiler);
                            npc.velocity = new Vector2(16f * speedMultipiler) * radians.ToRotationVector2();
                            //Main.NewText($"Timer:{Timer}, Radians:{radians}, Velocity:{npc.velocity}.");

                            // 射击（向与NPC移动方向垂直的圈内向量射击）（其实我也不知道我这注释写的什么）
                            Vector2 shootTarget = npc.Center + npc.velocity;
                            // 旋转90°，也就是垂直，是向圆心射击
                            Shooting(3, npc.Center, shootTarget, 90f, 24f);
                            if (Main.expertMode && Timer % 5 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Shooting(3, npc.Center, shootTarget, 90f, 10f);
                            }

                            // for模拟一圈，用于显示粒子特效。因为属于特效所以服务器就别掺和了
                            if (!Main.dedServ)
                            {
                                Vector2 _position = recordedStartRoundPos;
                                for (int _Timer = 0; _Timer <= 360; _Timer += UpgradeOutBeach ? 2 : 1)
                                {
                                    float _radians = MathHelper.ToRadians(_Timer);
                                    Vector2 _velocity = new Vector2(16f) * _radians.ToRotationVector2();
                                    _position += _velocity;
                                    // 生成粒子
                                    Dust d = Dust.NewDustDirect(_position, 4, 4, MyDustId.LightBlueParticle);
                                    d.noGravity = true;
                                }
                            }

                            // 转了2.75圈
                            if (Timer >= 2.75 * (360 / speedMultipiler))
                            {
                                switch (Main.rand.Next(2))
                                {
                                    case 0:
                                        SwitchState((int)NPCState.Move_State2);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.Magic_State2);
                                        break;
                                }
                                npc.ai[2] = npc.ai[3] = Timer = 0;
                                return;
                            }
                        }
                        else
                        {
                            if (npc.ai[3] >= 240)
                            {
                                switch (Main.rand.Next(2))
                                {
                                    case 0:
                                        SwitchState((int)NPCState.Move_State2);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.Magic_State2);
                                        break;
                                }
                                npc.ai[2] = npc.ai[3] = Timer = 0;
                                return;
                            }
                        }
                        break;
                    }
                case NPCState.PlayerDead:
                    {
                        npc.TargetClosest(true);
                        Timer++;
                        if (!player.dead && player.active && player.ZoneBeach)
                        {
                            SwitchState((int)NPCState.Break);
                            Timer = 0;
                        }
                        else if (Timer >= 180)
                        {
                            npc.TargetClosest(true);
                            if (player.dead || !player.active || !player.ZoneBeach)
                            {
                                if (Timer <= 195)
                                    npc.alpha -= 255 / 15 + 1;
                                else npc.active = false;
                            }
                            else
                            {
                                SwitchState((int)NPCState.Break);
                                Timer = 0;
                            }
                        }
                        break;
                    }
            }
        }
        public void Shooting(int type, Vector2 position, Vector2 target, float rad, float speed, float StartY = -15f)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            if (type == 0)
            {
                int damage = (int)((Main.expertMode ? 0.72f : 1f) * (int)(npc.damage * 0.42) * (1f + (float)water / (float)waterMax * 0.62f));
                int shot = Projectile.NewProjectile(position, ((target - position).ToRotation() + rad).ToRotationVector2() * speed, ProjectileType<污染精华>(), damage, 0f, 255, type);
                Projectile shots = Main.projectile[shot];
                shots.scale = 1f + (float)water / (float)waterMax * 2f;
                npc.netUpdate = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shots.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (type == 1)
            {
                target.Y += 32;
                Vector2 sVelo = ((target - position).ToRotation() + rad).ToRotationVector2() * speed;
                NPC shark = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<PolluShark>(), 0, 0, 0, sVelo.Y - 5f, sVelo.X)];
                shark.scale = 1f + (float)water / (float)waterMax * 0.43f;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (type == 2)
            {
                const float fallSpeed = 0.32f;
                float min = -speed, max = speed;
                float mid = (min + max) * 0.5f;
                while (Math.Abs(max - min) > 0.001f)
                {
                    Vector2 vShark = npc.Center;
                    mid = (min + max) * 0.5f;
                    float maxHeight = 0f;
                    float beyond = 0f;
                    float veloY = StartY;
                    for (int i = 0; i < 1000; i++)
                    {
                        vShark.X += mid;
                        vShark.Y += veloY;
                        veloY += fallSpeed;
                        maxHeight = Math.Min(vShark.Y, maxHeight);
                        if (veloY > 1f && vShark.Y > target.Y)
                        {
                            beyond = vShark.X - target.X;
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
                NPC shark = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<PolluShark>(), 0, 0, 0, StartY, mid)];
                shark.scale = 1f + (float)water / (float)waterMax * 0.43f;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            if (type == 3)
            {
                // 专家模式敌对会翻4倍，所以改成40即为40*4=160
                int damage = Main.expertMode ? 40 : 80;
                int shot = Projectile.NewProjectile(position, ((target - position).ToRotation() + rad).ToRotationVector2() * speed, ProjectileType<ContimatedSpikeRound>(), damage, 0f, 255, type);
                if (speed >= 15f)
                {
                    Main.projectile[shot].timeLeft = 160;
                }
                npc.netUpdate = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shot, -1f, 0f, 0f, 0, 0, 0);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            npc.SimpleDrawShadow(spriteBatch, drawColor, Math.Min(10, (int)(npc.velocity.GetLength() * 0.7f)));
            return false;
        }
        internal bool FindWater(int areaX, int areaY)
        {
            for (int i = 0; i >= -areaX; i--)
            {
                for (int j = 0; j >= -areaY; j--)
                {
                    if (WorldGen.InWorld((int)npc.Center.X / 16 + i, (int)npc.Center.Y / 16 + j))
                    {
                        Tile tileSafely = Framing.GetTileSafely((int)npc.Center.X / 16 + i, (int)npc.Center.Y / 16 + j);
                        if (tileSafely.liquidType() == 0 && tileSafely.liquid > 50)
                        {
                            waterPos = new Vector2(npc.Center.X + i * 16f, npc.Center.Y + j * 16f);
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i <= areaX; i++)
            {
                for (int j = 0; j <= areaY; j++)
                {
                    if (WorldGen.InWorld((int)npc.Center.X / 16 + i, (int)npc.Center.Y / 16 + j))
                    {
                        Tile tileSafely = Framing.GetTileSafely((int)npc.Center.X / 16 + i, (int)npc.Center.Y / 16 + j);
                        if (tileSafely.liquidType() == 0 && tileSafely.liquid > 50)
                        {
                            waterPos = new Vector2(npc.Center.X + i * 16f, npc.Center.Y + j * 16f);
                            return true;
                        }
                    }
                }
            }
            waterPos = Vector2.Zero;
            return false;
        }
        internal static void TileSafe(int x, int y)
        {
            if (Main.tile[x, y] == null)
            {
                Main.tile[x, y] = new Tile();
            }
        }
        internal static bool Sponge(int x, int y, byte amount = 1)
        {
            Tile tileSafely = Framing.GetTileSafely(x, y);
            if (tileSafely.liquidType() == 0)
            {
                int num = (int)tileSafely.liquidType();
                int num2 = 0;
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                            TileSafe(i, j);
                            if ((int)Main.tile[i, j].liquidType() == num)
                            {
                                num2 += (int)Main.tile[i, j].liquid;
                            }
                        }
                    }
                }
                if (tileSafely.liquid >= amount)
                {
                    if (Main.rand.NextBool(6))
                    {
                        int liquidType = (int)tileSafely.liquidType();
                        int num3 = (int)tileSafely.liquid;
                        tileSafely.liquid -= amount;
                        tileSafely.lava(false);
                        tileSafely.honey(false);
                        WorldGen.SquareTileFrame(x, y, false);
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.sendWater(x, y);
                        }
                        else
                        {
                            Liquid.AddWater(x, y);
                        }
                        for (int k = x - 1; k <= x + 1; k++)
                        {
                            for (int l = y - 1; l <= y + 1; l++)
                            {
                                Tile tileSafely2 = Framing.GetTileSafely(k, l);
                                if (num3 < 256 && (int)tileSafely2.liquidType() == num)
                                {
                                    int num4 = (int)tileSafely2.liquid;
                                    if (num4 + num3 > 255)
                                    {
                                        num4 = 255 - num3;
                                    }
                                    num3 += num4;
                                    Tile tile = tileSafely2;
                                    tile.liquid -= (byte)num4;
                                    tileSafely2.liquidType(liquidType);
                                    if (tileSafely2.liquid == 0)
                                    {
                                        tileSafely2.lava(false);
                                        tileSafely2.honey(false);
                                    }
                                    WorldGen.SquareTileFrame(k, l, false);
                                    if (Main.netMode != NetmodeID.SinglePlayer)
                                    {
                                        NetMessage.sendWater(k, l);
                                    }
                                    else
                                    {
                                        Liquid.AddWater(k, l);
                                    }
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        internal static bool PlaceWater(int x, int y, byte amount)
        {
            Tile tileSafely = Framing.GetTileSafely(x, y);
            if (WorldGen.InWorld(x, y))
            {
                if (tileSafely.liquid < 230 && (!tileSafely.nactive() || !Main.tileSolid[(int)tileSafely.type] || Main.tileSolidTop[(int)tileSafely.type]) && (tileSafely.liquid == 0 || tileSafely.liquidType() == 0))
                {
                    tileSafely.liquidType(0);
                    tileSafely.liquid = amount;
                    WorldGen.SquareTileFrame(x, y, true);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        NetMessage.sendWater(x, y);
                    }
                    return true;
                }
            }
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            water -= Main.rand.Next(1, 4);
            if ((float)npc.life <= 0)
            {
                if (!EntrogicWorld.IsDownedPollutionElemental)
                {
                    Item.NewItem(npc.getRect(), ItemType<PETrophy>());

                    string text2 = "Mods.Entrogic.Pollution_Pollutional";
                    string text3 = "Mods.Entrogic.Pollution_SkyDarkened";
                    Color orange = Color.Orange;
                    Color GreenYellow = Color.GreenYellow;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(Language.GetTextValue(text2), orange);
                        Main.NewText(Language.GetTextValue(text3), GreenYellow);
                    }
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        NetTextModule.SerializeServerMessage(NetworkText.FromKey(text2, new object[0]), orange);
                        NetTextModule.SerializeServerMessage(NetworkText.FromKey(text3, new object[0]), GreenYellow);
                    }
                    EntrogicWorld.IsDownedPollutionElemental = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
                else
                {
                    if (Main.rand.Next(10) == 0)
                        Item.NewItem(npc.getRect(), ItemType<PETrophy>());
                }
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage;
        }
    }
    public class 污染精华 : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Projectiles/污染尖刺"; } }
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.Size = new Vector2(12, 12);
            projectile.penetrate = 6;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 450;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            if (projectile.ai[0] < 3f)
            {
                Dust d = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.White, projectile.velocity.X, projectile.velocity.Y, 50, new Color(83, 173, 155), projectile.scale - 0.2f)];
                d.noGravity = true;
                d.velocity *= 0.3f;
            }
            else
                projectile.ai[0] = 0f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 83 / 255f, 173 / 255f, 155 / 255f);
        }
    }
}