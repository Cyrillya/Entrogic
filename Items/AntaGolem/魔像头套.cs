using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.AntaGolem
{
    [AutoloadEquip(EquipType.Head)]
    public class 魔像头套 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 1;
            item.vanity = true;
            item.value = Item.sellPrice(0, 0, 20, 0);
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}