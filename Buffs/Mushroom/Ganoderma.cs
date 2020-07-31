using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Mushroom
{
    public class Ganoderma : MushroomBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            int num = player.statLifeMax;
            player.statLifeMax2 += (int)((double)num * 0.3);
            player.lifeRegen += 9;
        }
    }
}
