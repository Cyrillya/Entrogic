using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Voidshroom : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Mushroom.Voidshroom>(), 480);
            return true;
        }
    }
}
