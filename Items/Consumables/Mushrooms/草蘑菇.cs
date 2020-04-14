using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 草蘑菇 : ModMushroom
    {
        public override void SetStaticDefaults()
        {
            item.healLife = 70;
        }
        public override bool UseItem(Player player)
        {
            player.statLife += 70;
            player.HealEffect(70);
            return true;
        }
    }
}
