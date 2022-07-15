using Entrogic.Content.Tiles.Athanasy;
using Entrogic.Content.Tiles.Furnitures.SpookyLamps;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Entrogic.Common.WorldGeneration
{
    public class ImmortalGolemRoom : GenPass
    {
        public static Rectangle TrapZone = default(Rectangle);
        public static Rectangle BossZone = default(Rectangle);
        internal static ushort BrickType;
        internal static ushort WallType;
        internal static ushort PlatformStyle;

        public int t;

        public ImmortalGolemRoom() : base("Athanasy Arena Gen", 237.4298f) { }

        private void PlaceStairs() {
            /*
			foreach (Tuple<Point, Point> item3 in CreateStairsList()) {
				Point item = item3.Item1;
				Point item2 = item3.Item2;
				int num = (item2.X > item.X) ? 1 : (-1);
				ShapeData shapeData = new ShapeData();
				for (int i = 0; i < item2.Y - item.Y; i++) {
					shapeData.Add(num * (i + 1), i);
				}

				WorldUtils.Gen(item, new ModShapes.All(shapeData), Actions.Chain(new Actions.PlaceTile(19, 0), new Actions.SetSlope((num == 1) ? 1 : 2), new Actions.SetFrames(frameNeighbors: true)));
				WorldUtils.Gen(new Point(item.X + ((num == 1) ? 1 : (-4)), item.Y - 1), new Shapes.Rectangle(4, 1), Actions.Chain(new Actions.Clear(), new Actions.PlaceWall(WallType), new Actions.PlaceTile(19, 0), new Actions.SetFrames(frameNeighbors: true)));
			}
			*/
            foreach (var item in CreatePlatformsList()) {
                WorldUtils.Gen(item, new Shapes.Rectangle(1, 1), Actions.Chain(new Actions.PlaceTile(19, 0), new Actions.SetSlope(2), new Actions.SetFrames(frameNeighbors: true)));
            }
            WorldUtils.Gen(Main.MouseWorld.ToTileCoordinates() + new Point(-2, -5), new Shapes.Rectangle(5, 1), Actions.Chain(new Actions.PlaceTile(19, 0), new Actions.SetFrames(frameNeighbors: true)));
        }
        private List<Point> CreatePlatformsList() {
            List<Point> list = new();
            for (int i = 0; i <= 5; i++) {
                list.Add(Main.MouseWorld.ToTileCoordinates() + new Point(i, -i));
            }

            return list;
        }

        private List<Tuple<Point, Point>> CreateStairsList() {
            List<Tuple<Point, Point>> list = new();
            List<Rectangle> Rooms = new();
            var mouse = Main.MouseWorld.ToTileCoordinates();
            Rooms.Add(new Rectangle(mouse.X, mouse.Y, 16, 8));
            Rooms.Add(new Rectangle(mouse.X - 8, mouse.Y + 8, 16, 8));
            Rooms.Add(new Rectangle(mouse.X, mouse.Y + 16, 16, 8));
            for (int i = 1; i < Rooms.Count; i++) {
                Rectangle rectangle = Rooms[i];
                Rectangle rectangle2 = Rooms[i - 1];
                int num = rectangle2.X - rectangle.X;
                int num2 = rectangle.X + rectangle.Width - (rectangle2.X + rectangle2.Width);
                if (num > num2)
                    list.Add(new Tuple<Point, Point>(new Point(rectangle.X + rectangle.Width - 1, rectangle.Y + 1), new Point(rectangle.X + rectangle.Width - rectangle.Height + 1, rectangle.Y + rectangle.Height - 1)));
                else
                    list.Add(new Tuple<Point, Point>(new Point(rectangle.X, rectangle.Y + 1), new Point(rectangle.X + rectangle.Height - 1, rectangle.Y + rectangle.Height - 1)));
            }

            return list;
        }

        /// <summary>
        /// 应该可以选取到那种全是刺的陷阱房间
        /// </summary>
        private readonly int[,] _matchSpikeStructure = new int[,]{
            { 0, 2, 2, 1, 1 },
            { 0, 0, 2, 1, 1 },
            { 0, 2, 2, 1, 1 },
            { 0, 0, 2, 1, 1 },
            { 0, 2, 2, 1, 1 }
        };
        private readonly int[,] _matchNormalWallStructure = new int[,]{
            { 0, 0, 0, 1, 1 },
            { 0, 0, 0, 1, 1 },
            { 0, 0, 0, 1, 1 }
        };

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
            progress.Message = "Enshrining the Immortal Golem";
            ChooseDungeonColor(FindDungeonColor(out short dungeonSide), out BrickType, out WallType, out PlatformStyle); // 选择当前世界地牢搭配的砖块

            List<Point> availablePositions = new();
            progress.Set(0.1f);
            IEnumerable<Point> matches = MatchDungeonBricks(dungeonSide);
            foreach (var pos in matches) {
                bool available = true;
                // pos.X要-3，因为match是选取地牢砖，只有-3才会使选取时第三列刚好是地牢砖列
                if (!WorldGen.InWorld(pos.X, pos.Y) || !DealWithMatchStructure(new Point(pos.X - 3, pos.Y), _matchSpikeStructure) || !ConfirmSafeArena(pos)) {
                    available = false;
                }
                if (!available) continue;
                availablePositions.Add(new Point(pos.X, pos.Y - 6));
            }
            // 找刺房找不到合适的，那就只能找普通的了
            if (availablePositions.Count == 0) {
                foreach (var pos in matches) {
                    bool available = true;
                    // pos.X要-3，因为match是选取地牢砖，只有-3才会使选取时第三列刚好是地牢砖列
                    if (!WorldGen.InWorld(pos.X, pos.Y) || !DealWithMatchStructure(new Point(pos.X - 3, pos.Y), _matchNormalWallStructure) || !ConfirmSafeArena(pos)) {
                        available = false;
                    }
                    if (!available) continue;
                    availablePositions.Add(new Point(pos.X, pos.Y - 6));
                }
            }
            // 还是找不到，弹报错吧
            if (availablePositions.Count == 0) {
                return;
            }

            Point origin = availablePositions[0];

            int depthMaximum = 0;
            foreach (var pt in availablePositions) { // 选择最深的，以免覆盖了血腥之地之类的
                if (pt.Y > depthMaximum) {
                    depthMaximum = pt.Y;
                    origin = pt;
                }
            }

            Point thePoint = origin;
            // 清地刺
            for (int i = -3; i <= 0; i++) {
                for (int j = 0; j <= 6; j++) {
                    var t = Framing.GetTileSafely(origin.X + i, origin.Y + j + 6);
                    if (t.TileType == TileID.Spikes) {
                        t.ClearTile();
                    }
                }
            }
            // 有点难看，有时间修复
            StructureGenHelper.GenerateRoom(new Rectangle(origin.X, origin.Y, 30, 16), BrickType, WallType, 0, 5); // 连接地牢与区域的通道
            progress.Set(0.3f);

            origin += new Point(30, -10);
            TrapZone = new Rectangle((int)origin.X, (int)origin.Y, 80, 40);
            StructureGenHelper.GenerateRoom(new Rectangle(origin.X, origin.Y, 80, 40), BrickType, WallType, 5, 5); // 陷阱大厅
            for (int i = 5; i <= 75; i++) {
                WorldGen.PlaceTile((int)origin.X + i, (int)origin.Y + 5, TileID.Spikes);
                if (i % 2 == 0) {
                    WorldGen.PlaceTile((int)origin.X + i, (int)origin.Y + 6, TileID.Spikes);
                    WorldGen.PlaceTile((int)origin.X + i, (int)origin.Y + 7, TileID.Spikes);
                }
            }
            progress.Set(0.35f);

            origin += new Point(80, 10);
            StructureGenHelper.GenerateRoom(new Rectangle(origin.X, origin.Y, 32, 16), BrickType, WallType, 0, 5); // 连接陷阱大厅与Boss大厅的通道
            progress.Set(0.5f);

            int bossRoomWidth = 110 + 20;
            int bossRoomHeight = 80 + 20;
            origin += new Point(31, 22);
            origin.Y -= bossRoomHeight;
            BossZone = new Rectangle((int)origin.X, (int)origin.Y, bossRoomWidth, bossRoomHeight);
            StructureGenHelper.GenerateRoom(new Rectangle(origin.X, origin.Y, bossRoomWidth, bossRoomHeight), BrickType, WallType, 10, 10); // Boss大厅

            // 挖空通道，安置高门
            int length = origin.X - thePoint.X;
            origin = thePoint;
            origin.Y += 7;
            WorldUtils.Gen(origin, new Shapes.Rectangle(length + 10, 5), Actions.Chain(new Actions.ClearTile(), new Actions.PlaceWall(WallType))); // 挖空通道
            Point orig = origin;
            for (int i = 0; i <= length + 10; i++) {
                if (WorldGen.SolidTile(i + orig.X, orig.Y - 1) && (!WorldGen.SolidTile(i + orig.X - 1, orig.Y - 1) || !WorldGen.SolidTile(i + orig.X + 1, orig.Y - 1))) {
                    StructureGenHelper.GenerateHighGate(origin + new Point(i, 0));
                    //WorldGen.SquareTileFrame(i + orig.X, orig.Y);
                    //WorldGen.PlaceTile(i + orig.X, orig.Y, TileID.ClosedDoor, true, style: 15);
                    // 放置地牢平台，用PlaceTile，如果已有物块则不会放置
                    WorldGen.PlaceTile((int)origin.X + i + 1, (int)origin.Y + 5, TileID.Platforms, style: PlatformStyle);
                    WorldGen.PlaceTile((int)origin.X + i - 1, (int)origin.Y + 5, TileID.Platforms, style: PlatformStyle);
                }
            }
            // 放置诡异灯，灯不在Boss房生成（length本身不包含Boss房那部分了）
            origin.Y += 2;
            orig = origin;
            ushort lampType = BrickType == TileID.GreenDungeonBrick ? (ushort)ModContent.TileType<GreenSpookyLampTile>() :
                BrickType == TileID.PinkDungeonBrick ? (ushort)ModContent.TileType<PinkSpookyLampTile>() :
                (ushort)ModContent.TileType<BlueSpookyLampTile>(); // 选择相应台灯样式，默认当然是蓝
            for (int i = 3; i <= length; i += 6) {
                if (WorldGen.SolidTile(i + orig.X, orig.Y + 3) && Framing.GetTileSafely(i + orig.X + 1, orig.Y).TileType != TileID.TallGateClosed && Framing.GetTileSafely(i + orig.X - 1, orig.Y).TileType != TileID.TallGateClosed && Framing.GetTileSafely(i + orig.X, orig.Y).TileType != TileID.TallGateClosed) { // 地下站立的方块要Solid，不能靠着或代替高门而立
                    StructureGenHelper.GenerateSpookyLamp(origin + new Point(i, 0), lampType);
                }
            }
            progress.Set(0.9f);
            GeneratePlatformFromTexture(new Point(BossZone.X, BossZone.Y) + new Point(10, 10));
        }

        private static void GeneratePlatformFromTexture(Point spawn) {
            List<Point> platforms = new();
            List<Point> campfires = new();
            for (int y = 0; y < WorldGenSystem.AthanasyPlatform.GetLength(1); y++) {
                for (int x = 0; x < WorldGenSystem.AthanasyPlatform.GetLength(0); x++) {
                    Color c = WorldGenSystem.AthanasyPlatform[x, y];
                    var coord = new Point(spawn.X + x, spawn.Y + y);
                    if (c.R == 255) {
                        campfires.Add(coord); // 记录下来，最后再铺置
                        continue;
                    }
                    if (c.G == 100) { // 横排平台
                        platforms.Add(coord); // 记录下来，最后再铺置
                        continue;
                    }
                    if (c.G == 200) { // 左延伸平台
                        foreach (var item in CreateLeftStairsList(coord, c.R)) {
                            WorldUtils.Gen(item, new Shapes.Rectangle(1, 1), Actions.Chain(new Actions.PlaceTile(TileID.Platforms, PlatformStyle), new Actions.SetSlope(1), new Actions.SetFrames(frameNeighbors: true)));
                        }
                        continue;
                    }
                    if (c.G == 150) { // 右延伸平台
                        foreach (var item in CreateRightStairsList(coord, c.R)) {
                            WorldUtils.Gen(item, new Shapes.Rectangle(1, 1), Actions.Chain(new Actions.PlaceTile(TileID.Platforms, PlatformStyle), new Actions.SetSlope(2), new Actions.SetFrames(frameNeighbors: true)));
                        }
                        continue;
                    }
                    if (c.G == 255 && c.B == 255) {
                        WorldGen.PlaceTile(coord.X, coord.Y, ModContent.TileType<RockAltar>(), mute: true);
                    }
                }
            }
            foreach (var plat in platforms) {
                WorldUtils.Gen(plat, new Shapes.Rectangle(110, 1), Actions.Chain(new Actions.PlaceTile(TileID.Platforms, PlatformStyle), new Actions.SetFrames(frameNeighbors: true)));
            }
            foreach (var coord in campfires) {
                WorldGen.PlaceTile(coord.X, coord.Y, TileID.Campfire, mute: true);
            }
        }

        private static List<Point> CreateLeftStairsList(Point spawn, byte length) {
            List<Point> list = new();
            for (int i = 0; i <= length; i++) {
                list.Add(spawn + new Point(-i, -i));
            }
            return list;
        }

        private static List<Point> CreateRightStairsList(Point spawn, byte length) {
            List<Point> list = new();
            for (int i = 0; i <= length; i++) {
                list.Add(spawn + new Point(i, -i));
            }
            return list;
        }

        private bool ConfirmSafeArena(Point leftTop) {
            int brickAmount = 0;
            // 要求一定范围内·不要超过[指定]格地牢块
            for (int x = 0; x <= 220; x++)
                for (int y = -30; y <= 30; y++) {
                    int k = x + leftTop.X;
                    int l = y + leftTop.Y;
                    if (!WorldGen.InWorld(k, l)) { Main.NewText(leftTop); return false; }
                    Tile tile = Framing.GetTileSafely(k, l);
                    if (tile.HasUnactuatedTile && Main.tileDungeon[tile.TileType]) {
                        brickAmount++;
                    }
                    if (brickAmount >= 1000) {
                        return false;
                    }
                }
            return true;
        }

        private bool DealWithMatchStructure(Point pos, int[,] matcher) {
            // 为什么是y,x，因为原版Main.tile数组搞反了
            //Main.NewText(Framing.GetTileSafely(pos.X, pos.Y).HasUnactuatedTile.ToString() + ' ' + Main.tileDungeon[Framing.GetTileSafely(pos.X, pos.Y).TileType]);
            for (int y = 0; y < matcher.GetLength(0); y++) {
                for (int x = 0; x < matcher.GetLength(1); x++) {
                    int k = pos.X + x;
                    int l = pos.Y + y;
                    Tile tile = Framing.GetTileSafely(k, l);
                    switch (matcher[y, x]) {
                        case 0:
                            if (tile.HasUnactuatedTile || tile.LiquidAmount > 0) return false; // { Main.NewText(x + " " + y); return false; }
                            break;
                        case 1:
                            if (!Main.tileDungeon[tile.TileType]) return false; // { Main.NewText(x + " " + y); return false; }
                            break;
                        case 2:
                            if (tile.TileType != TileID.Spikes) return false; // { Main.NewText(x + " " + y); return false; }
                            break;
                    }
                }
            }
            return true;
        }

        private IEnumerable<Point> MatchDungeonBricks(short side) {
            for (int x = Main.maxTilesX / 2; ; x += side) {
                if (x <= 20 || x >= Main.maxTilesX - 20) break;
                for (int y = (int)Main.rockLayer; y <= Main.maxTilesY - 200; y++) {
                    if (Main.tile[x, y].HasUnactuatedTile && Main.tileDungeon[Main.tile[x, y].TileType])
                        yield return new Point(x, y);
                }
            }
            yield return Point.Zero;
        }

        private char FindDungeonColor(out short dungeonSide) {
            for (int x = 20; x <= Main.maxTilesX - 20; x++) {
                for (int y = (int)Main.rockLayer; y <= Main.maxTilesY - 200; y++) {
                    if (!Main.tile[x, y].HasUnactuatedTile || !Main.tileDungeon[Main.tile[x, y].TileType]) continue;

                    dungeonSide = CheckSide(x);
                    if (Main.tile[x, y].TileType == TileID.BlueDungeonBrick) return 'B';
                    else if (Main.tile[x, y].TileType == TileID.GreenDungeonBrick) return 'G';
                    else if (Main.tile[x, y].TileType == TileID.PinkDungeonBrick) return 'P';
                }
            }
            dungeonSide = 0;
            return '0';
        }

        private void ChooseDungeonColor(char dungeonColor, out ushort brickType, out ushort wallType, out ushort platform) {
            switch (dungeonColor) {
                case 'B':
                    brickType = TileID.BlueDungeonBrick;
                    wallType = WallID.BlueDungeonUnsafe;
                    platform = 6;
                    break;
                case 'G':
                    brickType = TileID.GreenDungeonBrick;
                    wallType = WallID.GreenDungeonUnsafe;
                    platform = 8;
                    break;
                case 'P':
                    brickType = TileID.PinkDungeonBrick;
                    wallType = WallID.PinkDungeonUnsafe;
                    platform = 7;
                    break;
                default:
                    //return;
                    brickType = TileID.BlueDungeonBrick;
                    wallType = WallID.BlueDungeonUnsafe;
                    platform = 6;
                    break;
            }
        }

        private short CheckSide(int x) {
            if (x >= Main.maxTilesX / 2) return 1;
            else return -1;
        }

        public static void SaveWorldData(TagCompound tag) {
            tag["TrapLeft"] = TrapZone.X;
            tag["TrapTop"] = TrapZone.Y;
            tag["TrapWidth"] = TrapZone.Width;
            tag["TrapHeight"] = TrapZone.Height;
            tag["BossLeft"] = BossZone.X;
            tag["BossTop"] = BossZone.Y;
            tag["BossWidth"] = BossZone.Width;
            tag["BossHeight"] = BossZone.Height;
            tag[nameof(BrickType)] = (short)BrickType;
            tag[nameof(WallType)] = (short)WallType;
        }

        public static void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey("TrapLeft") && tag.ContainsKey("TrapTop") && tag.ContainsKey("TrapWidth") && tag.ContainsKey("TrapHeight"))
                TrapZone = new Rectangle(tag.GetInt("TrapLeft"), tag.GetInt("TrapTop"), tag.GetInt("TrapWidth"), tag.GetInt("TrapHeight"));
            if (tag.ContainsKey("BossLeft") && tag.ContainsKey("BossTop") && tag.ContainsKey("BossWidth") && tag.ContainsKey("BossHeight"))
                BossZone = new Rectangle(tag.GetInt("BossLeft"), tag.GetInt("BossTop"), tag.GetInt("BossWidth"), tag.GetInt("BossHeight"));
            if (tag.ContainsKey(nameof(BrickType)))
                BrickType = (ushort)tag.GetShort(nameof(BrickType));
            if (tag.ContainsKey(nameof(WallType)))
                WallType = (ushort)tag.GetShort(nameof(WallType));
        }
    }
}
