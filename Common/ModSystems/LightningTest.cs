using Entrogic.Interfaces.UI.BookUI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Entrogic.Common.ModSystems
{
	public struct Offset
    {
		internal float minX;
		internal float maxX;
		internal float minY;
		internal float maxY;
    }
	public class Lightning
	{
		public Lightning(Vector2 pos, int nodeAmount, int lifeSpan, int width, Offset offset, int length = 500) {
			Position = pos;
			NodeAmount = nodeAmount;
			LifeSpan = lifeSpan;
			Width = width;
			Length = length;
			Offset = offset;
		}
		internal readonly Vector2 Position; // 位置
		internal readonly int NodeAmount; // 结点数量（不算上基础位置的，基础位置不会移动，独立绘制）
		internal readonly int Width; // 宽度
		internal readonly int Length; // 竖向长度
		internal readonly Offset Offset; // 每个节点初始偏移量
		internal int LifeSpan; // 存活时间

		internal bool ShouldDraw = true;

		private int Timer;
		private readonly List<Vector2> _nodes = new List<Vector2>();
		private readonly List<Vector2> _nodeVelocity = new List<Vector2>();
		public virtual void Draw() {
			if (Timer == 0) {
				for (int i = 1; i <= NodeAmount; i++) {
					var offset = new Vector2(Main.rand.NextFloat(Offset.minX, Offset.maxX), Main.rand.NextFloat(Offset.minY, Offset.maxY));
					_nodes.Add(Position + new Vector2(0, Length / NodeAmount * i) + offset);
					_nodeVelocity.Add(new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)));
				}
            }
			Timer++;
			if (Timer >= LifeSpan) ShouldDraw = false;
			List<Vector2> centers = new List<Vector2>();
			List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
			List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
			for (int i = 0; i < _nodes.Count; i++) {
				var node = _nodes[i];
				_nodes[i] += _nodeVelocity[i] * Main.rand.NextFloat(0.5f, 0.8f);

				centers.Add(node);
			}
			// 把所有的点都生成出来，按照顺序
			for (int i = 1; i < centers.Count; ++i) {
				var normalDir = centers[i - 1] - centers[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)); // 转换（单元化+PixelShader坐标系）

				//var factor = i / (float)centers.Count; // 遍历时以0-1递增
				var color = Color.Blue; //Color.Lerp(Color.White, Color.Blue, factor);
				var w = 1f; // MathHelper.Lerp(1f, 0.05f, factor); // 做1-0.05插值 这个是透明度

				bars.Add(new CustomVertexInfo(centers[i] + normalDir * Width, color, new Vector3(0, 1, w)));
				bars.Add(new CustomVertexInfo(centers[i] + normalDir * -Width, color, new Vector3(0, 0, w)));
			}
			triangleList.Add(bars[0]);
			var vertex = new CustomVertexInfo(Position, Color.White, new Vector3(0, 0.5f, 1));
			triangleList.Add(bars[1]);
			triangleList.Add(vertex);
			int ind;
			for (ind = 0; ind < bars.Count - 2; ind += 2) {
				// 这是一个四边形 [i] [i+2] [i+1] [i+3]
				triangleList.Add(bars[ind]);
				triangleList.Add(bars[ind + 2]);
				triangleList.Add(bars[ind + 1]);

				triangleList.Add(bars[ind + 1]);
				triangleList.Add(bars[ind + 2]);
				triangleList.Add(bars[ind + 3]);
			}

			ResourceManager.Trail.Value.Parameters["uOpacity"].SetValue(MathHelper.Lerp(1f, 0f, (float)Timer / (float)LifeSpan));
			Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
		}
	}
	public class LightningTest : ModSystem
	{
		private GameTime _lastUpdateUiGameTime;
		private int Timer;
		public static List<Lightning> Lightnings = new List<Lightning>();

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			base.ModifyInterfaceLayers(layers);
			int CursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
			if (CursorIndex != -1) {
				layers.Insert(CursorIndex, new LegacyGameInterfaceLayer(
					"Entrogic: Lightning Test",
					delegate {
						//Timer++;
						if (Timer % 30 == 1) {
							Lightnings.Add(new Lightning(Main.MouseWorld, 10, 10, 80, new Offset { minX = -20, maxX = 20, minY = -5, maxY = 5 }, -1000));
							Lightnings.Add(new Lightning(Main.MouseWorld, 10, 10, 80, new Offset { minX = -20, maxX = 20, minY = -5, maxY = 5 }, -1000));
							Lightnings.Add(new Lightning(Main.MouseWorld, 10, 10, 80, new Offset { minX = -20, maxX = 20, minY = -5, maxY = 5 }, -1000));
							Lightnings.Add(new Lightning(Main.MouseWorld, 10, 10, 80, new Offset { minX = -20, maxX = 20, minY = -5, maxY = 5 }, -1000));
						}

						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
						RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
						// 干掉注释掉就可以只显示三角形栅格
						//RasterizerState rasterizerState = new RasterizerState();
						//rasterizerState.CullMode = CullMode.None;
						//rasterizerState.FillMode = FillMode.WireFrame;
						//Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

						var screenCenter = Main.screenPosition + Main.ScreenSize.ToVector2() / 2f;
						var screenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
						var screenPos = screenCenter - screenSize / 2f;
						var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
						var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

						// 把变换和所需信息丢给shader

						ResourceManager.Trail.Value.Parameters["uTransform"].SetValue(model * projection);
						ResourceManager.Trail.Value.Parameters["uTime"].SetValue(-(float)Main.gameTimeCache.TotalGameTime.TotalMilliseconds % 30000 * 0.003f);
						Main.instance.GraphicsDevice.Textures[0] = ResourceManager.TrailMainColor.Value;
						Main.instance.GraphicsDevice.Textures[1] = ResourceManager.TrailMainShape.Value;
						Main.instance.GraphicsDevice.Textures[2] = ResourceManager.TrailMaskColor.Value;
						Main.instance.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
						Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
						Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
						//Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
						//Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
						//Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

						ResourceManager.Trail.Value.CurrentTechnique.Passes[0].Apply();

						List<Lightning> shouldBeDeleted = new List<Lightning>();
						foreach (var light in Lightnings) {
							light.Draw();
							if (!light.ShouldDraw) {
								shouldBeDeleted.Add(light);
                            }
						}
						foreach(var i in shouldBeDeleted) {
							Lightnings.Remove(i);
                        }

						Main.instance.GraphicsDevice.RasterizerState = originalState;

						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}

	// 自定义顶点数据结构，注意这个结构体里面的顺序需要和shader里面的数据相同
	public struct CustomVertexInfo : IVertexType
	{
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
		{
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
				new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
		});
		public Vector2 Position;
		public Color Color;
		public Vector3 TexCoord;

		public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord) {
			Position = position;
			Color = color;
			TexCoord = texCoord;
		}

		public VertexDeclaration VertexDeclaration {
			get {
				return _vertexDeclaration;
			}
		}
	}

	// 类似于 https://www.bilibili.com/video/BV1tE411H7ow 的闪电
	/*
	public class Lightning
	{
		public Lightning(Point areaX, Point areaY, Point sinArea, int timerMax, int width, int randomArea, float sinOffset, int nodes = 10, int length = 360, float lengthMuilt = 1.5f) {
			PosAreaX = areaX;
			PosAreaY = areaY;
			SinFactorArea = sinArea;
			TimerMax = timerMax;
			Width = width;
			RandomArea = randomArea;
			SinOffset = sinOffset;
			Nodes = nodes;
			Length = length;
			LengthMuilt = lengthMuilt;
		}
		internal Point PosAreaX;
		internal Point PosAreaY;
		internal Point SinFactorArea;
		internal int TimerMax;
		internal int RandomArea;
		internal float SinOffset;
		internal int Nodes;
		internal int Length;
		internal float LengthMuilt;

		private Vector2 _lastLightningPos;
		internal int Mode = 1;
		internal float SinFactor;
		internal int Timer;
		internal int Width;
		public virtual void Draw() {
			if (Timer >= TimerMax) {
				Mode = -Mode;
				_lastLightningPos.X = Main.rand.Next(PosAreaX.X, PosAreaX.Y);
				_lastLightningPos.Y = Main.rand.Next(PosAreaY.X, PosAreaY.Y) * Mode;
				Timer = 0;
				SinFactor = Main.rand.Next(SinFactorArea.X, SinFactorArea.Y) * 0.01f;
			}
			Timer++;
			var muilt = Timer;
			var pos = _lastLightningPos + Main.MouseWorld;
			List<Vector2> centers = new List<Vector2>();
			List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
			List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
			for (int i = 0; i <= Nodes; i++) {
				int a = i * (Length / Nodes);
				var y = (float)(Math.Sin((MathHelper.ToRadians(a) + SinOffset) * SinFactor * Mode) * MathHelper.Lerp(0.4f, 0.6f, muilt / TimerMax) * 50f);
				var vec = new Vector2(pos.X + a * LengthMuilt, pos.Y + y);
				var random = new Random(i);
				var area = RandomArea;
				vec += new Vector2(random.Next(-6, 7) + (float)random.NextDouble() * muilt, random.Next(-area, area + 1) + (float)random.NextDouble() * muilt);
				centers.Add(vec);
			}
			// 把所有的点都生成出来，按照顺序
			for (int i = 1; i < centers.Count; ++i) {
				var normalDir = centers[i - 1] - centers[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)); // 转换（单元化+PixelShader坐标系）

				//var factor = i / (float)centers.Count; // 遍历时以0-1递增
				var color = Color.Blue; //Color.Lerp(Color.White, Color.Blue, factor);
				var w = 1f; // MathHelper.Lerp(1f, 0.05f, factor); // 做1-0.05插值 这个是透明度

				bars.Add(new CustomVertexInfo(centers[i] + normalDir * Width, color, new Vector3(0, 1, w)));
				bars.Add(new CustomVertexInfo(centers[i] + normalDir * -Width, color, new Vector3(0, 0, w)));
			}
			triangleList.Add(bars[0]);
			var vertex = new CustomVertexInfo(Main.MouseWorld, Color.White, new Vector3(0, 0.5f, 1));
			triangleList.Add(bars[1]);
			triangleList.Add(vertex);
			int ind;
			for (ind = 0; ind < bars.Count - 2; ind += 2) {
				// 这是一个四边形 [i] [i+2] [i+1] [i+3]
				triangleList.Add(bars[ind]);
				triangleList.Add(bars[ind + 2]);
				triangleList.Add(bars[ind + 1]);

				triangleList.Add(bars[ind + 1]);
				triangleList.Add(bars[ind + 2]);
				triangleList.Add(bars[ind + 3]);
			}
			ind -= 2;
			triangleList.Add(bars[ind + 2]);
			triangleList.Add(bars[ind + 3]);
			triangleList.Add(new CustomVertexInfo(Main.MouseWorld + new Vector2(Length * LengthMuilt + 100f, 0f), Color.White, new Vector3(0, 0.5f, 1)));

			Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
		}
    }
	public class LightningTest : ModSystem
	{
		private GameTime _lastUpdateUiGameTime;
		private int Timer;
		private List<Lightning> lightnings = new List<Lightning>();

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			base.ModifyInterfaceLayers(layers);
			int CursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
			if (CursorIndex != -1) {
				layers.Insert(CursorIndex, new LegacyGameInterfaceLayer(
					"Entrogic: Lightning Test",
					delegate {
						Timer++;
						if (lightnings.Count < 8) {
							lightnings.Add(new Lightning(new Point(20, 40), new Point(10, 31), new Point(80, 140), 14, 18, 25, 2f));
							lightnings.Add(new Lightning(new Point(20, 40), new Point(10, 31), new Point(80, 140), 6, 18, 30, 1.18f));
							lightnings.Add(new Lightning(new Point(10, 20), new Point(5, 21), new Point(120, 150), 10, 40, 15, 0f));
							lightnings.Add(new Lightning(new Point(5, 11), new Point(5, 8), new Point(120, 150), 11, 60, 7, 1.6f));
							lightnings.Add(new Lightning(new Point(5, 11), new Point(5, 8), new Point(120, 150), 8, 60, 5, 0.23f));
							lightnings.Add(new Lightning(new Point(5, 11), new Point(5, 8), new Point(120, 150), 7, 60, 5, 3.2f));
							lightnings.Add(new Lightning(new Point(5, 11), new Point(5, 8), new Point(120, 150), 13, 63, 8, 1.4f));
							lightnings.Add(new Lightning(new Point(5, 11), new Point(8, 14), new Point(120, 150), 8, 60, 4, 2f));
						}

						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
						RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
						// 干掉注释掉就可以只显示三角形栅格
						//RasterizerState rasterizerState = new RasterizerState();
						//rasterizerState.CullMode = CullMode.None;
						//rasterizerState.FillMode = FillMode.WireFrame;
						//Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

						var screenCenter = Main.screenPosition + Main.ScreenSize.ToVector2() / 2f;
						var screenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
						var screenPos = screenCenter - screenSize / 2f;
						var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
						var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

						// 把变换和所需信息丢给shader

						TrailingHandler.DefaultEffect.Value.Parameters["uTransform"].SetValue(model * projection);
						TrailingHandler.DefaultEffect.Value.Parameters["uTime"].SetValue(-(float)Main.gameTimeCache.TotalGameTime.TotalMilliseconds % 30000 * 0.003f);
						Main.instance.GraphicsDevice.Textures[0] = TrailingHandler.MainColor.Value;
						Main.instance.GraphicsDevice.Textures[1] = TrailingHandler.MainShape.Value;
						Main.instance.GraphicsDevice.Textures[2] = TrailingHandler.MaskColor.Value;
						Main.instance.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
						Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
						Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
						//Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
						//Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
						//Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

						TrailingHandler.DefaultEffect.Value.CurrentTechnique.Passes[0].Apply();

						foreach (var light in lightnings) {
							light.Draw();
                        }
						Main.instance.GraphicsDevice.RasterizerState = originalState;

						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}

	// 自定义顶点数据结构，注意这个结构体里面的顺序需要和shader里面的数据相同
	public struct CustomVertexInfo : IVertexType
	{
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
		{
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
				new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
		});
		public Vector2 Position;
		public Color Color;
		public Vector3 TexCoord;

		public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord) {
			Position = position;
			Color = color;
			TexCoord = texCoord;
		}

		public VertexDeclaration VertexDeclaration {
			get {
				return _vertexDeclaration;
			}
		}
	}
	*/
}
