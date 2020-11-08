using Entrogic.Items.PollutElement;
using Entrogic.NPCs.Boss.凝胶Java盾;
using Entrogic.Items.VoluGels;
using Entrogic.Items.VoluGels.Armor;
using Entrogic.Items.Miscellaneous.Placeable.Trophy;
using Entrogic.UI.Books;
using Entrogic.UI.CardGame;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.AntaGolem;
using Entrogic.NPCs.Boss.AntaGolem;
using Entrogic.NPCs.Boss.PollutElement;
using Entrogic.Items.PollutElement.Armor;
using Entrogic.UI.Cards;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Entrogic.Items.Weapons.Card;
using Terraria.GameContent.UI;
using Terraria.UI.Chat;
using MonoMod.Cil;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader.Engine;
using Entrogic.Items.Materials;
using Entrogic.NPCs;
using System.Text;
using System.Net;
using Terraria.Graphics;
using Terraria.ObjectData;
using Terraria.GameContent.Events;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Entrogic.NPCs.CardFightable.CardBullet;
using Entrogic.NPCs.CardMerchantSystem;
using Entrogic.Projectiles.Miscellaneous;
using Entrogic.Tiles;
using ReLogic.Content;
using Terraria.GameContent.NetModules;

namespace Entrogic
{
    public class Entrogic : Mod
    {
        #region Fields
        internal bool cursurIconEnabled = false;
        internal Texture2D cursorIconTexture = null;
        internal static int MimicryFrameCounter = 8;
        internal static List<ModItem> ModItems => new List<ModItem>(typeof(ItemLoader).GetFields(BindingFlags.Static | 
            BindingFlags.NonPublic).Where(field => field.Name == "items").First().GetValue(null) as IList<ModItem>);
        internal static List<ModTile> ModTiles => new List<ModTile>(typeof(TileLoader).GetFields(BindingFlags.Static | 
            BindingFlags.NonPublic).Where(field => field.Name == "tiles").First().GetValue(null) as IList<ModTile>);
        internal static List<ModProjectile> ModProjectiles => new List<ModProjectile>(typeof(ProjectileLoader).GetFields(BindingFlags.Static |
            BindingFlags.NonPublic).Where(field => field.Name == "projectiles").First().GetValue(null) as IList<ModProjectile>);
        internal static int MaxItemTypes => (int)(typeof(ItemLoader).GetFields(BindingFlags.Static |
            BindingFlags.NonPublic).Where(field => field.Name == "nextItem").First().GetValue(null));
        internal static ModHotKey PassHotkey;
        internal static ModHotKey WashHotkey;
        internal static ModHotKey HookCursorHotKey;
        internal Vector2 HookCursor;

        internal static Dictionary<string, Asset<Texture2D>> ModTexturesTable = new Dictionary<string, Asset<Texture2D>>();
        [Obsolete]
        internal static Dictionary<string, CardFightBullet> cfBullets = new Dictionary<string, CardFightBullet>();
        public static List<Quest> CardQuests = new List<Quest>();
        private int foolTexts = 0;
        internal static bool Unloading = false;
        internal static readonly string ModFolder;
        internal static bool IsDev = false;
        internal static int ModTimer; 
        internal static Entrogic Instance;
        public static Asset<DynamicSpriteFont> PixelFont => Instance.GetFont("Fonts/JOJOHOTXiangSubeta");

        public static int MimicryCustomCurrencyId;
        #endregion
        public override uint ExtraPlayerBuffSlots => (uint)(GetInstance<BEntrogicConfigServer>().MaxBuffSlots - 22);
        public override void AddRecipes() => RecipeManager.Load(this);
        static Entrogic()
        {
            ModFolder = $"{Main.SavePath}{Path.DirectorySeparatorChar}Mod Configs{Path.DirectorySeparatorChar}Entrogic{Path.DirectorySeparatorChar}";
        }
        public Entrogic()
        {
            Instance = this;
            Directory.CreateDirectory(ModFolder);
        }
        public override void Load()
        {
            PassHotkey = RegisterHotKey("过牌快捷键", "E");
            WashHotkey = RegisterHotKey("洗牌快捷键", "Q");
            HookCursorHotKey = RegisterHotKey("设置钩爪指针快捷键", "C");
            On.Terraria.Player.QuickGrapple += Player_QuickGrapple;
            On.Terraria.Player.ItemCheck += UnderworldTransportCheck;
            On.Terraria.Main.DrawInterface_40_InteractItemIcon += CustomHandIcon;
            foolTexts = Main.rand.Next(3);
            Unloading = false;
            MimicryCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new EntrogicMimicryCurrency(ItemType<拟态魔能>(), 999L));
            if (!Main.dedServ)
            {
                ResourceLoader.LoadAllTextures();
                ResourceLoader.LoadAllCardMissions();

                AddEquipTexture(new PollutionElementalMask1(), new PollutionElementalMask(), EquipType.Head, "Entrogic/Items/PollutElement/PollutionElementalMask1_Head");
                AddEquipTexture(new PollutionElementalMask2(), new PollutionElementalMask(), EquipType.Head, "Entrogic/Items/PollutElement/PollutionElementalMask2_Head");
                AddEquipTexture(new PollutionElementalMask3(), new PollutionElementalMask(), EquipType.Head, "Entrogic/Items/PollutElement/PollutionElementalMask3_Head");
                AddEquipTexture(new PollutionElementalMask4(), new PollutionElementalMask(), EquipType.Head, "Entrogic/Items/PollutElement/PollutionElementalMask4_Head");

                AddEquipTexture(new PolluWings1(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings1_Wings");
                AddEquipTexture(new PolluWings2(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings2_Wings");
                AddEquipTexture(new PolluWings3(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings3_Wings");
                AddEquipTexture(new PolluWings4(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings4_Wings");
                AddEquipTexture(new PolluWings5(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings5_Wings");
                AddEquipTexture(new PolluWings6(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings6_Wings");
                AddEquipTexture(new PolluWings7(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings7_Wings");
                AddEquipTexture(new PolluWings8(), new BottleofStorm(), EquipType.Wings, "Entrogic/Items/PollutElement/PolluWings8_Wings");

                ResourceLoader.LoadAllShaders();
            }
            Buildings.Cache("Buildings/CardShrine0.ebuilding", "Buildings/CardShrine1.ebuilding", "Buildings/UnderworldPortal.ebuilding");
            #region Armor Translations
            Translation.RegisterTranslation("mspeed", GameCulture.CultureName.Chinese, "移动速度", " movement speed");
            Translation.RegisterTranslation("and", GameCulture.CultureName.Chinese, "与", " and");
            Translation.RegisterTranslation("csc", GameCulture.CultureName.Chinese, "暴击率", " critical strike chance");
            Translation.RegisterTranslation("knockback", GameCulture.CultureName.Chinese, "击退", " knockback");
            Translation.RegisterTranslation("damage", GameCulture.CultureName.Chinese, "伤害", " damage");
            Translation.RegisterTranslation("cntca", GameCulture.CultureName.Chinese, "的几率不消耗弹药", " chance not to consume ammo");
            Translation.RegisterTranslation("immb", GameCulture.CultureName.Chinese, "最大魔力值增加", "Increases maximum mana by ");
            Translation.RegisterTranslation("rmub", GameCulture.CultureName.Chinese, "魔力消耗减少", "Reduces mana usage by ");
            #endregion
            #region Boss Checklist Translations
            ModTranslation bctext = CreateTranslation("BossSpawnInfo.GelSymb");
            bctext.AddTranslation((int)GameCulture.CultureName.Chinese, "在地下原汤湖使用 [i:" + ItemType<GelCultureFlask>() + "] 召唤一只史莱姆, 并将其掷于地下原汤湖中召唤");
            bctext.SetDefault("Use [i:" + ItemType<GelCultureFlask>() + "] in the underground pool to summon a slime, and thorw it into the pool to summon.");
            AddTranslation(bctext);
            bctext = CreateTranslation("BossSpawnInfo.Athanasy");
            bctext.AddTranslation((int)GameCulture.CultureName.Chinese, "使用 [i:" + ItemType<TitansOrder>() + "] 召唤（由地牢怪物掉落或在上锁的金箱中找到）");
            bctext.SetDefault("Use [i:" + ItemType<TitansOrder>() + "] to spawn, you can find it from locked chests or drop from dungeon monsters");
            AddTranslation(bctext);
            bctext = CreateTranslation("BossSpawnInfo.PollutionElement");
            bctext.AddTranslation((int)GameCulture.CultureName.Chinese, "在海边使用 [i:" + ItemType<ContaminatedLiquor>() + "] 召唤");
            bctext.SetDefault("Use [i:" + ItemType<ContaminatedLiquor>() + "] in ocean to spawn");
            AddTranslation(bctext);
            #endregion
            #region Another Translations
            ModTranslation transform = CreateTranslation("RightClickToTransform");
            transform.AddTranslation((int)GameCulture.CultureName.Chinese, "右键点击物品以切换状态");
            transform.SetDefault("Right click to switch status");
            AddTranslation(transform);
            ModTranslation modTranslation = CreateTranslation("Pollution_SkyDarkened");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "天空变得更加黑暗");
            modTranslation.SetDefault("The sky is becomes darkened");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Pollutional");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "污染生物正在聚集...");
            modTranslation.SetDefault("Pollutional creatures are gathering...");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Summon");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "永远不要尝试去挑战自然...");
            modTranslation.SetDefault("Never try to challenge the nature...");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Summon2");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "呵...");
            modTranslation.SetDefault("Heh...");
            AddTranslation(modTranslation);
            ModTranslation text = CreateTranslation("NPCTalk");
            text.SetDefault("<{0}> {1}");
            AddTranslation(text);
            text = CreateTranslation("Common.RandomCardImage");
            text.SetDefault($"[i:{ItemType<RandomCard>()}] [c/ffeb6e:卡牌系统自定义项]");
            AddTranslation(text);
            ModTranslation modGen = CreateTranslation("GenLifeLiquid");
            modGen.AddTranslation((int)GameCulture.CultureName.Chinese, "正在生成生命湖");
            modGen.SetDefault("Life.");
            AddTranslation(modGen);
            modGen = CreateTranslation("SmoothLifeLiquid");
            modGen.AddTranslation((int)GameCulture.CultureName.Chinese, "正在平整生命湖");
            modGen.SetDefault("Life.");
            AddTranslation(modGen);
            modGen = CreateTranslation("GenCardShrine");
            modGen.AddTranslation((int)GameCulture.CultureName.Chinese, "正在生成卡牌神龛");
            modGen.SetDefault("Card.");
            AddTranslation(modGen);
            #endregion
        }

        private void CustomHandIcon(On.Terraria.Main.orig_DrawInterface_40_InteractItemIcon orig, Main self)
        {
            if (cursurIconEnabled && cursorIconTexture != null)
            {
                float scale = Main.cursorScale;
                Main.spriteBatch.Draw(cursorIconTexture, 
                    new Vector2((float)(Main.mouseX + 12), (float)(Main.mouseY + 12)), 
                    null, Color.White, 0f, default, scale, SpriteEffects.None, 0f);

                cursurIconEnabled = false;
                return;
            }
            orig(self);
        }

        private void UnderworldTransportCheck(On.Terraria.Player.orig_ItemCheck orig, Player player, int i)
        {
            Item item = player.inventory[player.selectedItem];
            if (player.controlUseItem && player.controlUseTile && item.type == ItemID.LavaBucket && 
                Main.tile[Player.tileTargetX, Player.tileTargetY].type == TileID.Obsidian &&
                ModWorldHelper.CreateUnderworldTransport(Player.tileTargetX, Player.tileTargetY)) // 最后这个是生成
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DoorOpen, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                item.stack--;
                player.PutItemInInventoryFromItemUsage(ItemID.EmptyBucket, player.selectedItem);
                player.itemTime = (int)((float)item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }
            else
            {
                orig(player, i);
            }
        }

        public override void Unload()
        {
            Unloading = true;
            try
            {
                //Main.OnPostDraw -= Main_OnPostDraw;
                //Main.OnPreDraw -= Main_OnPreDraw;
                ModTexturesTable.Clear();
                PassHotkey = null;
                WashHotkey = null;
                HookCursorHotKey = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Instance = null;
                GC.Collect();
            }

            base.Unload();

        }
        private void Player_QuickGrapple(On.Terraria.Player.orig_QuickGrapple orig, Player self)
        {
            if (AEntrogicConfigClient.Instance.HookMouse)
            {
                int mouseX = Main.mouseX;
                int mouseY = Main.mouseY;
                Main.mouseX = (int)HookCursor.X;
                Main.mouseY = (int)HookCursor.Y;
                orig(self);
                Main.mouseX = mouseX;
                Main.mouseY = mouseY;
            }
            else orig(self);
        }

        /// <summary>
        /// Change the following code sequence in Main.DrawMenu
        ///
		/// else if (menuMode == -7)
		/// {
		///	    array9[2] = Language.GetTextValue("UI.Normal");
		///	    array9[3] = Language.GetTextValue("UI.Expert");
		///	    array9[4] = Language.GetTextValue("UI.Back");
		///	    num5 = 5;
        /// }
        /// 
        /// to 
        /// 
		/// else if (menuMode == -7)
		/// {
		///	    array9[2] = Language.GetTextValue("UI.Normal");
		///	    array9[3] = Language.GetTextValue("UI.Expert");
		///	    array9[4] = Language.GetTextValue("UI.Back");
		///	    array9[5] = "TEST";
		///	    num5 = 6;
        /// }
        /// 
        /// </summary>
        /// <param name="il"></param>
        private void UpdateExtraWorldDiff(ILContext il)
        {
            // obtain a cursor positioned before the first instruction of the method
            // the cursor is used for navigating and modifying the il
            var c = new ILCursor(il);
            // 为了这个我TM反射都用上了
            FieldInfo fieldInfo = typeof(Main).GetFields(BindingFlags.Static | BindingFlags.Public).Where(field => field.Name == "menuMode").First();
            while (c.TryGotoNext(i => i.MatchLdsfld(fieldInfo)))
            {
                // ldc.i4.s -7
                if (!c.TryGotoNext(i => i.MatchLdcI4(-7)))
                {
                    continue;
                }
                if (!c.TryGotoNext(i => i.MatchLdstr("UI.Expert")))
                {
                    continue;
                }
                c.Index += 2;
                c.EmitDelegate<Func<string, string>>(str =>
                {
                    str = "我没在开玩笑！！";
                    return str;
                });

                // hook applied successfully
                return;
            }

            // couldn't find the right place to insert
            throw new Exception("未在Main.DrawMenu找到if(menuMode==-7)，请将此Bug报告给作者Cyril!联系方式在Mod简介");
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EntrogicModMessageType msgType = (EntrogicModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case EntrogicModMessageType.ReceiveMagicStormRequest: // 由服务器通告全体客户端
                    {
                        EntrogicWorld.magicStorm = reader.ReadBoolean();
                        break;
                    }
                case EntrogicModMessageType.ReceiveMagicStormMPC: // 客户端发出接受请求，服务器发送参数
                    if (Main.dedServ) // 在服务器下
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)EntrogicModMessageType.ReceiveMagicStormMPC);
                        packet.Write(EntrogicWorld.magicStorm);
                        // 发回给发送者
                        packet.Send(whoAmI, -1);
                        break;
                    }
                    else // 客户端下接收
                    {
                        EntrogicWorld.magicStorm = reader.ReadBoolean();
                        if (EntrogicWorld.magicStorm == true/*规范！*/) // 肯定是要开始了才提示啊，不然提示什么东西哦
                            Main.NewText("魔力开始涌动...", 150, 150, 250); // 补一个提示
                        break;
                    }
                case EntrogicModMessageType.FindUWTeleporter:
                    {
                        if (Main.dedServ) // 在服务器下
                        {
                            int i = reader.ReadInt32();
                            int j = reader.ReadInt32();
                            int playernum = reader.ReadInt32();
                            UnderworldPortal.HandleTransportion(i, j, playernum, true);
                            break;
                        }
                        else // 在客户端下，其实就是给玩家回复提示信息
                        {
                            string warn = reader.ReadString();
                            Main.NewText(warn);
                        }
                        break;
                    }
                case EntrogicModMessageType.NPCSpawnOnPlayerAction:
                    {
                        int plr = (int)reader.ReadByte();
                        int type = (int)reader.ReadInt16();
                        if (!NPC.AnyNPCs(type))
                        {
                            NPC.SpawnOnPlayer(plr, type);
                        }
                        break;
                    }
                case EntrogicModMessageType.NPCSpawn:
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient) break;
                        int plr = (int)reader.ReadByte();
                        int type = (int)reader.ReadInt16();
                        bool showSpawnText = reader.ReadBoolean();
                        float posx = reader.ReadSingle();
                        float posy = reader.ReadSingle();
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, Main.player[plr].Center, 0);
                        int npc = NPC.NewNPC((int)posx, (int)posy, type, 1);
                        if (npc == 200)
                            break;
                        Main.npc[npc].target = plr;
                        Main.npc[npc].timeLeft *= 20;
                        if (npc < 200)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
                        }
                        if (showSpawnText == true)
                        {
                            NetTextModule.SerializeServerMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[npc].GetTypeNetName()), new Color(175, 75, 255));
                        }
                        break;
                    }
                case EntrogicModMessageType.SyncExplode:
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            Explode(reader.ReadPackedVector2(), reader.ReadPackedVector2(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadBoolean());
                        else
                        {
                            MessageHelper.SendExplode(reader.ReadPackedVector2(), reader.ReadPackedVector2(), reader.ReadInt32(), -1, whoAmI, reader.ReadInt32(), reader.ReadInt32(), reader.ReadBoolean());
                        }
                        break;
                    }
                case EntrogicModMessageType.SyncCardGamingInfos:
                    {
                        int playernumber = reader.ReadByte();
                        EntrogicPlayer entrogicPlayer = Main.player[playernumber].GetModPlayer<EntrogicPlayer>();
                        // 如何传输List？首先传输一个List.Count，然后遍历传输List数值，接收者根据List.Count逐个接收，最后List.Add发给第三方
                        //entrogicPlayer.IsBookActive = reader.ReadBoolean();
                        //// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                        //if (Main.netMode == NetmodeID.Server)
                        //{
                        //    var packet = GetPacket();
                        //    packet.Write((byte)EntrogicModMessageType.SyncBookBubbleInfo);
                        //    packet.Write((byte)playernumber);
                        //    packet.Write(entrogicPlayer.IsBookActive);
                        //    packet.Send(-1, playernumber);
                        //}
                        break;
                    }
                case EntrogicModMessageType.SyncBookBubbleInfo:
                    {
                        int playernumber = reader.ReadByte();
                        EntrogicPlayer entrogicPlayer = Main.player[playernumber].GetModPlayer<EntrogicPlayer>();
                        entrogicPlayer.PageNum = reader.ReadByte();
                        entrogicPlayer.IsBookActive = reader.ReadBoolean();
                        // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                        if (Main.netMode == NetmodeID.Server)
                        {
                            MessageHelper.SendBookInfo(playernumber, entrogicPlayer.PageNum, entrogicPlayer.IsBookActive, -1, playernumber);
                        }
                        break;
                    }
                case EntrogicModMessageType.SendCompletedCardMerchantMissionRequest:
                    {
                        byte playernumber = reader.ReadByte();
                        CardMerchantQuest cardMerchantQuest = Main.player[playernumber].GetModPlayer<CardMerchantQuest>();
                        cardMerchantQuest.Complete = reader.ReadString();
                        // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                        if (Main.netMode == NetmodeID.Server)
                        {
                            MessageHelper.SendCardMission(playernumber, cardMerchantQuest.Complete, -1, playernumber);
                        }
                        break;
                    }
                case EntrogicModMessageType.BuildBuilding:
                    {
                        string name = reader.ReadString();
                        Vector2 pos = reader.ReadPackedVector2();
                        bool useAir = reader.ReadBoolean();
                        Buildings.Build(name, pos, useAir);
                        break;
                    }
                default:
                    Logger.WarnFormat("Entrogic: Unknown Message type: {0}", msgType);
                    break;
            }
        }
        internal enum EntrogicModMessageType : byte
        {
            ReceiveMagicStormRequest,
            ReceiveMagicStormMPC,
            FindUWTeleporter,
            NPCSpawnOnPlayerAction,
            NPCSpawn,
            SyncExplode,
            SyncBookBubbleInfo,
            SyncCardGamingInfos,
            SendCompletedCardMerchantMissionRequest,
            BuildBuilding
        }
        public static void DebugModeNewText(string message, bool debug = false)
        {
            if (Main.netMode == NetmodeID.SinglePlayer && debug)
            {
                Main.NewText(message);
            }
        }
        public static void WriteAndOpenFile(string path, string message)
        {
            FileInfo fieldInfo = new FileInfo(path);
            File.WriteAllText(path, message);
            Console.Read();
            Process.Start(path);
        }
        public static Vector2 Vector2Abs(Vector2 vec)
        {
            return new Vector2(Math.Abs(vec.X), Math.Abs(vec.Y));
        }
        public static Point PointAdd(Point poi1, Point poi2)
        {
            return new Point(poi1.X + poi2.X, poi1.Y + poi2.Y);
        }
        /// <summary>
        /// 当前Tile是否属于原版中像泥土一样松软的物块
        /// </summary>
        public static bool TileDirt(Tile tile)
        {
            if (tile != null)
            {
                if (tile.active() && Main.tileSolid[tile.type]) // tileSolid看上去没必要，实际上是为了防止我写判断条件的时候写错
                {
                    return tile.type == TileID.Dirt || tile.type == TileID.SnowBlock || tile.type == TileID.Mud || tile.type == TileID.Grass || tile.type == TileID.JungleGrass || tile.type == TileID.HallowedGrass || tile.type == TileID.CrimsonGrass || tile.type == TileID.CorruptGrass || tile.type == TileID.MushroomGrass || tile.type == TileID.Sand || tile.type == TileID.Silt || tile.type == TileID.Ebonsand || tile.type == TileID.Crimsand || tile.type == TileID.Pearlsand || tile.type == TileID.ClayBlock || tile.type == TileID.Ash || tile.type == TileID.Cloud || tile.type == TileID.RainCloud || tile.type == TileID.SnowCloud || tile.type == TileID.Slush;
                }
            }
            return false;
        }
        /// <summary>
        /// 当前Item是否存在
        /// </summary>
        public static bool ItemSafe(Item item)
        {
            if (item != null && item.active && item.type != ItemID.None)
            {
                return true;
            }
            return false;
        }
        /// <param name="friendly">0为敌对，1为友好，2为全部伤害</param>
        internal static void Explode(Vector2 position, Vector2 size, int damage, int friendly = 0, int goreTimes = 1, bool useSomke = true)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, position);
            if (friendly == 2)
            {
                int explode = Projectile.NewProjectile(position, Vector2.Zero, ProjectileType<SimpleExplode>(), damage, 2f);
                Main.projectile[explode].friendly = true;
                Main.projectile[explode].hostile = false;
                Main.projectile[explode].width = (int)size.X;
                Main.projectile[explode].height = (int)size.Y;
                Main.projectile[explode].position = position - size / 2f;
                ((SimpleExplode)Main.projectile[explode].modProjectile).goreTimes = goreTimes;
                ((SimpleExplode)Main.projectile[explode].modProjectile).useSmoke = useSomke;

                int explode2 = Projectile.NewProjectile(position, Vector2.Zero, ProjectileType<SimpleExplode>(), damage, 2f, Main.myPlayer);
                Main.projectile[explode2].friendly = false;
                Main.projectile[explode2].hostile = true;
                Main.projectile[explode2].width = (int)size.X;
                Main.projectile[explode2].height = (int)size.Y;
                Main.projectile[explode2].position = position - size / 2f;
                ((SimpleExplode)Main.projectile[explode2].modProjectile).goreTimes = goreTimes;
                ((SimpleExplode)Main.projectile[explode2].modProjectile).useSmoke = false;
            }
            else
            {
                int explode = Projectile.NewProjectile(position, Vector2.Zero, ProjectileType<SimpleExplode>(), damage, 2f, (friendly == 1 ? Main.myPlayer : 255));
                Main.projectile[explode].friendly = friendly == 1;
                Main.projectile[explode].hostile = friendly == 0;
                Main.projectile[explode].width = (int)size.X;
                Main.projectile[explode].height = (int)size.Y;
                Main.projectile[explode].position = position - size / 2f;
                ((SimpleExplode)Main.projectile[explode].modProjectile).goreTimes = goreTimes;
                ((SimpleExplode)Main.projectile[explode].modProjectile).useSmoke = useSomke;
            }
        }
    }
}