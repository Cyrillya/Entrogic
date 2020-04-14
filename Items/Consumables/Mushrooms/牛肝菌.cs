using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class 牛肝菌 : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.AddBuff(mod.BuffType("牛肝菌"), 7200);
            return true;
        }
    }
}
