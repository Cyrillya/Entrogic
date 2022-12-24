namespace Entrogic.Core.Global
{
    public class AutoswingHandler : ModPlayer
    {
        public List<DamageClass> AllowAutoSwing = new List<DamageClass>();

        public override void ResetEffects() {
            AllowAutoSwing = new();
        }

        public override void UpdateDead() {
            AllowAutoSwing = new();
        }

        public override bool? CanAutoReuseItem(Item item) {
            foreach (var c in from a in AllowAutoSwing where item.CountsAsClass(a) select a)
                return true;
            return base.CanAutoReuseItem(item);
        }
    }
}
