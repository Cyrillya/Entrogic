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
            item.DefaultToWhip(ModContent.ProjectileType<CopperSwordWhip_Proj>(), 10, 3f, 1.9f*2, 6);
            item.SetShopValues((ItemRarityColor)RareID.LV2, Item.sellPrice(0, 0, 10, 0));
			item.useAnimation = 30;
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
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
			projectile.ai[0] += 1f;
			Projectile.GetWhipSettings(projectile, out float timeToFlyOut, out int num11, out float num12);
			projectile.Center = Main.GetPlayerArmPosition(projectile) + projectile.velocity * (projectile.ai[0] - 1f);
			projectile.spriteDirection = ((Vector2.Dot(projectile.velocity, Vector2.UnitX) >= 0f) ? 1 : -1);
			if (projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
			{
				projectile.Kill();
				return;
			}
			player.itemAnimation = player.itemAnimationMax - (int)(projectile.ai[0] / (float)projectile.MaxUpdates);
			if (projectile.ai[0] == (float)((int)(timeToFlyOut / 2f)))
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(projectile, _whipPointsForCollision);
				Vector2 position = _whipPointsForCollision[_whipPointsForCollision.Count - 1];
				SoundEngine.PlaySound(SoundID.Item153, position);
			}
			num11 = projectile.type;
			if (num11 != 952)
			{
				return;
			}
			float t8 = projectile.ai[0] / timeToFlyOut;
			float num10 = Utils.GetLerpValue(0.1f, 0.7f, t8, true) * Utils.GetLerpValue(0.9f, 0.7f, t8, true);
			if (num10 > 0.15f && Main.rand.NextFloat() < num10)
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(projectile, _whipPointsForCollision);
				Rectangle r9 = Utils.CenteredRectangle(_whipPointsForCollision[_whipPointsForCollision.Count - 1], new Vector2(30f, 30f));
				Vector2 value6 = _whipPointsForCollision[_whipPointsForCollision.Count - 2].DirectionTo(_whipPointsForCollision[_whipPointsForCollision.Count - 1]).SafeNormalize(Vector2.Zero);
				Dust dust4 = Dust.NewDustDirect(r9.TopLeft(), r9.Width, r9.Height, 26, 0f, 0f, 0, default(Color), 0.7f);
				dust4.noGravity = (Main.rand.Next(3) == 0);
				if (dust4.noGravity)
				{
					dust4.scale += 0.4f;
				}
				dust4.velocity += value6 * 2f;
				return;
			}
		}
    }
}
