using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Tiles;

namespace Entrogic.Items.Miscellaneous.Placeable.Trophy
{
    public class PETrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{$Mods.Entrogic.NPCName.污染之灵}" + "{$MapObject.Trophy}");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染之灵纪念章");
            DisplayName.AddTranslation((int)GameCulture.CultureName.German, "Verschmutzung Element-Trophäe");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Trofeo elemento de contaminación");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Trophée de Élément Pollution");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Italian, "Elemento di inquinamento trofeo");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Portuguese, "Troféu elemento de poluição");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Трофей элемента загрязнения");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Trofeum – Elementu Zanieczyszczenia");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SkeletronPrimeTrophy);
            item.createTile = TileType<BossTrophy>();
            item.placeStyle = 0;
        }
    }
}
