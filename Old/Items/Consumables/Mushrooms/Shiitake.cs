using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Shiitake : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.statLife += 180;
            player.HealEffect(180);
            return true;
        }
    }
}
