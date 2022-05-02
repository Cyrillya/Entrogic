using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Common.ItemDropRules
{
    public class MultipleItemsDropRule : IItemDropRule
    {
        private int[] _dropIDs;
        private int _outOfY;
        private int _xOutOfY;

        public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

        public MultipleItemsDropRule(int outOfY, int xOutOfY, params int[] items)
        {
            _dropIDs = items;
            _outOfY = outOfY;
            _xOutOfY = xOutOfY;
            ChainedRules = new List<IItemDropRuleChainAttempt>();
        }

        public bool CanDrop(DropAttemptInfo info) => true;

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result;
            if (info.player.RollLuck(_outOfY) < _xOutOfY)
            {
                for (int item = 0; item < _dropIDs.Length; item++)
                {
                    CommonCode.DropItemFromNPC(info.npc, _dropIDs[item], 1);
                }

                result = default;
                result.State = ItemDropAttemptResultState.Success;
                return result;
            }

            result = default;
            result.State = ItemDropAttemptResultState.FailedRandomRoll;
            return result;
        }

        public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
        {
            float num = _xOutOfY / _outOfY;
            float num2 = num * ratesInfo.parentDroprateChance;
            float dropRate = 1f / _dropIDs.Length * num2;

            for (int i = 0; i < _dropIDs.Length; i++)
            {
                drops.Add(new DropRateInfo(_dropIDs[i], 1, 1, dropRate, ratesInfo.conditions));
            }

            Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
        }
    }
}