using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Entrogic.Shaders
{
	// Token: 0x02000116 RID: 278
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct RainbowRodDrawer
	{
		public void Draw(Projectile proj, Color color, float Saturation, float Opacity)
		{
			this.trailColor = color;
			EMiscShaderData miscShaderData = ResourceLoader.MagicMissile;
			miscShaderData.UseImage0((Texture2D)Entrogic.ModTexturesTable["MagicMissile0"]);
			miscShaderData.UseImage1((Texture2D)Entrogic.ModTexturesTable["MagicMissile-RainbowRod"]);
			miscShaderData.UseImage2((Texture2D)Entrogic.ModTexturesTable["MagicMissile2"]);
			miscShaderData.UseSaturation(-Saturation);
			miscShaderData.UseOpacity(Opacity);
			miscShaderData.Apply(null);
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f, true);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001BA90 File Offset: 0x00019C90
		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.White, this.trailColor, ModHelper.GetLerpValue(-0.2f, 0.5f, progressOnStrip, true)) * (1f - ModHelper.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A = 0;
			return result;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001BAE4 File Offset: 0x00019CE4
		public static Color HslToRgb(Vector3 hslVector)
		{
			return RainbowRodDrawer.HslToRgb(hslVector.X, hslVector.Y, hslVector.Z);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001BB00 File Offset: 0x00019D00
		public static Color HslToRgb(float Hue, float Saturation, float Luminosity)
		{
			byte r;
			byte g;
			byte b;
			if (Saturation == 0f)
			{
				r = (byte)Math.Round((double)Luminosity * 255.0);
				g = (byte)Math.Round((double)Luminosity * 255.0);
				b = (byte)Math.Round((double)Luminosity * 255.0);
			}
			else
			{
				double num5 = (double)Hue;
				double num2;
				if ((double)Luminosity < 0.5)
				{
					num2 = (double)Luminosity * (1.0 + (double)Saturation);
				}
				else
				{
					num2 = (double)(Luminosity + Saturation - Luminosity * Saturation);
				}
				double t = 2.0 * (double)Luminosity - num2;
				double num3 = num5 + 0.3333333333333333;
				double num4 = num5;
				double c = num5 - 0.3333333333333333;
				num3 = Main.hue2rgb(num3, t, num2);
				num4 = Main.hue2rgb(num4, t, num2);
				double num6 = Main.hue2rgb(c, t, num2);
				r = (byte)Math.Round(num3 * 255.0);
				g = (byte)Math.Round(num4 * 255.0);
				b = (byte)Math.Round(num6 * 255.0);
			}
			return new Color((int)r, (int)g, (int)b);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001BC08 File Offset: 0x00019E08
		private float StripWidth(float progressOnStrip)
		{
			float num = 1f;
			float lerpValue = ModHelper.GetLerpValue(0f, 0.2f, progressOnStrip, true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}

		// Token: 0x040000CF RID: 207
		public Color trailColor;

		// Token: 0x040000D0 RID: 208
		private static VertexStrip _vertexStrip = new VertexStrip();
	}
}
