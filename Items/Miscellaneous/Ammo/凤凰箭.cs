using Terraria;
using Terraria.GameContent.Creative;
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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			item.damage = 33;
			item.DamageType = DamageClass.Ranged;
			item.width = 14;
			item.height = 32;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1.5f;
            item.value = Item.sellPrice(copper: 20);
            item.rare = ItemRarityID.Red;
			item.shoot = ProjectileType<Projectiles.Ammos.凤凰箭>();
			item.shootSpeed = 6f;
			item.ammo = AmmoID.Arrow;
			}
		public override void AddRecipes()
		{
			CreateRecipe(333)
				.AddIngredient(ItemID.FragmentVortex, 5)
				.AddIngredient(ItemID.FlamingArrow, 333)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
