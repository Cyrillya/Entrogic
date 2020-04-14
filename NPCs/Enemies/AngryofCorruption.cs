using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.NPCs.Enemies
{
    public class AngryofCorruption : FSM_NPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        enum NPCState
        {
            Attack,
            Defense,
            Flee
        }

        public override void SetDefaults()
        {
            banner = npc.type;
            bannerItem = mod.ItemType("AngryofCorruptionBanner");
            npc.width = 48;
            npc.height = 40;
            npc.npcSlots = 0.5f;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;
            //aiType = 250;
            animationType = 250;
            npc.aiStyle = -1;
            npc.scale = 1;
            npc.noGravity = true;

            npc.lifeMax = !Main.hardMode ? 350 : !NPC.downedGolemBoss ? 750 : !NPC.downedMoonlord ? 1250 : 1800;
            npc.defense = !Main.hardMode ? 16 : !NPC.downedGolemBoss ? 32 : !NPC.downedMoonlord ? 48 : 64;
            npc.damage = !Main.hardMode ? 25 : !NPC.downedGolemBoss ? 100 : !NPC.downedMoonlord ? 135 : 200;
            npc.knockBackResist = !NPC.downedMoonlord ? 0.1f : 0f;

            var num = !Main.hardMode ? 33 : !NPC.downedGolemBoss ? 73 : !NPC.downedMoonlord ? 50 : 27;
            var num2 = !NPC.downedGolemBoss ? 0 : !NPC.downedMoonlord ? 2 : 14;
            npc.value = Item.buyPrice(0, num2, num, 0);
        }

        /*public override void NPCLoot()
        {
            if (Main.rand.NextFloat() < .667f)
            {
            Item.NewItem(npc.getRect(), null, CorruptionNimbus_Rod);
            }
        }*/
        public override void HitEffect(int hitDirection, double damage)
        {
            int num5;
            if (npc.life > 0)
            {
                int num333 = 0;
                while ((double)num333 < damage / (double)npc.lifeMax * 100.0)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 14, 0f, 0f, npc.alpha, npc.color, 1f);
                    num5 = num333;
                    num333 = num5 + 1;
                }
            }
            else
            {
                for (int num334 = 0; num334 < 120; num334 = num5 + 1)
                {
                    int num335 = Dust.NewDust(npc.position, npc.width, npc.height, MyDustId.CorruptionParticle, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f, 0, default(Color), 2f);
                    Dust dust = Main.dust[num335];
                    dust.velocity *= 1.8f;
                    num5 = num334;
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.CountNPCS(mod.NPCType("AngryofCorruption")) < 1 && !Main.dayTime && spawnInfo.player.ZoneCorrupt && NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
            {
                if (spawnInfo.player.ZoneOverworldHeight) return Main.raining ? 0.07f : 0.03f;
                else return Main.raining ? 0.01f : 0.007f;
            }
            return 0f;
        }
        public override void NPCLoot()
        {

        }
        public int eyetimer;
        public override void AI()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.buffImmune[20] = false;
            npc.buffImmune[23] = false;
            npc.buffImmune[24] = false;
            npc.buffImmune[30] = false;
            npc.buffImmune[31] = false;
            npc.buffImmune[32] = false;
            npc.buffImmune[39] = false;
            npc.buffImmune[46] = false;
            npc.buffImmune[47] = false;
            npc.buffImmune[67] = false;
            npc.buffImmune[68] = false;
            npc.buffImmune[69] = false;
            npc.buffImmune[70] = false;
            npc.buffImmune[156] = false;
            npc.buffImmune[164] = false;
            npc.buffImmune[169] = false;
            /*if (!Main.hardMode)
            {*/
                npc.defense = !Main.hardMode ? 16 : !NPC.downedGolemBoss ? 32 : !NPC.downedMoonlord ? 48 : 64;
                npc.knockBackResist = !NPC.downedMoonlord ? 0.1f : 0f;
            /*}
            else if (!NPC.downedGolemBoss)
            {
                npc.defense = 32;
                npc.knockBackResist = 0.1f;
            }
            else if (!NPC.downedMoonlord)
            {
                npc.defense = 48;
                npc.knockBackResist = 0.1f;
            }
            else
            {
                npc.defense = 64;
                npc.knockBackResist = 0f;
            }*/

            if (NPC.downedGolemBoss)
            {
                eyetimer++;
                if (eyetimer % 240 == 0)
                {
                    NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 50, mod.NPCType("CorruptionEye"));
                }
            }

            var num723 = 4f;
            var num724 = 0.25f;
            var vector88 = new Vector2(npc.Center.X, npc.Center.Y);
            var num725 = Main.player[npc.target].Center.X - vector88.X;
            var num726 = Main.player[npc.target].Center.Y - vector88.Y - 200f;
            var num727 = (float)Math.Sqrt((double)(num725 * num725 + num726 * num726));

            if (Main.hardMode && NPC.downedGolemBoss)
            {
                if (npc.life < npc.lifeMax * 0.2f)
                {
                    Timer = 0;
                    SwitchState((int)NPCState.Flee);
                }
            }
            switch ((NPCState)State)
            {
                // 攻击状态下
                case NPCState.Attack:
                    {
                        npc.TargetClosest(true);
                        if (num727 < 20f)
                        {
                            num725 = npc.velocity.X;
                            num726 = npc.velocity.Y;
                        }
                        else
                        {
                            num727 = num723 / num727;
                            num725 *= num727;
                            num726 *= num727;
                        }

                        if (npc.velocity.X < num725)
                        {
                            npc.velocity.X += num724 * 8f;
                            if (npc.velocity.X < 0f && num725 > 0f)
                            {
                                npc.velocity.X += num724 * 6f;
                            }
                        }
                        else if (npc.velocity.X > num725)
                        {
                            npc.velocity.X -= num724 * 8f;
                            if (npc.velocity.X > 0f && num725 < 0f)
                            {
                                npc.velocity.X -= num724 * 3f;
                            }
                        }
                        if (npc.velocity.Y < num726)
                        {
                            npc.velocity.Y += num724;
                            if (npc.velocity.Y < 0f && num726 > 0f)
                            {
                                npc.velocity.Y += num724 * 3f;
                            }
                        }
                        else if (npc.velocity.Y > num726)
                        {
                            npc.velocity.Y -= num724;
                            if (npc.velocity.Y > 0f && num726 < 0f)
                            {
                                npc.velocity.Y -= num724 * 3f;
                            }
                        }

                        if (npc.position.X + (float)npc.width > Main.player[npc.target].position.X &&
                            npc.position.X < Main.player[npc.target].position.X + (float)Main.player[npc.target].width &&
                            npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y &&
                            Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position,
                            Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != 1)
                        {
                            npc.ai[2] += 1f;
                            if (npc.ai[2] > 8f)
                            {
                                npc.ai[2] = 0f;
                                var num728 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
                                var num729 = (int)(npc.position.Y + (float)npc.height + 4f);
                                Projectile.NewProjectile(((int)npc.Center.X - 3), ((int)npc.Center.Y + 16), 0f, 6f, mod.ProjectileType("CorruptionRain"), 20, 1f, 0, 0f, 0f);
                                Projectile.NewProjectile(((int)npc.Center.X + 3), ((int)npc.Center.Y + 16), 0f, 6f, mod.ProjectileType("CorruptionRain"), 20, 1f, 0, 0f, 0f);
                            }
                        }
                        if (Main.hardMode)
                        {
                            if (npc.life < npc.lifeMax / 1.25)
                            {
                                Timer++;
                                if (Timer % (Main.rand.Next(300, 600)) == 0 || Timer >= 720)
                                {
                                    Timer = 0;
                                    SwitchState((int)NPCState.Defense);
                                }
                            }
                        }
                        break;
                    }
                // 防御状态下
                case NPCState.Defense:
                    {
                        if (!Main.hardMode)
                        {
                            Timer = 0;
                            SwitchState((int)NPCState.Attack);
                        }
                        else
                        {
                            Timer++;
                            if (Timer % 150 == 0)
                            {
                                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 50, mod.NPCType("AngryofCorruptionMinion"));
                            }
                            if (Timer % (Main.rand.Next(300, 600)) == 0 || Timer >= 720)
                            {
                                Timer = 0;
                                SwitchState((int)NPCState.Attack);
                            }
                            npc.velocity.Y = 0f;
                            npc.velocity.X = 0f;
                            npc.defense = 9999;
                            npc.knockBackResist = 0f;
                            for (int i = 0; i < npc.buffImmune.Length; i++)
                            {
                                npc.buffImmune[i] = true;
                            }
                        }
                        break;
                    }
                // 逃逸状态下
                case NPCState.Flee:
                    {
                        if (!Main.hardMode || !NPC.downedGolemBoss)
                        {
                            Timer = 0;
                            SwitchState((int)NPCState.Attack);
                        }
                        else
                        {
                            npc.aiStyle = 0;
                            npc.velocity.X *= 0.3f;
                            npc.velocity.Y *= 0.3f;

                            npc.alpha += 2;
                            if (npc.alpha >= 255)
                            {
                                npc.localAI[0]++;
                                npc.dontTakeDamage = true;
                                if (npc.localAI[0] >= 45) npc.active = false;
                            }
                        }
                        break;
                    }
                // 其他状态
                default:
                    //深陷地心2333
                    npc.noGravity = false;
                    npc.noTileCollide = true;
                    break;
            }
        }
    }
}