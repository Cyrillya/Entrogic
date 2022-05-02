using Terraria;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Flammulina : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.Mushroom.Flammulina>(), 5400);
            return true;
        }
    }
}
