using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 香菇 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.statLife += 180;
            player.HealEffect(180);
            return true;
        }
    }
}
