using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 双生菇 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("双生菇"), 1800);
            return true;
        }
    }
}
