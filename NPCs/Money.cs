using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Globalization;

namespace Entrogic.NPCs
{
    public struct Money
    {
		public int Platinum;
        public int Gold;
        public int Silver;
        public int Copper;
        public Money(int platinum = 0, int gold = 0, int silver = 0, int copper = 0)
        {
            Platinum = platinum;
            Gold = gold;
            Silver = silver;
            Copper = copper;
        }
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{{{铂金:{0} 金:{1} 银:{2} 铜:{3}}}}}", Platinum.ToString(currentCulture), Gold.ToString(currentCulture), Silver.ToString(currentCulture), Copper.ToString(currentCulture));
        }
    }
}
