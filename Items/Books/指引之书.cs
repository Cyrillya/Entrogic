using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Entrogic.UI;
using Entrogic.UI.Books;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Materials;

namespace Entrogic.Items.Books
{
    public class 指引之书 : ModBook
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/FF7200:指引之书]\n" +
         "[c/D5BB84:简介：它有过许多主人，也曾经为他们指引前行的道路]\n" +
         "[c/F7DDA8:—————————————————]\n" +
         "[c/D5BB84:分类：杂类]\n" +
         "[c/D4B169:作者：未知]\n");
        }
        public override void SetBookInformations()
        {
            MaxPage = 2;
            for (int i = 1; i <= MaxPage * 2f; i++)
                textScale[i] = 0.7f;
            for (int i = 1; i <= MaxPage * 2f; i++)
                lineDistance[i] = -4f;
            textScale[1] = 0.68f;
            lineDistance[1] = -8f;
            PageText[1] = "笔者注释：翻书时记得[c/409940:用你的手提起书页的一角]，轻轻的翻页；文明人都需要学习用手翻书，而不是血迹斑斑的武器。\n\n如果你想要合上书，只需要打开你的库存，书页会自行合上，用右手触碰书页也可将书合上，切换到其他物品也会合书";
            PageText[2] = "创造者，现在你需要学习一些关于生存与创造的不同寻常的知识。\n\n关于这个“现象”的世界我觉得无须多向你解释，想必远道而来的你已经历过无数的冒险，而今不过是想从零开始以期获得一段[c/404099:新的体验]而已。";
            PageText[3] = "我们以脑中的幻想为蓝本编织了一个新世界，这是一个尚充满魔力的世界，魔力将一切推向巅峰，又使一切从峰顶跌落。因此这也是个充满[c/994040:机遇与危险]的世界。你可以从我们撰写的书籍中了解他。";
            PageText[4] = "不同的魔力催化出了不同的、新式的敌人，更多友善的朋友和奇妙的道具，这将又是一个你没体验过的新世界。\n\n现在以任意你喜欢的方式开启这段探索吧。";
        }
        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.anyWood = true;
            r.AddIngredient(ItemType<SoulOfPure>(), 5);
            r.AddTile(TileID.Sawmill);
            r.SetResult(this);
            r.AddRecipe();
        }
    }
}