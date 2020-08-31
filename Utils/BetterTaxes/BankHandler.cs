using Entrogic;

using System;
using Terraria;
using Terraria.ID;

namespace BetterTaxes
{
    public static class BankHandler
    {
        public static bool LastCheckBank = true;
        public static readonly ushort[] SafeTypes = new ushort[] { TileID.PiggyBank, TileID.Safes, TileID.DefendersForge };

        public static bool[] HasBank()
        {
            bool[] data = new bool[SafeTypes.Length];
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int collector = NPC.FindFirstNPC(NPCID.TaxCollector);
                if (collector >= 0 && !Main.npc[collector].homeless)
                {
                    if (Main.npc[collector].homeTileX > 10 && Main.npc[collector].homeTileY > 10 && Main.npc[collector].homeTileX < Main.maxTilesX - 10 && Main.npc[collector].homeTileY < Main.maxTilesY)
                    {
                        data = HasBank(Main.npc[collector].homeTileX, Main.npc[collector].homeTileY - 1);
                    }
                }
            }
            return data;
        }

        public static bool[] HasBank(int x, int y)
        {
            bool[] data = new bool[SafeTypes.Length];

            bool succeededRoomCheck = false;
            try
            {
                succeededRoomCheck = WorldGen.StartRoomCheck(x, y); // this seems to have some problems in some scenarios, so we catch it just in case to avoid outright error
            }
            catch (Exception e)
            {
                Entrogic.Entrogic.Instance.Logger.Warn("Failed to check for the Tax Collector's house (please report): " + e.ToString());
                return data;
            }
            if (!succeededRoomCheck) return data;

            for (int i = 0; i < SafeTypes.Length; i++)
            {
                data[i] = WorldGen.houseTile[SafeTypes[i]];
            }
            return data;
        }

#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        internal static void DoCoins(Chest bank, int i)
        {
            if (bank.item[i].stack != 100 || (bank.item[i].type != 71 && bank.item[i].type != 72 && bank.item[i].type != 73))
            {
                return;
            }
            bank.item[i].SetDefaults(bank.item[i].type + 1);
            for (int j = 0; j < bank.item.Length; j++)
            {
                if (bank.item[j].IsTheSameAs(bank.item[i]) && j != i && bank.item[j].type == bank.item[i].type && bank.item[j].stack < bank.item[j].maxStack)
                {
                    bank.item[j].stack++;
                    bank.item[i].SetDefaults();
                    bank.item[i].active = false;
                    bank.item[i].TurnToAir();
                    DoCoins(bank, j);
                }
            }
        }

        public static bool AddCoins(Chest bank, int num)
        {
            Item[] array = new Item[bank.item.Length];
            for (int i = 0; i < bank.item.Length; i++)
            {
                array[i] = new Item();
                array[i] = bank.item[i].Clone();
            }

            bool flag = false;
            while (num >= 1000000 && !flag)
            {
                int num2 = -1;
                for (int num3 = bank.item.Length - 1; num3 >= 0; num3--)
                {
                    if (num2 == -1 && (bank.item[num3].type == 0 || bank.item[num3].stack == 0))
                    {
                        num2 = num3;
                    }
                    while (bank.item[num3].type == 74 && bank.item[num3].stack < bank.item[num3].maxStack && num >= 1000000)
                    {
                        bank.item[num3].stack++;
                        num -= 1000000;
                        DoCoins(bank, num3);
                        if (bank.item[num3].stack == 0 && num2 == -1)
                        {
                            num2 = num3;
                        }
                    }
                }
                if (num >= 1000000)
                {
                    if (num2 == -1)
                    {
                        flag = true;
                        continue;
                    }
                    bank.item[num2].SetDefaults(74);
                    num -= 1000000;
                }
            }
            while (num >= 10000 && !flag)
            {
                int num4 = -1;
                for (int num5 = bank.item.Length - 1; num5 >= 0; num5--)
                {
                    if (num4 == -1 && (bank.item[num5].type == 0 || bank.item[num5].stack == 0))
                    {
                        num4 = num5;
                    }
                    while (bank.item[num5].type == 73 && bank.item[num5].stack < bank.item[num5].maxStack && num >= 10000)
                    {
                        bank.item[num5].stack++;
                        num -= 10000;
                        DoCoins(bank, num5);
                        if (bank.item[num5].stack == 0 && num4 == -1)
                        {
                            num4 = num5;
                        }
                    }
                }
                if (num >= 10000)
                {
                    if (num4 == -1)
                    {
                        flag = true;
                        continue;
                    }
                    bank.item[num4].SetDefaults(73);
                    num -= 10000;
                }
            }
            while (num >= 100 && !flag)
            {
                int num6 = -1;
                for (int num7 = bank.item.Length - 1; num7 >= 0; num7--)
                {
                    if (num6 == -1 && (bank.item[num7].type == 0 || bank.item[num7].stack == 0))
                    {
                        num6 = num7;
                    }
                    while (bank.item[num7].type == 72 && bank.item[num7].stack < bank.item[num7].maxStack && num >= 100)
                    {
                        bank.item[num7].stack++;
                        num -= 100;
                        DoCoins(bank, num7);
                        if (bank.item[num7].stack == 0 && num6 == -1)
                        {
                            num6 = num7;
                        }
                    }
                }
                if (num >= 100)
                {
                    if (num6 == -1)
                    {
                        flag = true;
                        continue;
                    }
                    bank.item[num6].SetDefaults(72);
                    num -= 100;
                }
            }
            while (num >= 1 && !flag)
            {
                int num8 = -1;
                for (int num9 = bank.item.Length - 1; num9 >= 0; num9--)
                {
                    if (num8 == -1 && (bank.item[num9].type == 0 || bank.item[num9].stack == 0))
                    {
                        num8 = num9;
                    }
                    while (bank.item[num9].type == 71 && bank.item[num9].stack < bank.item[num9].maxStack && num >= 1)
                    {
                        bank.item[num9].stack++;
                        num--;
                        DoCoins(bank, num9);
                        if (bank.item[num9].stack == 0 && num8 == -1)
                        {
                            num8 = num9;
                        }
                    }
                }
                if (num >= 1)
                {
                    if (num8 == -1)
                    {
                        flag = true;
                        continue;
                    }
                    bank.item[num8].SetDefaults(71);
                    num--;
                }
            }
            if (flag)
            {
                for (int j = 0; j < bank.item.Length; j++)
                {
                    bank.item[j] = array[j].Clone();
                }
                return false;
            }
            return true;
        }
    }
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
}
