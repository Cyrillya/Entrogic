using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Entrogic.UI.Books;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Books.卡牌入门手册
{
    public class 卡牌入门手册 : ModBook
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/FF7200:卡牌从初学到入门]\n" +
         "[c/D5BB84:简介：初学者必读的卡牌速成手册！]\n" +
         "[c/F7DDA8:—————————————————]\n" +
         "[c/D5BB84:分类：教学类]\n" +
         "[c/D4B169:作者：Drarrior]\n");
        }
        public override void SetBookInformations()
        {
            item.value = 0;

            MaxPage = 4;
            for (int i = 1; i <= MaxPage * 2f; i++)
                textScale[i] = 0.64f;
            for (int i = 1; i <= MaxPage * 2f; i++)
                lineDistance[i] = -10f;
            lineDistance[1] = -6f;
            PageText[1] = "\n   想象一下，在一段漫长的跋涉之后踱步进入一间酒馆，点一杯最爱的饮料，拿出你珍藏的套牌，和牌友们来一局，实在是再惬意不过了。但前提是，你要学会驾驭这些火焰和雷电的化身，从这里开启你的[c/0000AA:牌王之路]！";
            PageText[2] = "卡牌分为[c/333333:引力]、[c/858585:电磁]、[c/8f7a0d:弱核]、[c/eba329:强核]、[c/420399:大统一]五种品质，一个卡组中最多只能携带6张相同的[c/333333:引力]、3张相同的[c/858585:电磁]、2张相同的[c/8f7a0d:弱核]、1张相同的[c/eba329:强核]和1张[c/BB0000:独一无二]的[c/420399:大统一]";
            PageTexture[1] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_1");
            PageText[3] = "\n[c/0039AA:卡牌背包]是你打造和升级你的套牌船坞，一个卡组只能携带9张牌，巧妙的卡牌搭配是你致胜的关键。你可在背包的下方见到[c/0039AA:卡牌背包]";
            PageText[4] = "优秀的牌手应当有优雅的操作，一双洁净的[c/125623:手套]很适合你！\n\n\n\n\n\n\n\n你使用的每一张牌都需要一定的[c/125623:费用]，费用的合理规划是关键。";
            PageTexture[2] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_2");
            PageText[5] = "卡牌被使用后会被置入[c/125623:墓地]\n\n" +
"过牌会将全部手牌置入墓地并抽取一定牌，[c/AA1212:重置魔力]\n\n" +
"洗牌会重置你的牌库并[c/AA1212:清空墓地]，卡牌背包的牌要经过[c/125623:洗牌]进入[c/125623:牌库]";
            PageText[6] = "卡牌分为[c/AA5619:攻击牌]和[c/1256AA:效果牌]，攻击牌可使你的手套可施放攻击性魔法（即便你不会魔法！），但有次数限制\n\n\n\n\n\n\n" +
"效果牌使用后会立即见效，可以用于应对一些突发状况或保持卡组的持久运行";
            PageTexture[3] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_3");
            PageText[7] = "术语\n\n\n抽牌：将牌库中的牌置入手牌\n\n弃牌：将手牌中的牌置入墓地\n\n移除：将任意位置某张牌移出，并不进入墓地";
            PageText[8] = "你初始手牌数为三张，费用数也为三个，但可以通过饰品的穿戴来改变";
        }
    }
}
