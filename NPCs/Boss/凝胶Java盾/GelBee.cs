using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Entrogic.NPCs.Boss.凝胶Java盾.Projectiles;
using Entrogic.Common;

namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    public class GelBee : ModNPC
    {
        public Color gelcolor = new Color(0, 70, 118, 137);
        int cloneNPC = NPCID.Hornet;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[cloneNPC];
        }
        public override void SetDefaults()
        {
            aiType = -1;
            animationType = -1;
            npc.aiStyle = -1;
            npc.value = 10;
            npc.knockBackResist = 0f;
            npc.lifeMax = 105;
            npc.damage = 24;
            if (CrossModHandler.ModLoaded("CalamityMod"))
            {
                if (CrossModHandler.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax *= 2;
                    npc.damage *= 2;
                }
                else if (CrossModHandler.IsCalamityModDeathMode)
                {
                    npc.lifeMax *= 3;
                    npc.damage *= 3;
                }
            }
            npc.defense = 18;
            npc.width = 34;
            npc.height = 32;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[20] = true;
            npc.buffImmune[31] = false;
            npc.dontCountMe = true;
            npc.noTileCollide = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), (int)(Main.rand.Next(90, 151) * Main.GameModeInfo.DebuffTimeMultiplier));
        }
        public override void AI()
        {
            npc.dontTakeDamage = false;
            npc.reflectsProjectiles = false;
            if (npc.ai[3] < 240)
            {
                npc.dontTakeDamage = true;
                npc.ReflectProjectiles(npc.Hitbox);
                npc.reflectsProjectiles = true;
                npc.alpha = 255;
                for (int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position - npc.Size, npc.width * 2, npc.height * 2, MyDustId.GreyPebble, 0, 0, 0, gelcolor, 1.5f);
                    d.noGravity = true;
                }
            }
            else
            {
                npc.knockBackResist = 0.5f;
                animationType = cloneNPC;
                npc.alpha = 30;
                if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
                {
                    npc.TargetClosest(true);
                }
                float num = 6f;
                float num2 = 0.05f;
                num = 3.5f;
                num2 = 0.021f;
                num *= 1f - npc.scale;
                num2 *= 1f - npc.scale;
                Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num4 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
                float num5 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
                num4 = (float)((int)(num4 / 8f) * 8);
                num5 = (float)((int)(num5 / 8f) * 8);
                vector.X = (float)((int)(vector.X / 8f) * 8);
                vector.Y = (float)((int)(vector.Y / 8f) * 8);
                num4 -= vector.X;
                num5 -= vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                float num7 = num6;
                if (num6 == 0f)
                {
                    num4 = npc.velocity.X;
                    num5 = npc.velocity.Y;
                }
                else
                {
                    num6 = num / num6;
                    num4 *= num6;
                    num5 *= num6;
                }
                if (num7 > 100f)
                {
                    npc.ai[0] += 1f;
                    if (npc.ai[0] > 0f)
                    {
                        npc.velocity.Y = npc.velocity.Y + 0.023f;
                    }
                    else
                    {
                        npc.velocity.Y = npc.velocity.Y - 0.023f;
                    }
                    if (npc.ai[0] < -100f || npc.ai[0] > 100f)
                    {
                        npc.velocity.X = npc.velocity.X + 0.023f;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X - 0.023f;
                    }
                    if (npc.ai[0] > 200f)
                    {
                        npc.ai[0] = -200f;
                    }
                }
                if (Main.player[npc.target].dead)
                {
                    num4 = (float)npc.direction * num / 2f;
                    num5 = -num / 2f;
                }
                if (npc.velocity.X < num4)
                {
                    npc.velocity.X = npc.velocity.X + num2;
                }
                else if (npc.velocity.X > num4)
                {
                    npc.velocity.X = npc.velocity.X - num2;
                }
                if (npc.velocity.Y < num5)
                {
                    npc.velocity.Y = npc.velocity.Y + num2;
                }
                else if (npc.velocity.Y > num5)
                {
                    npc.velocity.Y = npc.velocity.Y - num2;
                }
                if (npc.velocity.X > 0f)
                {
                    npc.spriteDirection = 1;
                }
                if (npc.velocity.X < 0f)
                {
                    npc.spriteDirection = -1;
                }
                npc.rotation = npc.velocity.X * 0.1f;
                float num12 = 0.7f;
                if (npc.type == NPCID.EaterofSouls || npc.type == NPCID.Crimera)
                {
                    num12 = 0.4f;
                }
                if (npc.collideX)
                {
                    npc.netUpdate = true;
                    npc.velocity.X = npc.oldVelocity.X * -num12;
                    if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                    {
                        npc.velocity.X = 2f;
                    }
                    if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                    {
                        npc.velocity.X = -2f;
                    }
                }
                if (npc.collideY)
                {
                    npc.netUpdate = true;
                    npc.velocity.Y = npc.oldVelocity.Y * -num12;
                    if (npc.velocity.Y > 0f && (double)npc.velocity.Y < 1.5)
                    {
                        npc.velocity.Y = 2f;
                    }
                    if (npc.velocity.Y < 0f && (double)npc.velocity.Y > -1.5)
                    {
                        npc.velocity.Y = -2f;
                    }
                }
                else if (Main.rand.Next(40) == 0)
                {
                    int num16 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), MyDustId.GreyPebble, npc.velocity.X, 2f, 0, gelcolor, 1f);
                    Dust dust5 = Main.dust[num16];
                    dust5.velocity.X = dust5.velocity.X * 0.5f;
                    Dust dust6 = Main.dust[num16];
                    dust6.velocity.Y = dust6.velocity.Y * 0.1f;
                }
                if (npc.wet)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y = npc.velocity.Y * 0.95f;
                    }
                    npc.velocity.Y = npc.velocity.Y - 0.5f;
                    if (npc.velocity.Y < -4f)
                    {
                        npc.velocity.Y = -4f;
                    }
                    npc.TargetClosest(true);
                }
                if (npc.ai[1] == 101f)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item17, npc.position);
                    npc.ai[1] = 0f;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * npc.scale;
                    if (npc.type == NPCID.MossHornet)
                    {
                        npc.ai[1] += (float)Main.rand.Next(5, 20) * 0.1f * npc.scale;
                    }
                    if (Main.player[npc.target].stealth == 0f && Main.player[npc.target].itemAnimation == 0)
                    {
                        npc.ai[1] = 0f;
                    }
                    if (npc.ai[1] >= 150f)
                    {
                        float num17 = 8f;
                        Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)(npc.height / 2));
                        float num18 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2.X + (float)Main.rand.Next(-20, 21);
                        float num19 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector2.Y + (float)Main.rand.Next(-20, 21);
                        if ((num18 < 0f && npc.velocity.X < 0f) || (num18 > 0f && npc.velocity.X > 0f))
                        {
                            float num20 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
                            num20 = num17 / num20;
                            num18 *= num20;
                            num19 *= num20;
                            int num21 = (int)(10f * npc.scale);
                            if (npc.type == NPCID.MossHornet)
                            {
                                num21 = (int)(30f * npc.scale);
                            }
                            int num22 = ProjectileType<GelSting>();
                            int num23 = Projectile.NewProjectile(vector2.X, vector2.Y, num18, num19, num22, num21, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num23].timeLeft = 300;
                            npc.ai[1] = 101f;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            npc.ai[1] = 0f;
                        }
                    }
                }
                if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                {
                    npc.netUpdate = true;
                }
            }
            npc.ai[3]++;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            for (int i = 0; i < 200; i++)
            {
                NPC master = Main.npc[i];
                if (master.active && npc.ai[3] < 240 && master.type == NPCType<Embryo>())
                {
                    Vector2 vector = new Vector2(npc.Center.X, npc.Center.Y);
                    float num = master.Center.X - vector.X;
                    float num2 = master.Center.Y - vector.Y;
                    float rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
                    bool flag = true;
                    while (flag)
                    {
                        int num3 = 20;
                        int num4 = 52;
                        float num5 = (float)Math.Sqrt((double)(num * num + num2 * num2));
                        if (num5 < (float)num4)
                        {
                            num3 = (int)num5 - num4 + num3;
                            flag = false;
                        }
                        num5 = (float)num3 / num5;
                        num *= num5;
                        num2 *= num5;
                        vector.X += num;
                        vector.Y += num2;
                        num = master.Center.X - vector.X;
                        num2 = master.Center.Y - vector.Y;
                        Color color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f));
                        Main.spriteBatch.Draw((Texture2D)GetTexture("NPCs/Boss/凝胶Java盾/GelChain"),
                            new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y),
                            new Rectangle?(new Rectangle(0, 0, GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width(), num3)),
                            color,
                            rotation,
                            new Vector2((float)GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width() * 0.5f,
                            (float)GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return !(npc.ai[3] < 240);
        }
    }
}
