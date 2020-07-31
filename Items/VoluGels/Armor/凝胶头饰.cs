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
    /// 凝胶头饰 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/18 14:35:27
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class 凝胶头饰 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 80) * 2;
            item.rare = RareID.LV2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CastIronBar>(), 10);
            recipe.AddIngredient(ItemType<GelOfLife>(), 3);
            recipe.SetResult(this);
            recipe.AddTile(TileType<MagicDiversionPlatformTile>());
            recipe.AddRecipe();
        }
    }
}
