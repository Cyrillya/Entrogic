using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Materials
{
    public class CBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 7));
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 55);
            item.rare = ItemRarityID.Pink;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.useAnimation = 15;
            item.useTime = 10;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.consumable = true;
            //item.placeStyle = 6;
        }
        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ItemID.HallowedBar, 3)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}