using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Common.ItemDropRules
{
    public static class ExtraItemDropRules
    {
        public class Downed : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            private string _key;

            public Downed(string key)
            {
                _key = key;
            }
            public bool CanDrop(DropAttemptInfo info) => EntrogicWorld.Downeds.ContainsKey(_key);

            public bool CanShowItemDropInUI() => true;

            public string GetConditionDescription() => null;
        }

        /// <summary>
        /// Makes the NPC drop multiple items at the same time.
        /// </summary>
        /// <param name="dropsOutOfX"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IItemDropRule AllFromOptions(int dropsOutOfX, params int[] items) => new MultipleItemsDropRule(dropsOutOfX, 1, items);
    }
}