using Entrogic.Items.VoluGels;


using Microsoft.Xna.Framework;
using System;
using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    /// <summary>
    /// 创建时间：2019/8/1 9:05:38
    /// </summary>
    [AutoloadBossHead]
    public class 嘉沃顿 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 148;
            npc.height = 280;
            npc.aiStyle = -1;
            npc.damage = 50;
            if (Main.expertMode)
            {
                npc.damage = 65;
                if (Entrogic.IsCalamityLoaded)
                {
                    if (Entrogic.IsCalamityModRevengenceMode)
                    {
                        npc.damage = 85;
                    }
                    if (Entrogic.IsCalamityModDeathMode)
                    {
                        npc.damage = 120;
                    }
                }
                npc.damage /= 2;
            }
            npc.defense = 8;
            npc.lifeMax = 1175;
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax = 1680;
                }
                if (Entrogic.IsCalamityModDeathMode)
                {
                    npc.lifeMax = 2020;
                }
            }
            npc.npcSlots = 15f;
            npc.value = Item.buyPrice(0, 7, 0, 0);
            npc.knockBackResist = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 30;
            npc.value = 15000f;
            npc.buffImmune[20] = true;
            npc.timeLeft = NPC.activeTime * 30;
            music = MusicID.Boss2;
            musicPriority = MusicPriority.BossLow;
            bossBag = ItemType<瓦卢提奥宝藏袋>();
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if ((float)npc.life <= 0)
            {
                if (!EntrogicWorld.downedGelSymbiosis)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("VTrophy"));
                    EntrogicWorld.downedGelSymbiosis = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
                else
                {
                    if (Main.rand.Next(10) == 0)
                        Item.NewItem(npc.getRect(), mod.ItemType("VTrophy"));
                }
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.69f * bossLifeScale);
            npc.defense += 4;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return base.DrawHealthBar(hbPosition, ref scale, ref position);
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 0;
            if (npc.ai[3] >= 240 && npc.ai[3] < 300)
                npc.frame.Y = frameHeight;
            if (npc.ai[3] >= 301 && npc.ai[3] < 480)
                npc.frame.Y = frameHeight * 2;
        }
        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0) Item.NewItem(npc.getRect(), mod.ItemType("瓦卢提奥头套"));
                Item.NewItem(npc.getRect(), mod.ItemType("生命凝胶"), Main.rand.Next(11, 14 + 1));//11-14
            }
            Item.NewItem(npc.getRect(), mod.ItemType("魔力导流台"));
        }
        const float DefMax = 0.4f;// 这里是伤害减免(目前40%) 下面的Vector2: X = Def, Y = Damage
        Vector2 Melee = Vector2.Zero;
        Vector2 Magic = Vector2.Zero;
        Vector2 Ranged = Vector2.Zero;
        Vector2 Thrown = Vector2.Zero;
        Vector2 Summon = Vector2.Zero;
        Vector2 Another = Vector2.Zero;

        int[] Timer = new int[4];
        float SpeedX = 0;
        float SpeedY = 0;

        int dire = -1;
        int direY = -1;
        float radLaser = 0.0f;
        public override void SendExtraAI(BinaryWriter writer)
        {
            for (int i = 0; i < Timer.Length; i++)
                writer.Write(Timer[i]);
            writer.Write(SpeedX);
            writer.Write(SpeedY);

            writer.WriteVector2(Melee);
            writer.WriteVector2(Magic);
            writer.WriteVector2(Ranged);
            writer.WriteVector2(Thrown);
            writer.WriteVector2(Summon);
            writer.WriteVector2(Another);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < Timer.Length; i++)
                Timer[i] = reader.ReadInt32();
            SpeedX = reader.ReadSingle();
            SpeedY = reader.ReadSingle();

            Melee = reader.ReadVector2();
            Magic = reader.ReadVector2();
            Summon = reader.ReadVector2();
            Thrown = reader.ReadVector2();
            Ranged = reader.ReadVector2();
            Another = reader.ReadVector2();
        }
        public override void AI()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            Vector2 distance = new Vector2(Math.Abs(npc.Center.X - Main.player[npc.target].Center.X), Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y));
            distance /= 16;
            if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneRockLayerHeight || Math.Max((byte)distance.X, (byte)distance.Y) > 255)
            {
                npc.TargetClosest(true);
                distance = new Vector2(Math.Abs(npc.Center.X - Main.player[npc.target].Center.X), Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y));
                distance /= 16;
                if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneRockLayerHeight || Math.Max((byte)distance.X, (byte)distance.Y) > 255)
                {
                    npc.noGravity = false;
                    npc.noTileCollide = true;
                    return;
                }
            }
            else
                npc.timeLeft = 10;
            #region Move
            npc.rotation = npc.velocity.X / 15;
            if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }
            npc.alpha = 0;
            float num312 = EntrogicWorld.Check(npc.Center.X, npc.Center.Y) ? 4f : 7.5f;
            npc.TargetClosest(true);
            if ((Main.player[npc.target].Center.Y - npc.height - Main.rand.Next(64, 80)) > npc.Center.Y)
                npc.directionY = 1;
            else
                npc.directionY = -1;
            if (npc.timeLeft > 10)
            {
                npc.timeLeft = 10;
            }
            if (npc.direction < 0 && npc.velocity.X < 0f)
            {
                Timer[0]++;
                if (Timer[0] > 30)
                    SpeedX -= 0.05f;
            }
            if (npc.direction > 0 && npc.velocity.X > 0f)
            {
                Timer[0]++;
                if (Timer[0] > 30)
                    SpeedX += 0.05f;
            }
            if (npc.direction < 0 && npc.velocity.X > 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.9f;
                SpeedX = 0;
                Timer[0] = 0;
            }
            if (npc.direction > 0 && npc.velocity.X < 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.9f;
                SpeedX = 0;
                Timer[0] = 0;
            }
            if (npc.direction == -1 && npc.velocity.X > -num312 + SpeedX)
            {
                npc.velocity.X = npc.velocity.X - 0.1f + SpeedX;
                if (npc.velocity.X > num312)
                {
                    npc.velocity.X = npc.velocity.X - 0.1f + SpeedX;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X = npc.velocity.X + (0.05f + -SpeedX);
                }
                if (npc.velocity.X < -num312 + SpeedX)
                {
                    npc.velocity.X = -num312 + SpeedX;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num312 + SpeedX)
            {
                npc.velocity.X = npc.velocity.X + 0.1f + SpeedX;
                if (npc.velocity.X < -num312)
                {
                    npc.velocity.X = npc.velocity.X + 0.1f + SpeedX;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X = npc.velocity.X - (0.05f + -SpeedX);
                }
                if (npc.velocity.X > num312 + SpeedX)
                {
                    npc.velocity.X = num312 + SpeedX;
                }
            }
            if (npc.directionY < 0 && npc.velocity.Y < 0f)
            {
                Timer[1]++;
                if (Timer[1] > 30)
                    SpeedY -= 0.05f;
            }
            if (npc.directionY > 0 && npc.velocity.Y > 0f)
            {
                Timer[1]++;
                if (Timer[1] > 30)
                    SpeedY += 0.05f;
            }
            if (npc.directionY < 0 && npc.velocity.Y > 0f)
            {
                npc.velocity.Y = npc.velocity.Y * 0.9f;
                SpeedY = 0;
                Timer[1] = 0;
            }
            if (npc.directionY > 0 && npc.velocity.Y < 0f)
            {
                npc.velocity.Y = npc.velocity.Y * 0.9f;
                SpeedY = 0;
                Timer[1] = 0;
            }
            if (npc.directionY == -1 && npc.velocity.Y > -num312 + SpeedY)
            {
                npc.velocity.Y = npc.velocity.Y - 0.1f + SpeedY;
                if (npc.velocity.Y > num312)
                {
                    npc.velocity.Y = npc.velocity.Y - 0.1f + SpeedY;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + (0.05f + -SpeedY);
                }
                if (npc.velocity.Y < -num312 + SpeedY)
                {
                    npc.velocity.Y = -num312 + SpeedY;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < num312 + SpeedY)
            {
                npc.velocity.Y = npc.velocity.Y + 0.1f + SpeedY;
                if (npc.velocity.Y < -num312)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.1f + SpeedY;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - (0.05f + -SpeedY);
                }
                if (npc.velocity.Y > num312 + SpeedY)
                {
                    npc.velocity.Y = num312 + SpeedY;
                }
            }
            #endregion
            if (EntrogicWorld.Check(npc.Center.X, npc.Center.Y))
            {
                Timer[2]++;
                if (Timer[2] % 12 == 0)
                {
                    int healAmount = 2;// 每秒回复5次, 2*5=10
                    int disLife = npc.lifeMax - npc.life;
                    if (disLife < 2)
                        healAmount = disLife;
                    npc.life += healAmount;
                    if (healAmount > 0)
                        npc.HealEffect(healAmount);
                }
            }
            Vector2 vec = new Vector2(0f, 0.0001f);
            vec = Vector2.Normalize(vec);
            npc.ai[3]++;
            if (npc.ai[3] >= 240 && npc.ai[3] < 300)// 蓄力
            {
                npc.velocity = Vector2.Zero;
                Vector2 vector109 = npc.Center + new Vector2((float)(npc.direction * 2), 100f);
                float scaleFactor9 = MathHelper.Lerp(30f, 10f, (npc.ai[0] - 10f) / 180f);
                float num910 = Main.rand.NextFloat() * 6.28318548f;
                for (float num911 = 0f; num911 < 2f; num911 += 1f)
                {
                    Vector2 value46 = Vector2.UnitY.RotatedBy((double)(num911 / 1f * 6.28318548f + num910), default(Vector2));
                    Dust dust104 = Main.dust[Dust.NewDust(vector109, 0, 0, MyDustId.GreyPebble, 0f, 0f, 0, new Color(255, 98, 71, 137), 2f)];
                    dust104.position = vector109 + value46 * scaleFactor9;
                    dust104.noGravity = true;
                    dust104.velocity = value46 * -2f;
                }
            }
            else if (npc.ai[3] == 300)
            {
                if (Main.netMode != 1)
                {
                    Main.PlaySound(SoundID.Zombie, npc.Center, 104);
                    if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) > npc.position.X + (float)(npc.width / 2))
                        dire = 1;
                    else
                        dire = -1;
                    if (Main.player[npc.target].Center.Y > npc.Center.Y + 100f)
                        direY = 1;
                    else
                        direY = -1;
                    if (dire == -1 && direY == -1)//0为Down
                        radLaser = MathHelper.TwoPi / 360 * 135;
                    else if (dire == 1 && direY == 1)
                        radLaser = MathHelper.TwoPi / 360 * 315;
                    else if (dire == 1 && direY == -1)
                        radLaser = MathHelper.TwoPi / 360 * 45;
                    else if (dire == -1 && direY == 1)
                        radLaser = MathHelper.TwoPi / 360 * 225;
                    // 定位到Player
                    Vector2 finalVec = (vec.ToRotation() + radLaser).ToRotationVector2();
                    finalVec = Vector2.Normalize(finalVec);
                    npc.velocity = Vector2.Zero;
                    int damage = Main.expertMode ? 50 : 100;
                    if (Entrogic.IsCalamityLoaded)
                    {
                        if (Entrogic.IsCalamityModRevengenceMode)
                        {
                            damage = 125; // 250
                        }
                        else if (Entrogic.IsCalamityModDeathMode)
                        {
                            damage = 300; // 600
                        }
                    }
                    Projectile.NewProjectile(npc.Center + new Vector2(0f, 120f), new Vector2(14f), mod.ProjectileType("共生体死光"), damage, 10, Main.myPlayer, 0, npc.whoAmI);
                    npc.netUpdate = true;
                }
            }
            else if (npc.ai[3] >= 301 && npc.ai[3] < 480)
            {
                npc.velocity = Vector2.Zero;
                radLaser += (MathHelper.TwoPi / 360);
                Vector2 finalVec = (vec.ToRotation() + radLaser).ToRotationVector2();
                npc.localAI[0] = finalVec.X; npc.localAI[1] = finalVec.Y;
            }
            else if (npc.ai[3] == 480)
            {
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<IREUL细胞>());
                npc.ai[3] = 0;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<Buffs.Enemies.溶解>(), Main.rand.Next(90, 151) * (Main.expertMode ? (int)Main.expertDebuffTime : 1));
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.melee)
            {
                if (Melee.X < DefMax)
                {
                    Melee.Y += damage;
                }
                while (Melee.Y >= 60)
                {
                    Melee.X += 0.01f;
                    Melee.Y -= 60;
                }
                if (Melee.X >= DefMax)
                    Melee.X = DefMax;
                damage = (int)((float)damage * (1f - Melee.X));
            }
            else if (proj.ranged)
            {
                if (Ranged.X < DefMax)
                {
                    Ranged.Y += damage;
                }
                while (Ranged.Y >= 60)
                {
                    Ranged.X += 0.01f;
                    Ranged.Y -= 60;
                }
                if (Ranged.X >= DefMax)
                    Ranged.X = DefMax;
                damage = (int)((float)damage * (1f - Ranged.X));
            }
            else if (proj.minion)
            {
                if (Summon.X < DefMax)
                {
                    Summon.Y += damage;
                }
                while (Summon.Y >= 60)
                {
                    Summon.X += 0.01f;
                    Summon.Y -= 60;
                }
                if (Summon.X >= DefMax)
                    Summon.X = DefMax;
                damage = (int)((float)damage * (1f - Summon.X));
            }
            else if (proj.magic)
            {
                if (Magic.X < DefMax)
                {
                    Magic.Y += damage;
                }
                while (Magic.Y >= 60)
                {
                    Magic.X += 0.01f;
                    Magic.Y -= 60;
                }
                if (Magic.X >= DefMax)
                    Magic.X = DefMax;
                damage = (int)((float)damage * (1f - Magic.X));
            }
            else if (proj.thrown)
            {
                if (Thrown.X < DefMax)
                {
                    Thrown.Y += damage;
                }
                while (Thrown.Y >= 60)
                {
                    Thrown.X += 0.01f;
                    Thrown.Y -= 60;
                }
                if (Thrown.X >= DefMax)
                    Thrown.X = DefMax;
                damage = (int)((float)damage * (1f - Thrown.X));
            }
            else
            {
                if (Another.X < DefMax)
                {
                    Another.Y += damage;
                }
                while (Another.Y >= 60)
                {
                    Another.X += 0.01f;
                    Another.Y -= 60;
                }
                if (Another.X >= DefMax)
                    Another.X = DefMax;
                damage = (int)((float)damage * (1f - Another.X));
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.melee)
            {
                if (Melee.X < DefMax)
                {
                    Melee.Y += damage;
                }
                while (Melee.Y >= 60)
                {
                    Melee.X += 0.01f;
                    Melee.Y -= 60;
                }
                if (Melee.X >= DefMax)
                    Melee.X = DefMax;
                damage = (int)((float)damage * (1f - Melee.X));
            }
            else if (item.ranged)
            {
                if (Ranged.X < DefMax)
                {
                    Ranged.Y += damage;
                }
                while (Ranged.Y >= 60)
                {
                    Ranged.X += 0.01f;
                    Ranged.Y -= 60;
                }
                if (Ranged.X >= DefMax)
                    Ranged.X = DefMax;
                damage = (int)((float)damage * (1f - Ranged.X));
            }
            else if (item.summon)
            {
                if (Summon.X < DefMax)
                {
                    Summon.Y += damage;
                }
                while (Summon.Y >= 60)
                {
                    Summon.X += 0.01f;
                    Summon.Y -= 60;
                }
                if (Summon.X >= DefMax)
                    Summon.X = DefMax;
                damage = (int)((float)damage * (1f - Summon.X));
            }
            else if (item.magic)
            {
                if (Magic.X < DefMax)
                {
                    Magic.Y += damage;
                }
                while (Magic.Y >= 60)
                {
                    Magic.X += 0.01f;
                    Magic.Y -= 60;
                }
                if (Magic.X >= DefMax)
                    Magic.X = DefMax;
                damage = (int)((float)damage * (1f - Magic.X));
            }
            else if (item.thrown)
            {
                if (Thrown.X < DefMax)
                {
                    Thrown.Y += damage;
                }
                while (Thrown.Y >= 60)
                {
                    Thrown.X += 0.01f;
                    Thrown.Y -= 60;
                }
                if (Thrown.X >= DefMax)
                    Thrown.X = DefMax;
                damage = (int)((float)damage * (1f - Thrown.X));
            }
            else
            {
                if (Another.X < DefMax)
                {
                    Another.Y += damage;
                }
                while (Another.Y >= 60)
                {
                    Another.X += 0.01f;
                    Another.Y -= 60;
                }
                if (Another.X >= DefMax)
                    Another.X = DefMax;
                damage = (int)((float)damage * (1f - Another.X));
            }
        }
    }
}
