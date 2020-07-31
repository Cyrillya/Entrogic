using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class Fungus : ModMushroom
    {
        public override void SetStaticDefaults()
        {
            item.healMana = 300;
        }
        public override bool UseItem(Player player)
        {
            player.statMana += 300;
            player.ManaEffect(300);
            player.buffImmune[94] = true;
            return true;
        }
    }
}
