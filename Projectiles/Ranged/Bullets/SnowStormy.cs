using System;
using Entrogic.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace Entrogic.Projectiles.Ranged.Bullets
{
	public class SnowStormy : ModProjectile
	{
        public override void Load()
        {
            On.Terraria.Graphics.RainbowRodDrawer.Draw += RainbowRodDrawer_Draw;
        }

		private static VertexStrip _vertexStrip = new VertexStrip();
		private void RainbowRodDrawer_Draw(On.Terraria.Graphics.RainbowRodDrawer.orig_Draw orig, ref Terraria.Graphics.RainbowRodDrawer self, Projectile proj)
        {
			if (proj.type == Type)
			{
				MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
				miscShaderData.UseSaturation(-2.8f);
				miscShaderData.UseOpacity(3f);
				miscShaderData.Apply(null);
				_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, false);
				_vertexStrip.DrawTrail();
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
			else orig(ref self, proj);
		}
		private Color StripColors(float progressOnStrip)
		{
			Color value = Main.hslToRgb((progressOnStrip * 1.6f - Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.5f, byte.MaxValue);
			Color result = Color.Lerp(Color.White, Color.White, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A = 0;
			return result;
		}
		private float StripWidth(float progressOnStrip)
		{
			float num = 1f;
			float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}

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
			default(RainbowRodDrawer).Draw(projectile/*, Color.White, 2.8f, 40f*/);
			base.PostDraw(spriteBatch, lightColor);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			// 1.4 tML特性，PreDraw为false才会调用PostDraw
			return false;//base.PreDraw(spriteBatch, lightColor);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath3, base.projectile.position);
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
