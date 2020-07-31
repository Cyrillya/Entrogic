using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Buffs.Mushroom
{
    public class Flammulina : MushroomBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeSpeed += 0.2f;
            player.moveSpeed += 0.06f;
        }
    }
}
