namespace Entrogic.Content.Items.Misc.Materials
{
    public class CuteWidget : ModItem
	{
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 30;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightPurple;
		}
    }
}
