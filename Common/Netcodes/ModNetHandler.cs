using Entrogic.Common.Netcodes.PlayerStatus;
using Entrogic.Common.Netcodes.SpecialEffect;
using Entrogic.Common.Netcodes.World;

namespace Entrogic.Common.Netcodes
{
    internal class ModNetHandler
    {
        internal enum EntrogicMessageType : byte
        {
            Dodge,
            BookInfo,
            PlayerStat,
            SpecialEffect,
            WorldData
        }
        internal static DodgePack Dodge = new(EntrogicMessageType.Dodge);
        internal static BookInfoPack BookInfo = new(EntrogicMessageType.BookInfo);
        internal static PlayerDataPack PlayerData = new(EntrogicMessageType.PlayerStat);
        internal static SpecialEffectPack SpecialEffect = new(EntrogicMessageType.SpecialEffect);
        internal static WorldDataPack WorldData = new(EntrogicMessageType.WorldData);
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
                case EntrogicMessageType.WorldData:
                    WorldData.HandlePacket(r, fromWho);
                    break;
                default:
                    Entrogic.Instance.LoggerWarn(string.Format("Entrogic: Unknown Message type: {0}", msgType));
                    break;
            }
        }
    }
}
