using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.Symbiosis
{
    [AutoloadEquip(EquipType.Legs)]
    public class GelLeggings : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Gel Leggings");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "凝胶护胫");
            Tooltip.SetDefault("\"Hard to put on, harder to take off\"");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "“穿上很难，脱下更难”");
        }
        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 45) * 2;
            Item.rare = RarityLevelID.MiddlePHM;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player) {
            player.maxMinions++;
        }
        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemType<CastIronBar>(), 15)
            //    .AddIngredient(ItemType<GelOfLife>(), 3)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }
    }
}
