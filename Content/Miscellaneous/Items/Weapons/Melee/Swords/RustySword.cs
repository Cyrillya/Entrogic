namespace Entrogic.Content.Miscellaneous.Items.Weapons.Melee.Swords
{
    public class RustySword : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.WoodenSword);
            Item.damage = 14;
            Item.knockBack = 5f;
            Item.crit += 14;
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.scale = 0.9f;
            Item.width = 52;
            Item.height = 52;
            Item.value = Item.sellPrice(0, 0, 90, 0);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            target.AddBuff(32, 240, false);
            target.AddBuff(36, 240, false);
        }
    }
}
