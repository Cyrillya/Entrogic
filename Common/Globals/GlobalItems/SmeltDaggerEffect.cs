using Entrogic.Content.Items.Misc.Weapons.Melee.Swords;
using Terraria.UI;

namespace Entrogic.Common.Globals.GlobalItems
{
    internal class SmeltDaggerEffect : GlobalItem
    {
        internal static int SmeltDaggerType => ModContent.ItemType<SmeltDagger>();
        /// <summary>
        /// 记录上一帧的重力方向，玩家翻转重力应清除刀光点
        /// </summary>
        internal static float OldGravDir;
        /// <summary>
        /// 记录旋转的起始点，仅使用物品玩家端记录
        /// </summary>
        internal static float StartRotation;
        /// <summary>
        /// 记录上几帧旋转角度，全端记录
        /// </summary>
        internal static float[] Rotations = new float[10];

        internal const int READY_TIME = 4;
        internal const int SLOW_TIME = 6;
        internal const int START_TIME = READY_TIME + SLOW_TIME;

        public override void Load() {
            On.Terraria.DataStructures.PlayerDrawLayers.DrawPlayer_27_HeldItem += SmeltDaggerPatch;
        }

        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox) {
            base.UseItemHitbox(item, player, ref hitbox, ref noHitbox);
        }

        private void SmeltDaggerPatch(On.Terraria.DataStructures.PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawinfo) {
            Item heldItem = drawinfo.heldItem;
            var player = drawinfo.drawPlayer;
            if (heldItem.type != SmeltDaggerType || !player.CanVisuallyHoldItem(heldItem) || player.frozen || player.itemAnimation == 0 ||
                player.dead || !player.active) {
                orig.Invoke(ref drawinfo);
                ClearRotationArray();
            }
            else {
                Main.instance.LoadItem(heldItem.type);

                // location绑定为玩家位置
                drawinfo.ItemLocation = player.RotatedRelativePoint(player.MountedCenter, true);
                var position = drawinfo.ItemLocation - Main.screenPosition;
                float offsetDirection = player.direction * player.gravDir;

                var tex = TextureAssets.Item[heldItem.type].Value;
                float adjustedItemScale = player.GetAdjustedItemScale(heldItem);
                var sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
                Vector2 origin = new((float)sourceRect.Width * 0.5f - (float)sourceRect.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)sourceRect.Height);
                if (drawinfo.drawPlayer.gravDir == -1f) {
                    origin.Y = (float)sourceRect.Height - origin.Y;
                }

                ref float rotation = ref drawinfo.drawPlayer.itemRotation;
                float factor = (float)(player.itemAnimation - SLOW_TIME) / (float)(player.itemAnimationMax - START_TIME);
                // 在最后给它停几帧慢下来，所以5之后慢速模式
                if (player.itemAnimation <= SLOW_TIME) {
                    factor = player.itemAnimation / SLOW_TIME;
                }
                if (player.whoAmI == Main.myPlayer) {
                    if (player.itemAnimation == player.itemAnimationMax - 2) {
                        var toCursorVector = drawinfo.ItemLocation.DirectionTo(Main.MouseWorld);
                        StartRotation = toCursorVector.ToRotation() ;
                        player.direction = Math.Sign(toCursorVector.X);
                    }
                    rotation = StartRotation + MathHelper.ToRadians(MathHelper.Lerp(-110, 130, (float)Math.Pow(1 - factor, 3))) * offsetDirection;
                    if (player.itemAnimation <= 6) {
                        rotation = StartRotation + MathHelper.ToRadians(MathHelper.Lerp(140, 150, 1 - factor)) * offsetDirection;
                    }
                }

                drawinfo.itemColor = Lighting.GetColor(player.Center.ToTileCoordinates());
                ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem, false);
                float drawRotation = rotation;
                if (player.direction == -1) {
                    drawRotation = rotation - 3.14f;
                }

                if (!Main.gamePaused) {
                    // 别管为啥这样写，写就完了
                    Rotations[0] = rotation + MathHelper.ToRadians(45f) * -player.direction * player.gravDir;
                    for (int i = Rotations.Length - 1; i >= 1; i--) {
                        Rotations[i] = Rotations[i - 1];
                    }
                }

                if (OldGravDir != player.gravDir) {
                    ClearRotationArray();
                }
                OldGravDir = player.gravDir;

                var triangle = PrepareTriangleList(drawinfo, sourceRect, adjustedItemScale);

                if (player.itemAnimation >= player.itemAnimationMax - 2) { // 前一帧先不绘制
                    ClearRotationArray();
                    return;
                }

                if (triangle.Count > 2) {
                    DrawTrail(triangle);
                }

                DrawData item = new(tex, position, sourceRect, Color.White, drawRotation, origin, adjustedItemScale, drawinfo.itemEffect, 0);
                drawinfo.DrawDataCache.Add(item);

                // 加点粒子
                if (player.itemAnimation >= SLOW_TIME && player.itemAnimation <= player.itemAnimationMax - START_TIME - 4)
                    for (float i = 0.4f; i <= 1.0f; i += 0.2f) {
                        int length = (int)(sourceRect.Size().Length() * adjustedItemScale * i);
                        var pos = drawinfo.ItemLocation + Rotations[0].ToRotationVector2() * length;
                        var velocity = (Rotations[0] + MathHelper.ToRadians(90f) * offsetDirection).ToRotationVector2() * 18f;
                        Dust d = Dust.NewDustDirect(pos, 20, 20, MyDustID.Fire, velocity.X, velocity.Y, 200, Scale: Main.rand.NextFloat(0.6f, 1.5f));
                        d.fadeIn = Main.rand.NextFloat(0.8f, 1.3f);
                        d.noGravity = true;
                    }
            }
        }

        internal void ClearRotationArray() {
            for (int i = 0; i < Rotations.Length; i++) {
                Rotations[i] = 114514; // 114514用来区分正常旋转角度和无旋转角度
            }
        }

        internal void DrawTrail(List<CustomVertexInfo> triangleList) {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
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

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        internal List<CustomVertexInfo> PrepareTriangleList(PlayerDrawSet drawinfo, Rectangle sourceRect, float scale) {
            List<CustomVertexInfo> bars = new();

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < Rotations.Length; i++) {
                if (Rotations[i] == 114514) { // 无旋转角度
                    continue;
                }

                int weaponLength = (int)(sourceRect.Size().Length() * scale); // 到剑端的长度
                int weaponExLength = (int)(weaponLength * 0.3f); // 到剑身的长度，我可不想在剑柄画刀光

                var factor = i / (float)Rotations.Length; // 遍历时以0-1递增
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor); // 把factor转换为1-0.05 （?） 这个是透明度

                bars.Add(new CustomVertexInfo(drawinfo.ItemLocation + Rotations[i].ToRotationVector2() * weaponLength, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(drawinfo.ItemLocation + Rotations[i].ToRotationVector2() * weaponExLength, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
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
    }
}
