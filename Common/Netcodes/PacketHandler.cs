namespace Entrogic.Common.Netcodes
{
    internal abstract class PacketHandler
	{
		internal ModNetHandler.EntrogicMessageType HandlerType { get; set; }

		public abstract void HandlePacket(BinaryReader reader, int fromWho);

		protected PacketHandler(ModNetHandler.EntrogicMessageType handlerType) {
			HandlerType = handlerType;
		}

		protected ModPacket GetPacket(byte packetType) {
			var p = Entrogic.Instance.GetPacket();
			p.Write((byte)HandlerType);
			p.Write(packetType);
			return p;
		}
	}
}
