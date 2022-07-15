namespace Entrogic.Common.ModSystems
{
    public class EffectManager : ModSystem
    {
        internal const string EffectBase = "Entrogic/Assets/Effects/";
        internal static Asset<Effect> Trail;
        internal static Asset<Effect> BladeTrail;
        internal static Asset<Effect> Stoned;
        internal static Asset<Effect> CoverRenderer;
        internal static Asset<Effect> ColorReverse;

        public override void PostSetupContent() {
            if (!Main.dedServ) {
                CoverRenderer = ModContent.Request<Effect>($"{EffectBase}CoverRenderer");
                ColorReverse = ModContent.Request<Effect>($"{EffectBase}ColorReverse");
                Stoned = ModContent.Request<Effect>($"{EffectBase}Stoned");
                Trail = ModContent.Request<Effect>($"{EffectBase}Trail");
                BladeTrail = ModContent.Request<Effect>($"{EffectBase}BladeTrail");
            }
        }

        public override void Unload() {
            CoverRenderer = null;
            ColorReverse = null;
            Stoned = null;
            Trail = null;
            BladeTrail = null;
        }
    }
}
