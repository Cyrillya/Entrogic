using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.IO;

namespace Entrogic.NPCs.Enemies.PE
{
    internal class EroderWormHead : EroderWorm
    {
        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 68;
            npc.damage = 90;
            npc.defense = 13;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = (float)Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            npc.npcSlots = 2f;
            aiType = -1;
            animationType = 10;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override void Init()
        {
            base.Init();
            head = true;
        }
    }

    internal class EroderWormBody : EroderWorm
    {
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 68;
            npc.damage = 20;
            npc.defense = 24;
            npc.lifeMax = 165;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.dontCountMe = true;
        }
    }

    internal class EroderWormTail : EroderWorm
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            npc.width = 26;
            npc.height = 68;
            npc.damage = 26;
            npc.defense = 26;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 2, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 10;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
            npc.dontCountMe = true;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 3.0)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
            {
                npc.frame.Y = 0;
            }
        }

        public override void Init()
        {
            base.Init();
            tail = true;
        }
    }

    // I made this 2nd base class to limit code repetition.
    public abstract class EroderWorm : Worm
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eroder");
            DisplayName.AddTranslation(GameCulture.Chinese, "侵蚀者");
        }
        public override void Init()
        {
            minLength = 12;
            maxLength = 18;
            tailType = NPCType<EroderWormTail>();
            bodyType = NPCType<EroderWormBody>();
            headType = NPCType<EroderWormHead>();
            speed = 15.5f;
            turnSpeed = 0.125f;
            splitting = true;
        }
        protected int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void CustomBehavior()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--;
                }

                Player target = Main.player[npc.target];
                if (attackCounter <= 0 && Vector2.Distance(npc.Center, target.Center) < 1024 && Collision.CanHit(npc.Center, 1, 1, target.Center, 1, 1))
                {
                    Vector2 direction = ModHelper.GetFromToVector(target.Center, npc.Center);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(npc.Center, direction * 4f, ProjectileID.CursedFlameHostile, 15, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 600;
                    attackCounter = Main.rand.Next(200, 801);
                    npc.netUpdate = true;
                }
            }
        }
    }
}
