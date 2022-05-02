
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Nones
{
    public class Foresight : ModCard
    {
        public override void PreCreated()
        {
            said = "要多看";
            rare = CardRareID.Electromagnetism;
            tooltip = "抽一张牌，并使其的费用-2";
            costMana = 1;
            minion = true;
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
            cost -= 2;
            cost = Math.Max(0, cost);
            return true;
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return NPC.downedBoss1;
        }
    }
}
