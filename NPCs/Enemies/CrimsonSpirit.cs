using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class CrimsonSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            banner = npc.type;
            bannerItem = mod.ItemType("CrimsonSpiritBanner");
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 18;
            npc.height = 22;
            npc.npcSlots = 0.5f;
            npc.HitSound = SoundID.NPCHit36;
            npc.DeathSound = SoundID.NPCDeath39;
            if (!Main.hardMode)
            {
                npc.lifeMax = 100;
                npc.damage = 10;
                npc.defense = 5;
                npc.knockBackResist = 0.2f;
                npc.value = Item.buyPrice(0, 0, 50, 0);
            }
            else if (!NPC.downedPlantBoss && Main.hardMode)
            {
                npc.damage = 30;
                npc.defense = 15;
                npc.lifeMax = 500;
                npc.knockBackResist = 0.2f;
                npc.value = Item.buyPrice(0, 1, 50, 0);
            }
            else if (!NPC.downedMoonlord && NPC.downedPlantBoss && Main.hardMode)
            {
                npc.damage = 100;
                npc.defense = 35;
                npc.lifeMax = 750;
                npc.knockBackResist = 0.1f;
                npc.value = Item.buyPrice(0, 3, 0, 0);
            }
            else if (NPC.downedMoonlord && NPC.downedPlantBoss && Main.hardMode)
            {
                npc.damage = 135;
                npc.defense = 55;
                npc.lifeMax = 1050;
                npc.knockBackResist = 0f;
                npc.value = Item.buyPrice(0, 25, 75, 0);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 6.0)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 vec = npc.Center - player.Center;
            Vector2 finalVec = (vec.ToRotation() + MathHelper.Pi).ToRotationVector2() * 12;
            npc.velocity.X = (npc.velocity.X * 150f + finalVec.X) / 151f;
            npc.velocity.Y = (npc.velocity.Y * 150f + finalVec.Y) / 151f;
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
            var position67 = npc.position;
            var width52 = npc.width;
            var height49 = npc.height;

            int num795 = Dust.NewDust(npc.position, npc.width, npc.height, 5, 0f, 0f, 192, default(Color), 1f);
            Dust dust3 = Main.dust[num795];
            dust3.velocity *= 0.1f;
            Main.dust[num795].scale = 1.3f;
            Main.dust[num795].noGravity = true;
            if (Main.rand.Next(450) == 0)
            {
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 50, mod.NPCType("CrimsonSpiritMinion"));
            }
        }

        public override void NPCLoot()
        {
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3 && !Main.dayTime && spawnInfo.player.ZoneCrimson)
            {
                if (!spawnInfo.player.ZoneOverworldHeight) return 0.05f;
                else return 0.01f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 90);
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.Ichor, 150);
            }
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(BuffID.Ichor, 240);
            }
        }
    }
}