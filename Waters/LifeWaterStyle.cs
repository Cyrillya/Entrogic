using Entrogic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Waters
{
    public class LifeWaterStyle : ModWaterStyle
    {
        public override bool ChooseWaterStyle()
        {
            return Main.player[Main.myPlayer].GetModPlayer<EntrogicPlayer>().IsZoneLife;
        }

        public override int ChooseWaterfallStyle()
        {
            return mod.GetWaterfallStyleSlot("LifeWaterfallStyle");
        }

        public override int GetSplashDust()
        {
            return mod.DustType("LifeWaterSplash");
        }

        public override int GetDropletGore()
        {
            return mod.GetGoreSlot("Gores/LifeDroplet");
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override Color BiomeHairColor()
        {
            return Color.OrangeRed;
        }
    }
}