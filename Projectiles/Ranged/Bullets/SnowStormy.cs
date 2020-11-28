using System;
using Entrogic.Dusts;
using Entrogic.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Projectiles.Ranged.Bullets
{
	public class SnowStormy : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.ownerHitCheck = true;
			projectile.friendly = true;
			projectile.timeLeft = 600;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 5;
			projectile.light = 0.5f;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 2;
			projectile.localNPCHitCooldown = 8;
			projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			projectile.rotation = Utils.ToRotation(projectile.velocity);
			projectile.ai[0]++;
			if (projectile.ai[0] <= 20f) projectile.velocity *= 1.05f;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			default(RainbowRodDrawer).Draw(projectile, Color.White, 2.8f, 40f);
			base.PostDraw(spriteBatch, lightColor);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return base.PreDraw(spriteBatch, lightColor);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCDeath3, base.projectile.position);
			for (int i = 0; i < 22; i++)
			{
				int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<SnowStormyDust>(), 0f, 0f, 100, default, 1.8f);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num].scale = 0.5f;
					Main.dust[num].fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
		}
	}
}
