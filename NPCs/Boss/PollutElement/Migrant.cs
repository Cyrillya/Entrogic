using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class Migrant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15000000596046448;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int num = (int)npc.frameCounter;
            npc.frame.Y = num * frameHeight;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 56;
            npc.height = 56;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lifeMax = 350;
            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
            npc.npcSlots = 1f;
            npc.defense = 10;
        }
        NPC master = null;
        public override void AI()
        {
            if (NPC.CountNPCS(NPCType<污染之灵>()) < 1)
            {
                npc.active = false;
                npc.life = -1;
                npc.HitEffect();
                npc.netUpdate = true;
                return;
            }
            npc.defense = 10;
            npc.ai[0]++;
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == NPCType<污染之灵>() && npc.active)
                {
                    master = npc;
                }
            }
            if (master == null)
            {
                npc.active = false;
                npc.life = -1;
                npc.HitEffect();
                npc.netUpdate = true;
                return;
            }
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * 2f;
            if (npc.ai[0] >= 820f)
            {
                npc.velocity = (master.Center - npc.Center).ToRotation().ToRotationVector2() * 12.5f;
                npc.rotation = npc.velocity.ToRotation() - MathHelper.ToRadians(90f);
                if (Math.Abs(master.Center.X - npc.Center.X) <= 52f && Math.Abs(master.Center.Y - npc.Center.Y) <= 38f)
                {
                    master.life += 100 + npc.life;
                    if (master.life > master.lifeMax)
                        master.life = master.lifeMax;
                    ((污染之灵)master.modNPC).water += 50;
                    npc.active = false;
                }
                return;
            }
            if (npc.ai[0] % 115 == 0 && master.ai[0] != 4f && Main.netMode != 1)
            {
                for (int i = -1; i <= 1; i++)
                {
                    Vector2 velo = ((player.Center + player.velocity * 120) - npc.Center).ToRotation().ToRotationVector2() * 10f + (i * 30f / 360f * MathHelper.TwoPi).ToRotationVector2();
                    Projectile shots = Main.projectile[Projectile.NewProjectile(npc.Center, velo, ProjectileType<污染精华>(), (int)(23 * npc.scale), 2f)];
                    shots.scale = 1f + (npc.scale - 1f) * 0.87f;
                    npc.netUpdate = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, shots.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (master.ai[0] == 4f)
            {
                npc.velocity = (player.Center - npc.Center).ToRotation().ToRotationVector2() * 6.65f;
                npc.defense = 20;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            Player player = Main.player[npc.target];
            if (npc.life <= 0)
            {
                const float StartY = -15f;
                const float fallSpeed = 0.32f;
                float min = -7.6f, max = 7.6f;
                float mid = (min + max) * 0.5f;
                while (Math.Abs(max - min) > 0.005f)
                {
                    Vector2 vShark = npc.Center;
                    mid = (min + max) * 0.5f;
                    float maxHeight = 0f;
                    float beyond = 0f;
                    float veloY = StartY;
                    for (int i = 0; i < 1000; i++)
                    {
                        vShark.X += mid;
                        vShark.Y += veloY;
                        veloY += fallSpeed;
                        maxHeight = Math.Min(vShark.Y, maxHeight);
                        if (veloY > 1f && vShark.Y > player.Center.Y)
                        {
                            beyond = vShark.X - player.Center.X;
                            break;
                        }
                    }
                    if (beyond > 0f)
                    {
                        max = mid;
                    }
                    else
                    {
                        min = mid;
                    }
                }
                int shark = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<PolluShark>(), 0, 0, 0, -15, mid);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, shark, -1f, 0f, 0f, 0, 0, 0);
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (master != null && master.ai[0] == 4f)
                damage = (int)(damage * 0.40f);
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (master != null && master.ai[0] == 4f)
                damage = (int)(damage * 0.25f);
        }
    }
}