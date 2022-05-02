using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Materials;
using Entrogic.Tiles;

namespace Entrogic.Items.VoluGels.Armor
{
    /// <summary>
    /// 凝胶护胫 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/18 14:28:11
    /// </summary>
    [AutoloadEquip(EquipType.Legs)]
    public class 凝胶护胫 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 45) * 2;
            item.rare = RareID.LV2;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }
        public override void AddRecipes()
        {
<<<<<<< HEAD
            CreateRecipe()
                .AddIngredient(ItemType<CastIronBar>(), 15)
                .AddIngredient(ItemType<GelOfLife>(), 3)
                .AddTile(TileType<MagicDiversionPlatformTile>())
                .Register();
=======
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CastIronBar>(), 15);
            recipe.AddIngredient(ItemType<GelOfLife>(), 3);
            recipe.SetResult(this);
            recipe.AddTile(TileType<MagicDiversionPlatformTile>());
            recipe.AddRecipe();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
    }
}
