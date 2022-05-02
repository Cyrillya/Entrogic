using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Ganoderma : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Mushroom.Ganoderma>(), 5400);
            return true;
        }
    }
}
