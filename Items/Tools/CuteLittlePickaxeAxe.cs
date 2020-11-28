using Entrogic.Items.Materials;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Tools
{
	public class CuteLittlePickaxeAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/7BFFFF:小稿斧，可挖掘速度不容小觑]");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.DamageType = DamageClass.Melee;
			item.width = 28;
			item.height = 28;
			item.useTime = 8;
			item.useAnimation = 10;
			item.pick = 145;
            item.axe = 16;
<<<<<<< HEAD
			item.useStyle = ItemUseStyleID.Swing;
=======
			item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
			item.knockBack = 4;
			item.value = 30000;
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.crit += 4;
            item.useTurn = true;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofLight, 6)
				.AddIngredient(ItemID.Wire, 12)
				.AddIngredient(ItemType<CuteWidget>(), 1)
				.AddRecipeGroup("IronBar", 4)
				.AddTile(TileID.MythrilAnvil)
				.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(25) == 0)
			{
				int Crapsky233 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                Main.dust[Crapsky233].scale = 0.75f;
            }
		}
	}
}