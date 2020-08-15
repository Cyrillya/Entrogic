using Microsoft.Xna.Framework;

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
        public static void SendExplode(Vector2 position, Vector2 size, int damage, int toClient = -1, int ignoreClient = -1, int friendly = 0, int goreTimes = 1, bool useSomke = true)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) return;
            var packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.SyncExplode);
            packet.WritePackedVector2(position);
            packet.WritePackedVector2(size);
            packet.Write(damage);
            packet.Write(friendly);
            packet.Write(goreTimes);
            packet.Write(useSomke);
            packet.Send(-1, -1);
        }
    }
}
