using Terraria.ModLoader.IO;

namespace Entrogic.Common.ModSystems
{
    public class DownedHandler : ModSystem
    {
        public static bool DownedSymbiosis;
        public static bool DownedAthanasy;
        public static bool DownedContyElemental;

        public override void OnWorldLoad() {
            base.OnWorldLoad();
            DownedSymbiosis = false;
            DownedAthanasy = false;
            DownedContyElemental = false;
        }

        public override void SaveWorldData(TagCompound tag) {
            base.SaveWorldData(tag);
            if (DownedSymbiosis) tag[nameof(DownedSymbiosis)] = true;
            if (DownedAthanasy) tag[nameof(DownedAthanasy)] = true;
            if (DownedContyElemental) tag[nameof(DownedContyElemental)] = true;
        }

        public override void LoadWorldData(TagCompound tag) {
            base.LoadWorldData(tag);
            DownedSymbiosis = tag.ContainsKey(nameof(DownedSymbiosis));
            DownedAthanasy = tag.ContainsKey(nameof(DownedAthanasy));
            DownedContyElemental = tag.ContainsKey(nameof(DownedContyElemental));
        }

        public override void NetSend(BinaryWriter writer) {
            BitsByte flags = new BitsByte();
            flags[0] = DownedSymbiosis;
            flags[1] = DownedAthanasy;
            flags[2] = DownedContyElemental;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader) {
            BitsByte flags = reader.ReadByte();
            DownedSymbiosis = flags[0];
            DownedAthanasy = flags[1];
            DownedContyElemental = flags[2];
            // As mentioned in NetSend, BitBytes can contain 8 values. If you have more, be sure to read the additional data:
            // BitsByte flags2 = reader.ReadByte();
            // downed9thBoss = flags[0];
        }
    }
}
