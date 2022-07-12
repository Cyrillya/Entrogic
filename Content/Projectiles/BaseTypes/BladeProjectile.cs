using Terraria.Audio;
using Terraria.Enums;

namespace Entrogic.Content.Projectiles.BaseTypes
{
    public abstract class BladeProjectile : ProjectileBase
    {
        public override void SetStaticDefaults() {
            // 不使用原版的记录了
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 36;
        }

        public override void SetDefaults() {
            Projectile.DefaultToWhip();
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.extraUpdates = 3;

            int length = ProjectileID.Sets.TrailCacheLength[Type];
            if (length != oldScale.Length) {
                Array.Resize(ref oldScale, length);
            }
        }

        public ref float Timer => ref Projectile.ai[0];
        public ref float Stage => ref Projectile.ai[1];

        public float[] oldScale = new float[10];
        /// <summary>
        /// 记录上一帧的重力方向，玩家翻转重力应清除刀光点
        /// </summary>
        internal float OldGravDir;
        /// <summary>
        /// 记录旋转的起始点，仅使用物品玩家端记录
        /// </summary>
        internal float StartRotation;

        /// <summary>
        /// 挥剑起始角度
        /// </summary>
        public int StartDegree = -120;
        /// <summary>
        /// 挥剑终止角度
        /// </summary>
        public int FinalDegree = 100;
        /// <summary>
        /// 准备阶段偏移角度
        /// </summary>
        public int ReadyDegree = 10;
        /// <summary>
        /// 终止阶段缓速角度
        /// </summary>
        public int EndDegree = 10;
        /// <summary>
        /// 准备阶段时间在总使用时间中的占比
        /// </summary>
        public float ReadyTimePercent = 0.3f;
        /// <summary>
        /// 终止阶段时间在总使用时间中的占比
        /// </summary>
        public float FinalTimePercent = 0.3f;
        /// <summary>
        /// 刀光主颜色
        /// </summary>
        public Color TrailColor = Color.White;
        /// <summary>
        /// 原大小（即武器受加成后应有大小）
        /// </summary>
        public float OriginalScale = 1f;

        public virtual void PreSwingAI() { }
        public virtual void PostSwingAI() { }

        public override void AI() {
            Player player = Main.player[Projectile.owner];

            float timeToAttackOut = player.itemAnimationMax * Projectile.MaxUpdates;
            // 使用占百分比的形式，是为了更好地适配攻速加成
            float readyTime = timeToAttackOut * ReadyTimePercent;
            float finalTime = timeToAttackOut * FinalTimePercent;
            // 挥剑实际时长
            float swingTimer = timeToAttackOut - finalTime - readyTime;

            Timer++;
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));

            player.heldProj = Projectile.whoAmI;

            // Center绑定为玩家位置
            Projectile.Center = Main.GetPlayerArmPosition(Projectile);

            PreSwingAI();

            switch (Stage) {
                // ReadyTime - 准备阶段，武器向挥动反方向旋转准备
                case 0: {
                        float factor = Timer / readyTime;
                        // 初始化
                        if (Timer == 1f) {
                            ClearRotationArray(Projectile.oldRot);
                            StartRotation = Projectile.velocity.ToRotation() * player.direction + player.fullRotation;
                            // 在这里设置direction和scale是为了防止联机出现大小和方向不同步Bug
                            OriginalScale = player.GetAdjustedItemScale(player.HeldItem);
                        }
                        float offsetDegree = MathHelper.Lerp(ReadyDegree, 0f, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(offsetDegree + StartDegree);
                        Projectile.scale = MathHelper.Lerp(OriginalScale / 1.2f, OriginalScale, factor * 2f);
                        Projectile.scale = MathHelper.Clamp(Projectile.scale, OriginalScale / 1.2f, OriginalScale);
                        if (Timer >= readyTime) {
                            Stage = 1;
                            Timer = 0;
                            InitScaleArray(oldScale);
                        }
                        player.itemAnimation = player.itemAnimationMax;
                        break;
                    }
                // 挥剑阶段
                case 1: {
                        // +1防止切换到最终阶段时旋转速度脱节导致刀光拉出去一块
                        float factor = Timer / (swingTimer + 1);
                        float degree = MathHelper.Lerp(StartDegree, FinalDegree, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(degree);
                        Projectile.scale = OriginalScale;
                        if (Timer >= swingTimer) {
                            Stage = 2;
                            Timer = 0;
                        }
                        // 挥动音效
                        if (Timer == (int)(swingTimer / 2f) && Main.netMode != NetmodeID.Server) {
                            SoundEngine.PlaySound(SoundID.Item1.WithVolumeScale(2f).WithPitchOffset(0.2f), Projectile.Center);
                        }
                        player.itemAnimation = (int)MathHelper.Lerp(player.itemAnimationMax, 3, factor);
                        break;
                    }
                // FinalTime - 最终阶段，武器缓速停下
                case 2: {
                        float factor = Timer / finalTime;
                        float offsetDegree = MathHelper.Lerp(0f, EndDegree, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(offsetDegree + FinalDegree);
                        // 叠攻速过快的话只运行两帧
                        if (Timer >= finalTime) {
                            Projectile.Kill();
                            player.itemAnimation = 0;
                        }
                        player.itemAnimation = 2;
                        break;
                    }
            }

            // 一些处理
            player.itemTime = player.itemAnimation;
            Projectile.rotation *= player.direction;
            FlipCenterAndRotationForGravity(player);

            // 切换重力时清空数组，防止前后连在一起了
            if (OldGravDir != player.gravDir) {
                ClearRotationArray(Projectile.oldRot);
            }
            OldGravDir = player.gravDir;

            PostSwingAI();

            UpdateOldArray();
        }

        public virtual void UpdateOldArray() {
            RefreshOldArray();
            if (Stage == 1 || (Stage == 2 && Timer <= 1)) {
                RecordToArray(Projectile.position, Projectile.rotation, Projectile.scale);
            }
        }

        public void RefreshOldArray() {
            for (int i = Projectile.oldPos.Length - 1; i > 0; i--) {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
                oldScale[i] = oldScale[i - 1];
            }
        }

        public void RecordToArray(Vector2 pos, float rotation, float scale) {
            Projectile.oldPos[0] = pos;
            Projectile.oldRot[0] = rotation;
            oldScale[0] = scale;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            if (Stage == 0) {
                return false;
            }

            for (float r = 0f; r <= 1f; r += 0.5f) { // 平滑角度
                if (Projectile.oldRot[0] == 114514 || Projectile.oldRot[1] == 114514) { // 无旋转角度
                    return false;
                }

                float radiansPassed = Projectile.oldRot[0] - Projectile.oldRot[1];
                float radians = radiansPassed * r + Projectile.oldRot[1];
                int length = (int)(Projectile.Size.Length() * Projectile.scale);
                float point = 0f;
                bool canCollide = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + radians.ToRotationVector2() * length, Projectile.width * Projectile.scale, ref point);

                // 如果这碰撞了，直接为true，否则进行下一个判定
                if (canCollide) {
                    return true;
                }
            }

            return false;
        }

        public override void CutTiles() {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            int length = (int)(Projectile.Size.Length() * Projectile.scale);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * length,
                (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }

        public void FlipCenterAndRotationForGravity(Player player) {
            if (player.gravDir == -1f) {
                Projectile.rotation = 0f - Projectile.rotation;
                float num = player.position.Y - Projectile.Center.Y;
                Projectile.Center = new Vector2(Projectile.Center.X, player.Bottom.Y + num);
            }
        }

        internal static void ClearRotationArray(float[] array) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = 114514; // 114514用来区分正常旋转角度和无旋转角度
            }
        }

        internal void InitScaleArray(float[] array) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = Projectile.scale;
            }
        }

        public virtual void SelectTrailTextures(out Texture2D mainColor, out Texture2D mainShape, out Texture2D maskColor) {
            mainColor = TextureAssets.MagicPixel.Value;
            mainShape = ResourceManager.BladeTrailShape1.Value;
            maskColor = ResourceManager.BladeTrailErosion.Value;
        }

        internal void DrawTrail(List<CustomVertexInfo> triangleList) {
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + Main.ScreenSize.ToVector2() / 2f;
            var screenSize = Main.ScreenSize.ToVector2() / Main.GameViewMatrix.Zoom;
            if (Main.LocalPlayer.gravDir == -1) {
                screenSize.Y = -screenSize.Y;
            }
            var screenPos = screenCenter - screenSize / 2f;

            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader

            ResourceManager.BladeTrail.Value.Parameters["uDetectMode"].SetValue(0);
            ResourceManager.BladeTrail.Value.Parameters["uTransform"].SetValue(model * projection);
            ResourceManager.BladeTrail.Value.Parameters["uTime"].SetValue(-(float)Main.gameTimeCache.TotalGameTime.TotalMilliseconds % 30000 * 0.003f);
            SelectTrailTextures(out Texture2D mainColor, out Texture2D mainShape, out Texture2D maskColor);
            Main.instance.GraphicsDevice.Textures[0] = mainColor;
            Main.instance.GraphicsDevice.Textures[1] = mainShape;
            Main.instance.GraphicsDevice.Textures[2] = maskColor;
            Main.instance.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicWrap;
            Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicWrap;

            ResourceManager.BladeTrail.Value.CurrentTechnique.Passes[0].Apply();

            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.instance.GraphicsDevice.RasterizerState = originalState;
        }

        public static void SmoothArray(out List<float> smoothedList, float[] originalArray, float ignoreValue = 114514, float smoothStep = .33f) {
            smoothedList = new();
            for (int i = 1; i < originalArray.Length; i++) {
                float valuePassed = originalArray[i] - originalArray[i - 1];
                if (originalArray[i] == ignoreValue || originalArray[i - 1] == ignoreValue) { // 无旋转角度
                    continue;
                }
                for (float s = 0; s < 0.9f; s += smoothStep) {
                    smoothedList.Add(originalArray[i - 1] + valuePassed * s);
                }
            }
        }

        internal List<CustomVertexInfo> PrepareTriangleList(Rectangle sourceRect) {
            List<CustomVertexInfo> bars = new();

            // 平滑处理
            SmoothArray(out List<float> drawRotations, Projectile.oldRot);
            SmoothArray(out List<float> drawScales, oldScale); // 暂时弃用

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < drawRotations.Count; i++) {
                var factor = i / (float)drawRotations.Count; // 遍历时以0-1递增
                var color = Color.Lerp(Color.White, TrailColor, factor);
                var lerpValue = (float)Math.Pow(factor, 2);
                var w = MathHelper.Lerp(1f, 0.05f, lerpValue); // 基础透明度插值

                int weaponLength = (int)(sourceRect.Size().Length() * Projectile.scale); // 到剑端的长度
                int weaponExLength = (int)(weaponLength * 0.35f); // 到剑身的长度，我可不想在剑柄画刀光

                var rotationVector = drawRotations[i].ToRotationVector2();
                bars.Add(new CustomVertexInfo(Projectile.Center + rotationVector * weaponLength, color, new Vector3(factor, 1, w)));
                bars.Add(new CustomVertexInfo(Projectile.Center + rotationVector * weaponExLength, color, new Vector3(factor, 0, w)));

                // 显示三角形所有的点
                //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center + rotationVector * weaponLength - Main.screenPosition,
                //    new Rectangle?(new Rectangle(0, 0, 4, 4)), Color.White);
            }

            List<CustomVertexInfo> triangleList = new();

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

        public virtual void GetBladeDrawStats(ref Color lightColor, out Color bladeColor, out BlendState blendState) {
            bladeColor = Color.White;
            blendState = BlendState.AlphaBlend;
        }

        public override bool PreDraw(ref Color lightColor) {
            Player player = Main.player[Projectile.owner];

            // location绑定为玩家位置
            var position = (Projectile.Center - Main.screenPosition).Floor();

            var tex = TextureAssets.Projectile[Type].Value;
            var sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 origin = new((float)sourceRect.Width * 0.5f - (float)sourceRect.Width * 0.5f, (float)sourceRect.Height);

            float drawRotation = Projectile.rotation;
            if (player.gravDir == -1) {
                drawRotation = 0f - drawRotation;
            }
            drawRotation += MathHelper.ToRadians(45f);

            if (Timer == 1f && Stage == 0) { // 前一帧先不绘制
                return false;
            }

            GetBladeDrawStats(ref lightColor, out Color bladeColor, out BlendState blendState);

            var triangle = PrepareTriangleList(sourceRect);

            if (triangle.Count > 2) {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                DrawTrail(triangle);
                // 重启spriteBatch以重置shader
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendState, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.EntitySpriteDraw(tex, position, sourceRect, bladeColor, drawRotation, origin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition() {
            return false;
        }
    }
}
