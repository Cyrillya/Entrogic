using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using Terraria.Localization;
using Terraria.Graphics.Effects;
using static Entrogic.Entrogic;
using Entrogic.Walls;
using Entrogic.Tiles;
using Entrogic.Items.AntaGolem;
using Entrogic.Items.Weapons.Ranged.Gun;
using Entrogic.Items.Weapons.Melee.Sword;

namespace Entrogic
{
    public class EntrogicWorld : ModWorld
    {
        public static bool IsDownedPollutionElemental;
        public static bool downedAthanasy;
        public static bool downedGelSymbiosis;
        public static int slimesIn;
        public static bool magicStorm;
        public static bool beArrivedAtUnderworld;
        public static bool _oldIsNight = false;
        public static bool _oldIsDay = false;
        public static int SnowZoneTiles;
        public override void Initialize()
        {
            IsDownedPollutionElemental = false;
            downedAthanasy = false;
            downedGelSymbiosis = false;
            magicStorm = false;
            beArrivedAtUnderworld = false;
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (IsDownedPollutionElemental) downed.Add("pollutionElemental");
            if (downedAthanasy) downed.Add("athanasy");
            if (downedGelSymbiosis) downed.Add("gelSymbiosis");

            var events = new List<string>();
            if (magicStorm) events.Add("magicStorm");
            if (beArrivedAtUnderworld) events.Add("beArrivedAtUnderworld");

            return new TagCompound {
                {"downed", downed},
                {"events", events}
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            IsDownedPollutionElemental = downed.Contains("pollutionElemental");
            downedAthanasy = downed.Contains("athanasy");
            downedGelSymbiosis = downed.Contains("gelSymbiosis");

            var events = tag.GetList<string>("events");
            magicStorm = events.Contains("magicStorm");
            beArrivedAtUnderworld = events.Contains("beArrivedAtUnderworld");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                IsDownedPollutionElemental = flags[0];
                downedAthanasy = flags[1];
                downedGelSymbiosis = flags[2];
                magicStorm = flags[3];
                beArrivedAtUnderworld = flags[4];
            }
            else
            {
                mod.Logger.WarnFormat("Entrogic: Unknown loadVersion: {0}" + loadVersion);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = IsDownedPollutionElemental;
            flags[1] = downedAthanasy;
            flags[2] = downedGelSymbiosis;
            flags[3] = magicStorm;
            flags[4] = beArrivedAtUnderworld;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            IsDownedPollutionElemental = flags[0];
            downedAthanasy = flags[1];
            downedGelSymbiosis = flags[2];
            magicStorm = flags[3];
            beArrivedAtUnderworld = flags[4];
            // As mentioned in NetSend, BitBytes can contain 8 values. If you have more, be sure to read the additional data:
            // BitsByte flags2 = reader.ReadByte();
            // downed9thBoss = flags[0];
        }
        public override void ResetNearbyTileEffects()
        {
            EntrogicPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            SnowZoneTiles = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            SnowZoneTiles = tileCounts[TileID.IceBlock] + tileCounts[TileID.SnowBlock] + tileCounts[TileID.SnowBrick] + tileCounts[TileID.IceBrick] + tileCounts[TileID.BreakableIce] + tileCounts[TileID.CorruptIce] + tileCounts[TileID.FleshIce] + tileCounts[TileID.HallowedIce];
        }
        public override void PostUpdate()
        {
            if (!Main.dayTime)
            {
                if (_oldIsDay && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(15) && NPC.downedBoss1)
                    {
                        ActiveMagicStorm();
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    CustomSky customSky = SkyManager.Instance["Entrogic:MagicStormScreen"];
                    if (magicStorm)
                    {
                        if (!customSky.IsActive())
                        {
                            SkyManager.Instance.Activate("Entrogic:MagicStormScreen", default(Vector2), new object[]
                            {
                            true
                            });
                        }
                    }
                    else if (customSky.IsActive())
                    {
                        SkyManager.Instance.Deactivate("Entrogic:MagicStormScreen", new object[]
                        {
                        default(Vector2),
                        true
                        });
                    }
                }
                int mimicrySpawnRate = 962;
                if (Main.netMode != NetmodeID.MultiplayerClient && magicStorm)
                {
                    mimicrySpawnRate = 565;
                }
                if (Main.hardMode) mimicrySpawnRate -= 182;
                if (Main.rand.NextBool(mimicrySpawnRate) && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    foreach (Player player in Main.player)
                    {
                        if (player.ZoneOverworldHeight && player.active && !player.dead)
                        {
                            Point rand = new Point(Main.rand.Next(-80, 81), Main.rand.Next(-60, 61));
                            Point tilePlrPos = player.position.ToTileCoordinates();
                            if (!SummonMimicryAvailable(new Point(rand.X + (int)tilePlrPos.X, rand.Y + (int)tilePlrPos.Y)))
                                continue;
                            int mimicry = Projectile.NewProjectile(player.position + rand.ToWorldCoordinates(), Vector2.Zero, ProjectileType<Projectiles.Miscellaneous.Arcana>(), 0, 0f, player.whoAmI);
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, mimicry);
                        }
                    }
                }
            }
            else
            {
                if (_oldIsNight && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (magicStorm)
                    {
                        magicStorm = false;
                        if (Main.dedServ)
                        {
                            NetworkText text = NetworkText.FromKey("魔力平静了下来");
                            NetMessage.BroadcastChatMessage(text, new Color(150, 150, 250));
                            var packet = mod.GetPacket();
                            packet.Write((byte)EntrogicModMessageType.ReceiveMagicStormRequest);
                            packet.Write(magicStorm);
                            packet.Send(); // 不填即为发给服务器
                        }
                        else
                        {
                            string text = "魔力平静了下来";
                            Main.NewText(text, 150, 150, 250);
                        }
                    }
                }
            }
            _oldIsDay = Main.dayTime;
            _oldIsNight = !Main.dayTime;
        }
        public bool SummonMimicryAvailable(Point tilePos)
        {
            //if (!Main.rand.NextBool(15000))
            //    return false;
            if (!WorldGen.InWorld(tilePos.X, tilePos.Y))
            {
                DebugModeNewText("Break: Not Safe");
                return false;
            }
            Tile tile = Main.tile[tilePos.X, tilePos.Y];
            if (tile == null)
            {
                DebugModeNewText("Break: Null");
                return false;
            }
            if (tile.active() && Main.tileSolid[tile.type])
            {
                DebugModeNewText("Break: Tile Solid");
                return false;
            }
            return true;
        }
        public void ActiveMagicStorm()
        {
            magicStorm = true;
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("魔力开始涌动...", 150, 150, 250);
            }
            else
            {
                NetworkText text = NetworkText.FromKey("魔力开始涌动...");
                NetMessage.BroadcastChatMessage(text, new Color(150, 150, 250));
                var packet = mod.GetPacket();
                packet.Write((byte)EntrogicModMessageType.ReceiveMagicStormRequest);
                packet.Write(magicStorm);
                packet.Send();
            }
        }
        private int MaxTilesX => Main.maxTilesX;
        private int MaxTilesY => Main.maxTilesY;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int MicroBiomesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (MicroBiomesIndex != -1)
            {
                tasks.Insert(MicroBiomesIndex + 1, new PassLegacy("Generating Card Shrine", delegate (GenerationProgress progress)
                {
                    progress.Message = Language.GetTextValue("Mods.Entrogic.GenCardShrine");
                    try
                    {
                        for (int l = 0; l < (int)((double)(MaxTilesX * MaxTilesY) * 0.00003); l++)
                        {
                            Point psw = new Point(WorldGen.genRand.Next(50, MaxTilesX - 50), WorldGen.genRand.Next((int)((float)MaxTilesY * 0.35f), (int)((float)MaxTilesY * 0.85f)));
                            bool goodPlace = true;
                            for (int i = psw.X; i <= psw.X + 5; i++)
                            {
                                int j = psw.Y + 9;
                                if (!Main.tile[i, j].active() || !Main.tileSolid[Main.tile[i, j].type])
                                {
                                    goodPlace = false;
                                    break;
                                }
                            }
                            if (goodPlace && WorldGen.InWorld(psw.X, psw.Y) && WorldGen.InWorld(psw.X + 8, psw.Y + 16) && Main.tile[psw.X, psw.Y].active() && Main.tile[psw.X, psw.Y].type == TileID.Stone && Main.tile[psw.X + 5, psw.Y].active() && Main.tile[psw.X + 5, psw.Y].type == TileID.Stone)
                            {
                                Rectangle r = Buildings.Build($"Buildings/CardShrine{WorldGen.genRand.Next(2)}.ebuilding", psw.ToWorldCoordinates());
                                ModHelper.FindAndReplaceWall(r, (ushort)WallType<黑陨岩墙>());
                                Chest c = ModHelper.FindAndCreateChest(r, TileType<Tiles.ExplodeGraniteChest>());
                                int index = 0;
                                c.AddItem(ModHelper.GetRandomCard(Main.LocalPlayer, WorldGen.genRand, CardRareID.GrandUnified, -1, false).type, 1, ref index);
                                Item item = ModHelper.GetRandomCard(Main.LocalPlayer, WorldGen.genRand);
                                c.AddItem(item.type, Math.Max(WorldGen.genRand.Next(item.maxStack) + 1 - 2, 1), ref index);
                                c.AddNormalChestItem(psw.Y, index);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("生成世界时发生错误！请将错误发送在QQ群798484146并将 Terraria/ModLoader/Logs/client.txt 发给群主！\n" + e.Message);
                    }
                }));
                int maxX = Main.maxTilesX / 2;
                int maxY = Main.maxTilesY / 2 + 160;
                tasks.Insert(MicroBiomesIndex + 1, new PassLegacy("Generating Life Liquid", delegate (GenerationProgress progress)
                {
                    try
                    {
                        progress.Message = Language.GetTextValue("Mods.Entrogic.GenLifeLiquid");
                        int r = 55;
                        ModGenHelper.RoundTile(maxX, maxY, r, r, 70, 75, true, TileType<BlackMeteorite>(), 1, 0.2);
                        progress.Set(0.20f);
                        ModGenHelper.RoundTile(maxX, maxY, r, r, 0, 24.5, true, TileType<BlackMeteorite>());
                        progress.Set(0.40f);
                        ModGenHelper.RoundTile(maxX, maxY, r - 7, r - 7, 0, 18, true, 0, 2, 0.5, 1, 140);
                        progress.Set(0.60f);
                        int length = 15;
                        for (int j = maxY - 46; j >= maxY - 80; j--)
                        {
                            for (int i = maxX - WorldGen.genRand.Next(-1, 2) - length; i <= maxX + WorldGen.genRand.Next(-1, 2) + length; i++)
                            {
                                if (Main.tile[i, j].type == TileType<BlackMeteorite>())
                                    Main.tile[i, j].active(false);
                            }
                        }
                        progress.Set(0.80f);
                        ModGenHelper.RoundWall(maxX, maxY, r, r, 0, 25, true, WallType<Walls.黑陨岩墙>(), 0, 1);
                        progress.Set(1.00f);

                        progress.Message = Language.GetTextValue("Mods.Entrogic.SmoothLifeLiquid");
                        ModGenHelper.SmoothTile(maxX - 80, maxY - 80, maxX + 80, maxY + 80, progress, true);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("生成世界时发生错误！请将错误发送在QQ群798484146并将 Terraria/ModLoader/Logs/client.txt 发给群主！\n" + e.Message);
                    }
                }));
            }
        }
        public override void PostWorldGen()
        {
            #region 某烤肉
            // 给地牢箱上魔像召唤物
            bool FirstChest = true;
            int itemsToPlaceInDungeonChests = ItemType<TitansOrder>();
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                // If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 3rd chest is the Locked Gold Chest. Since we are counting from 0, this is where 2 comes from. 36 comes from the width of each tile including padding. 
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 2 * 36)
                {
                    if (Main.rand.NextBool(5) || FirstChest)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInDungeonChests);
                                // Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInIceChests));
                                FirstChest = false;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 地表木箱子
            bool FirstChest2 = true;
            int[] itemsToPlaceInWoodenChests = { ItemType<老旧的步枪>(), ItemType<RustySword>() };
            int itemsToPlaceInWoodenChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36)
                {
                    if (Main.rand.NextBool(4) || FirstChest2)
                    {
                        chest.item[0].type = itemsToPlaceInWoodenChests[itemsToPlaceInWoodenChestsChoice];
                        itemsToPlaceInWoodenChestsChoice = (itemsToPlaceInWoodenChestsChoice + 1) % itemsToPlaceInWoodenChests.Length;
                        FirstChest2 = false;
                    }
                }
            }
            #endregion
        }
        #region 生命湖判定
        public static bool Check(float x,float y)
        {
            float X = Main.maxTilesX * 8;
            float Y = Main.maxTilesY * 8 + 160 * 16;
            int R = 66 * 16;
            if ((x - X) * (x - X) + (y - Y) * (y - Y) <= R * R)
                return true;
            return false;
        }
        #endregion

        #region 扩大世界
        /*
           private static void Change(GenerationProgress progress)
        {
            string name = Main.worldName;
            try
            {
                string[] s = name.Split('_');
                if (s[1] == "larger")
                {
                    Main.worldName = s[0];
                    int num = Main.maxTilesY + 200;
                    FieldInfo field = typeof(WorldFileData).GetField("WorldSizeY", BindingFlags.Instance | BindingFlags.Public);
                    field.SetValue(Main.ActiveWorldFileData, num);
                    FieldInfo field2 = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
                    field2.SetValue(null, num);
                    Main.maxTilesY = num;
                    Main.bottomWorld = (float)(num * 16);
                    Main.maxSectionsY = Main.maxTilesY / 150;
                    MethodInfo method = typeof(Main).GetMethod("InitMap", BindingFlags.Instance | BindingFlags.NonPublic);
                    method.Invoke(Main.instance, null);
                    int num2 = Main.maxTilesX * 2;
                    Main.maxTilesX = num2;
                    Tile[,] array = new Tile[16801*2, 2601*2];
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            array[i, j] = new Tile();
                            if (j >= 200 && Math.Abs(i) < Main.maxTilesX / 2)
                            {
                                array[i, j].CopyFrom(Main.tile[i, j - 200]);
                                Main.tile[i, j - 200] = null;
                            }
                            else
                            {
                            }
                        }
                    }
                    Main.tile = array;
                    Main.worldSurface += 200.0;
                    Main.rockLayer += 200.0;
                    WorldGen.worldSurface += 200.0;
                    WorldGen.worldSurfaceHigh += 200.0;
                    WorldGen.worldSurfaceLow += 200.0;
                    WorldGen.rockLayer += 200.0;
                    WorldGen.rockLayerHigh += 200.0;
                    WorldGen.rockLayerLow += 200.0;
                    Main.spawnTileY += 200;
                    Main.dungeonY += 200;
                    for (int l = 0; l < Main.npc.Length; l++)
                    {
                        if (Main.npc[l].type == 22)
                        {
                            NPC npc = Main.npc[l];
                            npc.homeTileY = Main.spawnTileY;
                            npc.position.Y = npc.position.Y + 2800f;
                            return;
                        }
                    }
                }
            }
            catch { }
            }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("Expand World", new WorldGenLegacyMethod(IWorld.Change)));
            int tasks1 = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Shinies"));
            if (tasks1 != -1)
            {
                tasks.Insert(tasks1 + 1, new PassLegacy("", delegate (GenerationProgress progress)
                {
                    progress.Message = "";
                }));
            }
            int tasks2 = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Oceanworld Biomes"));
            if (tasks2 != -1)
            {
                tasks.Insert(tasks2 + 1, new PassLegacy("", delegate (GenerationProgress progress)
                {
                }));
            }
        }
         */
        #endregion
    }
}