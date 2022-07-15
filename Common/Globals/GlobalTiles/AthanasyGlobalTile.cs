using Entrogic.Common.WorldGeneration;
using Entrogic.Content.NPCs.Enemies.Athanasy;

namespace Entrogic.Common.Globals.GlobalTiles
{
    public class AthanasyGlobalTile : GlobalTile
    {
        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b) {
            if (!ImmortalGolemRoom.BossZone.Contains(i, j))
                return;

            if (type == TileID.Campfire && Main.tile[i, j].TileFrameX < 54) {
                r *= 1.5f;
                g *= 1.5f;
                b *= 1.3f;
            }
            //if (Main.tile[i, j].TileFrameX == 18 && Main.tile[i, j].TileFrameY == 0) {
            //    var worldCoordCenter = new Vector2(i, j).ToWorldCoordinates(autoAddY: 16f);
            //    float distanceFromCenter = 60f;
            //    for (float k = 0; k <= 6.28f; k += 6.28f / 200f) {
            //        var position = Vector2.One.RotatedBy(k) * distanceFromCenter + worldCoordCenter;
            //        var d = Dust.NewDustPerfect(position, DustID.Torch, Alpha: 60, Scale: 1.2f);
            //        d.noGravity = true;
            //        d.fadeIn = 0.9f;
            //    }
            //}
        }
    }
}
