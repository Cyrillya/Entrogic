﻿using Entrogic.Common.Globals.Players;

namespace Entrogic.Common.Netcodes.PlayerStatus
{
    internal class DodgePacketHandler : PacketHandler
	{
		public const byte SyncContaminatedDodge = 1;

		public DodgePacketHandler(ModNetHandler.EntrogicMessageType handlerType) : base(handlerType) {
		}

		public override void HandlePacket(BinaryReader reader, int fromWho) {
			switch (reader.ReadByte()) {
				case (SyncContaminatedDodge):
					ReceiveContaminatedDodge(reader, fromWho);
					break;
			}
		}

		public void SendContaminatedDodge(int toWho, int fromWho, byte player) {
			ModPacket packet = GetPacket(SyncContaminatedDodge, fromWho);
			packet.Write(player);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveContaminatedDodge(BinaryReader reader, int fromWho) {
			byte plr = reader.ReadByte();
			if (Main.netMode == NetmodeID.Server)
				plr = (byte)fromWho;

			Main.player[plr].GetModPlayer<ContaEffectPlayer>().ContaminatedDodge();

			if (Main.netMode == NetmodeID.Server) {
				SendContaminatedDodge(-1, fromWho, plr);
			}
		}
	}
}