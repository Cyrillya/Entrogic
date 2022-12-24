namespace Entrogic.Content.Miscellaneous.Items.Materials
{
    public class CastIronBar : ModItem
	{
        public override void SetStaticDefaults()
		{
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));//后面那个是frame，前面那个是delay
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			Tooltip.SetDefault("“强大的魔法适容性”\n" +
                "“早在旧世纪，传说一位名为Cyril的术士在一次偶然\n" +
                "的实验中发现了这种伟大又奇妙的金属；可一直到Cyril\n" +
                "去世后一百余年，人们才发现它的特性与用处，真是讽刺。”");
		}

		public override void SetDefaults()
		{
            Item.width = 30;
			Item.height = 24;
			Item.maxStack = 99;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Orange;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SoulOfPure>(),3)
				.AddRecipeGroup("IronBar", 3)
				.AddRecipeGroup("Entrogic: Gold Bar", 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}
