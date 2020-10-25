using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Materials
{
	public class CastIronBar : ModItem
	{
        public override void SetStaticDefaults()
		{
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));//后面那个是frame，前面那个是delay

            Tooltip.SetDefault("“强大的魔法适容性”\n" +
                "“早在旧世纪，传说一位名为Cyril的术士在一次偶然\n" +
                "的实验中发现了这种伟大又奇妙的金属；可一直到Cyril\n" +
                "去世后一百余年，人们才发现它的特性与用处，真是讽刺。”");
		}

		public override void SetDefaults()
		{
            item.width = 30;
			item.height = 24;
			item.maxStack = 99;
            item.value = Item.sellPrice(silver: 30);
            item.rare = ItemRarityID.Orange;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<SoulOfPure>(),3)
				.AddRecipeGroup("IronBar", 3)
				.AddRecipeGroup("Entrogic:GoldBar", 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}
