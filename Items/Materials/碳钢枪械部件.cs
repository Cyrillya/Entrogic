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
            item.rare = 5;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(1198,4);
            recipe.AddIngredient(22, 10);
            recipe.AddIngredient(324, 1);
			recipe.SetResult(this);
            recipe.AddTile(134);
			recipe.AddRecipe();
			/*
			// Start a new Recipe. (Prepend with "ModRecipe " if 1st recipe in code block.)
			// 增加一个新的合成配方（如果这是第一个配方,预先加上"ModRecipe"）
			recipe = new ModRecipe(mod);
			// Add a Vanilla Ingredient. 
			// Look up ItemIDs: https://github.com/bluemagic123/tModLoader/wiki/Vanilla-Item-IDs
			// To specify more than one ingredient, use multiple recipe.AddIngredient() calls.
			//增加普通的成分(这里指合成需要的物品)
			//物品ID表:https://github.com/bluemagic123/tModLoader/wiki/Vanilla-Item-IDs
			//要指定多个成分(这里指合成需要的物品)，请使用多个recipe.AddIngredient()调用
			recipe.AddIngredient(ItemID.DirtBlock);
			// An optional 2nd argument will specify a stack of the item. 
			//可选的第二个参数将指定该物品需要的数量
			recipe.AddIngredient(ItemID.Acorn, 10);
			// We can also specify the current item as an ingredient
			//我们也可以指定当前物品作为一个成分
			recipe.AddIngredient(this, 2);
			// Add a Mod Ingredient. Do not attempt ItemID.EquipMaterial, it's not how it works.
			//添加一个使用Mod内物品合成成分。不要尝试ItemID.EquipMaterial,这不会成功
			recipe.AddIngredient(mod, "EquipMaterial", 3);
			// an alternate approach to the above.
			//上述的替代方法
			recipe.AddIngredient(mod.ItemType("EquipMaterial"), 3);
			// RecipeGroups allow you create a recipe that accepts items from a group of similar ingredients. For example, all varieties of Wood are in the vanilla "Wood" Group
			// RecipeGroups允许您创建一个接受来自一组相似成分的项目的配方。例如，所有品种的木材都在物品“木材”组中
			recipe.AddRecipeGroup("Wood"); // check here for other vanilla groups(在这里查看其他物品组): https://github.com/bluemagic123/tModLoader/wiki/ModRecipe#public-void-addrecipegroupstring-name-int-stack--1 
			// Here is using a mod recipe group. Check out ExampleMod.AddRecipeGroups() to see how to register a recipe group.
			// 这里是使用一个Mod配方组。查看ExampleMod.AddRecipeGroups()以查看如何注册配方组
			recipe.AddRecipeGroup("ExampleMod:ExampleItem", 2);
			// To specify a crafting station, specify a tile. Look up TileIDs: https://github.com/bluemagic123/tModLoader/wiki/Vanilla-Tile-IDs
			//要指定一个制作站(需要通过xxx制作),请指定一个物块。物块ID表：https://github.com/bluemagic123/tModLoader/wiki/Vanilla-Tile-IDs
			recipe.AddTile(TileID.WorkBenches);
			// A mod Tile example. To specify more than one crafting station, use multiple recipe.AddTile() calls.
			//一个Mod制作站例子。要指定多个制作站，请使用多个recipe.AddTile()
			recipe.AddTile(mod, "ExampleWorkbench");
			// There is a limit of 14 ingredients and 14 tiles to a recipe.
			// Special
			// Water, Honey, and Lava are not tiles, there are special bools for those. Also needSnowBiome. Water also specifies that it works with Sinks.
			//配方中有最多能有14种物品和14种制作站。
			//特别的工作站
			//水，蜂蜜和熔岩不是瓷砖，那有专门的控制代码。还需要雪地环境。水槽(某家具)也可以替代水来工作
			recipe.needHoney = true;
			// Set the result of the recipe. You can use stack here too. Since this is in a ModItem class, we can use "this" to specify this item as the result.
			//设置配方的结果。你也可以在这里使用堆叠物品。由于这是一个ModItem(Mod物品)类，我们可以使用“this”来指定这个项目作为结果
			recipe.SetResult(this, 999); // or, for a vanilla result, recipe.SetResult(ItemID.Muramasa); 或者,对于普通结果,recipe.SetResult（ItemID.Muramasa）;
			// Finish your recipe
			// 完成你的合成配方
			recipe.AddRecipe();
			*/
		}
	}
}
