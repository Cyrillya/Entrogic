
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class Funeral : ModCard
    {
        public override void PreCreated()
        {
            said = "让逝去的卡牌不再孤独";
            rare = CardRareID.StrongNuclear;
            tooltip = "当任意一张牌不是被使用或过牌而进入墓地时，若这张牌在牌库或手牌中，";
            special = true;
            costMana = 0;
            NoUseNormalDelete = true;
            series = CardSeriesID.Organism;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:则此牌也进入墓地，并抽一张生物牌]";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override void CardGraved(Player player, int number, int myNum, bool special, int packType, bool drawCard)
        {
            if (!special)
                return;
            EntrogicPlayer entrogicPlayer = player.GetModPlayer<EntrogicPlayer>();
            Item iiii = new Item();
            if (packType == 1)
            {
                iiii.SetDefaults(entrogicPlayer.CardHandType[number]);
            }
            else if (packType == 2)
            {
                iiii.SetDefaults(entrogicPlayer.CardReadyType[number]);
            }
            entrogicPlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.Funeral", iiii.Name), true);
            int type = entrogicPlayer.CardHandType[myNum];
            entrogicPlayer.CardHandType[myNum] = 0;
            entrogicPlayer.CardSetToGrave(type, myNum, true, 1);
            entrogicPlayer.CardHandCost[myNum] = 0;
        }
        public override void CardGravedWhileMeReady(Player player, int number, int myNum, bool special, int packType, bool drawCard)
        {
            if (!special)
                return;
            EntrogicPlayer entrogicPlayer = player.GetModPlayer<EntrogicPlayer>();
            Item iiii = new Item();
            if (packType == 1)
            {
                iiii.SetDefaults(entrogicPlayer.CardHandType[number]);
            }
            else if (packType == 2)
            {
                iiii.SetDefaults(entrogicPlayer.CardReadyType[number]);
            }
            entrogicPlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.Funeral", iiii.Name), true);
            int type = entrogicPlayer.CardReadyType[myNum];
            entrogicPlayer.CardReadyType[myNum] = 0;
            entrogicPlayer.CardSetToGrave(type, myNum, true, 2);
            entrogicPlayer.CardReadyCost[myNum] = 0;
        }
        public override void SpecialEffects(Player player, Vector2 position, int damage, float knockBack, int number, int packType, bool special, bool drawCard)
        {
            if (!special)
                return;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            int gotCard = 0;
            List<int> organismCardNumber = new List<int>();
            for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
            {
                if (ePlayer.CardReadyType[i] == 0 || (i == number && packType == 2))
                    continue;
                Item it = new Item();
                it.SetDefaults(ePlayer.CardReadyType[i]);
                ModItem modItem = it.modItem;
                ModCard modCard = (ModCard)modItem;
                if (modCard.series == CardSeriesID.Organism)
                {
                    organismCardNumber.Add(i);
                }
            }
            if (organismCardNumber.Count > 0)
            {
                if (packType != 1)
                {
                    for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                    {
                        if (ePlayer.CardHandType[i] == 0)
                        {
                            int chooseCard = Main.rand.Next(0, organismCardNumber.Count);
                            ePlayer.CardHandType[i] = ePlayer.CardReadyType[organismCardNumber[chooseCard]];
                            ePlayer.CardHandCost[i] = ePlayer.CardReadyCost[organismCardNumber[chooseCard]];
                            ePlayer.CardReadyType[organismCardNumber[chooseCard]] = 0;
                            ePlayer.CardReadyCost[organismCardNumber[chooseCard]] = 0;
                            gotCard++;
                            break;
                        }
                    }
                }
                else
                {
                    int chooseCard = Main.rand.Next(0, organismCardNumber.Count);
                    ePlayer.CardHandType[number] = ePlayer.CardReadyType[organismCardNumber[chooseCard]];
                    ePlayer.CardHandCost[number] = ePlayer.CardReadyCost[organismCardNumber[chooseCard]];
                    ePlayer.CardReadyType[organismCardNumber[chooseCard]] = 0;
                    ePlayer.CardReadyCost[organismCardNumber[chooseCard]] = 0;
                    gotCard++;
                }
            }
            ePlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.CardDrawCard", item.Name, gotCard, Language.GetTextValue("Mods.Entrogic.SeriesOrganism"), ""), true);
        }
    }
}
