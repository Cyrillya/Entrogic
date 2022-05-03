namespace Entrogic.Common.ModSystems
{
    internal class ResourceManager : ModSystem
    {
        internal const string EffectBase = "Entrogic/Assets/Effects/Content/";

        internal const string ImageBase = "Entrogic/Assets/Images/";
        internal const string Images = ImageBase + "Miscellaneous/";
        internal const string Trailling = ImageBase + "Trail/";
        internal const string WorldGen = ImageBase + "WorldGeneration/";
        internal const string BookAssets = ImageBase + "BookUI/";
        internal const string Dusts = ImageBase + "Dusts/";

        internal const string Blank = Images + "Blank";

        internal static Asset<Effect> Trail;
        internal static Asset<Texture2D> TrailMainColor;
        internal static Asset<Texture2D> TrailMainShape;
        internal static Asset<Texture2D> TrailMaskColor;
        internal static Asset<Effect> BladeTrail;
        internal static Asset<Texture2D> Heatmap;
        internal static Asset<Texture2D> BladeTrailShape1;
        internal static Asset<Texture2D> BladeTrailShape2;
        internal static Asset<Texture2D> BladeTrailErosion;

        internal static Asset<Effect> Stoned;
        internal static Asset<Texture2D> StonedImage;

        internal static Asset<Effect> CoverRenderer;
        internal static Asset<Effect> ColorReverse;

        internal static Asset<Texture2D> BookBubble;
        internal static Asset<Texture2D> BookPanel;
        internal static Asset<Texture2D> BookNext;
        internal static Asset<Texture2D> BookBack;

        internal static Dictionary<string, Asset<Texture2D>> Miscellaneous = new();

        public delegate void SetupContentDelegate();
        public static event SetupContentDelegate SetupContentEvent;

        public override void PostSetupContent() {
            if (!Main.dedServ) {
                SetupMiscellaneous();
                SetupStoned();
                SetupBooks();
                SetupTrailing();
                SetupContentEvent?.Invoke();
            }
            base.PostSetupContent();
        }

        private static void SetupMiscellaneous() {
            // 用上反射，发现tModLoader现在只会存Request过的资源了，放弃自动读取
            /*
			var targetBool = Entrogic.Instance.Assets.GetType().GetField("_assets", BindingFlags.NonPublic | BindingFlags.Instance);
			var targetValue = (Dictionary<string, IAsset>)targetBool.GetValue(Entrogic.Instance.Assets);
			foreach (var asset in from a in targetValue
                                  where
                                  a.Value is Asset<Texture2D> &&
                                  (a.Value as Asset<Texture2D>).Name.StartsWith("Assets/Images/Miscellaneous/")
                                  select a) {
            }
			*/

            void AddMisc(string name) => Miscellaneous.Add(name, ModContent.Request<Texture2D>($"Entrogic/Assets/Images/Miscellaneous/{name}"));
            AddMisc("ComboRing");
            AddMisc("ContaEffect");
            CoverRenderer = ModContent.Request<Effect>($"{EffectBase}CoverRenderer");
            ColorReverse = ModContent.Request<Effect>($"{EffectBase}ColorReverse");
        }

        private static void SetupStoned() {
            Stoned = ModContent.Request<Effect>($"{EffectBase}Stoned");
            StonedImage = ModContent.Request<Texture2D>($"{Images}StonedImage");
        }

        private static void SetupBooks() {
            BookBubble = ModContent.Request<Texture2D>($"{BookAssets}ReadingBubble");
            BookPanel = ModContent.Request<Texture2D>($"{BookAssets}Panel");
            BookNext = ModContent.Request<Texture2D>($"{BookAssets}Next");
            BookBack = ModContent.Request<Texture2D>($"{BookAssets}Back");
        }

        private static void SetupTrailing() {
            Trail = ModContent.Request<Effect>($"{EffectBase}Trail");
            TrailMainColor = ModContent.Request<Texture2D>($"{Trailling}Cyromap");
            TrailMainShape = ModContent.Request<Texture2D>($"{Trailling}Extra_197");
            TrailMaskColor = ModContent.Request<Texture2D>($"{Trailling}Extra_196");
            BladeTrail = ModContent.Request<Effect>($"{EffectBase}BladeTrail");
            Heatmap = ModContent.Request<Texture2D>($"{Trailling}Heatmap");
            BladeTrailShape1 = ModContent.Request<Texture2D>($"{Trailling}BladeTrailShape1");
            BladeTrailShape2 = ModContent.Request<Texture2D>($"{Trailling}BladeTrailShape2");
            BladeTrailErosion = ModContent.Request<Texture2D>($"{Trailling}Extra_193");
        }
    }
}
