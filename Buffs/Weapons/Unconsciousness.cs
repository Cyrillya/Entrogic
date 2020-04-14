using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Weapons
{
    public class Unconsciousness : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.confused = true;
            if (player.lifeRegen > 0) player.lifeRegen = 0;
            player.lifeRegenTime = 0;
            player.lifeRegen -= 10;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.confused = true;
            if (npc.lifeRegen > 0) npc.lifeRegen = 0;
            npc.lifeRegen -= 12;
        }
    }
}