﻿namespace Entrogic.Common.Globals.Players
{
    internal class GunPlayer : ModPlayer
    {
        public float RecoilDegree;

        public override void PreUpdate() {
            RecoilDegree = MathHelper.Lerp(RecoilDegree, 0f, 10f * (1f / 60f));
        }
    }
}
