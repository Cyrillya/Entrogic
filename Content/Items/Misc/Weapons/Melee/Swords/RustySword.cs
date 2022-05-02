namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class RustySword : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 14;
            Item.knockBack = 5f;
            Item.crit += 14;
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 0, 90, 0);
            Item.UseSound = SoundID.Item1;
            Item.scale = 0.9f;
            Item.width = 52;
            Item.height = 52;
            Item.maxStack = 1;
            Item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            target.AddBuff(32, 240, false);
            target.AddBuff(36, 240, false);
        }
    }
}
