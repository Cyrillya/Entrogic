using Microsoft.Xna.Framework;

using System;
using System.IO;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Entrogic.Entrogic;

namespace Entrogic
{
    public static class MessageHelper
    {
        public static void BuildBuilding(string name, Vector2 position, bool useAir = true)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) return;
            var packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.BuildBuilding);
            packet.Write(name);
            packet.WritePackedVector2(position);
            packet.Write(useAir);
            packet.Send(-1, -1);
        }
        public static void FindUWTeleporter(int i, int j, int playernum)
        {
            if (Main.netMode == NetmodeID.SinglePlayer) return;
            var packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.FindUWTeleporter);
            packet.Write(i);
            packet.Write(j);
            packet.Write(playernum);
            packet.Send(-1, -1);
        }
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
            packet.Send(toClient, ignoreClient);
        }

        public static void WritePackedRectangle(this BinaryWriter writer, Rectangle rectangle)
        {
            writer.Write(rectangle.X);
            writer.Write(rectangle.Y);
            writer.Write(rectangle.Width);
            writer.Write(rectangle.Height);
        }
        public static Rectangle ReadPackedRectangle(this BinaryReader reader)
        {
            Rectangle rec = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            return rec;
        }
        public static void WritePackedTile(this BinaryWriter bw, Tile tile)
        {
            bw.Write(tile.type);
            if (TileLoader.GetTile(tile.type) != null)
            {
                bw.Write(TileLoader.GetTile(tile.type).Name);
            }
            else
            {
                bw.Write("");
            }
            bw.Write(tile.wall);
            bw.Write(tile.liquid);
            bw.Write(tile.sTileHeader);
            bw.Write(tile.bTileHeader);
            bw.Write(tile.bTileHeader2);
            bw.Write(tile.bTileHeader3);
            bw.Write(tile.frameX);
            bw.Write(tile.frameY);
        }
        public static Tile ReadPackedTile(this BinaryReader br)
        {
            var from = new Tile();
            from.type = br.ReadUInt16();
            var str = br.ReadString();
            if (str != "")
                from.type = (ushort)ModHelper.TileType(str);
            from.wall = br.ReadUInt16();
            from.liquid = br.ReadByte();
            from.sTileHeader = br.ReadUInt16();
            from.bTileHeader = br.ReadByte();
            from.bTileHeader2 = br.ReadByte();
            from.bTileHeader3 = br.ReadByte();
            from.frameX = br.ReadInt16();
            from.frameY = br.ReadInt16();
            return from;
        }
    }
}
