namespace Entrogic.Common.Globals.GlobalItems
{
    internal class AmmoGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstatiation) {
            return item.type == ItemID.IceBlock;
        }

        public override void SetDefaults(Item item) {
            base.SetDefaults(item);
            item.ammo = item.type;
        }
    }
}
