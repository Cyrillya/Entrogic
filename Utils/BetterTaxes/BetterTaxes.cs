using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Entrogic;

namespace BetterTaxes
{
    public static class UsefulThings
    {
        public static string ValueToCoins(int num, string zeroString = "0 copper")
        {
            if (zeroString == "0 copper") zeroString = "0 " + Language.GetTextValue("LegacyInterface.18");
            if (num < 1) return zeroString;
            return Main.ValueToCoins(num);
        }

        public static string ValueToCoinsWithColor(double num, string zeroString = "0 copper")
        {
            if (zeroString == "0 copper") zeroString = "0 " + Language.GetTextValue("LegacyInterface.18");
            if (double.IsPositiveInfinity(num)) return "[c/" + Colors.CoinPlatinum.Hex3() + ":" + Language.GetTextValue("Mods.BetterTaxes.Status.ALotOfMoney") + "]";
            if (double.IsNegativeInfinity(num)) return "[c/" + Colors.CoinCopper.Hex3() + ":" + Language.GetTextValue("Mods.BetterTaxes.Status.NotALotOfMoney") + "]";
            if (double.IsNaN(num)) return "[c/" + Colors.CoinGold.Hex3() + ":" + Language.GetTextValue("Mods.BetterTaxes.Status.Unknown") + "]";
            return ValueToCoinsWithColor((int)num, zeroString);
        }

        public static string ValueToCoinsWithColor(int num, string zeroString = "0 copper")
        {
            if (zeroString == "0 copper") zeroString = "0 " + Language.GetTextValue("LegacyInterface.18");
            string data = ValueToCoins(num, zeroString);
            data = Regex.Replace(data, @"(\d+ " + Language.GetTextValue("LegacyInterface.15") + ")", "[c/" + Colors.CoinPlatinum.Hex3() + ":$1]");
            data = Regex.Replace(data, @"(\d+ " + Language.GetTextValue("LegacyInterface.16") + ")", "[c/" + Colors.CoinGold.Hex3() + ":$1]");
            data = Regex.Replace(data, @"(\d+ " + Language.GetTextValue("LegacyInterface.17") + ")", "[c/" + Colors.CoinSilver.Hex3() + ":$1]");
            data = Regex.Replace(data, @"(\d+ " + Language.GetTextValue("LegacyInterface.18") + ")", "[c/" + Colors.CoinCopper.Hex3() + ":$1]");
            return data;
        }

        public static string SecondsToHMS(int num, string zeroString = "0 seconds")
        {
            if (zeroString == "0 seconds") zeroString = "0 " + Language.GetTextValue("Mods.BetterTaxes.Config.Seconds");
            if (num < 1) return zeroString;

            string res = "";
            int hours = num / 3600;
            if (hours == 1) res += hours + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Hour") + " ";
            if (hours > 1) res += hours + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Hours") + " ";
            num %= 3600;
            int minutes = num / 60;
            if (minutes == 1) res += minutes + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Minute") + " ";
            if (minutes > 1) res += minutes + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Minutes") + " ";
            num %= 60;
            if (num == 1) res += num + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Second") + " ";
            if (num > 1) res += num + " " + Language.GetTextValue("Mods.BetterTaxes.Config.Seconds") + " ";

            return res.TrimEnd();
        }

        public static string SecondsToHMSCasual(int num, string zeroString = "1 tick")
        {
            if (zeroString == "1 tick") zeroString = "1 " + Language.GetTextValue("Mods.BetterTaxes.Config.Tick");
            if (num < 1) return zeroString;

            return SecondsToHMS(num, zeroString);
        }

        public static int CalculateNPCCount()
        {
            int npcCount = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].homeless && NPC.TypeToHeadIndex(Main.npc[i].type) > 0) npcCount++;
            }
            return npcCount;
        }
    }
}
