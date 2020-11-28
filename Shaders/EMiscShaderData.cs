using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace Entrogic.Shaders
{
	public class EMiscShaderData : ShaderData
	{
		public EMiscShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
		{
		}

		public virtual void Apply(DrawData? drawData = null)
		{
			base.Shader.Parameters["uColor"].SetValue(this._uColor);
			base.Shader.Parameters["uSaturation"].SetValue(this._uSaturation);
			base.Shader.Parameters["uSecondaryColor"].SetValue(this._uSecondaryColor);
			base.Shader.Parameters["uTime"].SetValue((float)(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds % 3600.0));
			base.Shader.Parameters["uOpacity"].SetValue(this._uOpacity);
			base.Shader.Parameters["uShaderSpecificData"].SetValue(this._shaderSpecificData);
			if (drawData != null)
			{
				DrawData value = drawData.Value;
				Vector4 zero = Vector4.Zero;
				if (drawData.Value.sourceRect != null)
				{
					zero = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				}
				base.Shader.Parameters["uSourceRect"].SetValue(zero);
				base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				base.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			if (this._uImage0 != null)
			{
				Main.graphics.GraphicsDevice.Textures[0] = this._uImage0;
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)this._uImage0.Width, (float)this._uImage0.Height));
			}
			if (this._uImage1 != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = this._uImage1;
				Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float)this._uImage1.Width, (float)this._uImage1.Height));
			}
			if (this._uImage2 != null)
			{
				Main.graphics.GraphicsDevice.Textures[2] = this._uImage2;
				Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;
				base.Shader.Parameters["uImageSize2"].SetValue(new Vector2((float)this._uImage2.Width, (float)this._uImage2.Height));
			}
			if (this._useProjectionMatrix)
			{
				base.Shader.Parameters["uMatrixTransform0"].SetValue(EMiscShaderData.GetNormalizedTransformationmatrix(Main.GameViewMatrix));
			}
			base.Apply();
		}

		private static Matrix GetNormalizedTransformationmatrix(SpriteViewMatrix matrix)
		{
			Viewport viewport = (Viewport)EMiscShaderData.vpinfo.GetValue(matrix);
			Vector2 vector = new Vector2((float)viewport.Width, (float)viewport.Height);
			Matrix matrix2 = Matrix.CreateOrthographicOffCenter(0f, vector.X, vector.Y, 0f, 0f, 1f);
			return Matrix.Invert(matrix.EffectMatrix) * matrix.ZoomMatrix * matrix2;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0047A505 File Offset: 0x00478705
		public EMiscShaderData UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0047A515 File Offset: 0x00478715
		public EMiscShaderData UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0047A524 File Offset: 0x00478724
		public EMiscShaderData UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0047A52E File Offset: 0x0047872E
		public EMiscShaderData UseImage0(Texture2D tex)
		{
			this._uImage0 = tex;
			return this;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0047A543 File Offset: 0x00478743
		public EMiscShaderData UseImage1(Texture2D tex)
		{
			this._uImage1 = tex;
			return this;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0047A558 File Offset: 0x00478758
		public EMiscShaderData UseImage2(Texture2D tex)
		{
			this._uImage2 = tex;
			return this;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0047A56D File Offset: 0x0047876D
		public EMiscShaderData UseOpacity(float alpha)
		{
			this._uOpacity = alpha;
			return this;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0047A577 File Offset: 0x00478777
		public EMiscShaderData UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0047A587 File Offset: 0x00478787
		public EMiscShaderData UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0047A596 File Offset: 0x00478796
		public EMiscShaderData UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0047A5A0 File Offset: 0x004787A0
		public EMiscShaderData UseProjectionMatrix(bool doUse)
		{
			this._useProjectionMatrix = doUse;
			return this;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0047A5AA File Offset: 0x004787AA
		public EMiscShaderData UseSaturation(float saturation)
		{
			this._uSaturation = saturation;
			return this;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0047A5B4 File Offset: 0x004787B4
		public virtual EMiscShaderData GetSecondaryShader(Entity entity)
		{
			return this;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0047A5B7 File Offset: 0x004787B7
		public EMiscShaderData UseShaderSpecificData(Vector4 specificData)
		{
			this._shaderSpecificData = specificData;
			return this;
		}

		private static FieldInfo vpinfo = typeof(SpriteViewMatrix).GetField("_viewport", BindingFlags.Instance | BindingFlags.NonPublic);

		private Vector3 _uColor = Vector3.One;

		private Vector3 _uSecondaryColor = Vector3.One;

		private float _uSaturation = 1f;

		// Token: 0x04003B41 RID: 15169
		private float _uOpacity = 1f;

		// Token: 0x04003B42 RID: 15170
		private Texture2D _uImage0;

		// Token: 0x04003B43 RID: 15171
		private Texture2D _uImage1;

		// Token: 0x04003B44 RID: 15172
		private Texture2D _uImage2;

		// Token: 0x04003B45 RID: 15173
		private bool _useProjectionMatrix;

		// Token: 0x04003B46 RID: 15174
		private Vector4 _shaderSpecificData = Vector4.Zero;
	}
}
