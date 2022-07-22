using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.Netcodes.PlayerStatus
{
    internal class BookInfoPack : PacketHandler
	{
		public const byte SyncPage = 1;
		public const byte SyncName = 2;

		public BookInfoPack(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
		}

		public override void HandlePacket(BinaryReader reader, int fromWho) {
			switch (reader.ReadByte()) {
				case (SyncPage):
					ReceivePage(reader, fromWho);
					break;
				case (SyncName):
					ReceiveName(reader, fromWho);
					break;
			}
		}

		public void SendPage(int toWho, int fromWho, byte player, int page, bool open) {
			if (Main.netMode == NetmodeID.SinglePlayer) return;
			var packet = GetPacket(SyncPage);
			packet.Write(player);
			packet.Write(page);
			packet.Write(open);
			packet.Send(toWho, fromWho);
		}

		public void ReceivePage(BinaryReader reader, int fromWho) {
			byte player = reader.ReadByte();
			int page = reader.ReadInt32();
			bool reading = reader.ReadBoolean();
			if (Main.netMode == NetmodeID.Server) {
				SendPage(-1, fromWho, player, page, reading);
			}
			else {
				BookInfoPlayer book = Main.player[player].GetModPlayer<BookInfoPlayer>();
				book.CurrentPage = page;
				book.IsReading = reading;
			}
		}

		public void SendName(int toWho, int fromWho, byte player, string name) {
			if (Main.netMode == NetmodeID.SinglePlayer) return;
			var packet = GetPacket(SyncName);
			packet.Write(player);
			packet.Write(name);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveName(BinaryReader reader, int fromWho) {
			byte player = reader.ReadByte();
			string name = reader.ReadString();
			if (Main.netMode == NetmodeID.Server) {
				SendName(-1, fromWho, player, name);
			}
			else {
				BookInfoPlayer book = Main.player[player].GetModPlayer<BookInfoPlayer>();
				book.BookName = name;
			}
		}
	}
}