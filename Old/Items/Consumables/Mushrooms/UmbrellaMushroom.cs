using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class UmbrellaMushroom : ModMushroom
    {
        public override void SetStaticDefaults()
        {
            item.healLife = 80;
            item.healMana = 200;
        }
        public override bool UseItem(Player player)
        {
            player.statMana += 200;
            player.ManaEffect(200);
            player.statLife += 80;
            player.HealEffect(80);
            return true;
        }
    }
}
