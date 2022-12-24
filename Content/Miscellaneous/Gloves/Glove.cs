using Entrogic.Core.Systems.CardSystem;
using Terraria.Enums;

namespace Entrogic.Content.Miscellaneous.Gloves;

[AutoloadEquip(EquipType.HandsOn)]
public class Glove : ItemGlove, IOverrideHand
{
    public override void SetDefaults() {
        SetGloveDefaults(20, 2.4f);
        Item.SetWeaponValues(6, 2f, 20);
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(0, 1));
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noUseGraphic = true;
    }
}