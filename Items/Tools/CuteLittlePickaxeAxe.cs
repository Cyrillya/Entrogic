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
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 28;
			item.height = 28;
			item.useTime = 8;
			item.useAnimation = 10;
			item.pick = 145;
            item.axe = 16;
			item.useStyle = ItemUseStyleID.SwingThrow;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.Wire, 12);
            recipe.AddIngredient(null, "CuteWidget", 1);
            recipe.AddRecipeGroup("IronBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
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