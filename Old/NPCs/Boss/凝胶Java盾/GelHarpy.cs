﻿using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Entrogic.NPCs.Boss.凝胶Java盾.Projectiles;
<<<<<<< HEAD:NPCs/Boss/凝胶Java盾/GelHarpy.cs
using Entrogic.Common;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:NPCs/Boss/凝胶Java盾/凝胶鸟妖.cs

namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    public class GelHarpy : ModNPC
    {
        public Color gelcolor = new Color(0, 70, 118, 137);
        int cloneNPC = NPCID.Harpy;
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
            npc.width = 24;
            npc.height = 34;
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
<<<<<<< HEAD:NPCs/Boss/凝胶Java盾/GelHarpy.cs
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), (int)(Main.rand.Next(90, 151) * Main.GameModeInfo.DebuffTimeMultiplier));
=======
                target.AddBuff(BuffType<Buffs.Enemies.Dissolve>(), Main.rand.Next(90, 151) * (Main.expertMode ? (int)Main.expertDebuffTime : 1));
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:NPCs/Boss/凝胶Java盾/凝胶鸟妖.cs
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
                npc.knockBackResist = 0.6f;
                npc.aiStyle = 14;
                animationType = cloneNPC;
                npc.alpha = 30;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0] += 1f;
                    if (npc.ai[0] == 45f || npc.ai[0] == 75)
                    {
                        float num210 = 6f;
                        Vector2 vector27 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num211 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-100, 101);
                        float num212 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-100, 101);
                        float num213 = (float)Math.Sqrt((double)(num211 * num211 + num212 * num212));
                        num213 = num210 / num213;
                        num211 *= num213;
                        num212 *= num213;
                        int num214 = 13;
                        int type = ProjectileType<GelFeather>();
                        int num216 = Projectile.NewProjectile(vector27.X, vector27.Y, num211, num212, type, num214, 0f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num216].timeLeft = 300;
                    }
                    else if (npc.ai[0] >= (float)(400 + Main.rand.Next(400)))
                    {
                        npc.ai[0] = 0f;
                    }
                }
            }
            npc.ai[3]++;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            for (int i = 0; i < 200; i++)
            {
                NPC master = Main.npc[i];
<<<<<<< HEAD:NPCs/Boss/凝胶Java盾/GelHarpy.cs
                if (master.active && npc.ai[3] < 240 && master.type == NPCType<Embryo>())
=======
                if (master.active && npc.ai[3] < 240 && master.type == NPCType<Volutio>())
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:NPCs/Boss/凝胶Java盾/凝胶鸟妖.cs
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
<<<<<<< HEAD:NPCs/Boss/凝胶Java盾/GelHarpy.cs
                        Main.spriteBatch.Draw((Texture2D)GetTexture("NPCs/Boss/凝胶Java盾/GelChain"), 
                            new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), 
                            new Rectangle?(new Rectangle(0, 0, GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width(), num3)), 
                            color, 
                            rotation, 
                            new Vector2((float)GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width() * 0.5f, 
                            (float)GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Height() * 0.5f), 1f, SpriteEffects.None, 0f);
=======
                        Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain"), new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width, num3)), color, rotation, new Vector2((float)mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width * 0.5f, (float)mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Height * 0.5f), 1f, SpriteEffects.None, 0f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:NPCs/Boss/凝胶Java盾/凝胶鸟妖.cs
                    }
                }
            }
            return !(npc.ai[3] < 240);
        }
    }
}
