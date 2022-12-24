using Entrogic.Core.Global.Player;

namespace Entrogic.Core.Netcodes.PlayerStatus
{
    internal class PlayerDataPack : PacketHandler
    {
        public const byte SyncMouseWorld = 1;

        public PlayerDataPack(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho) {
            switch (reader.ReadByte()) {
                case (SyncMouseWorld):
                    ReceiveMouseWorld(reader, fromWho);
                    break;
            }
        }

        public void SendMouseWold(int toClient, int ignoreClient, Vector2 mouseWorld) {
            ModPacket packet = GetPacket(SyncMouseWorld);
            packet.WriteVector2(mouseWorld);
            // 服务器传给其他客户端时要带上玩家索引信息
            if (Main.netMode == NetmodeID.Server)
                packet.Write((byte)ignoreClient);
            packet.Send(toClient, ignoreClient);
        }

        public void ReceiveMouseWorld(BinaryReader reader, int fromWho) {
            var mouseWorld = reader.ReadVector2();
            var plr = fromWho;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                plr = reader.ReadByte();

            Main.player[plr].GetModPlayer<SyncedDataPlayer>().MouseWorld = mouseWorld;

            if (Main.netMode == NetmodeID.Server) {
                SendMouseWold(-1, fromWho, mouseWorld);
            }
        }
    }
}
