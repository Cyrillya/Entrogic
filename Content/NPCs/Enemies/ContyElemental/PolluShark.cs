using System;
using Entrogic.Content.NPCs.BaseTypes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.NPCs.Enemies.ContyElemental
{
    public class PolluShark : NPCBase
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            DisplayName.SetDefault("Contaminated Shark");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染鲨");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { //Influences how the NPC looks in the Bestiary
                Velocity = 1f,
                Position = new Vector2(40, 10),
                PortraitPositionXOverride = 10,
                PortraitPositionYOverride = 10
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[1] { //Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean
            });
            bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement( //Sets the description of this NPC that is listed in the bestiary.
                "一只污染之灵的仆从"
            ));
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 80;
            NPC.height = 24;
            NPC.aiStyle = -1;
            NPC.damage = 64;
            NPC.defense = 5;
            NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.friendly = false;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCKilled, (int)NPC.Center.X, (int)NPC.Center.Y, 19);
                NPC.ai[0]++;
                NPC.velocity.Y = NPC.ai[2];
                NPC.velocity.X = NPC.ai[3];
            }
            NPC.rotation = NPC.velocity.ToRotation();
            if (NPC.velocity.X <= 0)
            {
                NPC.rotation += MathHelper.Pi;
                NPC.spriteDirection = -1;
            }
            else NPC.spriteDirection = 1;
            NPC.velocity.Y += 0.32f;
            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCKilled, (int)NPC.position.X, (int)NPC.position.Y, 19);
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
                return;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                int num160 = 0;
                while ((double)num160 < damage / (double)NPC.lifeMax * 100.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)hitDirection, -1f, 0, default(Color), 1f);
                    num160++;
                }
            }
            else
            {
                for (int num161 = 0; num161 < 60; num161++)
                {
                    int num162 = Dust.NewDust(NPC.Center - Vector2.One * 25f, 50, 50, DustID.Blood, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                    Main.dust[num162].velocity /= 2f;
                }
                //Gore.NewGore(NPC.Center, NPC.velocity * 0.8f, Mod.GetGoreSlot("Gores/PolluSharkGore"), 1f);
                //Gore.NewGore(NPC.Center, NPC.velocity * 0.9f, Mod.GetGoreSlot("Gores/PolluSharkGore2"), 1f);
                //Gore.NewGore(NPC.Center, NPC.velocity, Mod.GetGoreSlot("Gores/PolluSharkGore3"), 1f);
            }
        }
    }
}