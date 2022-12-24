using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Items
{
    public class ContyElementalTrophy : ItemBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Contaminated Elemental Trophy");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染之灵纪念章");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.EyeofCthulhuTrophy);
            Item.createTile = ModContent.TileType<ContyElementalTrophyTile>();
        }
    }
}
