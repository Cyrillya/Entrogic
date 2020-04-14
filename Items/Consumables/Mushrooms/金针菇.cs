using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 金针菇 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("金针菇"), 5400);
            return true;
        }
    }
}
