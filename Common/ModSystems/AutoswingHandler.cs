namespace Entrogic
{
    public class AutoswingHandler : ModPlayer
    {
        public bool allowAutoReuse = false;
        public bool allowAutoReuseWhip = false;
        public bool allowAutoReuseMelee = false;
        public bool allowAutoReuseRanged = false;

        public override bool? CanAutoReuseItem(Item item) {
            if (allowAutoReuse) {
                return true;
            }
            if (allowAutoReuseWhip && item.CountsAsClass(DamageClass.SummonMeleeSpeed)) {
                return true;
            }
            if (allowAutoReuseMelee && item.CountsAsClass(DamageClass.Melee)) {
                return true;
            }
            if (allowAutoReuseRanged && item.CountsAsClass(DamageClass.Ranged)) {
                return true;
            }
            return base.CanAutoReuseItem(item);
        }
    }
}
