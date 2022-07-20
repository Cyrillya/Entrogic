namespace Entrogic.Content.Items.Athanasy
{
    [AutoloadEquip(EquipType.Head)]
    public class AthanasyMask : ItemBase
    {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;
        }

        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 28;
            Item.rare = RarityLevelID.EarlyHM;
            Item.value = Item.sellPrice(silver: 75);
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}