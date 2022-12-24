namespace Entrogic.Core.Global
{
    internal class AmmoGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstatiation) {
            return item.type == ItemID.IceBlock;
        }

        public override void SetDefaults(Item item) {
            item.ammo = item.type;
        }
    }
}
