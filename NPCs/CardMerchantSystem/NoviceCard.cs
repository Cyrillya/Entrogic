using Entrogic.Items.Weapons.Card.Elements;
using Entrogic.Items.Weapons.Card.Organisms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.CardMerchantSystem
{
    public class NoviceCard : Quest
    {
        public NoviceCard()
        {
            ID = "NoviceCard";
			MissionText = "...你看上去十分迷惑，每个人刚开始的时候都难免，我将作为一个伟大的引导者，引导你深入探索这个魔力的世界，我会给你一些“任务”" +
				"，完成“任务”我会给你一些奖赏，帮助你更好地完成理想中的大业。现在，请你购买一份新手卡包，阅读卡牌手册，尝试尝试“新事物”吧" +
				"\n\n（装备卡牌，并试着使用一次！）\n（将卡牌放入卡牌背包，手拿手套，点击“墓碑”再点击“过牌”，随后点击出现的卡牌）";
			ThanksMessage = $"你出色地完成了任务！现在让我送你一张卡牌[i:{ItemType<EnergyRecovery>()}]，这是一张出色的卡牌，相信他会发挥伟大的作用的。";
			CornerItem = ItemType<ArcaneMissle>();
		}
        public override bool CheckCompletion(Player player)
        {
            return EntrogicPlayer.ModPlayer(player).CardUseCount >= 1;
        }
        public override void SpawnReward(Player player, NPC npc)
        {
            player.QuickSpawnItem(ItemType<EnergyRecovery>());
            player.QuickSpawnItem(ItemID.HiTekSunglasses);

            base.SpawnReward(player, npc);
        }
    }
}
