using Entrogic.Items.Materials;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Tools
{
	public class CuteLittleHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("[c/7BFFFF:看上去十分像一把电磁叉...]");
		}

		public override void SetDefaults()
		{
			item.damage = 28;
			item.DamageType = DamageClass.Melee;
			item.width = 28;
			item.height = 28;
			item.useTime = 16;
			item.useAnimation = 20;
            item.hammer = 70;
            item.useStyle = ItemUseStyleID.Swing;
			item.knockBack = 5;
			item.value = 30000;
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.crit += 6;
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
				int Crapsky322 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                Main.dust[Crapsky322].scale = 0.75f;
			}
		}
	}
}