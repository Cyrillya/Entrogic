using System;
using Entrogic.Content.NPCs.BaseTypes;
using Entrogic.Content.Projectiles.ContyElemental;
using Entrogic.Content.Projectiles.ContyElemental.Hostile;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.NPCs.Enemies.ContyElemental
{
    public class Migrant : NPCBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Migrant");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "护卫");

            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { //Influences how the NPC looks in the Bestiary
                Velocity = 1f,
                Scale = 0.8f
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

        public override void FindFrame(int frameHeight) {
            NPC.frameCounter += 0.15000000596046448;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int num = (int)NPC.frameCounter;
            NPC.frame.Y = num * frameHeight;
        }
        public override void SetDefaults() {
            NPC.aiStyle = -1;
            NPC.width = 56;
            NPC.height = 56;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.lifeMax = 350;
            for (int i = 0; i < NPC.buffImmune.Length; i++)
                NPC.buffImmune[i] = true;
            NPC.npcSlots = 1f;
            NPC.defense = 10;
            NPC.friendly = false;
        }
        NPC master = null;
        public override void AI() {
            if (NPC.CountNPCS(ModContent.NPCType<ContaminatedElemental>()) < 1) {
                NPC.active = false;
                NPC.life = -1;
                NPC.HitEffect();
                NPC.netUpdate = true;
                return;
            }
            NPC.defense = 10;
            NPC.ai[0]++;
            master = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<ContaminatedElemental>())];
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.velocity = (player.Center - NPC.Center).ToRotation().ToRotationVector2() * 2f;
            if (NPC.ai[0] >= 820f) {
                NPC.velocity = (master.Center - NPC.Center).ToRotation().ToRotationVector2() * 12.5f;
                NPC.rotation = NPC.velocity.ToRotation() - MathHelper.ToRadians(90f);
                if (Math.Abs(master.Center.X - NPC.Center.X) <= 52f && Math.Abs(master.Center.Y - NPC.Center.Y) <= 38f) {
                    master.life += 100 + NPC.life;
                    if (master.life > master.lifeMax)
                        master.life = master.lifeMax;
                    ((ContaminatedElemental)master.ModNPC).Water += 50;
                    NPC.active = false;
                }
                return;
            }
            if (NPC.ai[0] % 115 == 0 && master.ai[0] != 4f && Main.netMode != NetmodeID.MultiplayerClient) {
                //for (int i = -1; i <= 1; i++)
                //{
                Vector2 velo = (player.Center + player.velocity * 120 - NPC.Center).ToRotation().ToRotationVector2() * 10f + (/*i*/0 * 30f / 360f * MathHelper.TwoPi).ToRotationVector2();
                Projectile shots = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velo, ModContent.ProjectileType<ContaSpike>(), (int)(23 * NPC.scale), 2f)];
                shots.scale = 1f + (NPC.scale - 1f) * 0.87f;
                NPC.netUpdate = true;
                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shots.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
                //}
            }
            if (master.ai[0] == 4f) {
                NPC.velocity = (player.Center - NPC.Center).ToRotation().ToRotationVector2() * 6.65f;
                NPC.defense = 20;
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            Player player = Main.player[NPC.target];
            if (NPC.life <= 0) {
                const float StartY = -15f;
                const float fallSpeed = 0.32f;
                float min = -7.6f, max = 7.6f;
                float mid = (min + max) * 0.5f;
                while (Math.Abs(max - min) > 0.005f) {
                    Vector2 vShark = NPC.Center;
                    mid = (min + max) * 0.5f;
                    float maxHeight = 0f;
                    float beyond = 0f;
                    float veloY = StartY;
                    for (int i = 0; i < 1000; i++) {
                        vShark.X += mid;
                        vShark.Y += veloY;
                        veloY += fallSpeed;
                        maxHeight = Math.Min(vShark.Y, maxHeight);
                        if (veloY > 1f && vShark.Y > player.Center.Y) {
                            beyond = vShark.X - player.Center.X;
                            break;
                        }
                    }
                    if (beyond > 0f) {
                        max = mid;
                    }
                    else {
                        min = mid;
                    }
                }
                int shark = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolluShark>(), 0, 0, 0, -15, mid);
                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark, -1f, 0f, 0f, 0, 0, 0);
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item Item, ref int damage, ref float knockback, ref bool crit) {
            if (master != null && master.ai[0] == 4f)
                damage = (int)(damage * 0.40f);
        }
        public override void ModifyHitByProjectile(Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (master != null && master.ai[0] == 4f)
                damage = (int)(damage * 0.25f);
        }
    }
}