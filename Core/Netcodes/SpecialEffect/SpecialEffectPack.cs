namespace Entrogic.Core.Netcodes.SpecialEffect
{
    internal class SpecialEffectPack : PacketHandler
    {
        public SpecialEffectPack(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho) {
            switch (reader.ReadByte()) {
            }
        }
    }
}
