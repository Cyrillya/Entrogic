using System;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Buffs.Weapons
{
    public class Dragoul : ModBuff
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
            if (player.controlJump && player.wingTime > 0f)
            {
                player.jumpSpeedBoost += 16.4f;
                player.moveSpeed += 35f;
                player.maxRunSpeed += 34.2f;
                player.wingTimeMax += 480;
            }
        }
    }
}
