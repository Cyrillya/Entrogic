using Entrogic.Items.AntaGolem;
using Entrogic.NPCs.Boss.AntaGolem.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.AntaGolem
{
    [AutoloadBossHead]
    public class Antanasy : FSM_NPC
    {
        enum NPCState
        {
            // 初始化
            Start,
            StartTimer,
            // 休息时间
            Break,

            // 冲刺
            Dash,
            // 环绕石子传送门
            Spawner,
            // 石子360°×5
            Stones,

            // 第二阶段切换动画
            StrongCycle,
            // 第二阶段-横向冲刺
            LevelDash_Strong,
            // 第二阶段-全方位冲刺
            Dash_Strong,
            // 第二阶段-石子360°×6
            Stones_Strong,

            // 玩家死亡
            PlayerDead
        }
        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 74;
            npc.height = 104;
            npc.lifeMax = 1640;
            npc.defense = 8;
            npc.damage = 41;
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax = 2100;
                }
                if (Entrogic.IsCalamityModDeathMode)
                {
                    npc.lifeMax = 3000;
                    npc.damage = 45;
                }
            }
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.ShadowFlame] = true;
            npc.buffImmune[BuffID.StardustGuardianMinion] = true;
            npc.buffImmune[BuffID.StardustDragonMinion] = true;
            npc.knockBackResist = 0f;
            npc.npcSlots = 15;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 7, 0, 0);
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath37;
            npc.timeLeft = NPC.activeTime * 30;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/狂気の瞳");
            musicPriority = MusicPriority.BossLow;
            bossBag = ItemType<衰落魔像宝藏袋>();
            npc.alpha = 255;
        }
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0) Item.NewItem(npc.getRect(), mod.ItemType("魔像头套"));
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem(npc.getRect(), mod.ItemType("岩石猎枪"));
                        break;
                    case 1:
                        Item.NewItem(npc.getRect(), mod.ItemType("巨石长枪"));
                        break;
                    default:
                        Item.NewItem(npc.getRect(), mod.ItemType("StoneSlimeStaff"));
                        break;
                }
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return true;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity * 0.7f, mod.GetGoreSlot("Gores/衰落魔像Gore"), 1f);
                Gore.NewGore(npc.Center, npc.velocity * 0.8f, mod.GetGoreSlot("Gores/衰落魔像Gore2"), 1f);
                Gore.NewGore(npc.Center, npc.velocity * 0.9f, mod.GetGoreSlot("Gores/衰落魔像Gore2"), 1f);
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/衰落魔像Gore3"), 1f);
                if (!EntrogicWorld.downedAthanasy)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("ATrophy"));
                    for (int i = 0; i < 80; i++)
                    {
                        Gore.NewGore(npc.Center, npc.velocity * i / 80, mod.GetGoreSlot("Gores/衰落魔像Gore3"), 1f);
                    }
                    EntrogicWorld.downedAthanasy = true;
                    if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
                    return;
                }
                if (Main.rand.Next(10) == 0)
                    Item.NewItem(npc.getRect(), mod.ItemType("ATrophy"));
                return;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.expertMode)
                target.AddBuff(BuffID.Bleeding, 270);
            if (Entrogic.IsCalamityModDeathMode)
                target.AddBuff(BuffID.Confused, 180);
            if (!target.stoned && Main.rand.NextBool(2))
                target.AddBuff(BuffID.Stoned, Main.rand.Next(30, 90));
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModDeathMode)
                {
                    damage = (int)(damage * 0.75f);
                }
                return;
            }
            damage = (int)(damage * 1.15f);
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85f * bossLifeScale);
            if (numPlayers > 5)
                numPlayers = 5;
            npc.damage = (int)(npc.damage + (npc.damage * numPlayers * 0.2f));
            npc.defense += 6;
        }
        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.alpha = Math.Max(0, npc.alpha);
            npc.alpha = Math.Min(255, npc.alpha);
            npc.dontTakeDamage = npc.noGravity = npc.noTileCollide = false;
            if (npc.ai[2] != 0f)
                npc.noGravity = npc.noTileCollide = true;
            if (player.dead || !player.active)
                SwitchState((int)NPCState.PlayerDead);
            switch ((NPCState)State)
            {
                // 开始状态下
                case NPCState.Start:
                    {
                        npc.TargetClosest(true);
                        npc.Center = player.Center;
                        npc.position.Y -= 800;
                        npc.netUpdate = true;
                        npc.alpha = 255;
                        SwitchState((int)NPCState.StartTimer);
                        break;
                    }
                case NPCState.StartTimer:
                    {
                        Timer++;
                        if (Timer < 60)
                            npc.noTileCollide = true;
                        else
                        {
                            npc.TargetClosest(true);
                            npc.spriteDirection = npc.direction;
                            SwitchState((int)NPCState.Dash);
                            Timer = 0;
                        }
                        break;
                    }
                // 休息状态下
                case NPCState.Break:
                    {
                        Timer++;
                        if (npc.ai[2] != 0f)
                        {
                            if (Timer <= 15)
                                npc.alpha -= 255 / 15 + 1;
                            npc.dontTakeDamage = true;
                        }
                        int findTimer = 95;
                        if (Entrogic.IsCalamityLoaded)
                        {
                            findTimer = 80;
                            if (Entrogic.IsCalamityModDeathMode)
                            {
                                findTimer = 60;
                            }
                        }
                        if (npc.ai[2] != 0f)
                        {
                            findTimer -= 20;
                        }
                        if (Timer >= findTimer)
                        {
                            npc.TargetClosest(true);
                            npc.spriteDirection = npc.direction;
                            if (npc.ai[2] == 0f)
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        int insideTiles = 0;
                                        for (int i = 0; i < npc.width / 16; i++)
                                            for (int j = 0; j < npc.height / 16; j++)
                                            {
                                                if (WorldGen.InWorld(i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16))
                                                {
                                                    Tile tile = Main.tile[i + (int)npc.position.X / 16, j + (int)npc.position.Y / 16];
                                                    if (Main.tileSolid[tile.type] && tile.active())
                                                        insideTiles++;
                                                }
                                            }
                                        if (Math.Abs(player.Center.Y - npc.Center.Y) > 500 || insideTiles > 10)
                                            npc.Center = player.Center - new Vector2(480, 300);
                                        SwitchState((int)NPCState.Dash);
                                        Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, 0, 1f, 0f);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.Spawner);
                                        break;
                                    default:
                                        if (Math.Abs(player.Center.X - npc.Center.X) < 1024 && Math.Abs(player.Center.Y - npc.Center.Y) < 512)
                                            SwitchState((int)NPCState.Stones);
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        SwitchState((int)NPCState.Dash_Strong);
                                        break;
                                    case 1:
                                        SwitchState((int)NPCState.LevelDash_Strong);
                                        break;
                                    default:
                                        SwitchState((int)NPCState.Stones_Strong);
                                        break;
                                }
                            }
                            npc.netUpdate = true;
                            Timer = 0;
                            if (npc.ai[2] == 0 && npc.life <= npc.lifeMax * 0.39f)
                            {
                                SwitchState((int)NPCState.StrongCycle);
                                foreach (Player plr in Main.player)
                                {
                                    if (plr.active && !plr.dead)
                                    {
                                        EntrogicPlayer.ModPlayer(plr).AthanasyTimer++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                // 冲刺状态下
                case NPCState.Dash:
                    {
                        if (Math.Abs(npc.Center.X - player.Center.X) < 64 || Timer != 0)
                        {
                            Timer++;
                            if (Timer == 5)
                            {
                                int speed = 16;
                                ShootingStones(player.Center, 0f, speed);
                                ShootingStones(player.Center, 15f / 360f * MathHelper.TwoPi, speed);
                                ShootingStones(player.Center, -(15f / 360f * MathHelper.TwoPi), speed);
                                ShootingStones(player.Center, 30f / 360f * MathHelper.TwoPi, speed);
                                ShootingStones(player.Center, -(30f / 360f * MathHelper.TwoPi), speed);
                            }
                            if (Math.Abs(npc.Center.X - player.Center.X) <= 12)
                            {
                                npc.velocity.X = 0f;
                                if (Timer >= 5)
                                    SwitchState((int)NPCState.Break);
                                return;
                            }
                            if (Timer < 15)
                                npc.velocity.X *= 0.74f;
                            else
                            {
                                npc.velocity.X = 0f;
                                SwitchState((int)NPCState.Break);
                            }
                            return;
                        }
                        int dire = npc.Center.X > player.Center.X ? -1 : 1;
                        npc.spriteDirection = dire;
                        npc.velocity.X = dire * 18f;
                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        break;
                    }
                // 石子传送门状态下
                case NPCState.Spawner:
                    {
                        npc.noGravity = true;
                        npc.noTileCollide = true;
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2((Main.rand.NextBool(2) ? -1 : 1) * 256, -256);
                            npc.netUpdate = true;
                        }
                        if (Timer < 40)
                            npc.dontTakeDamage = true;
                        else
                        {
                            if (Timer == 40)
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<石子传送门>(), 0, 0f, Main.myPlayer, npc.whoAmI, npc.damage * 0.26f);
                                    npc.netUpdate = true;
                                }
                            if (Timer == 50)
                            {
                                npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * 35f;
                                Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, -1, 1f, 0f);
                            }
                            if (Timer < 70)
                            {
                                npc.direction = npc.Center.X > player.Center.X ? -1 : 1;
                                npc.spriteDirection = npc.direction;
                                npc.velocity *= 0.99f;
                            }
                            else
                            {
                                npc.velocity *= 0.67f;
                                if (Timer == 90)
                                {
                                    SwitchState((int)NPCState.Break);
                                    Timer = 0;
                                }
                            }
                        }
                        break;
                    }
                // 射石子状态下
                case NPCState.Stones:
                    {
                        Timer++;
                        int maxTimers = 180;
                        List<int> shootingTimers = new List<int>
                        {
                            maxTimers/5,
                            maxTimers/5*2,
                            maxTimers/5*3,
                            maxTimers/5*4,
                            maxTimers
                        };
                        foreach (int Timers in shootingTimers)
                            if (Timer == Timers)
                            {
                                for (float rad = 0f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / 12f)
                                    ShootingStones(player.position, rad, 20, 2.5f);
                                if (Timer == maxTimers)
                                {
                                    SwitchState((int)NPCState.Break);
                                    Timer = 0;
                                }
                            }
                        break;
                    }
                // 第二阶段切换动画
                case NPCState.StrongCycle:
                    {
                        npc.noGravity = true;
                        npc.noTileCollide = true;
                        npc.dontTakeDamage = true;
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.defense = 0;
                            npc.damage += 28;
                        }
                        if (Timer > 90)
                        {
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                            npc.ai[2] = 1;
                        }
                        break;
                    }
                // 全方位冲刺-第二阶段状态下
                case NPCState.Dash_Strong:
                    {
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2(0f, (Main.rand.NextBool(2) ? 1f : -1f) * 20 * 16);
                            npc.netUpdate = true;
                        }
                        if (Timer <= 15)
                            npc.alpha += 255 / 15 + 1;
                        List<int> Timers = new List<int>
                        {
                            32,
                            54,
                            76
                        };
                        foreach (int timers in Timers)
                            if (Timer == timers)
                            {
                                npc.direction = npc.Center.X > player.Center.X ? -1 : 1;
                                npc.spriteDirection = npc.direction;
                                npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * 20f;
                                Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, 0, 1f, 0.2f);
                            }
                        if (Timer < 98)
                            npc.velocity *= 0.97f;
                        if (Timer == 98)
                        {
                            npc.velocity = Vector2.Zero;
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                // 横向冲刺-第二阶段状态下
                case NPCState.LevelDash_Strong:
                    {
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2((Main.rand.NextBool(2) ? 1f : -1f) * 20 * 16, 0f);
                            npc.netUpdate = true;
                        }
                        if (Timer <= 20)
                            npc.alpha += 255 / 20 + 1;
                        if (Timer == 40)
                        {
                            npc.direction = npc.Center.X > player.Center.X ? -1 : 1;
                            npc.spriteDirection = npc.direction;
                            npc.velocity.X = npc.direction * 75;
                            Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, -1, 1f, 0f);
                        }
                        if (Timer < 80)
                            npc.velocity.X *= 0.94f;
                        else
                        {
                            npc.velocity = Vector2.Zero;
                            Timer = 0;
                            SwitchState((int)NPCState.Break);
                        }
                        break;
                    }
                // 射石子-第二阶段状态下
                case NPCState.Stones_Strong:
                    {
                        Timer++;
                        if (Timer == 1)
                        {
                            npc.Center = player.Center + new Vector2(16 * 16 * -player.direction, 0f);
                            npc.netUpdate = true;
                        }
                        if (Timer <= 25)
                        {
                            npc.alpha += 255 / 25 + 1;
                            npc.dontTakeDamage = true;
                        }
                        int delay = 25;
                        List<int> shootingTimers = new List<int>
                        {
                            delay,
                            delay*2,
                            delay*3,
                            delay*4,
                            delay*5
                        };
                        foreach (int Timers in shootingTimers)
                            if (Timer == Timers + 40)
                            {
                                for (float rad = 0f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / 16f)
                                    ShootingStones(player.position, rad, 20, 2.5f);
                            }
                        if (Timer == delay * 5 + 40)
                        {
                            SwitchState((int)NPCState.Break);
                            Timer = 0;
                        }
                        break;
                    }
                // 玩家死亡状态下
                case NPCState.PlayerDead:
                    {
                        npc.TargetClosest(true);
                        if (player.dead || !player.active)
                        {
                            npc.TargetClosest(true);
                            if (player.dead || !player.active)
                            {
                                npc.noTileCollide = true;
                                npc.noGravity = false;
                                Timer++;
                                if (Timer >= 100)
                                {
                                    npc.life = 0;
                                    npc.active = false;
                                    npc.netUpdate = true;
                                }
                                break;
                            }
                            else
                                SwitchState((int)NPCState.Break);
                        }
                        else
                            SwitchState((int)NPCState.Break);
                        break;
                    }
                // 其他状态
                default:
                    break;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            switch ((NPCState)State)
            {
                case NPCState.Break:
                    if (npc.ai[2] != 0)
                        texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
                case NPCState.Dash_Strong:
                    texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
                case NPCState.LevelDash_Strong:
                    texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
            }

            SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[npc.type] / 2));
            for (int i = 1; i < npc.oldPos.Length; i++)
            {
                Vector2 drawCenter = npc.Center - new Vector2(0f, 8f) + -(npc.velocity * 0.5f * i);
                int alpha = 40 - 40 / (npc.oldPos.Length - 1) * i - (255 - npc.alpha);
                alpha = Math.Max(0, alpha);
                Color drawOn = ModHelper.QuickAlpha(drawColor, (float)alpha / 255f);
                Main.spriteBatch.Draw(texture, drawCenter - Main.screenPosition, new Rectangle?(npc.frame), npc.GetAlpha(drawOn), 0f, origin, npc.scale, effects, 0f);
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            bool drawHighLight = false;
            Texture2D texture = Main.npcTexture[npc.type];
            SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            switch ((NPCState)State)
            {
                case NPCState.Break:
                    DrawingCharging(spriteBatch, drawColor);
                    if (npc.ai[2] != 0)
                        texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
                case NPCState.Spawner:
                    if (Timer < 40)
                    {
                        DrawingCharging(spriteBatch, drawColor);
                        drawHighLight = true;
                        texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    }
                    break;
                case NPCState.Stones:
                    drawHighLight = true;
                    break;
                case NPCState.StrongCycle:
                    int alpha = 255 / 90 * Timer;
                    alpha = Math.Max(0, alpha);
                    Color onDrawColor = ModHelper.QuickAlpha(drawColor, (float)alpha / 255f);
                    spriteBatch.Draw(Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow"), npc.Center - Main.screenPosition + new Vector2(0f, -8f), null, npc.GetAlpha(onDrawColor), 0f, texture.Size() * 0.5f, npc.scale, effects, 0f);
                    break;
                case NPCState.Dash_Strong:
                    if (Timer >= 10)
                        drawHighLight = true;
                    texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
                case NPCState.Stones_Strong:
                    if (Timer >= 10)
                        drawHighLight = true;
                    texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
                case NPCState.LevelDash_Strong:
                    if (Timer >= 10)
                        drawHighLight = true;
                    texture = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                    break;
            }
            drawColor = ModHelper.QuickAlpha(drawColor, (float)npc.alpha / 255f);
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0f, -8f), null, npc.GetAlpha(drawColor), npc.rotation, texture.Size() * 0.5f, npc.scale, effects, 0f);
            Texture2D tex = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_HighLight_SD");
            if (drawHighLight)
                spriteBatch.Draw(tex, npc.Center - Main.screenPosition + new Vector2(0f, -8f), null, npc.GetAlpha(drawColor), npc.rotation, tex.Size() * 0.5f, npc.scale, effects, 0f);
        }
        public void ShootingStones(Vector2 shotOn, float rad, float speed, float scale = 1.5f, int dmg = -1)
        {
            Vector2 vec = ((shotOn - npc.Center).ToRotation() + rad).ToRotationVector2() * speed;
            int damage = (int)((Main.expertMode ? 0.8f : 1f) * (dmg == -1 ? (int)(npc.damage * 0.32) : dmg));
            int proj = Projectile.NewProjectile(npc.Center, vec, mod.ProjectileType("魔像飞弹"), damage, 0f, Main.myPlayer);
            Main.projectile[proj].scale = scale;
            npc.netUpdate = true;
        }
        float chargeScale = 2f;
        public void DrawingCharging(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.ai[2] == 0)
            {
                Texture2D tex = Main.npcTexture[npc.type];
                switch ((NPCState)State)
                {
                    case NPCState.Spawner:
                        if (Timer < 40)
                            tex = Entrogic.Instance.GetTexture("NPCs/Boss/AntaGolem/Antanasy_Shadow");
                        break;
                }
                const float scaleLess = 1f;
                chargeScale -= 0.08f;
                if (chargeScale < scaleLess) chargeScale = 2f;
                int alpha = 40;
                SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, new Color(drawColor.R, drawColor.G, drawColor.B, alpha), npc.rotation, tex.Size() * 0.5f, chargeScale, effects, 0f);
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage && npc.alpha >= 200;
        }
    }
}