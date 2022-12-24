using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Symbiosis
{
    public class SymbiosisTrophy : ItemBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gel Symbiosis Trophy");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "凝胶共生体纪念章");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.EyeofCthulhuTrophy);
            Item.createTile = ModContent.TileType<SymbiosisTrophyTile>();
        }
    }
}
