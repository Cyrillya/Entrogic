namespace Entrogic.Content.Projectiles.ContyElemental.Friendly
{
    public class RainstormProj : ProjectileBase
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12; // 记录trail的长度
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.5f;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 0;
            Projectile.timeLeft = 45;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer && !player.controlUseItem) {
                Projectile.Kill();
                return;
            }

            if (player.dead || !player.active || player.ghost) {
                Projectile.Kill();
                return;
            }

            Projectile.ai[0]++;

            if (Projectile.ai[0] % 20 == 0)
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);

            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter);

            const float maxRotation = MathHelper.Pi / 6.85f / 1.5f;
            float modifier = maxRotation * Math.Min(1f, Projectile.ai[0] / 50f);

            Projectile.numHits = 0;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            Projectile.rotation += modifier * player.direction;
            Projectile.velocity = Projectile.rotation.ToRotationVector2();
            Projectile.position -= Projectile.velocity;

            if (Projectile.ai[0] == 40) {
                Projectile.ai[1] = 1f;
            }

            if (Projectile.ai[1] != 0f) {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.UltraBrightTorch, 0f, 0f, 100, default, 1f);
                d.noGravity = true;
                var d2 = Dust.CloneDust(d.dustIndex);
                d2.noGravity = true;
            }

            Projectile.Center = ownerMountedCenter;
            Projectile.oldDirection = player.direction; // 为后文作铺垫
            if (player.velocity.X != 0)
                player.ChangeDir(Math.Sign(player.velocity.X));
            // direction更改了，清空old数组
            if (Projectile.oldDirection != player.direction) {
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++) {
                    Projectile.oldPos[i] = Vector2.Zero;
                    Projectile.oldRot[i] = Projectile.rotation;
                }
            }
            Projectile.direction = player.direction;
            Projectile.timeLeft = 2;
            Projectile.QuickHeldProjBasic(player);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            hitDirection = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }

        public override bool? CanDamage() {
            return Projectile.ai[1] != 0f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            return Projectile.Distance(ModHelper.ClosestPointInHitbox(targetHitbox, Projectile.Center)) <= Projectile.width / 2;
        }

        public override bool? CanHitNPC(NPC target) {
            if (!target.noTileCollide && !Collision.CanHitLine(Projectile.Center, 0, 0, target.Center, 0, 0))
                return false;

            return base.CanHitNPC(target);
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            Rectangle rectangle = new(0, frameY, tex.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;

            var projColor = Projectile.GetAlpha(lightColor);
            float trailLength = 6; // 残影长度固定6

            if (Projectile.ai[1] != 0f) {
                for (int i = 0; i < trailLength; i++) {
                    float factor = Utils.GetLerpValue(trailLength - 1, 0, i);
                    Color alphaColor = projColor * 0.5f * factor;
                    float rotation = Projectile.oldRot[i];
                    Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), rectangle, alphaColor, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.BeginGameSpriteBatch(deferred: false, alphaBlend: false);

            var triangle = PrepareTriangleList(projColor, rectangle);
            DrawTriangle(triangle);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginGameSpriteBatch();

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle,
                projColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        private void DrawTriangle(List<VertexInfo> triangleList) {
            var screenCenter = Main.screenPosition + Main.ScreenSize.ToVector2() / 2f;
            var screenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
            var screenPos = screenCenter - screenSize / 2f;

            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            float speed = 10f; // 变换速度
            int milliSeconds = (int)Main.gameTimeCache.TotalGameTime.TotalMilliseconds;
            float uTime = milliSeconds % 10000f / 10000f * speed;

            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new() {
                CullMode = CullMode.None
            };
            Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            // 把变换和所需信息丢给shader
            var effect = ShaderManager.DirectVertex.Value;
            effect.Parameters["uTransform"].SetValue(model * projection);
            effect.Parameters["uTime"].SetValue(uTime);
            Main.instance.GraphicsDevice.Textures[0] = TextureManager.RainstormSpinTrail.Value;
            
            effect.CurrentTechnique.Passes[0].Apply();

            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
        }

        private List<VertexInfo> PrepareTriangleList(Color projColor, Rectangle rectangle) {
            List<VertexInfo> bars = new();

            float halfLength = rectangle.Length() * Projectile.scale / 2f;
            float trailLength = ProjectileID.Sets.TrailCacheLength[Type];
            var center = Projectile.Center + new Vector2(0f, Projectile.gfxOffY);

            float alphaMultipiler = Utils.GetLerpValue(0f, 40f, Projectile.ai[0], true) * 2f;

            for (int i = 0; i < trailLength; i++) {
                float factor = Utils.GetLerpValue(0, trailLength - 1, i); // 从 0-1 递增
                Color alphaColor = projColor * (1f - factor) * alphaMultipiler;
                alphaColor = alphaColor.MultiplyRGB(Color.SkyBlue);

                float rotation = Projectile.oldRot[i] - 0.785f; // 由于贴图是斜左上角的，所以这里要调一下-0.785f
                var rotVector = rotation.ToRotationVector2();

                bars.Add(new(center + rotVector * halfLength, alphaColor, new(factor, 0f, 0)));
                bars.Add(new(center + rotVector * halfLength * 0.4f, alphaColor, new(factor, 1f, 0)));
            }

            List<VertexInfo> triangleList = new();

            if (bars.Count > 2) {
                // 按照顺序连接三角形
                for (int i = 0; i < bars.Count - 2; i += 2) {
                    // 这是一个四边形 [i] [i+2] [i+1] [i+3]
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }
            }

            return triangleList;
        }
    }
}
