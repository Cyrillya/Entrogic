using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Boletus : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.Mushroom.Boletus>(), 7200);
            return true;
        }
    }
}
