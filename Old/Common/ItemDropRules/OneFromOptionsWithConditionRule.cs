using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Common.ItemDropRules
{
    public class OneFromOptionsWithConditionRule : IItemDropRule
    {
        private int[] _dropIds;
        private int _outOfY;
        private int _xOutOfY;
        private IItemDropRuleCondition _condition;

        public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

        public OneFromOptionsWithConditionRule(int[] itemIds, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition condition, int dropsXOutOfY = 1)
        {
            _dropIds = itemIds;
            _outOfY = dropsOutOfY;
            _xOutOfY = dropsXOutOfY;
            _condition = condition;
            ChainedRules = new List<IItemDropRuleChainAttempt>();
        }

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result;

            if (info.player.RollLuck(_outOfY) < _xOutOfY)
            {
                CommonCode.DropItemFromNPC(info.npc, _dropIds[info.rng.Next(_dropIds.Length)], 1);

                result = default;
                result.State = ItemDropAttemptResultState.Success;

                return result;
            }

            result = default;
            result.State = ItemDropAttemptResultState.FailedRandomRoll;

            return result;
        }

        public bool CanDrop(DropAttemptInfo info) => _condition.CanDrop(info);

        public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
        {
            float num = _xOutOfY / (float)_outOfY;
            float num2 = num * ratesInfo.parentDroprateChance;
            float dropRate = 1f / _dropIds.Length * num2;

            for (int i = 0; i < _dropIds.Length; i++)
            {
                drops.Add(new DropRateInfo(_dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
            }

            Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
        }
    }
}