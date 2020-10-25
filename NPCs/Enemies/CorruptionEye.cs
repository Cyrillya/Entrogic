using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class CorruptionEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 350;
            npc.damage = 60;
            npc.defense = 18;
            npc.knockBackResist = 0.1f;
            npc.width = 16;
            npc.height = 16;
            npc.scale *= 1.25f;
            npc.npcSlots = 0.5f;
            npc.value = 0;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath33;
            npc.dontTakeDamage = true;
            npc.alpha = 255;

            npc.noGravity = true;
            npc.noTileCollide = true;
            if (NPC.downedMoonlord)
            {
                npc.lifeMax = 600;
                npc.damage = 100;
                npc.defense = 30;
                npc.knockBackResist = 0f;
            }
        }
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
                for (int num334 = 0; num334 < 30; num334 = num5 + 1)
                {
                    int num335 = Dust.NewDust(npc.position, npc.width, npc.height, MyDustId.CorruptionParticle, Main.rand.Next(-15, 16) * 0.2f, Main.rand.Next(-15, 16) * 0.2f, 0, default(Color), 1.3f);
                    Dust dust = Main.dust[num335];
                    dust.velocity *= 1.2f;
                    num5 = num334;
                }
            }
        }
        public override void AI()
        {
            npc.alpha -= 2;
            if (npc.alpha <= 0)
            {
                npc.alpha = 0;
                npc.dontTakeDamage = false;
            }

            npc.TargetClosest(true);
            Vector2 vector102 = new Vector2(npc.Center.X, npc.Center.Y);
            float num791 = Main.player[npc.target].Center.X - vector102.X;
            float num792 = Main.player[npc.target].Center.Y - vector102.Y;
            float num793 = (float)Math.Sqrt((double)(num791 * num791 + num792 * num792));
            float num794 = 12f;
            num793 = num794 / num793;
            num791 *= num793;
            num792 *= num793;
            npc.rotation = (float)Math.Atan2((double)num792, (double)num791) - 1.57f;
            npc.velocity.X = (npc.velocity.X * 140f + num791) / 140f;
            npc.velocity.Y = (npc.velocity.Y * 140f + num792) / 140f;
            if (NPC.CountNPCS(NPCType<AngryofCorruption>()) < 1)
            {
                npc.life = 0;
                int num5;
                for (int num334 = 0; num334 < 30; num334 = num5 + 1)
                {
                    int num335 = Dust.NewDust(npc.position, npc.width, npc.height, MyDustId.CorruptionParticle, Main.rand.Next(-15, 16) * 0.2f, Main.rand.Next(-15, 16) * 0.2f, 0, default(Color), 1.3f);
                    Dust dust = Main.dust[num335];
                    dust.velocity *= 2f;
                    num5 = num334;
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]);
            SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY + 2f), new Rectangle?(npc.frame), Color.White, npc.rotation, Utils.Size(npc.frame) / 2f, npc.scale, effects, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2((float)(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]).Width / 2), (float)(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]).Height / Main.npcFrameCount[npc.type] / 2));
            for (int i = 1; i < npc.oldPos.Length; i++)
            {
                Color color = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
                Color color2 = color;
                color2 = Color.Lerp(color2, Color.White, 0.5f);
                color2 = npc.GetAlpha(color2);
                color2 *= (float)(npc.oldPos.Length - i) / 15f;
                Main.spriteBatch.Draw(((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]), new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width / 2) - (float)((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]).Width * npc.scale / 2f + origin.X * npc.scale, npc.position.Y - Main.screenPosition.Y + (float)npc.height - (float)((Texture2D)Terraria.GameContent.TextureAssets.Npc[npc.type]).Height * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + origin.Y * npc.scale) - npc.velocity * (float)i * 0.5f, new Rectangle?(npc.frame), color2, npc.rotation, origin, npc.scale, effects, 0f);
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 9.0)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }
    }
}