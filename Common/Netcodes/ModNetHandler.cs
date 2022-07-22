﻿using Entrogic.Common.Netcodes.PlayerStatus;
using Entrogic.Common.Netcodes.SpecialEffect;

namespace Entrogic.Common.Netcodes
{
    internal class ModNetHandler
    {
        // When a lot of handlers are added, it might be wise to automate
        // creation of them
        internal enum EntrogicMessageType : byte
        {
            Dodge,
            BookInfo,
            PlayerStat,
            SpecialEffect
        }
        internal static DodgePacketHandler Dodge = new(EntrogicMessageType.Dodge);
        internal static BookInfoPacketHandler BookInfo = new(EntrogicMessageType.BookInfo);
        internal static PlayerDataPacketHandler PlayerData = new(EntrogicMessageType.PlayerStat);
        internal static SpecialEffectPacketHandler SpecialEffect = new(EntrogicMessageType.SpecialEffect);
        public static void HandlePacket(BinaryReader r, int fromWho) {
            EntrogicMessageType msgType = (EntrogicMessageType)r.ReadByte();
            switch (msgType) {
                case EntrogicMessageType.Dodge:
                    Dodge.HandlePacket(r, fromWho);
                    break;
                case EntrogicMessageType.BookInfo:
                    BookInfo.HandlePacket(r, fromWho);
                    break;
                case EntrogicMessageType.PlayerStat:
                    PlayerData.HandlePacket(r, fromWho);
                    break;
                case EntrogicMessageType.SpecialEffect:
                    SpecialEffect.HandlePacket(r, fromWho);
                    break;
                default:
                    Entrogic.Instance.LoggerWarn(string.Format("Entrogic: Unknown Message type: {0}", msgType));
                    break;
            }
        }
    }
}
