namespace Entrogic.Common.ModSystems
{
    public class ShaderManager : ModSystem
    {
        internal const string EffectBase = "Entrogic/Assets/Effects/";
        internal static Asset<Effect> Trail;
        internal static Asset<Effect> BladeTrail;
        internal static Asset<Effect> CoverRenderer;
        internal static Asset<Effect> ColorReverse;
        internal static Asset<Effect> Negative;
        internal static Asset<Effect> DirectVertex;
        internal static Asset<Effect> Bloom;
        internal static bool IsLoaded { get; private set; } = false;

        public override void PostSetupContent() {
            if (!Main.dedServ) {
                CoverRenderer = ModContent.Request<Effect>($"{EffectBase}CoverRenderer");
                ColorReverse = ModContent.Request<Effect>($"{EffectBase}ColorReverse");
                Trail = ModContent.Request<Effect>($"{EffectBase}Trail");
                BladeTrail = ModContent.Request<Effect>($"{EffectBase}BladeTrail");
                Negative = ModContent.Request<Effect>($"{EffectBase}Negative");
                DirectVertex = ModContent.Request<Effect>($"{EffectBase}DirectVertex");
                Bloom = ModContent.Request<Effect>($"{EffectBase}Bloom");
                IsLoaded = true;
            }
        }

        public override void Unload() {
            IsLoaded = false;
            CoverRenderer = null;
            ColorReverse = null;
            Trail = null;
            BladeTrail = null;
            Negative = null;
            DirectVertex = null;
            Bloom = null;
        }
    }
}
