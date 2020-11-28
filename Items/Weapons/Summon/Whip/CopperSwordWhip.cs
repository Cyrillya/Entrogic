using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Items.Weapons.Summon.Whip
{
    public class CopperSwordWhip : ModWhip
	{
        public override void WhipStaticDefaults()
        {
			DisplayName.SetDefault("Cipper Shortsword");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "铜短鞭");
        }
        public override void SetDefaults()
        {
            item.DefaultToWhip(ModContent.ProjectileType<CopperSwordWhip_Proj>(), 10, 3f, 1.9f*2, 24);
            item.SetShopValues((ItemRarityColor)RareID.LV2, Item.sellPrice(0, 0, 10, 0));
		}
        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.CopperShortsword)
				.AddIngredient(ItemID.Rope, 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
    public class CopperSwordWhip_Proj : ModWhipProj
	{
        public override void WhipDefaults()
        {
			LineColor = new Color(205, 134, 71);
			PenetrateDamageMultpiler = 0.8f;
            base.WhipDefaults();
        }
    }
}
