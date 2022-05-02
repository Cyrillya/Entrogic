using Terraria.IO;
using Terraria.WorldBuilding;

namespace Entrogic.Common.WorldGeneration
{
    internal class ImmortalGolemTrapRoom : GenPass
    {
        public ImmortalGolemTrapRoom() : base("Athanasy Trap Room", 237.4298f) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
            progress.Message = "Immortal Golem Arena Final Cleanup";
            // 陷阱房铺设岩浆（要在最后执行，否则可能被其他世界生成顶掉）
            Point origin = new Point(ImmortalGolemRoom.TrapZone.X, ImmortalGolemRoom.TrapZone.Y);
            WorldUtils.Gen(origin + new Point(5, 5), new Shapes.Rectangle(70, 30), new Actions.SetLiquid(0, 0)); // 清除所有液体
            WorldUtils.Gen(origin + new Point(5, 30), new Shapes.Rectangle(70, 5), new Actions.SetLiquid(LiquidID.Lava)); // 铺岩浆
                                                                                                                          // 在陷阱房中央生成一个小台子，为了规避原版的方块平滑所以放在最后
            for (int k = -1; k <= 1; k++)
                for (int l = 0; l <= 2; l++) {
                    Point pt = new Point(ImmortalGolemRoom.TrapZone.Center.X + k, ImmortalGolemRoom.TrapZone.Center.Y + l);
                    Tile tile = Framing.GetTileSafely(pt);
                    tile.Clear(TileDataType.Slope);
                    tile.IsActive = true;
                    tile.type = k == l - 1 && l == 1 ? TileID.WoodBlock : ImmortalGolemRoom.BrickType; // 特判的是中间
                    WorldGen.SquareTileFrame(ImmortalGolemRoom.TrapZone.Center.X + k, ImmortalGolemRoom.TrapZone.Center.Y + l);
                    // NOTE: SlopeID里面的方位是指 三角形物块直角所对的方位，所以应和生成位置对于中间的方位正好相反
                    if (k == -1 && l == 0) WorldGen.SlopeTile(pt.X, pt.Y, (int)SlopeType.SlopeDownRight); // 左上角
                    if (k == 1 && l == 0) WorldGen.SlopeTile(pt.X, pt.Y, (int)SlopeType.SlopeDownLeft); // 右上角
                    if (k == -1 && l == 2) WorldGen.SlopeTile(pt.X, pt.Y, (int)SlopeType.SlopeUpRight); // 左下角
                    if (k == 1 && l == 2) WorldGen.SlopeTile(pt.X, pt.Y, (int)SlopeType.SlopeUpLeft); // 右下角
                    progress.Set(MathHelper.Lerp(0f, 0.1f, (k + 1 + l) / 4f));
                }
            // 解决一切宝箱和机关
            origin = new Point(ImmortalGolemRoom.BossZone.X, ImmortalGolemRoom.BossZone.Y);                                                                                          // 在陷阱房中央生成一个小台子，为了规避原版的方块平滑所以放在最后
            for (int k = 0; k <= ImmortalGolemRoom.BossZone.Width; k++) {
                for (int l = 0; l <= ImmortalGolemRoom.BossZone.Height; l++) {
                    Point pt = new Point(origin.X + k, origin.Y + l);
                    var t = Framing.GetTileSafely(pt);
                    if (TileID.Sets.IsAContainer[t.type]) {
                        t.ClearTile();
                    }
                    t.RedWire = false;
                    t.BlueWire = false;
                    t.GreenWire = false;
                    t.YellowWire = false;
                }
                progress.Set(MathHelper.Lerp(0.1f, 1f, k / ImmortalGolemRoom.BossZone.Width));
            }
        }
    }
}
