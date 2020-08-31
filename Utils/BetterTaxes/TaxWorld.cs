using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BetterTaxes
{
    public class TaxWorld : ModWorld
    {
        private bool hasSynced = false;

        public static bool[] ClientBanksList = new bool[BankHandler.SafeTypes.Length];
        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            for (int i = 0; i < BankHandler.SafeTypes.Length; i++)
            {
                flags[i] = ClientBanksList[i];
            }
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            for (int i = 0; i < BankHandler.SafeTypes.Length; i++)
            {
                ClientBanksList[i] = flags[i];
            }
        }

        public override void PostUpdate()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.dayTime && hasSynced) hasSynced = false;
                bool EnableAutoCollect = true;
                if (EnableAutoCollect && !hasSynced && !Main.dayTime && Main.time >= 15000 && Main.time < 16200)
                {
                    ClientBanksList = BankHandler.HasBank();
                    if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
                    hasSynced = true;
                }
            }
        }
    }
}