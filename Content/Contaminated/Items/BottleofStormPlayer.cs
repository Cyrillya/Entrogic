namespace Entrogic.Content.Contaminated.Items
{
    public class BottleofStormPlayer : ModPlayer
    {
        public bool HasWings;
        internal int wingFrameCounter;
        internal int wingFrame;
        public override void ResetEffects() {
            HasWings = false;
        }
        public override void UpdateDead() {
            HasWings = false;
        }
    }
}
