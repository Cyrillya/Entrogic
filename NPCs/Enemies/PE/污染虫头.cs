using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies.PE
{
    #region 大
    public class 污染虫头 : ModNPC
    {
        private bool TailSpawned;
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 30;
            npc.defense = 22;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 1f;
            npc.value = (float)Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            npc.npcSlots = 0.1f;
            aiType = -1;
            animationType = 10;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player p = Main.player[Main.myPlayer];
            int spawnTileX = spawnInfo.spawnTileX;
            int spawnTileY = spawnInfo.spawnTileY;
            ushort type = Main.tile[spawnTileX, spawnTileY].type;

            return EntrogicWorld.IsDownedPollutionElemental && spawnInfo.player.ZoneBeach && spawnInfo.water ? 0.08f : 0f;
        }
        public override void AI()
        {
            if (!TailSpawned)
            {
                int s = Main.rand.Next(17, 21);
                int num = npc.whoAmI;
                for (int i = 0; i < s; i++)
                {
                    int num2;
                    if (i >= 0 && i < s - 1)
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫身>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    else
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫尾>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    Main.npc[num2].realLife = npc.whoAmI;
                    Main.npc[num2].ai[3] = npc.ai[3];
                    Main.npc[num2].ai[2] = (float)npc.whoAmI;
                    Main.npc[num2].ai[1] = (float)num;
                    Main.npc[num].ai[0] = (float)num2;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
                    num = num2;
                }
                TailSpawned = true;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life<=0)
            {
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/污染虫尸体"), 1.4f);
            }
        }
    }
#endregion
    #region 中
    public class 污染虫头2 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫头"; } }
        private bool TailSpawned;
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 30;
            npc.defense = 22;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.8f;
            npc.value = (float)Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            npc.npcSlots = 0.1f;
            aiType = -1;
            animationType = 10;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player p = Main.player[Main.myPlayer];
            int spawnTileX = spawnInfo.spawnTileX;
            int spawnTileY = spawnInfo.spawnTileY;
            ushort type = Main.tile[spawnTileX, spawnTileY].type;

            return EntrogicWorld.IsDownedPollutionElemental && spawnInfo.player.ZoneBeach && spawnInfo.water ? 0.08f : 0f;
        }
        public override void AI()
        {
            if (!TailSpawned)
            {
                int s = Main.rand.Next(9, 13);
                int num = npc.whoAmI;
                for (int i = 0; i < s; i++)
                {
                    int num2;
                    if (i >= 0 && i < s - 1)
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫身2>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    else
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫尾2>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    Main.npc[num2].realLife = npc.whoAmI;
                    Main.npc[num2].ai[3] = npc.ai[3];
                    Main.npc[num2].ai[2] = (float)npc.whoAmI;
                    Main.npc[num2].ai[1] = (float)num;
                    Main.npc[num].ai[0] = (float)num2;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
                    num = num2;
                }
                TailSpawned = true;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/污染虫尸体"), 1.2f);
            }
        }
    }
#endregion
    #region 小
    public class 污染虫头3 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫头"; } }
        private bool TailSpawned;
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 30;
            npc.defense = 22;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.6f;
            npc.value = (float)Item.buyPrice(0, 0, 6, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            npc.npcSlots = 0.1f;
            aiType = -1;
            animationType = 10;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.behindTiles = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player p = Main.player[Main.myPlayer];
            int spawnTileX = spawnInfo.spawnTileX;
            int spawnTileY = spawnInfo.spawnTileY;
            ushort type = Main.tile[spawnTileX, spawnTileY].type;

            return EntrogicWorld.IsDownedPollutionElemental && spawnInfo.player.ZoneBeach && spawnInfo.water ? 0.08f : 0f;
        }
        public override void AI()
        {
            if (!TailSpawned)
            {
                int s = Main.rand.Next(4, 7);
                int num = npc.whoAmI;
                for (int i = 0; i < s; i++)
                {
                    int num2;
                    if (i >= 0 && i < s - 1)
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫身3>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    else
                    {
                        num2 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, NPCType<污染虫尾3>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                    }
                    Main.npc[num2].realLife = npc.whoAmI;
                    Main.npc[num2].ai[3] = npc.ai[3];
                    Main.npc[num2].ai[2] = (float)npc.whoAmI;
                    Main.npc[num2].ai[1] = (float)num;
                    Main.npc[num].ai[0] = (float)num2;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
                    num = num2;
                }
                TailSpawned = true;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/污染虫尸体"), 1f);
            }
        }
    }
    #endregion
}