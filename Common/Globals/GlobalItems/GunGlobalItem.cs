using Terraria.GameContent.ItemDropRules;
using System.Reflection;

namespace Entrogic.Common.Globals.GlobalItems
{
    public class GunGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public float RecoilPower;
    }
}
