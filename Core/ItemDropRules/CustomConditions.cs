using Entrogic.Core.Global;
using Terraria.GameContent.ItemDropRules;

namespace Entrogic.Core.ItemDropRules
{
    public class CustomConditions
	{
		public class DownedContyElemental : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => DownedHandler.DownedContyElemental;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => null;
		}
		public class FristTimeKillingContyElemental : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => !DownedHandler.DownedContyElemental;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => null;
		}
	}
}
