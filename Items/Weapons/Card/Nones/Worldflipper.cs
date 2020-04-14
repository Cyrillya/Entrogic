
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Nones
{
    public class Worldflipper : ModCard
    {
        public override void PreCreated()
        {
            said = "不会将你对手的屏幕颠倒";
            rare = CardRareID.GrandUnified;
            tooltip = "复活所有牌并抽取牌库中剩余牌";
            minion = true;
            costMana = 3;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            int[] StayForWhile = new int[250];
            while (true)
            {
                for (int j = 0; j < ePlayer.CardReadyType.Length; j++)
                    if (ePlayer.CardReadyType[j] != 0 && Main.rand.NextBool(4))
                    {
                        bool hasGrid = false;
                        for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                            if (ePlayer.CardHandType[i] == 0)
                            {
                                ePlayer.CardHandType[i] = ePlayer.CardReadyType[j];
                                ePlayer.CardHandCost[i] = ePlayer.CardReadyCost[j] - 1;
                                ePlayer.CardHandCost[i] = Math.Max(ePlayer.CardHandCost[i], 0);
                                ePlayer.CardReadyType[j] = 0;
                                ePlayer.CardReadyCost[j] = 0;
                                hasGrid = true;
                                break;
                            }
                        if (!hasGrid)
                        {
                            StayForWhile[j] = ePlayer.CardReadyType[j];
                            ePlayer.CardReadyType[j] = 0;
                            ePlayer.CardReadyCost[j] = 0;
                        }
                    }
                if (ePlayer.LibNum <= 0)
                {
                    break;
                }
            }
            for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                if (ePlayer.CardReadyType[i] == 0)
                    for (int j = 0; j < ePlayer.CardGraveType.Length; j++)
                        if (ePlayer.CardGraveType[j] != 0)
                        {
                            ePlayer.CardReadyType[i] = ePlayer.CardGraveType[j];
                            Item item = new Item();
                            item.SetDefaults(ePlayer.CardGraveType[j]);
                            ModCard card = (ModCard)item.modItem;
                            ePlayer.CardReadyCost[i] = card.costMana;
                            ePlayer.CardGraveType[j] = 0;
                            break;
                        }
            for (int i = 0; i < ePlayer.CardGraveType.Length; i++)
                if (ePlayer.CardGraveType[i] == 0)
                    for (int j = 0; j < StayForWhile.Length; j++)
                        if (StayForWhile[j] != 0)
                        {
                            ePlayer.CardGraveType[i] = StayForWhile[j];
                            StayForWhile[j] = 0;
                            break;
                        }
        }
    }
}
