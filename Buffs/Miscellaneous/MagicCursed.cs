using System;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Buffs.Miscellaneous
{
    public class MagicCursed : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (!ePlayer.IsTwoCards_CryptTreasure)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
