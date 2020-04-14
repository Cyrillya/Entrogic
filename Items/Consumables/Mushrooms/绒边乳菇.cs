using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 绒边乳菇 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(20, 2400);
            return true;
        }
    }
}
