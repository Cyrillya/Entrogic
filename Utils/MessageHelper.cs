using System;
using System.IO;

using Terraria;
using Terraria.ID;

using static Entrogic.Entrogic;

namespace Entrogic
{
    public static class MessageHelper
    {
        /// <summary>
        /// 发送书籍是否打开的包
        /// </summary>
        /// <param name="playernum">第一个传入，表示发送/接收者的player.whoAmI</param>
        /// <param name="pageNum">第二个传入，表示发送/接收者当前书本页数</param>
        /// <param name="isOpen">第三个传入，表示书籍打开情况</param>
        public static void SendBookInfo(int playernum, byte pageNum, bool isOpen, int toClient = -1, int ignoreClient = -1)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) return;
            var packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.SyncBookBubbleInfo);
            packet.Write((byte)playernum);
            packet.Write(pageNum);
            packet.Write(isOpen);
            packet.Send(toClient, ignoreClient);
        }
        /// <summary>
        /// 发送卡牌任务完成的包
        /// </summary>
        /// <param name="playernum">第一个传入，表示发送/接收者的player.whoAmI</param>
        /// <param name="Complete">第二个传入，表示发送/接收者当前完成的任务的字符串</param>
        public static void SendCardMission(byte playernum, string Complete, int toClient = -1, int ignoreClient = -1)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) return;
            var packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.SendCompletedCardMerchantMissionRequest);
            packet.Write((byte)playernum);
            packet.Write(Complete);
            packet.Send(toClient, ignoreClient);
        }
    }
}
