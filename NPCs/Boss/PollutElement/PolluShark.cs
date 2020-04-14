using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class PolluShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 80;
            npc.height = 24;
            npc.aiStyle = -1;
            npc.damage = 64;
            npc.defense = 5;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                Main.PlaySound(SoundID.NPCKilled, (int)npc.Center.X, (int)npc.Center.Y, 19);
                npc.ai[0]++;
                npc.velocity.Y = npc.ai[2];
                npc.velocity.X = npc.ai[3];
            }
            npc.rotation = npc.velocity.ToRotation();
            if (npc.velocity.X <= 0)
            {
                npc.rotation += MathHelper.Pi;
                npc.spriteDirection = -1;
            }
            else npc.spriteDirection = 1;
            npc.velocity.Y += 0.32f;
            if (Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 19);
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
                return;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                int num160 = 0;
                while ((double)num160 < damage / (double)npc.lifeMax * 100.0)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
                    num160++;
                }
            }
            else
            {
                for (int num161 = 0; num161 < 60; num161++)
                {
                    int num162 = Dust.NewDust(npc.Center - Vector2.One * 25f, 50, 50, 5, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                    Main.dust[num162].velocity /= 2f;
                }
                Gore.NewGore(npc.Center, npc.velocity * 0.8f, mod.GetGoreSlot("Gores/PolluSharkGore"), 1f);
                Gore.NewGore(npc.Center, npc.velocity * 0.9f, mod.GetGoreSlot("Gores/PolluSharkGore2"), 1f);
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/PolluSharkGore3"), 1f);
            }
        }
    }
}