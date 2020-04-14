using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Buffs.Weapons;

namespace Entrogic.Buffs.Miscellaneous
{
    public class Antibody : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<EntrogicPlayer>().HasAntibody = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<EntrogicNPC>().antibody = true;
        }
    }
}
