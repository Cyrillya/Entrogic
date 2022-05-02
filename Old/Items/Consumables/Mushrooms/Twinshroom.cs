using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Twinshroom : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Mushroom.Twinshroom>(), 1800);
            return true;
        }
    }
}
