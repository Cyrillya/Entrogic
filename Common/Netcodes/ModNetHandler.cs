using Entrogic.Common.Netcodes.PlayerStatus;

namespace Entrogic.Common.Netcodes
{
    internal class ModNetHandler
	{
		// When a lot of handlers are added, it might be wise to automate
		// creation of them
		internal enum EntrogicMessageType : byte
		{
			Dodge,
			BookInfo
		}
		internal static DodgePacketHandler Dodge = new(EntrogicMessageType.Dodge);
		internal static BookInfoPacketHandler BookInfo = new(EntrogicMessageType.BookInfo);
		public static void HandlePacket(BinaryReader r, int fromWho) {
			EntrogicMessageType msgType = (EntrogicMessageType)r.ReadByte();
			switch (msgType) {
				case EntrogicMessageType.Dodge:
					Dodge.HandlePacket(r, fromWho);
					break;
				case EntrogicMessageType.BookInfo:
					BookInfo.HandlePacket(r, fromWho);
					break;
				default:
					Entrogic.Instance.LoggerWarn(string.Format("Entrogic: Unknown Message type: {0}", msgType));
					break;
			}
		}
	}
}
