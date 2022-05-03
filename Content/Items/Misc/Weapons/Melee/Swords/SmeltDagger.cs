using Entrogic.Common.Globals.Players;
using Entrogic.Content.Items.Symbiosis;
using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;
using Terraria.UI;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class SmeltDagger : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("“新鲜出炉”\n" +
                "有可能会有点软化");
        }

        public override void SetDefaults() {
            Item.damage = 143;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.scale = 2.8f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = RarityLevelID.LatePHM;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        /// <summary>
        /// 决定此帧是否已经绘制过了，防止某些多玩家绘制效果搞坏
        /// </summary>
        internal bool DrawnThisFrame;
        /// <summary>
        /// 记录上一帧的重力方向，玩家翻转重力应清除刀光点
        /// </summary>
        internal float OldGravDir;
        /// <summary>
        /// 记录旋转的起始点，仅使用物品玩家端记录
        /// </summary>
        internal static float StartRotation;

        internal const int READY_TIME = 4;
        internal const int SLOW_TIME = 6;
        internal const int START_TIME = READY_TIME + SLOW_TIME;

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox) {
            // 玩家没有在挥刀
            if (player.itemAnimation < SLOW_TIME || player.itemAnimation > player.itemAnimationMax - START_TIME) {
                noHitbox = true;
                return;
            }

            float adjustedItemScale = player.GetAdjustedItemScale(Item);
            int centerLength = (int)(Item.Size.Length() * adjustedItemScale * 0.5f); // 到中间的长度

            var rotationVec = player.itemRotation.ToRotationVector2();
            var center = (rotationVec * centerLength + player.Center).ToPoint();
            var origin = (Item.Size * adjustedItemScale * .6f).ToPoint(); // 稍微比我们武器看起来大一点点

            hitbox = new Rectangle(center.X - origin.X, center.Y - origin.Y, (int)origin.X * 2, (int)origin.Y * 2);

            player.attackCD = 0;

            Lighting.AddLight(center.ToVector2(), TorchID.Torch);
        }

        public override void Load() {
            On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayerFull += LegacyPlayerRenderer_DrawPlayerFull;
            On.Terraria.DataStructures.PlayerDrawLayers.DrawPlayer_27_HeldItem += SmeltDaggerPatch;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame) {
            if (Main.netMode == NetmodeID.Server)
                return;

            // location绑定为玩家位置
            player.itemLocation = player.RotatedRelativePoint(player.MountedCenter, true);
            float offsetDirection = player.direction * player.gravDir;

            // 基础参数
            var daggerItem = player.HeldItem.ModItem as SmeltDagger;

            ref float[] Rotations = ref player.GetModPlayer<PlayerStats>().RotationsForSmeltDagger;
            ref float oldGravDir = ref daggerItem.OldGravDir;

            ref float rotation = ref player.itemRotation;
            // Lerp用值，范围[0-1]，挥剑时间为[player.itemAnimationMax-START_TIME ~ SLOW_TIME]
            float factor = (float)(player.itemAnimation - SLOW_TIME) / (float)(player.itemAnimationMax - START_TIME);
            // 在最后给它停几帧慢下来，所以5之后慢速模式
            if (player.itemAnimation <= SLOW_TIME) {
                factor = player.itemAnimation / SLOW_TIME;
            }
            // 设置旋转和起始，而其他端的玩家只要知道rotation就行
            if (player.whoAmI == Main.myPlayer) {
                if (player.itemAnimation == player.itemAnimationMax - 2) {
                    ClearRotationArray(Rotations);
                    var toCursorVector = player.itemLocation.DirectionTo(Main.MouseWorld);
                    StartRotation = toCursorVector.ToRotation();
                    player.direction = Math.Sign(toCursorVector.X);
                }
                rotation = StartRotation + MathHelper.ToRadians(MathHelper.Lerp(-110, 130, (float)Math.Pow(1 - factor, 3))) * offsetDirection;
                if (player.itemAnimation <= 6) {
                    rotation = StartRotation + MathHelper.ToRadians(MathHelper.Lerp(140, 150, 1 - factor)) * offsetDirection;
                }
                // 别管为啥这样写，写就完了
                rotation -= MathHelper.ToRadians(45f) * player.direction * player.gravDir;

                // 更新数组
                Rotations[0] = player.itemRotation;
                for (int i = Rotations.Length - 1; i >= 1; i--) {
                    Rotations[i] = Rotations[i - 1];
                }

                // 联机同步狗都不用，下次回来写成手持弹幕
                if (Main.netMode == NetmodeID.MultiplayerClient) {
                    ModNetHandler.PlayerStat.SendSmeltDaggerStats(-1, player.whoAmI, (byte)player.whoAmI, Rotations);
                }
            }

            // 切换重力时清空数组，防止前后连在一起了
            if (oldGravDir != player.gravDir) {
                ClearRotationArray(Rotations);
            }
            oldGravDir = player.gravDir;

            // 加点粒子
            float adjustedItemScale = player.GetAdjustedItemScale(Item);
            if (player.itemAnimation >= SLOW_TIME && player.itemAnimation <= player.itemAnimationMax - START_TIME - 4)
                for (float i = 0.4f; i <= 1.0f; i += 0.3f) {
                    int length = (int)(Item.Hitbox.Size().Length() * adjustedItemScale * i);
                    var pos = player.itemLocation + Rotations[0].ToRotationVector2() * length;
                    var velocity = (Rotations[0] + MathHelper.ToRadians(90f) * offsetDirection).ToRotationVector2() * 18f;
                    Dust d = Dust.NewDustDirect(pos, 20, 20, MyDustID.Fire, velocity.X, velocity.Y, 200, Scale: Main.rand.NextFloat(0.6f, 1.5f));
                    d.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);
                    d.noGravity = true;
                }

            if (player.itemAnimation >= player.itemAnimationMax - 2) { // 前一帧先不绘制
                ClearRotationArray(Rotations);
                return;
            }
        }

        // 在这里重置DrawnThisFrame
        private void LegacyPlayerRenderer_DrawPlayerFull(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayerFull orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, Player player) {
            orig.Invoke(self, camera, player);

            Item heldItem = player?.HeldItem;

            if (heldItem is null || heldItem.type != Type || heldItem.ModItem is not SmeltDagger || !player.CanVisuallyHoldItem(heldItem) || player.frozen || player.itemAnimation == 0 ||
                player.dead || !player.active)
                return;

            (player.HeldItem.ModItem as SmeltDagger).DrawnThisFrame = false;
        }

        private void SmeltDaggerPatch(On.Terraria.DataStructures.PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawinfo) {
            Item heldItem = drawinfo.heldItem;
            var player = drawinfo.drawPlayer;
            if (heldItem.type != Type || heldItem.ModItem is not SmeltDagger || !player.CanVisuallyHoldItem(heldItem) || player.frozen || player.itemAnimation == 0 ||
                player.dead || !player.active) {
                orig.Invoke(ref drawinfo);
            }
            else if (!(player.HeldItem.ModItem as SmeltDagger).DrawnThisFrame) {
                Main.instance.LoadItem(heldItem.type);

                // location绑定为玩家位置
                var position = drawinfo.ItemLocation - Main.screenPosition;
                // 基础参数
                var daggerItem = player.HeldItem.ModItem as SmeltDagger;

                daggerItem.DrawnThisFrame = true;
                float[] rotations = player.GetModPlayer<PlayerStats>().RotationsForSmeltDagger;

                var tex = TextureAssets.Item[heldItem.type].Value;
                float adjustedItemScale = player.GetAdjustedItemScale(heldItem);
                var sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
                Vector2 origin = new((float)sourceRect.Width * 0.5f - (float)sourceRect.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)sourceRect.Height);
                if (drawinfo.drawPlayer.gravDir == -1f) {
                    origin.Y = (float)sourceRect.Height - origin.Y;
                }

                drawinfo.itemColor = Lighting.GetColor(player.Center.ToTileCoordinates());
                ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem, false);
                float drawRotation = rotations[0];
                if (player.direction == -1) {
                    drawRotation = rotations[0] - 3.14f;
                }
                drawRotation += MathHelper.ToRadians(45f) * player.direction * player.gravDir;

                var triangle = PrepareTriangleList(drawinfo, sourceRect, adjustedItemScale, rotations);

                if (player.itemAnimation >= player.itemAnimationMax - 2) { // 前一帧先不绘制
                    return;
                }

                if (triangle.Count > 2) {
                    DrawTrail(triangle);
                }

                DrawData item = new(tex, position, sourceRect, Color.White, drawRotation, origin, adjustedItemScale, drawinfo.itemEffect, 0);
                drawinfo.DrawDataCache.Add(item);
            }
        }

        internal void ClearRotationArray(float[] array) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = 114514; // 114514用来区分正常旋转角度和无旋转角度
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
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        internal List<CustomVertexInfo> PrepareTriangleList(PlayerDrawSet drawinfo, Rectangle sourceRect, float scale, float[] rotations) {
            List<CustomVertexInfo> bars = new();

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < rotations.Length; i++) {
                if (rotations[i] == 114514) { // 无旋转角度
                    continue;
                }

                int weaponLength = (int)(sourceRect.Size().Length() * scale); // 到剑端的长度
                int weaponExLength = (int)(weaponLength * 0.3f); // 到剑身的长度，我可不想在剑柄画刀光

                var factor = i / (float)rotations.Length; // 遍历时以0-1递增
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor); // 把factor转换为1-0.05 （?） 这个是透明度

                bars.Add(new CustomVertexInfo(drawinfo.ItemLocation + rotations[i].ToRotationVector2() * weaponLength, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(drawinfo.ItemLocation + rotations[i].ToRotationVector2() * weaponExLength, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
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

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ModContent.ItemType<GelOfLife>(), 7)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire3, 180);
    }
}