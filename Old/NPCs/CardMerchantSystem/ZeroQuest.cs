using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrogic.NPCs.CardMerchantSystem
{
    /// <summary>
    /// 任务的起始
    /// </summary>
    public class ZeroQuest : Quest
    {
        public ZeroQuest()
        {
            ID = "0";
            MissionText = "（再次点击“任务”按钮领取第一个任务！）";
            ThanksMessage = "（再次点击“任务”按钮领取第一个任务！）";
            RewardMoney.Gold = 30;
        }
    }
}
