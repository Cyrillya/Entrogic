using Terraria.Audio;
using Terraria.Enums;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class SmeltDaggerProjectile : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
        }

        public override void SetDefaults() {
            Projectile.DefaultToWhip();
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.extraUpdates = 1;
        }

        private ref float Timer => ref Projectile.ai[0];
        private ref float Stage => ref Projectile.ai[1];

        /// <summary>
        /// 记录上一帧的重力方向，玩家翻转重力应清除刀光点
        /// </summary>
        internal float OldGravDir;
        /// <summary>
        /// 记录旋转的起始点，仅使用物品玩家端记录
        /// </summary>
        internal float StartRotation;

        public override void AI() {
            Player player = Main.player[Projectile.owner];

            float timeToAttackOut = player.itemAnimationMax * Projectile.MaxUpdates;
            // 使用占百分比的形式，是为了更好地适配攻速加成
            float readyTime = timeToAttackOut * 0.3f;
            float finalTime = timeToAttackOut * 0.3f;
            int startDegree = -120; // 挥剑起始角度
            int finalDegree = 100; // 挥剑终止角度
            int readyDegree = 10; // 准备阶段偏移角度
            int endDegree = 10; // 终止阶段缓速角度

            Timer++;
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));

            player.heldProj = Projectile.whoAmI;

            // Center绑定为玩家位置
            Projectile.Center = Main.GetPlayerArmPosition(Projectile);

            switch (Stage) {
                // ReadyTime - 准备阶段，武器向挥动反方向旋转准备
                case 0: {
                        float factor = Timer / readyTime;
                        // 初始化
                        if (Timer == 1f) {
                            ClearRotationArray(Projectile.oldRot);
                            StartRotation = Projectile.velocity.ToRotation() * player.direction;
                            // 在这里设置direction和scale是为了防止联机出现大小和方向不同步Bug
                            Projectile.scale = player.GetAdjustedItemScale(player.HeldItem);
                            player.direction = Math.Sign(Projectile.velocity.X);
                        }
                        float offsetDegree = MathHelper.Lerp(readyDegree, 0f, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(offsetDegree + startDegree);
                        // 叠攻速过快的话直接跳过这个阶段（所以10和10以上基本上是质的提升）
                        if (Timer >= readyTime || player.itemAnimationMax <= 10) {
                            Stage = 1;
                            Timer = 0;
                            ClearRotationArray(Projectile.oldRot);
                        }
                        player.itemAnimation = player.itemAnimationMax;
                        break;
                    }
                // 挥剑阶段
                case 1: {
                        // 挥剑实际时长
                        float swingTimer = timeToAttackOut - finalTime - readyTime;
                        // +1防止切换到最终阶段时旋转速度脱节导致刀光拉出去一块
                        float factor = Timer / (swingTimer + 1);
                        if (player.itemAnimationMax <= 10) {
                            // 攻速过快选择直接干烂最终阶段，所以这里不+1
                            factor = Timer / swingTimer;
                        }
                        float degree = MathHelper.Lerp(startDegree, finalDegree, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(degree);
                        if (Timer >= swingTimer) {
                            Stage = 2;
                            Timer = 0;
                            // 叠攻速过快的话直接Kill射弹进行下一次攻击（所以10和10以上基本上是质的提升）
                            if (player.itemAnimationMax <= 10) {
                                Projectile.Kill();
                            }
                        }
                        // 挥动音效
                        if (Timer == (int)(swingTimer / 2f) && Main.netMode != NetmodeID.Server) {
                            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, volumeScale: 2f, pitchOffset: 0.2f);
                        }
                        player.itemAnimation = (int)MathHelper.Lerp(player.itemAnimationMax, 3, factor);
                        break;
                    }
                // FinalTime - 最终阶段，武器缓速停下
                case 2: {
                        float factor = Timer / finalTime;
                        float offsetDegree = MathHelper.Lerp(0f, endDegree, factor);
                        Projectile.rotation = StartRotation + MathHelper.ToRadians(offsetDegree + finalDegree);
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

            // 加点粒子
            if (Stage == 1)
                for (float r = 0f; r <= 1f; r += 0.5f) { // 平滑角度
                    for (float i = 0.4f; i <= 1f; i += 0.2f) { // 向内延申粒子
                        if (Projectile.oldRot[0] == 114514 || Projectile.oldRot[1] == 114514) { // 无旋转角度
                            return;
                        }

                        int length = (int)(Projectile.Size.Length() * Projectile.scale * i);
                        float radiansPassed = Projectile.oldRot[0] - Projectile.oldRot[1];
                        float radians = radiansPassed * r + Projectile.oldRot[1];

                        var pos = Projectile.Center + radians.ToRotationVector2() * length;
                        var velocity = (radians + MathHelper.ToRadians(90f) * player.direction * player.gravDir).ToRotationVector2() * 18f;

                        int spawnRange = 20;
                        pos -= new Vector2(spawnRange) / 2f;

                        Dust d = Dust.NewDustDirect(pos, spawnRange, spawnRange, MyDustID.Fire, velocity.X, velocity.Y, 200, Scale: Main.rand.NextFloat(0.6f, 1.5f));
                        d.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);
                        d.noGravity = true;
                    }
                }

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

        internal static void DrawTrail(List<CustomVertexInfo> triangleList) {
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

            ResourceManager.BladeTrail.Value.Parameters["uTransform"].SetValue(model * projection);
            ResourceManager.BladeTrail.Value.Parameters["uTime"].SetValue(-(float)Main.gameTimeCache.TotalGameTime.TotalMilliseconds % 30000 * 0.003f);
            Main.instance.GraphicsDevice.Textures[0] = ResourceManager.Heatmap.Value;
            Main.instance.GraphicsDevice.Textures[1] = ResourceManager.BladeTrailShape1.Value;
            Main.instance.GraphicsDevice.Textures[2] = ResourceManager.BladeTrailErosion.Value;
            //Main.instance.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            //Main.instance.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
            //Main.instance.GraphicsDevice.Textures[2] = TextureAssets.MagicPixel.Value;
            Main.instance.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            Main.instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
            Main.instance.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;

            ResourceManager.BladeTrail.Value.CurrentTechnique.Passes[0].Apply();

            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.instance.GraphicsDevice.RasterizerState = originalState;
        }

        internal List<CustomVertexInfo> PrepareTriangleList(Rectangle sourceRect) {
            List<CustomVertexInfo> bars = new();
            List<float> drawRotations = new();

            int weaponLength = (int)(sourceRect.Size().Length() * Projectile.scale); // 到剑端的长度
            int weaponExLength = (int)(weaponLength * 0.3f); // 到剑身的长度，我可不想在剑柄画刀光

            // 通过插值平滑处理顶点
            for (int i = 1; i < Projectile.oldRot.Length; i++) {
                float radiansPassed = Projectile.oldRot[i] - Projectile.oldRot[i - 1];
                if (Projectile.oldRot[i] == 114514 || Projectile.oldRot[i - 1] == 114514) { // 无旋转角度
                    continue;
                }
                drawRotations.Add(Projectile.oldRot[i - 1]);
                drawRotations.Add(Projectile.oldRot[i - 1] + radiansPassed * .2f);
                drawRotations.Add(Projectile.oldRot[i - 1] + radiansPassed * .4f);
                drawRotations.Add(Projectile.oldRot[i - 1] + radiansPassed * .6f);
                drawRotations.Add(Projectile.oldRot[i - 1] + radiansPassed * .8f);
            }

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < drawRotations.Count; i++) {
                var factor = i / (float)drawRotations.Count; // 遍历时以0-1递增
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, (float)Math.Sqrt(factor)); // 把factor转换为1-0.05 （?） 这个是透明度

                var rotationVector = drawRotations[i].ToRotationVector2();
                bars.Add(new CustomVertexInfo(Projectile.Center + rotationVector * weaponLength, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(Projectile.Center + rotationVector * weaponExLength, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
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

        public override bool PreDraw(ref Color lightColor) {
            Player player = Main.player[Projectile.owner];

            // location绑定为玩家位置
            var position = (Projectile.Center - Main.screenPosition).Floor();

            var tex = TextureAssets.Projectile[Type].Value;
            var sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 origin = new((float)sourceRect.Width * 0.5f - (float)sourceRect.Width * 0.5f, (float)sourceRect.Height);

            float drawRotation = Projectile.oldRot[0];
            if (player.gravDir == -1) {
                drawRotation = 0f - drawRotation;
            }
            drawRotation += MathHelper.ToRadians(45f);

            if (Timer == 1f && Stage == 0) { // 前一帧先不绘制
                return false;
            }

            // Additive模式用于绘制刀光和刀体时更好的效果
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            var triangle = PrepareTriangleList(sourceRect);

            if (triangle.Count > 2) {
                DrawTrail(triangle);
                // 重启spriteBatch以重置shader
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Main.EntitySpriteDraw(tex, position, sourceRect, Color.OrangeRed, drawRotation, origin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition() {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire3, 180);
    }
}
