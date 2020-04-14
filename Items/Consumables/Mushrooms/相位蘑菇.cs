using Terraria;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 相位蘑菇 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("相位蘑菇"), 480);
            return true;
        }
    }
}
