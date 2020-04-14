
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class Dragoul : ModCard
    {
        public override void PreCreated()
        {
            said = "龙魂将助你移动...";
            rare = CardRareID.StrongNuclear;
            tooltip = "获得16秒高速飞行能力，抽一张牌";
            costMana = 3;
            minion = true;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
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
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            player.AddBuff(BuffType<Buffs.Weapons.Dragoul>(), 16 * 60);
        }
    }
}
