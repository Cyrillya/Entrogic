
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class Strengthen : ModCard
    {
        public override void PreCreated()
        {
            said = "你被加强了，快上！";
            rare = CardRareID.WeakNuclear;
            tooltip = "抽取一张牌，奥术伤害加强一段时间";
            minion = true;
            costMana = 1;
            series = CardSeriesID.Organism;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.buffType = BuffType<Buffs.Weapons.Wind>();
            item.buffTime = 1800;
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            for (int i = 0; i < style.Length; i++)
                style[i] = true;
            return 1;
        }
    }
}
