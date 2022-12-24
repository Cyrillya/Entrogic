using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Symbiosis
{
    [AutoloadEquip(EquipType.Head)]
    public class VolutioMask : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }
        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.EarlyPHM;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 0, 20, 0);
        }
    }
}