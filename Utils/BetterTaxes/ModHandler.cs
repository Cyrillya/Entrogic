using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BetterTaxes
{
    public class ModHandler
    {
        internal static Dictionary<string, Dictionary<string, Func<bool>>> delegates = new Dictionary<string, Dictionary<string, Func<bool>>>();

        public static GateParser parser;

        public static bool NewList(string listName)
        {
            if (delegates.ContainsKey(listName)) delegates.Remove(listName);
            delegates.Add(listName, new Dictionary<string, Func<bool>>());
            return true;
        }

        public ModHandler()
        {
            delegates = new Dictionary<string, Dictionary<string, Func<bool>>>();
            parser = new GateParser();
        }
    }
}
