
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Items.Weapons.Card
{
    public class RandomCard : ModItem
    {
        public sealed override void SetDefaults()
        {
            item.consumable = true;
            item.maxStack = 1;
            item.rare = RareID.LV3;
            item.width = 42;
            item.height = 52;
        }
        public override void HoldItem(Player player)
        {
            Item i = ModHelper.GetRandomCard(Main.LocalPlayer, Main.rand);
            if (Main.mouseItem.type != item.type)
            {
                // 将原物品消除
                player.inventory[player.selectedItem].active = false;
                player.inventory[player.selectedItem].TurnToAir();
                // 把物品"塞"里面
                player.inventory[player.selectedItem] = i;
                return;
            }
            Main.mouseItem.active = false;
            Main.mouseItem.TurnToAir();
            Main.mouseItem = i;
        }
    }
}
