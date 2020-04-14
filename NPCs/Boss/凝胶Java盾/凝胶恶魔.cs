using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Entrogic.NPCs.Boss.凝胶Java盾
{
    public class 凝胶恶魔 : ModNPC
    {
        public Color gelcolor = new Color(0, 70, 118, 137);
        int cloneNPC = NPCID.Demon;
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
            if (Entrogic.IsCalamityLoaded)
            {
                if (Entrogic.IsCalamityModRevengenceMode)
                {
                    npc.lifeMax *= 2;
                    npc.damage *= 2;
                }
                else if (Entrogic.IsCalamityModDeathMode)
                {
                    npc.lifeMax *= 3;
                    npc.damage *= 3;
                }
            }
            npc.defense = 18;
            npc.width = 28;
            npc.height = 48;
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
                target.AddBuff(BuffType<Buffs.Enemies.溶解>(), Main.rand.Next(90, 151) * (Main.expertMode ? (int)Main.expertDebuffTime : 1));
        }
        public override void AI()
        {
            npc.dontTakeDamage = false;
            npc.reflectingProjectiles = false;
            if (npc.ai[3] < 240)
            {
                npc.dontTakeDamage = true;
                npc.ReflectProjectiles(npc.Hitbox);
                npc.reflectingProjectiles = true;
                npc.alpha = 255;
                for (int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position - npc.Size, npc.width * 2, npc.height * 2, MyDustId.GreyPebble, 0, 0, 0, gelcolor, 1.5f);
                    d.noGravity = true;
                }
            }
            else
            {
                npc.knockBackResist = 0.8f;
                npc.aiStyle = 14;
                animationType = cloneNPC;
                npc.alpha = 30;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[0] += 1f;
                    if (npc.ai[0] == 60f || npc.ai[0] == 80f)
                    {
                        float num217 = 0.2f;
                        Vector2 vector28 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num218 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector28.X + (float)Main.rand.Next(-100, 101);
                        float num219 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector28.Y + (float)Main.rand.Next(-100, 101);
                        float num220 = (float)Math.Sqrt((double)(num218 * num218 + num219 * num219));
                        num220 = num217 / num220;
                        num218 *= num220;
                        num219 *= num220;
                        int num221 = 13;
                        int type = mod.ProjectileType("凝胶镰刀");
                        int num223 = Projectile.NewProjectile(vector28.X, vector28.Y, num218, num219, type, num221, 0f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num223].timeLeft = 300;
                    }
                    else if (npc.ai[0] >= (float)(300 + Main.rand.Next(300)))
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
                if (master.active && npc.ai[3] < 240 && master.type == mod.NPCType("胚胎"))
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
                        Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain"), new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, base.mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width, num3)), color, rotation, new Vector2((float)base.mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Width * 0.5f, (float)base.mod.GetTexture("NPCs/Boss/凝胶Java盾/GelChain").Height * 0.5f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return !(npc.ai[3] < 240);
        }
    }
}
