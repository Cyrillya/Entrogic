using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Buffs.Miscellaneous;

namespace Entrogic.Buffs.Weapons
{
    public class Strong : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            EntrogicPlayer.ModPlayer(player).arcaneDamageAdd += 0.6f;
        }
    }
}
