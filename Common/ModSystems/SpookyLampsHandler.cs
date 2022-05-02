using System.Linq;

namespace Entrogic.Common.ModSystems
{
    internal class SpookyLampHandler : ModSystem
    {
        public override void Load() {
            base.Load();
            // 通过原版自动开关门的方法来调用我们灯的开关
            On.Terraria.GameContent.DoorOpeningHelper.Update += DoorOpeningHelper_Update;
        }

        // 存储灯的集合，只会保存单个物块左上角的坐标
        internal static List<Point> SpookyLamps = new();
        internal static List<int> Lamps = new(); // 所有注册的诡异台灯都在这里

        internal static bool IsSpookyLamp(int type) {
            foreach (var lamp in from l in Lamps where type == l && type != 0 select l) return true;
            return false;
        }

        private void DoorOpeningHelper_Update(On.Terraria.GameContent.DoorOpeningHelper.orig_Update orig, DoorOpeningHelper self, Player player) {
            orig.Invoke(self, player);
            HandleLamps(player);
        }

        internal static void HandleLamps(Player player, bool active = true) {
            Point pl = player.Center.ToTileCoordinates();
            if (!WorldGen.InWorld(pl.X, pl.Y, 10)) return;
            for (int x = -5; x <= 5; x++) {
                for (int y = -5; y <= 5; y++) {
                    Point pt = new(pl.X + x, pl.Y + y);
                    Tile t = Framing.GetTileSafely(pt);
                    if (Vector2.Distance(pl.ToVector2(), pt.ToVector2()) <= 4 && IsSpookyLamp(t.TileType)) { // 足够近
                        SpookyLamps.Add(pt); // 添加进集合
                        if (t.TileFrameX == 18) { // 如果是关着的那顺便开起来
                            ToggleLamp(pt.X, pt.Y);
                        }
                    }
                }
            }
            if (SpookyLamps.Count == 0) return;
            var removes = new List<int>();
            foreach (var p in from pt in SpookyLamps
                              where (Vector2.Distance(pl.ToVector2(), pt.ToVector2()) > 4 || !active) && Framing.GetTileSafely(pt).TileFrameX == 0 // 开启状态，足够远或玩家消失
                              select pt) {
                if (IsSpookyLamp(Framing.GetTileSafely(p).TileType)) // 有可能台灯被挖掉，所以要给个特判了
                    ToggleLamp(p.X, p.Y); // 关灯
                removes.Add(SpookyLamps.IndexOf(p)); // 添加入准备被删除的位置信息的一个集合，枚举操作后统一删除（枚举时删除会造成错误）
            }
            foreach (var i in removes) SpookyLamps.RemoveAt(i);
        }

        public static void ToggleLamp(int i, int j) {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 2].TileFrameX += frameAdjustment;

            // Avoid trying to send packets in singleplayer.
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
            }
        }
    }
}
