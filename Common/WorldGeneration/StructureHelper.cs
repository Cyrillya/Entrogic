using Entrogic.Content.Tiles.Furnitures;
using Entrogic.Content.Tiles.Furnitures.SpookyLamps;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Entrogic.Common.WorldGeneration
{
    public static class StructureGenHelper
    {
        public static void GenerateRoom(Rectangle room, ushort tileType, ushort wallType = WallID.None, int wallWidth = 1, int wallHeight = 1) {
            if (!WorldGen.InWorld(room.X, room.Y) || !WorldGen.InWorld(room.X + room.Width, room.Y + room.Width)) return; // 之后加入一个报错提示，现在先这样

            // 用原版的函数生成
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Actions.SetTileKeepWall(tileType), new Actions.SetFrames(frameNeighbors: true)));
            WorldUtils.Gen(new Point(room.X + wallWidth, room.Y + wallHeight), new Shapes.Rectangle(room.Width - wallWidth * 2, room.Height - wallHeight * 2), Actions.Chain(new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(wallType)));
        }

        public static bool GenerateHighGate(Point topLeft) {
            try {
                if (!Main.tile[topLeft.X, topLeft.Y - 1].IsActuated && Main.tileSolid[Main.tile[topLeft.X, topLeft.Y - 1].TileType] && WorldGen.SolidTile(topLeft.X, topLeft.Y + 5)) {
                    // 高门贴图第1, 5帧的长度是18，而第2-4帧的长度是16，需要特殊处理
                    var t = Main.tile[topLeft.X, topLeft.Y];
                    t.HasTile = true;
                    Main.tile[topLeft.X, topLeft.Y].TileType = TileID.TallGateClosed;
                    Main.tile[topLeft.X, topLeft.Y].TileFrameX = 0;
                    Main.tile[topLeft.X, topLeft.Y].TileFrameY = 0;
                    for (int i = 1; i < 5; i++) {
                        t = Main.tile[topLeft.X, topLeft.Y + i];
                        t.HasTile = true;
                        Main.tile[topLeft.X, topLeft.Y + i].TileType = TileID.TallGateClosed;
                        Main.tile[topLeft.X, topLeft.Y + i].TileFrameX = 0;
                        Main.tile[topLeft.X, topLeft.Y + i].TileFrameY = (short)(2 + 18 * i);
                    }
                    return true;
                }

                return false;
            }
            catch {
                return false;
            }
        }

        public static bool GenerateSpookyLamp(Point topLeft, ushort type) {
            try {
                for (short i = 0; i < 3; i++) {
                    var t = Main.tile[topLeft.X, topLeft.Y + i];
                    t.HasTile = true;
                    Main.tile[topLeft.X, topLeft.Y + i].TileType = type;
                    Main.tile[topLeft.X, topLeft.Y + i].TileFrameX = 18; // 默认关闭
                    Main.tile[topLeft.X, topLeft.Y + i].TileFrameY = (short)(18 * i);
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
