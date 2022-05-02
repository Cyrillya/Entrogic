using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Mushroom
{
    /// <summary>
    /// 创建时间：2019/8/12 11:58:04
    /// </summary>
    public class Voidshroom : MushroomBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.immune = true;
            player.immuneTime = 2;
            for (int m = 0; m < player.hurtCooldowns.Length; m++)
            {
                player.hurtCooldowns[m] = player.immuneTime;
            }
            if (player.whoAmI == Main.myPlayer)
            {
                NetMessage.SendData(MessageID.Dodge, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
            }
        }
    }
}
