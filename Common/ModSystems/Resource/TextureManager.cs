namespace Entrogic.Common.ModSystems
{
    public class TextureManager : ModSystem
    {
        internal const string EffectBase = "Entrogic/Assets/Effects/Content/";

        internal const string ImageBase = "Entrogic/Assets/Images/";
        internal const string Images = ImageBase + "Miscellaneous/";
        internal const string Trailling = ImageBase + "Trail/";
        internal const string WorldGen = ImageBase + "WorldGeneration/";
        internal const string BookAssets = ImageBase + "BookUI/";
        internal const string Dusts = ImageBase + "Dusts/";

        internal const string Blank = Images + "Blank";

        internal static Asset<Texture2D> Cyromap;
        internal static Asset<Texture2D> Cyromap2;
        internal static Asset<Texture2D> TrailMainShape;
        internal static Asset<Texture2D> TrailMaskColor;
        internal static Asset<Texture2D> Heatmap;
        internal static Asset<Texture2D> BladeTrailShape1;
        internal static Asset<Texture2D> BladeTrailShape2;
        internal static Asset<Texture2D> BladeTrailShape3;
        internal static Asset<Texture2D> BladeTrailCover;
        internal static Asset<Texture2D> BladeTrailErosion;

        internal static Asset<Texture2D> BookBubble;
        internal static Asset<Texture2D> BookPanel;
        internal static Asset<Texture2D> BookNext;
        internal static Asset<Texture2D> BookBack;

        internal static Dictionary<string, Asset<Texture2D>> Miscellaneous = new();

        internal static bool IsLoaded { get; private set; } = false;

        public override void PostSetupContent() {
            if (!Main.dedServ) {
                SetupMiscellaneous();
                SetupBooks();
                SetupTrailing();
                IsLoaded = true;
            }
            base.PostSetupContent();
        }

        private static void SetupMiscellaneous() {
            static void AddMisc(string name) => Miscellaneous.Add(name, ModContent.Request<Texture2D>($"Entrogic/Assets/Images/Miscellaneous/{name}"));
            AddMisc("ComboRing");
            AddMisc("ContaEffect");
            AddMisc("AthanasyBossRoomCutaway");
        }

        private static void SetupBooks() {
            BookBubble = ModContent.Request<Texture2D>($"{BookAssets}ReadingBubble");
            BookPanel = ModContent.Request<Texture2D>($"{BookAssets}Panel");
            BookNext = ModContent.Request<Texture2D>($"{BookAssets}Next");
            BookBack = ModContent.Request<Texture2D>($"{BookAssets}Back");
        }

        private static void SetupTrailing() {
            Cyromap = ModContent.Request<Texture2D>($"{Trailling}Cyromap");
            Cyromap2 = ModContent.Request<Texture2D>($"{Trailling}Cyromap2");
            TrailMainShape = ModContent.Request<Texture2D>($"{Trailling}Extra_197");
            TrailMaskColor = ModContent.Request<Texture2D>($"{Trailling}Extra_196");
            Heatmap = ModContent.Request<Texture2D>($"{Trailling}Heatmap");
            BladeTrailShape1 = ModContent.Request<Texture2D>($"{Trailling}BladeTrailShape1");
            BladeTrailShape2 = ModContent.Request<Texture2D>($"{Trailling}BladeTrailShape2");
            BladeTrailShape3 = ModContent.Request<Texture2D>($"{Trailling}BladeTrailShape3");
            BladeTrailCover = ModContent.Request<Texture2D>($"{Trailling}BladeTrailCover");
            BladeTrailErosion = ModContent.Request<Texture2D>($"{Trailling}Extra_193");
        }

        public override void Unload() {
            Miscellaneous.Clear();
            BookBubble = null;
            BookPanel = null;
            BookNext = null;
            BookBack = null;
            Cyromap = null;
            Cyromap2 = null;
            TrailMainShape = null;
            TrailMaskColor = null;
            Heatmap = null;
            BladeTrailShape1 = null;
            BladeTrailShape2 = null;
            BladeTrailShape3 = null;
            BladeTrailCover = null;
            BladeTrailErosion = null;
            IsLoaded = false;
        }
    }
}
