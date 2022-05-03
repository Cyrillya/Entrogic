using Entrogic.Content.Items.Symbiosis;
using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class SmeltDagger : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("“新鲜出炉”\n" +
                "有可能会有点软化");
        }

        public override void SetDefaults() {
            Item.damage = 24;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.scale = 2f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = RarityLevelID.LatePHM;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ModContent.ItemType<GelOfLife>(), 7)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire, 180);
    }
}