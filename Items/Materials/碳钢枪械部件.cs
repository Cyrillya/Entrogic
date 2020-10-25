using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Materials
{
	public class 碳钢枪械部件 : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("强大枪械的必备材料");
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 26;
			item.maxStack = 5;
            item.value = Item.buyPrice(gold: 1);
            item.rare = ItemRarityID.Pink;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.TitaniumBar,4)
				.AddIngredient(ItemID.IronBar, 10)
				.AddIngredient(ItemID.IllegalGunParts, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
