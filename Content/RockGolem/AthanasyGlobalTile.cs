using Entrogic.Core.WorldGeneration;

namespace Entrogic.Content.RockGolem
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
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) =>
            !ImmortalGolemRoom.BossZone.Contains(i, j);

        public override bool CanExplode(int i, int j, int type) =>
            !ImmortalGolemRoom.BossZone.Contains(i, j);

        public override bool CanPlace(int i, int j, int type) =>
            !ImmortalGolemRoom.BossZone.Contains(i, j);
    }
}
