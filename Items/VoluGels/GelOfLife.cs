using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.VoluGels
{
    public class GelOfLife : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));//后面那个是frame，前面那个是delay

            Tooltip.SetDefault("“生命的本源，近乎完美的魔法介质”\n" +
                "“生命总会找到自己的出路”");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 40;
            item.maxStack = 99;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 3, 0);
        }
    }
}