using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.VoluGels
{
    [AutoloadEquip(EquipType.Head)]
    public class VolutioMask : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
            item.value = Item.sellPrice(0, 0, 20, 0);
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}