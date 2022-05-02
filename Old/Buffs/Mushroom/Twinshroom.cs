using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Mushroom
{
    public class Twinshroom : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost *= 0.5f;
        }
    }
}
