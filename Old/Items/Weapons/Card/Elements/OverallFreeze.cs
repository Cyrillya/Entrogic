using Entrogic.Buffs.Weapons;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class OverallFreeze : ModCard
    {
        public override void PreCreated()
        {
            said = "冻住不许走！";
            rare = CardRareID.Gravitation;
            tooltip = "使以玩家为中点，半径为55格的圆内的敌人冰冻5秒，抽一张牌";
            costMana = 1;
            minion = true;
            series = CardSeriesID.Element;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:若当前世界存在Boss，则冰冻效果变为减速效果，持续时间为10秒]";
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
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            float r = 55 * 16;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(player.Center) <= r)
                {
                    if (EntrogicNPC.FightingWithBoss())
                        npc.AddBuff(BuffType<NPCChilled>(), 10 * 60);
                    else npc.AddBuff(BuffType<NPCFrozen>(), 5 * 60);
                }
            }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return NPC.downedBoss1;
        }
    }
}
