using Terraria.ModLoader.IO;

namespace Entrogic.Core.Global
{
    public class DownedHandler : ModSystem
    {
        public static bool DownedSymbiosis;
        public static bool DownedAthanasy;
        public static bool DownedContyElemental;

        public override void OnWorldLoad() {
            DownedSymbiosis = false;
            DownedAthanasy = false;
            DownedContyElemental = false;
        }

        public override void SaveWorldData(TagCompound tag) {
            if (DownedSymbiosis) tag["symbiosis"] = true;
            if (DownedAthanasy) tag["athanasy"] = true;
            if (DownedContyElemental) tag["contaminated"] = true;
        }

        public override void LoadWorldData(TagCompound tag) {
            DownedSymbiosis = tag.ContainsKey("symbiosis");
            DownedAthanasy = tag.ContainsKey("athanasy");
            DownedContyElemental = tag.ContainsKey("contaminated");
        }

        public override void NetSend(BinaryWriter writer) {
            writer.Write(new BitsByte(
                DownedSymbiosis,
                DownedAthanasy,
                DownedContyElemental
            ));
        }

        public override void NetReceive(BinaryReader reader) {
            BitsByte flags = reader.ReadByte();
            DownedSymbiosis = flags[0];
            DownedAthanasy = flags[1];
            DownedContyElemental = flags[2];
        }
    }
}