using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies.PE
{
    public class 污染虫尾 : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 15;
            npc.defense = 15;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 1f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 2, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 10;
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
                //Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体2"), 1.4f);
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
        }
    }
    public class 污染虫尾2 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫尾"; } }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 15;
            npc.defense = 15;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.8f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 2, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 10;
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
                //Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体2"), 1.2f);
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
        }
    }
    public class 污染虫尾3 : ModNPC
    {
        public override string Texture { get { return "Entrogic/NPCs/Enemies/PE/污染虫尾"; } }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 15;
            npc.defense = 15;
            npc.lifeMax = 290;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.scale = 0.6f;
            npc.npcSlots = 0.1f;
            npc.value = Item.buyPrice(0, 0, 2, 0);
            npc.knockBackResist = 0f;
            npc.aiStyle = 6;
            aiType = -1;
            animationType = 10;
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
                //Gore.NewGore(npc.Center, npc.velocity, Mod.GetGoreSlot("Gores/污染虫尸体2"), 1f);
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
        }
    }
}
