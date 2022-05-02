using Terraria.Audio;

namespace Entrogic.Content.Projectiles.ContyElemental.Friendly
{
    public class ContyCurrent_Proj : ProjectileBase
    {
        public override string Texture => ResourceManager.Blank;

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Electric Current");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "电流");
        }

        public override bool PreDraw(ref Color lightColor) {
            return false;
        }

        public override void PostDraw(Color lightColor) {
            base.PostDraw(lightColor);

            List<CustomVertexInfo> bars = new();
            List<Vector2> oldCenter = new();
            foreach (var pos in Projectile.oldPos) {
                if (pos == Vector2.Zero) continue;
                oldCenter.Add(pos + Projectile.Size / 2f);
            }

            var screenCenter = Main.screenPosition + Main.ScreenSize.ToVector2() / 2f;
            var screenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
            var screenPos = screenCenter - screenSize / 2f;

            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < oldCenter.Count; ++i) {
                //if (Projectile.oldPos[i] == Vector2.Zero) break;
				//Main.NewText(Projectile.oldPos[i]);
				//Main.EntitySpriteDraw(terraria.GameContent.TextureAssets.MagicPixel.Value, Projectile.oldPos[i] - Main.screenPosition,
				//	new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

				int width = 60;
                var normalDir = oldCenter[i - 1] - oldCenter[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X)); // 转换（单元化+PixelShader坐标系）

                var factor = i / (float)oldCenter.Count; // 遍历时以0-1递增
                var color = Color.Lerp(Color.White, Color.Blue, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor); // 把factor转换为1-0.05 （?） 这个是透明度

                bars.Add(new CustomVertexInfo(oldCenter[i] + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(oldCenter[i] + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new();

            if (bars.Count > 2) {

				// 按照顺序连接三角形
				triangleList.Add(bars[0]);
				var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White,
					new Vector3(0, 0.5f, 1));
				triangleList.Add(bars[1]);
				triangleList.Add(vertex);
				for (int i = 0; i < bars.Count - 2; i += 2) {
					// 这是一个四边形 [i] [i+2] [i+1] [i+3]
					triangleList.Add(bars[i]);
					triangleList.Add(bars[i + 2]);
					triangleList.Add(bars[i + 1]);

					triangleList.Add(bars[i + 1]);
					triangleList.Add(bars[i + 2]);
					triangleList.Add(bars[i + 3]);
				}
				//var sides = 36f;
				//var color = new Color(0.4f, 0.4f, 0.4f, 0.8f);
				//for (int i = 0; i < sides; i++) {
				//	var basic = MathHelper.TwoPi + 64f;
				//	float rotat = basic / sides * i;
				//	float rotat2 = basic / sides * (i + 1);

				//	var factor = i / (float)sides; // 遍历时以0-1递增
				//	var w = MathHelper.Lerp(1f, 0.05f, factor); // 把factor转换为1-0.05 （?） 这个是透明度

				//	var r1vertex1 = new CustomVertexInfo(screenCenter + rotat.ToRotationVector2() * 180f, color, new Vector3((float)Math.Sqrt(factor), 1, w));
				//	var r1vertex2 = new CustomVertexInfo(screenCenter + rotat2.ToRotationVector2() * 180f, color, new Vector3((float)Math.Sqrt(factor), 0, w));
				//	var r2vertex1 = new CustomVertexInfo(screenCenter + rotat.ToRotationVector2() * 150f, color, new Vector3((float)Math.Sqrt(factor), 0, w));
				//	var r2vertex2 = new CustomVertexInfo(screenCenter + rotat2.ToRotationVector2() * 150f, color, new Vector3((float)Math.Sqrt(factor), 1, w));
				//	triangleList.Add(r1vertex1);
				//	triangleList.Add(r1vertex2);
				//	triangleList.Add(r2vertex1);

				//	triangleList.Add(r1vertex2);
				//	triangleList.Add(r2vertex1);
				//	triangleList.Add(r2vertex2);
				//}

				Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader

                ResourceManager.Trail.Value.Parameters["uCustomColor"].SetValue(false);
                ResourceManager.Trail.Value.Parameters["uTransform"].SetValue(model * projection);
                ResourceManager.Trail.Value.Parameters["uTime"].SetValue(-(float)Main.gameTimeCache.TotalGameTime.TotalMilliseconds % 30000 * 0.003f);
                Main.instance.GraphicsDevice.Textures[0] = ResourceManager.TrailMainColor.Value;
                Main.instance.GraphicsDevice.Textures[1] = ResourceManager.TrailMainShape.Value;
                Main.instance.GraphicsDevice.Textures[2] = ResourceManager.TrailMainShape.Value;
                Main.instance.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ResourceManager.Trail.Value.CurrentTechnique.Passes[0].Apply();


                Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.instance.GraphicsDevice.RasterizerState = originalState;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }


        // 自定义顶点数据结构，注意这个结构体里面的顺序需要和shader里面的数据相同
        private struct CustomVertexInfo : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new(new VertexElement[3]
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

        private void Resize(int newWidth, int newHeight)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = newWidth;
            Projectile.height = newHeight;
            Projectile.Center = Projectile.position;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);

            // 烈焰火鞭的代码
            int width3 = Projectile.width;
            int height3 = Projectile.height;
            Resize(96, 96);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Resize(width3, height3);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Vector2 target5 = Projectile.Center;
            for (int num27 = 0; num27 < Projectile.oldPos.Length; num27++)
            {
                Vector2 vector5 = Projectile.oldPos[num27];
                if (vector5 == Vector2.Zero)
                    break;

                //Color newColor3 = Main.hslToRgb(Main.rand.NextFloat() * 0.11111111f, 1f, 0.5f);
                Color newColor3 = Color.DeepSkyBlue;
                int num28 = Main.rand.Next(1, 5);
                float num29 = MathHelper.Lerp(0.3f, 1f, Utils.GetLerpValue(Projectile.oldPos.Length, 0f, num27, clamped: true));
                if ((float)num27 >= (float)Projectile.oldPos.Length * 0.3f)
                    num28--;

                if ((float)num27 >= (float)Projectile.oldPos.Length * 0.75f)
                    num28 -= 2;

                Vector2 value4 = vector5.DirectionTo(target5).SafeNormalize(Vector2.Zero);
                target5 = vector5;
                for (float num30 = 0f; num30 < (float)num28; num30++)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        int num31 = Dust.NewDust(vector5, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 0, newColor3);
                        Dust dust = Main.dust[num31];
                        dust.velocity *= Main.rand.NextFloat() * 0.8f;
                        Main.dust[num31].noGravity = true;
                        Main.dust[num31].scale = Main.rand.NextFloat() * 1f;
                        Main.dust[num31].fadeIn = Main.rand.NextFloat() * 2f;
                        dust = Main.dust[num31];
                        dust.velocity += value4 * 8f;
                        dust = Main.dust[num31];
                        dust.scale *= num29;
                        if (num31 != 6000)
                        {
                            Dust dust9 = Dust.CloneDust(num31);
                            dust = dust9;
                            dust.scale /= 2f;
                            dust = dust9;
                            dust.fadeIn /= 2f;
                            dust9.color = new Color(255, 255, 255, 255);
                        }
                    }
                    else
                    {
                        Dust dust10 = Dust.NewDustDirect(vector5, Projectile.width, Projectile.height, DustID.IceTorch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 100);
                        Dust dust;
                        if (Main.rand.Next(2) == 0)
                        {
                            dust10.noGravity = true;
                            dust = dust10;
                            dust.scale *= 2.5f;
                        }

                        dust = dust10;
                        dust.velocity *= 2f;
                        dust = dust10;
                        dust.velocity += value4 * 6f;
                        dust = dust10;
                        dust.scale *= num29;
                        dust10.noLightEmittence = (dust10.noLight = true);
                    }
                }
            }

            for (int num32 = 0; num32 < 20; num32++)
            {
                Dust dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 100);
                dust11.noGravity = true;
                dust11.velocity = Main.rand.NextVector2Circular(1f, 1f) * 6f;
                dust11.scale = 1.6f;
                dust11.fadeIn = 1.3f + Main.rand.NextFloat() * 1f;
                dust11.noLightEmittence = (dust11.noLight = true);
                Dust dust = dust11;
                dust.velocity += Projectile.velocity * 0.1f;
                dust11 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 100);
                dust = dust11;
                dust.velocity *= 2f;
                dust11.noLightEmittence = (dust11.noLight = true);
                dust = dust11;
                dust.velocity += Projectile.velocity * 0.1f;
            }
        }

        public override void SetDefaults() {
            Projectile.tileCollide = true;
            Projectile.timeLeft = 270;
            Projectile.alpha = 0;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Magic;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private List<int> _attackedList = new();
        public override void AI() {
            NPC target = null;
            Projectile.ai[1]++;
            float distanceMax = 1800f;
            foreach (NPC NPC in Main.npc) {
                if (!_attackedList.Contains(NPC.whoAmI) && NPC.active && NPC.lifeMax >= 10 && NPC.type != NPCID.TargetDummy && !NPC.dontTakeDamage && !NPC.friendly && Collision.CanHit(Projectile.position, 8, 8, NPC.position, NPC.width, NPC.height)) {
                    float currentDistance = Vector2.Distance(NPC.Center, Projectile.Center);
                    if (currentDistance < distanceMax) {
                        distanceMax = currentDistance;
                        target = NPC;
                    }
                }
            }
            if (target != null && target.active) {
                // 需要移动到的位置，加权平均
                var pos = 0.9f * Projectile.Center + 0.1f * target.Center;
                // 然后假装自己是用速度实现的
                Projectile.velocity = (pos - Projectile.Center) * 2f;
            }
            else {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 24f;
            }
            Projectile.rotation = Utils.ToRotation(Projectile.velocity);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
            _attackedList.Add(target.whoAmI);
        }
    }
}
