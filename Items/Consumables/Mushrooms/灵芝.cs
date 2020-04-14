using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 灵芝 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("灵芝"), 5400);
            return true;
        }
    }
}
