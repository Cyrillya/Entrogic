using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.Netcodes.PlayerStatus
{
    internal class PlayerStatPacketHandler : PacketHandler
    {
        public const byte SyncSmeltDaggerStats = 1;

        public PlayerStatPacketHandler(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho) {
            switch (reader.ReadByte()) {
                case (SyncSmeltDaggerStats):
                    ReceiveSmeltDaggerStats(reader, fromWho);
                    break;
            }
        }

        public void SendSmeltDaggerStats(int toWho, int fromWho, byte player, float[] rotations) {
            ModPacket packet = GetPacket(SyncSmeltDaggerStats, fromWho);
            packet.Write(player);
            packet.Write((byte)rotations.Length);
            for (int i = 0; i < rotations.Length; i++) {
                packet.Write(rotations[i]);
            }
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSmeltDaggerStats(BinaryReader reader, int fromWho) {
            byte plr = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
                plr = (byte)fromWho;

            Main.player[plr].direction = reader.ReadByte();
            ref float[] rotations = ref Main.player[plr].GetModPlayer<PlayerStats>().RotationsForSmeltDagger;
            int length = reader.ReadByte();
            for (int i = 0; i < rotations.Length && i < length; i++) {
                rotations[i] = reader.ReadSingle();
            }

            if (Main.netMode == NetmodeID.Server) {
                SendSmeltDaggerStats(-1, fromWho, plr, rotations);
            }
        }
    }
}
