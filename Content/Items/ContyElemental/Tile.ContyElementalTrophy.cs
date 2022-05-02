using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Tiles.ContyElemental;
using Entrogic.Content.Tiles.Symbiosis;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.ContyElemental
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
            Item.createTile = ModContent.TileType<ContyElementalTrophy_Tile>();
        }
    }
}
