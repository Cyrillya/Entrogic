using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Miscellaneous.Placeable.Trophy
{
    public class PETrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{$Mods.Entrogic.NPCName.污染之灵}" + "{$MapObject.Trophy}");
            DisplayName.AddTranslation(GameCulture.Chinese, "污染之灵纪念章");
            DisplayName.AddTranslation(GameCulture.German, "Verschmutzung Element-Trophäe");
            DisplayName.AddTranslation(GameCulture.Spanish, "Trofeo elemento de contaminación");
            DisplayName.AddTranslation(GameCulture.French, "Trophée de Élément Pollution");
            DisplayName.AddTranslation(GameCulture.Italian, "Elemento di inquinamento trofeo");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Troféu elemento de poluição");
            DisplayName.AddTranslation(GameCulture.Russian, "Трофей элемента загрязнения");
            DisplayName.AddTranslation(GameCulture.Polish, "Trofeum – Elementu Zanieczyszczenia");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SkeletronPrimeTrophy);
            item.createTile = mod.TileType("BossTrophy");
            item.placeStyle = 0;
        }
    }
}
