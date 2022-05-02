using System;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.ContyElemental
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
