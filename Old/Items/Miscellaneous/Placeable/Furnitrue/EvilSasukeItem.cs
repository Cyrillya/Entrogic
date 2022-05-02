﻿using Entrogic.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Placeable.Furnitrue
{
    public class EvilSasukeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
<<<<<<< HEAD:Items/Miscellaneous/Placeable/Furnitrue/EvilSasukeItem.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Miscellaneous/Placeable/Furnitrue/邪神佐助.cs
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.createTile = TileType<EvilSasuke>();
            item.placeStyle = 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Entrogic:RCAV", 10)
                .AddRecipeGroup("Entrogic:DemBar", 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
