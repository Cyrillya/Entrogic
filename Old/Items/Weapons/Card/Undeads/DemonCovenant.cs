
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Undeads
{
    public class DemonCovenant : ModCard
    {
        public override void PreCreated()
        {
            said = "别看合同，快签！";
            rare = CardRareID.Gravitation;
            tooltip = "随机弃掉三张手牌，每实际弃一张牌，抽一张牌";
            costMana = 1;
            minion = true;
            series = CardSeriesID.Undead;
        }
        public override string AnotherMessages()
        {
            return "可能会疯魔到和自己签契约";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            int cardsWeOuted = 0;
            for (int a = 0; a < 3; a++)
            {
                List<int> canOutCards = new List<int>();
                for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                {
                    if (ePlayer.CardHandType[i] == 0 || i == number)
                        continue;
                    canOutCards.Add(i);
                }
                if (canOutCards.Count > 0)
                {
                    int chooseCard = Main.rand.Next(0, canOutCards.Count);
                    ePlayer.CardSetToGrave(ePlayer.CardHandType[canOutCards[chooseCard]], canOutCards[chooseCard], true);
                    ePlayer.CardHandType[canOutCards[chooseCard]] = 0;
                    ePlayer.CardHandCost[canOutCards[chooseCard]] = 0;
                    cardsWeOuted++;
                }
            }
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            for (int i = 0; i < style.Length; i++)
                style[i] = true;
            return cardsWeOuted;
        }
        public override bool HaveDrawCardMessage(Player player, ref string text, int cardGot, string eventMessageSeries, string eventMessageStyle, int number)
        {
            text = Language.GetTextValue("Mods.Entrogic.CardBurnAndDrawCard", item.Name, cardGot, eventMessageSeries, eventMessageStyle);
            return true;
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return EntrogicWorld.beArrivedAtUnderworld;
        }
    }
}
