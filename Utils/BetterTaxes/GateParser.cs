using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace BetterTaxes
{
    public class GateParser
    {
        public const float ExpertModeBoost = 1.5f;
        public static readonly Dictionary<string, SpecialInt> TaxRatesDefaults = new Dictionary<string, SpecialInt> {
            {"Base.always", 50},
            {"Base.mechAny", 100},
            {"Base.plantera", 200},
            {"Base.golem", 500},
            {"Base.moonlord", 1000},
        };
        public static readonly Dictionary<string, float> TaxTimerMuiltDefaults = new Dictionary<string, float> {
            {"Base.always", 1.2f},
            {"Base.mechAny", 1.5f},
            {"Base.plantera", 2f},
            {"Base.golem", 2.6f},
            {"Base.moonlord", 3f},
        };
        public Dictionary<string, SpecialInt> TaxRates
        {
            get
            {
                Dictionary<string, SpecialInt> result = TaxRatesDefaults.ToDictionary(i => i.Key, i => i.Value);

                // Here we go ahead and put every single new field we know about into the config with their recommended values, if they exist (because otherwise how are you supposed to know about them?)
                foreach (KeyValuePair<string, Dictionary<string, Func<bool>>> entry in ModHandler.delegates)
                {
                    foreach (KeyValuePair<string, Func<bool>> entry2 in entry.Value)
                    {
                        string statement = entry.Key + "." + entry2.Key;
                        Dictionary<string, int> rates = new Dictionary<string, int>();
                        rates.TryGetValue(statement, out int determinedValue);
                        if (!result.ContainsKey(statement) && determinedValue > -1) result.Add(statement, determinedValue);
                    }
                }

                return result;
            }
        }
        public Dictionary<string, float> TaxTimerMuilt
        {
            get
            {
                Dictionary<string, float> result = TaxTimerMuiltDefaults.ToDictionary(i => i.Key, i => i.Value);

                // Here we go ahead and put every single new field we know about into the config with their recommended values, if they exist (because otherwise how are you supposed to know about them?)
                foreach (KeyValuePair<string, Dictionary<string, Func<bool>>> entry in ModHandler.delegates)
                {
                    foreach (KeyValuePair<string, Func<bool>> entry2 in entry.Value)
                    {
                        string statement = entry.Key + "." + entry2.Key;
                        Dictionary<string, float> rates = new Dictionary<string, float>();
                        rates.TryGetValue(statement, out float determinedValue);
                        if (!result.ContainsKey(statement) && determinedValue > -1) result.Add(statement, determinedValue);
                    }
                }

                return result;
            }
        }
        public int CalculateRate()
        {
            int taxRate = -1;
            foreach (KeyValuePair<string, SpecialInt> entry in TaxRates)
            {
                if (entry.Value > taxRate && Interpret(entry.Key)) taxRate = entry.Value;
            }

            if (Main.expertMode && ExpertModeBoost >= 0) taxRate = (int)(taxRate * ExpertModeBoost); // Expert mode boost
            if (Main.xMas) taxRate = (int)(taxRate * 1.1); // Christmas boost
            return taxRate;
        }
        public float CalculateTimerRate()
        {
            float taxTimerRate = -1;
            foreach (KeyValuePair<string, float> entry in TaxTimerMuilt)
            {
                if (entry.Value > taxTimerRate && Interpret(entry.Key)) taxTimerRate = entry.Value;
            }

            if (Main.expertMode && ExpertModeBoost >= 0) taxTimerRate = taxTimerRate * ExpertModeBoost; // Expert mode boost
            if (Main.xMas) taxTimerRate = taxTimerRate * 1.1f; // Christmas boost
            return taxTimerRate;
        }

        public static readonly char[] validOpenBrackets = new char[] { '(', '[', '{' };
        public static readonly char[] validCloseBrackets = new char[] { ')', ']', '}' };
        public bool Interpret(string conditions)
        {
            // 1st pass: make sure everything is valid
            Stack<int> bracketStack = new Stack<int>();
            for (int i = 0; i < conditions.Length; i++)
            {
                if (validOpenBrackets.Contains(conditions[i])) bracketStack.Push(i);
                if (validCloseBrackets.Contains(conditions[i])) bracketStack.Pop();
            }
            if (bracketStack.Count > 0) throw new InvalidConfigException("Failed to parse parentheses in statement \"" + conditions + "\"");

            // 2nd pass: break it down bit by bit until we have an answer
            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                bracketStack = new Stack<int>();
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (validOpenBrackets.Contains(conditions[i]))
                    {
                        hasChanged = true;
                        bracketStack.Push(i);
                    }
                    if (validCloseBrackets.Contains(conditions[i]))
                    {
                        hasChanged = true;
                        int pos = bracketStack.Pop();
                        string textToReplace = conditions.Substring(pos + 1, i - pos - 1);
                        conditions = conditions.Substring(0, pos) + (Interpret(textToReplace) ? "true" : "false") + conditions.Substring(i + 1);
                        break;
                    }
                }
            }

            return InterpretGates(conditions);
        }

        public bool InterpretGates(string conditions)
        {
            List<string> terms = conditions.Split(' ').ToList();

            // gates that take 1 input
            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                for (int i = 0; i < terms.Count; i++)
                {
                    if (terms[i] == "not")
                    {
                        terms[i] = InterpretCondition(terms[i + 1]) ? "false" : "true";
                        terms.RemoveAt(i + 1);
                        hasChanged = true;
                        break;
                    }
                }
            }

            // gates that take 2 inputs
            hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                for (int i = 1; i < terms.Count - 1; i++) // first and last terms can't be a gate
                {
                    switch (terms[i])
                    {
                        case "and":
                            terms[i - 1] = (InterpretCondition(terms[i - 1]) && InterpretCondition(terms[i + 1])) ? "true" : "false";
                            terms.RemoveAt(i + 1);
                            terms.RemoveAt(i);
                            hasChanged = true;
                            break;
                        case "or":
                            terms[i - 1] = (InterpretCondition(terms[i - 1]) || InterpretCondition(terms[i + 1])) ? "true" : "false";
                            terms.RemoveAt(i + 1);
                            terms.RemoveAt(i);
                            hasChanged = true;
                            break;
                        case "xor":
                            terms[i - 1] = (InterpretCondition(terms[i - 1]) ^ InterpretCondition(terms[i + 1])) ? "true" : "false";
                            terms.RemoveAt(i + 1);
                            terms.RemoveAt(i);
                            hasChanged = true;
                            break;
                    }
                }
            }

            return InterpretCondition(string.Join(" ", terms.ToArray()));
        }

        public bool InterpretCondition(string condition)
        {
            if (condition == "true") return true;
            if (condition == "false") return false;

            string[] terms = condition.Split('.');

            if (terms.Length == 2 && terms[0] == "Base") // example: Base.downedMoonlord
            {
                switch (terms[1])
                {
                    case "always":
                        return true;
                    case "never":
                        return false;
                    case "moonlord":
                    case "downedMoonlord":
                        return NPC.downedMoonlord;
                    case "golem":
                    case "downedGolemBoss":
                        return NPC.downedGolemBoss;
                    case "plantera":
                    case "downedPlantBoss":
                        return NPC.downedPlantBoss;
                    case "mechAny":
                    case "downedMechBossAny":
                        return NPC.downedMechBossAny;
                    case "mechAll":
                    case "downedMechBossAll":
                        return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
                    case "cultist":
                    case "downedAncientCultist":
                        return NPC.downedAncientCultist;

                    // miscellaneous
                    case "expert":
                    case "expertMode":
                        return Main.expertMode;
                    case "crimson":
                        return WorldGen.crimson;
                    case "corruption": // equivalent to "not Base.crimson"
                        return !WorldGen.crimson;
                }
                throw new InvalidConfigException("Invalid condition \"" + terms[1] + "\" under list \"Base\"");
            }
            else if (terms.Length == 2 && terms[0] == "Invasion")
            {
                switch (terms[1])
                {
                    case "goblins":
                    case "downedGoblins":
                        return NPC.downedGoblins;
                    case "frost":
                    case "frostLegion":
                    case "downedFrost":
                        return NPC.downedFrost;
                    case "pirates":
                    case "downedPirates":
                        return NPC.downedPirates;
                    case "martians":
                    case "downedMartians":
                        return NPC.downedMartians;
                }
                throw new InvalidConfigException("Invalid condition \"" + terms[1] + "\" under list \"Invasion\"");
            }
            else if (terms.Length == 2)
            {
                string chosen_list = terms[0];
                string chosen_condition = terms[1];

                // delegate system
                if (ModHandler.delegates.ContainsKey(chosen_list))
                {
                    Dictionary<string, Func<bool>> checkers = ModHandler.delegates[chosen_list];
                    if (checkers != null)
                    {
                        if (!checkers.ContainsKey(chosen_condition)) throw new InvalidConfigException("Invalid condition \"" + chosen_condition + "\" under list \"" + chosen_list + "\"");
                        return checkers[chosen_condition]();
                    }
                    return false;
                }
                return false;
            }
            throw new InvalidConfigException("Failed to parse key \"" + condition + "\"");
        }
    }
}
