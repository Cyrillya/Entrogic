namespace Entrogic.Common.Netcodes.SpecialEffect
{
    internal class SpecialEffectPacketHandler : PacketHandler
    {
        public SpecialEffectPacketHandler(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho) {
            switch (reader.ReadByte()) {
            }
        }
    }
}
