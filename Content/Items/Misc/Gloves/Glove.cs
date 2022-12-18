using Entrogic.Core.CardSystem;
using Terraria.Enums;

namespace Entrogic.Content.Items.Misc.Gloves;

public class Glove : ItemGlove
{
    public override void SetDefaults() {
        SetGloveDefaults(20, 2.4f);
        Item.SetWeaponValues(6, 2f, 20);
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(0, 1));
        Item.useStyle = ItemUseStyleID.Swing;
    }
}