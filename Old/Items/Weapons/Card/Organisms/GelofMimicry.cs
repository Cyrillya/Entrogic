
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class GelofMimicry : ModCard
    {
        public override void PreCreated()
        {
            said = "将凝胶的力量封印在卡牌中...多么明智啊！";
            rare = CardRareID.StrongNuclear;
            tooltip = "赐予你的卡牌凝胶善于分裂的能力";
            minion = true;
            costMana = 2;
            NoUseNormalDelete = true;
            series = CardSeriesID.Organism;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:抽取一张牌并将其填满你的手牌]";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            for (int i = 0; i < style.Length; i++)
                style[i] = true;
            return 1;
        }
        public override bool HaveDrawCard(Player player, int number, int a, ref int type, ref int cost)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            int t = ePlayer.CardHandType[number];
            ePlayer.CardHandType[number] = 0;
            ePlayer.CardSetToGrave(t, number, false, 1, true);
            ePlayer.CardHandCost[number] = 0;
            for (int i = 0; i < ePlayer.CardHandType.Length; i++)
            {
                if (ePlayer.CardHandType[i] != 0)
                    continue;
                ePlayer.CardHandType[i] = type;
                ePlayer.CardHandCost[i] = cost;
            }
            Item iiii = new Item();
            iiii.SetDefaults(type);
            ePlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.CardDrawCopyCard", item.Name, iiii.Name));
            return false;
        }
        public override bool HaveDrawCardMessage(Player player, ref string text, int cardGot, string eventMessageSeries, string eventMessageStyle, int number)
        {
            return false;
        }
    }
}
