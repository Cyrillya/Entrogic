using Entrogic.Tiles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic
{
    public class EntrogicTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (type == TileID.Obsidian)
            {
                ModWorldHelper.DeactiveConnectedPortal(i + 1, j);
                ModWorldHelper.DeactiveConnectedPortal(i - 1, j);
                ModWorldHelper.DeactiveConnectedPortal(i, j + 1);
                ModWorldHelper.DeactiveConnectedPortal(i, j - 1);
            }
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}
