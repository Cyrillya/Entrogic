using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Symbiosis
{
    public class GelOfLife : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 4));//后面那个是frame，前面那个是delay
            Tooltip.SetDefault("生命的本源，近乎完美的魔法介质\n“生命总会找到自己的出路”");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 99;
            Item.rare = RarityLevelID.MiddlePHM;
            Item.value = Item.sellPrice(0, 0, 3, 0);
        }
    }
}