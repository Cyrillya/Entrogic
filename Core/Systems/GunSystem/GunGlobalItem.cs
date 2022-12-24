namespace Entrogic.Core.Systems.GunSystem
{
    public class GunGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public float RecoilPower;
    }
}
