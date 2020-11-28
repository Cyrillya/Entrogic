using Entrogic;
using Entrogic.Assets.Gores;
using Entrogic.Dusts;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            return WaterStyleID.Bloodmoon;//mod.GetWaterfallStyleSlot("LifeWaterfallStyle");
        }

        public override int GetSplashDust()
        {
            return DustType<LifeWaterSplash>();
        }

        public override int GetDropletGore()
        {
            return GoreType<LifeDroplet>();
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