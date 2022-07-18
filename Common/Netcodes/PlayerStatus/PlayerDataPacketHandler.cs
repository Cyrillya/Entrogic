using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.Netcodes.PlayerStatus
{
    internal class PlayerDataPacketHandler : PacketHandler
    {
        public const byte SyncMouseWorld = 1;
        public const byte SyncRecoilDegree = 2;

        public PlayerDataPacketHandler(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho) {
            switch (reader.ReadByte()) {
                case (SyncMouseWorld):
                    ReceiveMouseWorld(reader, fromWho);
                    break;
                case (SyncRecoilDegree):
                    ReceiveRecoilDegree(reader, fromWho);
                    break;
            }
        }

        public void SendMouseWold(int toClient, int ignoreClient, Vector2 mouseWorld) {
            ModPacket packet = GetPacket(SyncMouseWorld);
            packet.WritePackedVector2(mouseWorld);
            // 服务器传给其他客户端时要带上玩家索引信息
            if (Main.netMode == NetmodeID.Server)
                packet.Write((byte)ignoreClient);
            packet.Send(toClient, ignoreClient);
        }

        public void ReceiveMouseWorld(BinaryReader reader, int fromWho) {
            var mouseWorld = reader.ReadPackedVector2();
            var plr = fromWho;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                plr = reader.ReadByte();

            Main.player[plr].GetModPlayer<SyncedDataPlayer>().MouseWorld = mouseWorld;

            if (Main.netMode == NetmodeID.Server) {
                SendMouseWold(-1, fromWho, mouseWorld);
            }
        }

        public void SendRecoilDegree(int toClient, int ignoreClient, short degree) {
            ModPacket packet = GetPacket(SyncMouseWorld);
            packet.Write(degree);
            // 服务器传给其他客户端时要带上玩家索引信息
            if (Main.netMode == NetmodeID.Server)
                packet.Write((byte)ignoreClient);
            packet.Send(toClient, ignoreClient);
        }

        public void ReceiveRecoilDegree(BinaryReader reader, int fromWho) {
            short degree = reader.ReadInt16();
            var plr = fromWho;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                plr = reader.ReadByte();

            Main.player[plr].GetModPlayer<GunPlayer>().RecoilDegree = degree;

            if (Main.netMode == NetmodeID.Server) {
                SendRecoilDegree(-1, fromWho, degree);
            }
        }
    }
}
