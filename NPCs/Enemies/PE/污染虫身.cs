using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using System;

namespace Entrogic.NPCs.Enemies.PE
{
    public class 污染虫身 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 20;
            npc.defense = 28;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 1f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 250;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }
        
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体3"), 1.4f);
            }
        }
        public override void AI()
        {
            if (!Main.npc[(int)npc.ai[1]].active)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.localAI[0] += Main.rand.Next(4);
                if (npc.localAI[0] >= Main.rand.Next(1, 500000))
                {
                    npc.localAI[0] = 0f;
                    npc.TargetClosest(true);
                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        Vector2 vec = ModHelper.GetFromToVector(npc.Center, Main.player[npc.target].Center) * 4f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vec.X, vec.Y, ProjectileType<污染之虫射弹>(), 18, 0f, Main.myPlayer, 0f, 0f);
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
    public class 污染虫身2 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫身"; } }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 20;
            npc.defense = 28;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.8f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 250;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体3"), 1.2f);
            }
        }
        public override void AI()
        {
            if (!Main.npc[(int)npc.ai[1]].active)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.localAI[0] += Main.rand.Next(4);
                if (npc.localAI[0] >= Main.rand.Next(1, 500000))
                {
                    npc.localAI[0] = 0f;
                    npc.TargetClosest(true);
                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        Vector2 vec = ModHelper.GetFromToVector(npc.Center, Main.player[npc.target].Center) * 4f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vec.X, vec.Y, ProjectileType<污染之虫射弹>(), 18, 0f, Main.myPlayer, 0f, 0f);
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
    public class 污染虫身3 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫身"; } }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 20;
            npc.defense = 28;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.6f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 250;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return new bool?(false);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体3"), 1f);
            }
        }
        public override void AI()
        {
            if (!Main.npc[(int)npc.ai[1]].active)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.localAI[0] += Main.rand.Next(4);
                if (npc.localAI[0] >= Main.rand.Next(1, 500000))
                {
                    npc.localAI[0] = 0f;
                    npc.TargetClosest(true);
                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        Vector2 vec = ModHelper.GetFromToVector(npc.Center, Main.player[npc.target].Center) * 4f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vec.X, vec.Y, ProjectileType<污染之虫射弹>(), 18, 0f, Main.myPlayer, 0f, 0f);
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
    public class 污染之虫射弹 : ModProjectile
    {
        public override string Texture => "Entrogic/Images/Block";
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.Size = new Vector2(12, 12);
            projectile.penetrate = 6;
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 450;
        }
        public override void AI()
        {
            int num2;
            for (int i = 0; i < 3; i = num2 + 1)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.White, projectile.velocity.X, projectile.velocity.Y, 50, new Color(56, 114, 80), 1.2f);
                Main.dust[num].noGravity = true;
                Dust dust = Main.dust[num];
                dust.velocity *= 0.3f;
                num2 = i;
            }
        }
    }
}