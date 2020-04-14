
using Entrogic.Buffs.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class Sprint : ModCard
    {
        public override void PreCreated()
        {
            said = "将风的力量封印在卡牌中...多么明智啊！";
            rare = CardRareID.WeakNuclear;
            tooltip = "赐予你风的轻盈敏捷";
            minion = true;
            costMana = 1;
            cardProb = 0.20f;
            series = CardSeriesID.Organism;
            healMana = 2;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:抽取两张牌，玩家剩余费用+2]";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.buffType = BuffType<Wind>();
            item.buffTime = 600;
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            for (int i = 0; i < style.Length; i++)
                style[i] = true;
            return 2;
        }
    }
    public class Sprint_Strong : ModCard
    {
        public override void PreCreated()
        {
            said = "将风的力量封印在卡牌中...多么明智啊！";
            rare = CardRareID.StrongNuclear;
            tooltip = "赐予你风的轻盈敏捷";
            minion = true;
            costMana = 1;
            cardProb = 0.15f;
            series = CardSeriesID.Organism;
            healMana = 3;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:抽取三张牌，玩家剩余费用+3]";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.buffType = BuffType<Wind>();
            item.buffTime = 900;
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            for (int i = 0; i < style.Length; i++)
                style[i] = true;
            return 3;
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return Main.hardMode;
        }
    }
}

