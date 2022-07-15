using Entrogic.Common.WorldGeneration;
using Entrogic.Content.NPCs.Enemies.Athanasy;
using Terraria.Audio;

namespace Entrogic.Common.ModSystems
{
    public class AthanasySystem : ModSystem
    {
        internal static bool RunningApplyEquipFunctional;

        public override void Load() {
            On.Terraria.Player.ApplyEquipFunctional += ApplyEquipFunctionalNerf;
            On.Terraria.Lighting.AddLight_int_int_float_float_float += NerfEquipmentLight;
        }

        private void NerfEquipmentLight(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float r, float g, float b) {
            if (RunningApplyEquipFunctional) {
                r *= 0.3f;
                g *= 0.3f;
                b *= 0.3f;
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
            if (!NPC.AnyNPCs(ModContent.NPCType<Athanasy>()) || Main.netMode == NetmodeID.MultiplayerClient) {
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
                for (int i = -12; i <= 12; i++) {
                    for (int j = -12; j <= 12; j++) {
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
