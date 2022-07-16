namespace Entrogic.Common.ModSystems
{
    public class ShaderManager : ModSystem
    {
        internal const string EffectBase = "Entrogic/Assets/Effects/";
        internal static Asset<Effect> Trail;
        internal static Asset<Effect> BladeTrail;
        internal static Asset<Effect> Stoned;
        internal static Asset<Effect> CoverRenderer;
        internal static Asset<Effect> ColorReverse;
        internal static Asset<Effect> Negative;

        public override void PostSetupContent() {
            if (!Main.dedServ) {
                CoverRenderer = ModContent.Request<Effect>($"{EffectBase}CoverRenderer");
                ColorReverse = ModContent.Request<Effect>($"{EffectBase}ColorReverse");
                Stoned = ModContent.Request<Effect>($"{EffectBase}Stoned");
                Trail = ModContent.Request<Effect>($"{EffectBase}Trail");
                BladeTrail = ModContent.Request<Effect>($"{EffectBase}BladeTrail");
                Negative = ModContent.Request<Effect>($"{EffectBase}Negative");
            }
        }

        public override void Unload() {
            CoverRenderer = null;
            ColorReverse = null;
            Stoned = null;
            Trail = null;
            BladeTrail = null;
            Negative = null;
        }
    }
}
