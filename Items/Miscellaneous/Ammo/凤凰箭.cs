using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Miscellaneous.Ammo
{
	public class 凤凰箭 : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("“圣火永不灭”");
		}

		public override void SetDefaults()
		{
			item.damage = 33;
			item.ranged = true;
			item.width = 14;
			item.height = 32;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1.5f;
            item.value = Item.sellPrice(copper: 20);
            item.rare = 10;
			item.shoot = mod.ProjectileType("凤凰箭");
			item.shootSpeed = 6f;
			item.ammo = AmmoID.Arrow;
			}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3456, 5);
			recipe.AddIngredient(41, 333);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}
