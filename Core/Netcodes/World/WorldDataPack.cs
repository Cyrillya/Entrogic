using Entrogic.Core.WorldGeneration;

namespace Entrogic.Core.Netcodes.World
{
    internal class WorldDataPack : PacketHandler
	{
		public const byte SyncGolemRoom = 1;

		public WorldDataPack(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
		}

		public override void HandlePacket(BinaryReader reader, int fromWho) {
			switch (reader.ReadByte()) {
				case SyncGolemRoom:
					ReceiveGolemRoom(reader, fromWho);
					break;
			}
		}

		public void SendGolemRoom(int toWho, int fromWho, Rectangle? room = null) {
			// 客户端，鉴定为请求同步
			var packet = GetPacket(SyncGolemRoom);
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				packet.Send(-1, -1);
				return;
            }
			packet.Write((short)room.Value.X);
			packet.Write((short)room.Value.Y);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveGolemRoom(BinaryReader reader, int fromWho) {
			// 服务器，鉴定为收到请求同步
			if (Main.netMode == NetmodeID.Server) {
				SendGolemRoom(fromWho, -1, ImmortalGolemRoom.BossZone);
				return;
			}
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			ImmortalGolemRoom.BossZone = new(x, y, ImmortalGolemRoom.BossRoomWidth, ImmortalGolemRoom.BossRoomHeight);
		}
	}
}
