using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Symbiosis
{
    [AutoloadEquip(EquipType.Head)]
    public class GelHeadgear : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Gel Helmet");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "凝胶头盔");
            Tooltip.SetDefault("Softy");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "软软的");
        }
        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 80) * 2;
            Item.rare = RarityLevelID.MiddlePHM;
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player) {
            player.maxMinions++;
        }
        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemType<CastIronBar>(), 10)
            //    .AddIngredient(ItemType<GelOfLife>(), 3)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }
    }
}
