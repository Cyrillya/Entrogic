using Entrogic.Common.WorldGeneration;
using Entrogic.Content.NPCs.Enemies.Athanasy;
using Terraria.Audio;
using Terraria.Graphics.Shaders;

namespace Entrogic.Common.ModSystems
{
    public class AthanasySystem : ModSystem
    {
        internal static bool RunningApplyEquipFunctional;
        internal static CoroutineRunner TileRunner = new();

        public override void Load() {
            if (Main.dedServ)
                return;

            // Thanks for Starlight River!
            On.Terraria.Main.SetDisplayMode += RefreshCutawayTarget;
            On.Terraria.Main.DrawInfernoRings += DrawNegative;
            On.Terraria.Main.CheckMonoliths += DrawCutawayTarget;

            Main.QueueMainThreadAction(() => {
                cutawayTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, default, default, default, RenderTargetUsage.PreserveContents);
            });

            On.Terraria.Player.ApplyEquipFunctional += ApplyEquipFunctionalNerf;
            On.Terraria.Lighting.AddLight_int_int_float_float_float += NerfEquipmentLight;
        }

        private static float opacity = 1f;
        internal static RenderTarget2D cutawayTarget;

        public override void Unload() {
            cutawayTarget = null;
        }

        private void RefreshCutawayTarget(On.Terraria.Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen) {
            if (!Main.gameInactive && (width != Main.screenWidth || height != Main.screenHeight))
                cutawayTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, default, default, default, RenderTargetUsage.PreserveContents);

            orig(width, height, fullscreen);
        }

        private void DrawNegative(On.Terraria.Main.orig_DrawInfernoRings orig, Main self) {
            orig(self);

            if (opacity < 0.95f) {
                var effect = ShaderManager.Negative.Value;

                if (effect is null)
                    return;

                effect.Parameters["sampleTexture"].SetValue(cutawayTarget);
                effect.Parameters["uColor"].SetValue(Color.Black.ToVector3());
                effect.Parameters["opacity"].SetValue(1 - opacity);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, Main.GameViewMatrix.ZoomMatrix);

                Main.spriteBatch.Draw(cutawayTarget, Vector2.Zero, Color.White);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

                for (int i = 0; i < Main.maxNPCs; i++) {
                    if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<Athanasy>()) {
                        Main.instance.DrawNPC(i, false); // 带上石像
                    }
                }

                var rect = ImmortalGolemRoom.BossZone;
                rect.Offset(6, 6);
                rect = rect.OffsetSize(-12, -12);
                rect = rect.ToWorldCoordinates();
                ArmorShaderData armorShaderData = null;
                for (int i = 0; i < Main.maxDustToDraw; i++) {
                    Dust dust = Main.dust[i];
                    if (!dust.active || dust.type != MyDustID.BlueWhiteBubble || new Rectangle((int)dust.position.X, (int)dust.position.Y, 4, 4).Intersects(rect))
                        continue;
                    float scale = dust.GetVisualScale();
                    if (dust.shader != armorShaderData) {
                        Main.spriteBatch.End();
                        armorShaderData = dust.shader;
                        if (armorShaderData == null) {
                            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                        }
                        else {
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                            dust.shader.Apply(null);
                        }
                    }

                    Color newColor = Lighting.GetColor((int)((double)dust.position.X + 4.0) / 16, (int)((double)dust.position.Y + 4.0) / 16);
                    newColor = dust.GetAlpha(newColor);

                    Main.spriteBatch.Draw(TextureAssets.Dust.Value, dust.position - Main.screenPosition, dust.frame, newColor, dust.GetVisualRotation(), new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
                    if (dust.color.PackedValue != 0) {
                        Color color6 = dust.GetColor(newColor);
                        if (color6.PackedValue != 0)
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, dust.position - Main.screenPosition, dust.frame, color6, dust.GetVisualRotation(), new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        private void DrawCutawayTarget(On.Terraria.Main.orig_CheckMonoliths orig) {
            if (TextureManager.IsLoaded) {
                Main.spriteBatch.Begin();

                Main.graphics.GraphicsDevice.SetRenderTarget(cutawayTarget);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                var rect = ImmortalGolemRoom.BossZone.ToWorldCoordinates();
                var tex = TextureManager.Miscellaneous["AthanasyBossRoomCutaway"].Value;
                var position = rect.TopLeft() - Main.screenPosition + new Vector2(6f, 6f) * 16f;
                Main.spriteBatch.Draw(tex, position, Color.White);

                Main.spriteBatch.End();
                Main.graphics.GraphicsDevice.SetRenderTarget(null);
            }

            orig();
        }

        internal static IEnumerator ActuateSlopedPlatforms(bool isActuated) {
            for (int i = ImmortalGolemRoom.BossZone.Left; i < ImmortalGolemRoom.BossZone.Right; i++) {
                for (int j = ImmortalGolemRoom.BossZone.Top; j < ImmortalGolemRoom.BossZone.Bottom; j++) {
                    int type = Main.tile[i, j].TileType;
                    if (Main.tileSolidTop[type] && Main.tileSolid[type] && Main.tile[i, j].BlockType != BlockType.Solid && Main.tile[i - 1, j].TileType != type) {
                        Main.tile[i, j].Get<TileWallWireStateData>().IsActuated = isActuated;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendTileSquare(-1, i, j);
                        }
                    }
                }
                yield return 0;
            }
        }

        internal static void ActuatePlatforms(bool isActuated, int level) {
            int cenX = ImmortalGolemRoom.BossZone.Center.X;
            int l = 0;
            int i;
            for (i = ImmortalGolemRoom.BossZone.Top; i < ImmortalGolemRoom.BossZone.Bottom; i++) {
                int type = Main.tile[cenX, i].TileType;
                if (Main.tileSolidTop[type] && Main.tileSolid[type]) {
                    l++;
                }
                if (l == level) {
                    break;
                }
            }
            Main.tile[cenX, i].Get<TileWallWireStateData>().IsActuated = isActuated;
            ActuatePlatform(cenX - 1, i, isActuated, -1);
            ActuatePlatform(cenX + 1, i, isActuated, 1);
        }

        internal static void ActuatePlatform(int i, int j, bool isActuated, int direction) {
            if (!WorldGen.InWorld(i, j))
                return;
            int type = Main.tile[i, j].TileType;
            if (Main.tileSolidTop[type] && Main.tileSolid[type]) {
                if (!Main.tile[i, j - 1].HasTile)
                    Main.tile[i, j].Get<TileWallWireStateData>().IsActuated = isActuated;
                ActuatePlatform(i + direction, j, isActuated, direction);
            }
        }

        internal static IEnumerator ActuateTiles(bool isActuated, int minY, int maxY) {
            for (int i = ImmortalGolemRoom.BossZone.Left; i < ImmortalGolemRoom.BossZone.Right; i++) {
                for (int j = ImmortalGolemRoom.BossZone.Top + minY; j < ImmortalGolemRoom.BossZone.Top + maxY; j++) {
                    int type = Main.tile[i, j].TileType;
                    if (Main.tileSolid[type]) {
                        Main.tile[i, j].Get<TileWallWireStateData>().IsActuated = isActuated;
                        if (Main.netMode == NetmodeID.Server) {
                            NetMessage.SendTileSquare(-1, i, j);
                        }
                    }
                }
                yield return 0;
            }
        }

        internal bool HasAthanasy = false;

        public override void PostUpdateNPCs() {
            bool hasAthanasy = NPC.AnyNPCs(ModContent.NPCType<Athanasy>());
            if (!hasAthanasy && HasAthanasy && Main.netMode != NetmodeID.MultiplayerClient) { // 不知道哪去了
                TileRunner.Run(ActuateTiles(false, 10, 90));

                for (int i = ImmortalGolemRoom.BossZone.Left; i < ImmortalGolemRoom.BossZone.Right; i++) {
                    for (int j = ImmortalGolemRoom.BossZone.Top + 10; j < ImmortalGolemRoom.BossZone.Top + 90; j++) {
                        if (!WorldGen.InWorld(i, j))
                            return;

                        Tile t = Main.tile[i, j];
                        // 取篝火右上角
                        if (t.TileType == TileID.Campfire && t.TileFrameY == 36 && t.TileFrameX % 54 == 0) {
                            for (int k = 0; k < 3; k++) {
                                for (int l = 0; l < 2; l++) {
                                    Main.tile[i + k, j + l].TileFrameY = (short)(18 * l);
                                }
                            }
                            if (Main.netMode == NetmodeID.Server) {
                                NetMessage.SendTileSquare(-1, i, j, 3, 2);
                            }
                        }
                    }
                }
            }
            HasAthanasy = hasAthanasy;
            TileRunner.Update(1);
        }

        public override void ModifyLightingBrightness(ref float scale) {
            var rect = ImmortalGolemRoom.BossZone;
            rect.Offset(6, 6);
            rect = rect.OffsetSize(-12, -12);
            rect = rect.ToWorldCoordinates();
            if (rect.Contains(Main.LocalPlayer.Center.ToPoint()))
                opacity -= 0.1f;
            else
                opacity += 0.1f;
            opacity = MathHelper.Clamp(opacity, 0f, 1f);
        }

        private void NerfEquipmentLight(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float r, float g, float b) {
            if (RunningApplyEquipFunctional) {
                r *= 0.6f;
                g *= 0.6f;
                b *= 0.6f;
            }
            orig.Invoke(i, j, r, g, b);
        }

        private void ApplyEquipFunctionalNerf(On.Terraria.Player.orig_ApplyEquipFunctional orig, Player self, Item currentItem, bool hideVisual) {
            RunningApplyEquipFunctional = true;
            if (!NPC.AnyNPCs(ModContent.NPCType<Athanasy>()) || !ImmortalGolemRoom.BossZone.ToWorldCoordinates().Contains((int)self.Center.X, (int)self.Center.Y))
                RunningApplyEquipFunctional = false;
            orig.Invoke(self, currentItem, hideVisual);
            RunningApplyEquipFunctional = false;
        }

        public List<int> playerNearby = new();
        public override void PostUpdatePlayers() {
            int athanasyType = ModContent.NPCType<Athanasy>();

            if (!NPC.AnyNPCs(athanasyType) || Main.netMode == NetmodeID.MultiplayerClient) {
                playerNearby.Clear();
                return;
            }

            var oldPlayerNearby = playerNearby.ToArray();
            playerNearby.Clear();
            // 记录新加入的nearby，以进行关火操作
            List<int> newlyAdded = new();
            // 设置nearby
            for (int p = 0; p < Main.maxPlayers; p++) {
                if (Main.player[p] is null || Main.player[p].dead || !Main.player[p].active) {
                    continue;
                }
                var player = Main.player[p];
                var bossZoneRect = ImmortalGolemRoom.BossZone.ToWorldCoordinates();
                if (!bossZoneRect.Contains((int)player.Center.X, (int)player.Center.Y)) {
                    continue;
                }

                if (Main.getGoodWorld)
                    player.AddBuff(BuffID.Darkness, 1);

                var playerCenter = player.Center.ToTileCoordinates16();

                int range = 12;
                int n = NPC.FindFirstNPC(athanasyType);
                var npc = Main.npc[n];
                if (npc.active && npc.ModNPC is not null && npc.ModNPC is Athanasy && (npc.ModNPC as Athanasy).Stage == 1) {
                    range = 8;
                }

                for (int i = -range; i <= range; i++) {
                    for (int j = -range; j <= range; j++) {
                        int x = playerCenter.X + i;
                        int y = playerCenter.Y + j;
                        if (!WorldGen.InWorld(x, y))
                            return;

                        Tile t = Main.tile[x, y];
                        // 取篝火右上角记录
                        if (t.TileType == TileID.Campfire && t.TileFrameY % 36 == 0 && t.TileFrameX % 54 == 0) {
                            playerNearby.Add(x * 10000 + y);
                            // 记录新加入的
                            if (!oldPlayerNearby.Contains(x * 10000 + y)) {
                                newlyAdded.Add(x * 10000 + y);
                            }
                        }
                    }
                }
            }
            // 对这些新加入的进行关闭操作
            foreach (int packedCoord in newlyAdded) {
                int x = packedCoord / 10000;
                int y = packedCoord % 10000;
                SoundEngine.PlaySound(SoundID.MenuTick, new Vector2(x, y).ToWorldCoordinates());
                for (int k = 0; k < 3; k++) {
                    for (int l = 0; l < 2; l++) {
                        Main.tile[k + x, l + y].TileFrameY = (short)(36 + l * 18); // 灭火帧
                    }
                }
                NetMessage.SendTileSquare(-1, x, y, 3, 2);
            }
        }
    }
}
