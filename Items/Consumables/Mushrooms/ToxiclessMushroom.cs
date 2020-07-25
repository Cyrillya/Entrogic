using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public class ToxiclessMushroom : ModMushroom
    {
        public override bool UseItem(Player player)
        {
            player.KillMe(PlayerDeathReason.ByCustomReason(player.name + Language.GetTextValue("Mods.Entrogic.DieByMushroom")), 1000.0, 0, false);
            return true;
        }

    }
}
