#region So Much Usings
using Entrogic.Items.PollutElement;
using Entrogic.NPCs.Boss.凝胶Java盾;
using Entrogic.Items.VoluGels;
using Entrogic.Items.VoluGels.Armor;
using Entrogic.Items.Miscellaneous.Placeable.Trophy;
using Entrogic.UI.Books;
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
using TELib.UI.Buttons;
using Terraria.Graphics;
using Terraria.ObjectData;
using Terraria.GameContent.Events;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
#endregion

namespace Entrogic
{
    public class Entrogic : Mod
    {
        #region Values
        internal static int mimicryFrameCounter = 8;
        internal static List<ModItem> ModItems => new List<ModItem>(typeof(ItemLoader).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(field => field.Name == "items").First().GetValue(null) as IList<ModItem>);
        internal static List<ModItem> EntrogicItems
        {
            get
            {
                List<ModItem> i = ModItems;
                i.Sort((a, b) => // 将ModCard筛选到前面
                {
                    if (a.mod != ModLoader.GetMod("Entrogic") && b.mod == ModLoader.GetMod("Entrogic")) return 1; //如果返回1，则a就会排到b的后面
                if (a.mod == ModLoader.GetMod("Entrogic") && b.mod != ModLoader.GetMod("Entrogic")) return -1;//如果返回-1，则b会排到a的后面
                if (a.mod == b.mod) return 0;
                    return 0;
                });
                return i;
            }
        }
        internal static int FirstModItem
        {
            get
            {
                foreach (ModItem i in ModItems)
                {
                    if (i.mod == ModLoader.GetMod("Entrogic"))
                    {
                        return ModItems.IndexOf(i);
                    }
                }
                return 0;
            }
        }
        internal static int CardsCount
        {
            get
            {
                int i = 0;
                foreach (ModItem item in ModItems)
                {
                    if (!item.item.GetGlobalItem<EntrogicItem>().card)
                        return i;
                    i++;
                }
                return i;
            }
        }
        internal static ModHotKey PassHotkey;
        internal static ModHotKey WashHotkey;
        internal static ModHotKey HookCursorHotKey;
        internal Vector2 HookCursor;

        public static bool IsCalamityLoaded = false;
        public static bool IsCalamityModRevengenceMode => CalamityMod.World.CalamityWorld.revenge;
        public static bool IsCalamityModDeathMode => CalamityMod.World.CalamityWorld.death;

        public static Dictionary<string, Texture2D> ModTexturesTable = new Dictionary<string, Texture2D>();
        private int foolTexts = 0;
        internal static bool Unloading = false;
        internal static readonly string ModFolder;
        internal static bool IsDev = false;
        internal static bool IsChinese = false;
        internal static int ModTimer; 
        internal static Entrogic Instance;
        public Effect Blur;
        public static DynamicSpriteFont PixelFont { get { return Instance.GetFont("Fonts/JOJOHOTXiangSubeta"); } }
        internal BookUI BookUI { get; private set; }
        private UserInterface BookUIE;
        internal CardUI CardUI { get; private set; }
        private UserInterface CardUIE;
        internal CardInventoryUI CardInventoryUI { get; private set; }
        private UserInterface CardInventoryUIE;

        public static int MimicryCustomCurrencyId;
        #endregion
        static Entrogic()
        {
            ModFolder = string.Format("{0}{1}Mod Configs{2}Entrogic{3}", Main.SavePath, Path.DirectorySeparatorChar, Path.DirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
        public Entrogic()
        {
            Instance = this;
            Directory.CreateDirectory(ModFolder);
        }
        public override void PostSetupContent()
        {
            Main.OnPostDraw += OnPostDraw;
            //IL.Terraria.Main.DrawMenu += UpdateExtraWorldDiff;
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call(
            "AddBoss",
            2.7f,
            new List<int>() { ModContent.NPCType<嘉沃顿>(), ModContent.NPCType<胚胎>() },// Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.嘉沃顿", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedGelSymbiosis),
            ModContent.ItemType<凝胶培养瓶>(), // Summon Items
            new List<int> { ModContent.ItemType<瓦卢提奥头套>(), ModContent.ItemType<VTrophy>() }, // Collections
            new List<int> { ModContent.ItemType<凝胶安卡>(), ModContent.ItemType<GelOfLife>() }, // Normal Loots
            "$Mods.Entrogic.BossSpawnInfo.GelSymb", // Spawn Info
            "", // Despawn Info
            "Entrogic/Texture/GelSym_Textures");

                bossChecklist.Call(
            "AddBoss",
            5.7f,
            ModContent.NPCType<Antanasy>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.Antanasy", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedAthanasy),
            ModContent.ItemType<巨神的旨意>(),
            new List<int> { ModContent.ItemType<魔像头套>(), ModContent.ItemType<ATrophy>() },
            new List<int> { ModContent.ItemType<巨石长枪>(), ModContent.ItemType<岩石猎枪>(), ModContent.ItemType<衰落之眼>(), ModContent.ItemType<StoneSlimeStaff>() },
            "$Mods.Entrogic.BossSpawnInfo.Athanasy");

                bossChecklist.Call(
            "AddBoss",
            10.3f,
            ModContent.NPCType<污染之灵>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.污染之灵", // Boss Name
            (Func<bool>)(() => EntrogicWorld.IsDownedPollutionElemental),
            ModContent.ItemType<污染水源>(),
            new List<int> { ModContent.ItemType<PolluHead>(), ModContent.ItemType<PETrophy>() },
            new List<int> { ModContent.ItemType<风暴之瓶>(), ModContent.ItemType<水元素召唤杖>(), ModContent.ItemType<污痕长弓>(), ModContent.ItemType<污秽洋流>(),
                ModContent.ItemType<污染头盔>(), ModContent.ItemType<污染头饰>(), ModContent.ItemType<污染面具>(), ModContent.ItemType<污染胸甲>(), ModContent.ItemType<污染护胫>() },
            "$Mods.Entrogic.BossSpawnInfo.PollutionElement",
            " 在大海中继续沉睡...");
            }
            if (!Main.dedServ)
            {
                Main.logoTexture = ModTexturesTable["Logo"];
                Main.logo2Texture = ModTexturesTable["Logo2"];
            }
        }
        public override void Load()
        {
            if (!IsDev)
            {
                try
                {
                    //WebClient MyWebClient = new WebClient();
                    //MyWebClient.UseDefaultCredentials = true; // 设置用于向Internet资源的请求进行身份验证的网络凭据
                    //byte[] pageData = MyWebClient.DownloadData("http://134.175.161.86/Entrogic/VersionCheck.html"); // 从指定网站下载数据
                    ////string pageHtml = Encoding.Default.GetString(pageData);  // 如果获取网站页面采用的是GB2312，则使用这句   
                    //string pageHtml = Encoding.UTF8.GetString(pageData); // 如果获取网站页面采用的是UTF-8，则使用这句
                    //string[] pageHtmlArray = pageHtml.Split(new string[] { "ver" }, StringSplitOptions.RemoveEmptyEntries);

                    //string path = string.Format("{0}[Entrogic]请检查您的Mod版本.txt", ModFolder);
                    ////写文件
                    //if (pageHtmlArray[1] != Version.ToString())
                    //{
                    //    string URL = "http://134.175.161.86/Entrogic/VersionCheck.html";
                    //    WriteAndOpenFile(path,
                    //        "您当前的Mod版本为" + Version.ToString() + "，但最新的正式版为" + pageHtmlArray[1]
                    //        + "\n已为您打开下载页面，如打开未成功\n请访问以下网址以将版本更新至最新正式版！\n"
                    //        + "http://134.175.161.86/Entrogic/VersionCheck.html"
                    //        + "\n你也可以通过加入我们的QQ群：798484146以下载最新版");
                    //    Process.Start(URL);
                    //}
                    //else if (File.Exists(path))
                    //{
                    //    File.Delete(path);
                    //}
                }
                catch
                {

                }
            }
            PassHotkey = RegisterHotKey("过牌快捷键", "E");
            WashHotkey = RegisterHotKey("洗牌快捷键", "Q");
            HookCursorHotKey = RegisterHotKey("设置钩爪指针快捷键", "C");
            On.Terraria.Player.QuickGrapple += Player_QuickGrapple;
            //On.Terraria.Main.DrawTiles += Main_DrawTiles;
            foolTexts = Main.rand.Next(3);
            Unloading = false;
            IsCalamityLoaded = ModLoader.GetMod("CalamityMod") != null;
            Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
            if (yabhb != null)
            {
                #region Wlta yabhb
                yabhb.Call("RegisterCustomHealthBar",
                    ModContent.NPCType<胚胎>(),
                    null, //ForceSmall
                    null, //displayName
                    GetTexture("UI/yabhb/瓦卢提奥血条Fill"), //fillTexture
                    GetTexture("UI/yabhb/瓦卢提奥血条头"),
                    GetTexture("UI/yabhb/瓦卢提奥血条条"),
                    GetTexture("UI/yabhb/瓦卢提奥血条尾"),
                    null, //midBarOffsetX
                    0, //midBarOffsetY
                    null, //fillDecoOffsetX
                    32, //bossHeadCentreOffsetX
                    30, //bossHeadCentreOffsetY
                    null, //fillTextureSM
                    null, //leftBarSM
                    null, //midBarSM
                    null, //rightBarSM
                    null, //fillDecoOffsetXSM
                    null, //bossHeadCentreOffsetXSM
                    null, //bossHeadCentreOffsetYSM
                    true); //LoopMidBar
                yabhb.Call("RegisterCustomHealthBar",
                    ModContent.NPCType<嘉沃顿>(),
                    null, //ForceSmall
                    null, //displayName
                    GetTexture("UI/yabhb/瓦卢提奥血条Fill"), //fillTexture
                    GetTexture("UI/yabhb/瓦卢提奥血条头"),
                    GetTexture("UI/yabhb/瓦卢提奥血条条"),
                    GetTexture("UI/yabhb/瓦卢提奥血条尾"),
                    null, //midBarOffsetX
                    0, //midBarOffsetY
                    null, //fillDecoOffsetX
                    32, //bossHeadCentreOffsetX
                    32, //bossHeadCentreOffsetY
                    null, //fillTextureSM
                    null, //leftBarSM
                    null, //midBarSM
                    null, //rightBarSM
                    null, //fillDecoOffsetXSM
                    null, //bossHeadCentreOffsetXSM
                    null, //bossHeadCentreOffsetYSM
                    true); //LoopMidBar
                #endregion
            }
            MimicryCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new EntrogicMimicryCurrency(ModContent.ItemType<拟态魔能>(), 999L));
            if (!Main.dedServ)
            {
                ResourceLoader.LoadAllTextures();

                AddEquipTexture(new PolluHead1(), null, EquipType.Head, "PolluHead1", "Entrogic/Items/PollutElement/PolluHead1_Head");
                AddEquipTexture(new PolluHead2(), null, EquipType.Head, "PolluHead2", "Entrogic/Items/PollutElement/PolluHead2_Head");
                AddEquipTexture(new PolluHead3(), null, EquipType.Head, "PolluHead3", "Entrogic/Items/PollutElement/PolluHead3_Head");
                AddEquipTexture(new PolluHead4(), null, EquipType.Head, "PolluHead4", "Entrogic/Items/PollutElement/PolluHead4_Head");
                AddEquipTexture(new PolluWings1(), null, EquipType.Wings, "PolluWings1", "Entrogic/Items/PollutElement/PolluWings1_Wings");
                AddEquipTexture(new PolluWings2(), null, EquipType.Wings, "PolluWings2", "Entrogic/Items/PollutElement/PolluWings2_Wings");
                AddEquipTexture(new PolluWings3(), null, EquipType.Wings, "PolluWings3", "Entrogic/Items/PollutElement/PolluWings3_Wings");
                AddEquipTexture(new PolluWings4(), null, EquipType.Wings, "PolluWings4", "Entrogic/Items/PollutElement/PolluWings4_Wings");
                AddEquipTexture(new PolluWings5(), null, EquipType.Wings, "PolluWings5", "Entrogic/Items/PollutElement/PolluWings5_Wings");
                AddEquipTexture(new PolluWings6(), null, EquipType.Wings, "PolluWings6", "Entrogic/Items/PollutElement/PolluWings6_Wings");
                AddEquipTexture(new PolluWings7(), null, EquipType.Wings, "PolluWings7", "Entrogic/Items/PollutElement/PolluWings7_Wings");
                AddEquipTexture(new PolluWings8(), null, EquipType.Wings, "PolluWings8", "Entrogic/Items/PollutElement/PolluWings8_Wings");

                Filters.Scene["Entrogic:RainyDaysScreen"] = new Filter(new PollutionElementalScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.2f, 0.4f).UseOpacity(0.3f), EffectPriority.VeryHigh);
                SkyManager.Instance["Entrogic:RainyDaysScreen"] = new RainyDaysScreen();
                Filters.Scene["Entrogic:GrayScreen"] = new Filter(new AthanasyScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.2f, 0.2f).UseOpacity(0.7f), EffectPriority.High);
                SkyManager.Instance["Entrogic:GrayScreen"] = new GrayScreen();
                Filters.Scene["Entrogic:MagicStormScreen"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(-0.4f, -0.2f, 1.6f).UseOpacity(0.6f), EffectPriority.Medium);
                SkyManager.Instance["Entrogic:MagicStormScreen"] = new MagicStormScreen();
                // First, you load in your shader file.
                // You'll have to do this regardless of what kind of shader it is,
                // and you'll have to do it for every shader file.
                // This example assumes you have screen shaders.
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/IceScreen"));
                Filters.Scene["Entrogic:IceScreen"] = new Filter(new ScreenShaderData(screenRef, "IceScreen"), EffectPriority.High);
                Filters.Scene["Entrogic:IceScreen"].Load();
                Ref<Effect> screenRef2 = new Ref<Effect>(GetEffect("Effects/ReallyDark"));
                Filters.Scene["Entrogic:ReallyDark"] = new Filter(new ScreenShaderData(screenRef2, "ReallyDark"), EffectPriority.VeryHigh);
                Filters.Scene["Entrogic:ReallyDark"].Load();
                Ref<Effect> screenRef3 = new Ref<Effect>(GetEffect("Effects/GooddShader"));
                Filters.Scene["Entrogic:GooddShader"] = new Filter(new ScreenShaderData(screenRef3, "GooddShader"), EffectPriority.VeryHigh);
                Filters.Scene["Entrogic:GooddShader"].Load();
                Filters.Scene["Entrogic:Blur"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/Blur")), "Blur"), EffectPriority.VeryHigh);
                Filters.Scene["Entrogic:Blur"].Load();

                BookUI = new BookUI();
                BookUI.Activate();
                BookUIE = new UserInterface();
                BookUIE.SetState(BookUI);

                //BookPageUI = new BookPageUI();
                //BookPageUI.Activate();
                //BookPageUIE = new UserInterface();
                //BookPageUIE.SetState(BookPageUI);

                CardUI = new CardUI();
                CardUI.Activate();
                CardUIE = new UserInterface();
                CardUIE.SetState(CardUI);

                CardInventoryUI = new CardInventoryUI();
                CardInventoryUI.Activate();
                CardInventoryUIE = new UserInterface();
                CardInventoryUIE.SetState(CardInventoryUI);
                /*SinsBar.visible = true;
                Sinsbar = new SinsBar();
                Sinsbar.Activate();
                SinsBarInterface = new UserInterface();
                SinsBarInterface.SetState(Sinsbar);*/
            }
            Buildings.Cache("Buildings/CardShrine0.ebuiding", "Buildings/CardShrine1.ebuiding");
            #region Armor Translates
            ModTranslation template = CreateTranslation("mspeed");
            template.AddTranslation(GameCulture.Chinese, "移动速度");
            template.SetDefault(" movement speed");
            AddTranslation(template);
            template = CreateTranslation("and");
            template.AddTranslation(GameCulture.Chinese, "与");
            template.SetDefault(" and");
            AddTranslation(template);
            template = CreateTranslation("csc");
            template.AddTranslation(GameCulture.Chinese, "暴击率");
            template.SetDefault(" critical strike chance");
            AddTranslation(template);
            template = CreateTranslation("csc2");
            template.AddTranslation(GameCulture.Chinese, " 暴击率");
            template.SetDefault(" critical strike chance");
            AddTranslation(template);
            template = CreateTranslation("knockback");
            template.AddTranslation(GameCulture.Chinese, "击退");
            template.SetDefault(" knockback");
            AddTranslation(template);
            template = CreateTranslation("damage");
            template.AddTranslation(GameCulture.Chinese, "伤害");
            template.SetDefault(" damage");
            AddTranslation(template);
            template = CreateTranslation("cntca");
            template.AddTranslation(GameCulture.Chinese, "的几率不消耗弹药");
            template.SetDefault(" chance not to consume ammo");
            AddTranslation(template);
            template = CreateTranslation("immb");
            template.AddTranslation(GameCulture.Chinese, "最大魔力值增加");
            template.SetDefault("Increases maximum mana by ");
            AddTranslation(template);
            template = CreateTranslation("rmub");
            template.AddTranslation(GameCulture.Chinese, "魔力消耗减少");
            template.SetDefault("Reduces mana usage by ");
            AddTranslation(template);
            #endregion
            #region Boss Checklist Translates
            ModTranslation bctext = CreateTranslation("BossSpawnInfo.GelSymb");
            bctext.AddTranslation(GameCulture.Chinese, "在地下原汤湖使用 [i:" + ItemType("凝胶培养瓶") + "] 召唤一只史莱姆, 并将其掷于地下原汤湖中召唤");
            bctext.SetDefault("Use [i:" + ItemType("凝胶培养瓶") + "] in the underground pool to summon a slime, and thorw it into the pool to summon.");
            AddTranslation(bctext);
            bctext = CreateTranslation("BossSpawnInfo.Athanasy");
            bctext.AddTranslation(GameCulture.Chinese, "使用 [i:" + ItemType("巨神的旨意") + "] 召唤（由地牢怪物掉落或在上锁的金箱中找到）");
            bctext.SetDefault("Use [i:" + ItemType("巨神的旨意") + "] to spawn, you can find it from locked chests or drop from dungeon monsters");
            AddTranslation(bctext);
            bctext = CreateTranslation("BossSpawnInfo.PollutionElement");
            bctext.AddTranslation(GameCulture.Chinese, "在海边使用 [i:" + ItemType("污染水源") + "] 召唤");
            bctext.SetDefault("Use [i:" + ItemType("污染水源") + "] in ocean to spawn");
            AddTranslation(bctext);
            #endregion
            #region Another Translates
            ModTranslation transform = CreateTranslation("RightClickToTransform");
            transform.AddTranslation(GameCulture.Chinese, "右键点击物品以切换状态");
            transform.SetDefault("Right click to switch status");
            AddTranslation(transform);
            ModTranslation modTranslation = CreateTranslation("ArcaneDamage");
            modTranslation.AddTranslation(GameCulture.Chinese, "奥术");
            modTranslation.SetDefault("arcane ");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_SkyDarkened");
            modTranslation.AddTranslation(GameCulture.Chinese, "天空变得更加黑暗");
            modTranslation.SetDefault("The sky is becomes darkened");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Pollutional");
            modTranslation.AddTranslation(GameCulture.Chinese, "污染生物正在聚集...");
            modTranslation.SetDefault("Pollutional creatures are gathering...");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Summon");
            modTranslation.AddTranslation(GameCulture.Chinese, "永远不要尝试去挑战自然...");
            modTranslation.SetDefault("Never try to challenge the nature...");
            AddTranslation(modTranslation);
            modTranslation = CreateTranslation("Pollution_Summon2");
            modTranslation.AddTranslation(GameCulture.Chinese, "呵...");
            modTranslation.SetDefault("Heh...");
            AddTranslation(modTranslation);
            ModTranslation text = CreateTranslation("NPCTalk");
            text.SetDefault("<{0}> {1}");
            AddTranslation(text);
            text = CreateTranslation("Common.RandomCardImage");
            text.SetDefault($"[i:{ModContent.ItemType<RandomCard>()}] [c/ffeb6e:卡牌系统自定义项]");
            AddTranslation(text);
            ModTranslation modGen = CreateTranslation("GenLifeLiquid");
            modGen.AddTranslation(GameCulture.Chinese, "正在生成生命湖");
            modGen.SetDefault("Life.");
            AddTranslation(modGen);
            modGen = CreateTranslation("SmoothLifeLiquid");
            modGen.AddTranslation(GameCulture.Chinese, "正在平整生命湖");
            modGen.SetDefault("Life.");
            AddTranslation(modGen);
            modGen = CreateTranslation("GenCardShrine");
            modGen.AddTranslation(GameCulture.Chinese, "正在生成卡牌神龛");
            modGen.SetDefault("Card.");
            AddTranslation(modGen);
            #endregion
        }
        private int GetTreeVariant(int x, int y)
        {
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            switch (Main.tile[x, y].type)
            {
                case 23:
                    return 0;
                case 70:
                    return 6;
                case 60:
                    if ((double)y <= Main.worldSurface)
                    {
                        return 1;
                    }
                    return 5;
                case 109:
                    return 2;
                case 147:
                    return 3;
                case 199:
                    return 4;
                default:
                    return -1;
            }
        }
        private void Main_DrawTiles(On.Terraria.Main.orig_DrawTiles orig, Main self, bool solidOnly, int waterStyleOverride)
        {
            if (AEntrogicConfigClient.Instance.SuperZoom >= 0.1f)
            {
                Terraria.Utilities.UnifiedRandom rand = Main.rand;
                SpriteBatch spriteBatch = Main.spriteBatch;
                Vector2 screenPosition = Main.screenPosition;
                int num = (int)(255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality);
                int num2 = (int)(50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality);
                Vector2 value = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    value = Vector2.Zero;
                }
                int nextSpecialDrawIndex = 0;
                _ = Main.specX;
                int num3 = (int)((screenPosition.X - value.X) / 16f - 20f);
                int num4 = (int)((screenPosition.X + (float)Main.screenWidth + value.X) / 16f) + 21;
                int num5 = (int)((screenPosition.Y - value.Y) / 16f - 20f);
                int num6 = (int)((screenPosition.Y + (float)Main.screenHeight + value.Y) / 16f) + 24;
                if (num3 < 4)
                {
                    num3 = 4;
                }
                if (num4 > Main.maxTilesX - 4)
                {
                    num4 = Main.maxTilesX - 4;
                }
                if (num5 < 4)
                {
                    num5 = 4;
                }
                if (num6 > Main.maxTilesY - 4)
                {
                    num6 = Main.maxTilesY - 4;
                }
                if (Main.sectionManager.FrameSectionsLeft > 0)
                {
                    TimeLogger.DetailedDrawReset();
                    WorldGen.SectionTileFrameWithCheck(num3, num5, num4, num6);
                    TimeLogger.DetailedDrawTime(5);
                }
                Dictionary<Microsoft.Xna.Framework.Point, int> dictionary = new Dictionary<Microsoft.Xna.Framework.Point, int>();
                Dictionary<Microsoft.Xna.Framework.Point, int> dictionary2 = new Dictionary<Microsoft.Xna.Framework.Point, int>();
                Dictionary<Microsoft.Xna.Framework.Point, int> dictionary3 = new Dictionary<Microsoft.Xna.Framework.Point, int>();
                _ = Main.player[Main.myPlayer].team;
                if (Main.player[Main.myPlayer].active)
                {
                    _ = Main.netMode;
                }
                int width = 16;
                Microsoft.Xna.Framework.Color[] slices = new Microsoft.Xna.Framework.Color[9];
                for (int i = num5; i < num6 + 4; i++)
                {
                    for (int j = num3 - 2; j < num4 + 2; j++)
                    {

                        {
                            Tile tile = Main.tile[j, i];
                            if (tile == null)
                            {
                                tile = new Tile();
                                Main.tile[j, i] = tile;
                                Main.mapTime += 60;
                            }
                            ushort type = tile.type;
                            short num7 = tile.frameX;
                            short num8 = tile.frameY;
                            bool flag = Main.tileSolid[type];
                            if (type == 11)
                            {
                                flag = true;
                            }
                            if (!tile.active() || flag != solidOnly)
                            {
                                continue;
                            }
                            if (!Main.tileSetsLoaded[type])
                            {
                                Main.instance.LoadTiles(type);
                            }
                            SpriteEffects spriteEffects = SpriteEffects.None;
                            switch (type)
                            {
                                case 3:
                                case 13:
                                case 20:
                                case 24:
                                case 49:
                                case 50:
                                case 52:
                                case 61:
                                case 62:
                                case 71:
                                case 73:
                                case 74:
                                case 81:
                                case 82:
                                case 83:
                                case 84:
                                case 91:
                                case 92:
                                case 93:
                                case 110:
                                case 113:
                                case 115:
                                case 135:
                                case 141:
                                case 165:
                                case 174:
                                case 201:
                                case 205:
                                case 227:
                                case 270:
                                case 271:
                                case 372:
                                case 382:
                                    if (j % 2 == 1)
                                    {
                                        spriteEffects = SpriteEffects.FlipHorizontally;
                                    }
                                    break;
                                case 184:
                                    if (num8 < 108)
                                    {
                                        if (j % 2 == 1)
                                        {
                                            spriteEffects = SpriteEffects.FlipHorizontally;
                                        }
                                    }
                                    else if (i % 2 == 1)
                                    {
                                        spriteEffects = SpriteEffects.FlipVertically;
                                    }
                                    break;
                                case 185:
                                    if (num8 == 0 && j % 2 == 1)
                                    {
                                        spriteEffects = SpriteEffects.FlipHorizontally;
                                    }
                                    break;
                            }
                            TileLoader.SetSpriteEffects(j, i, type, ref spriteEffects);
                            Microsoft.Xna.Framework.Color drawColor = Lighting.GetColor(j, i);
                            int offsetY = 0;
                            int height = 16;
                            if (type >= 330 && type <= 333)
                            {
                                offsetY += 2;
                            }
                            if (type == 4 && WorldGen.SolidTile(j, i - 1))
                            {
                                offsetY = 2;
                                if (WorldGen.SolidTile(j - 1, i + 1) || WorldGen.SolidTile(j + 1, i + 1))
                                {
                                    offsetY = 4;
                                }
                            }
                            if (type == 336)
                            {
                                offsetY = 2;
                            }
                            if (type == 457)
                            {
                                offsetY = 2;
                            }
                            if (type == 466)
                            {
                                offsetY = 2;
                            }
                            if ((type >= 275 && type <= 282) || type == 414 || type == 413)
                            {
                                offsetY = 2;
                            }
                            if (type == 285 || type == 286 || type == 298 || type == 299 || type == 309 || type == 358 || type == 359 || type == 360 || type == 361 || type == 362 || type == 363 || type == 364 || type == 391 || type == 392 || type == 393 || type == 394 || type == 310)
                            {
                                offsetY = 2;
                            }
                            if (type == 100 || type == 173 || type == 283)
                            {
                                offsetY = 2;
                            }
                            if (type == 78 || type == 85 || type == 210 || type == 133 || type == 134 || type == 233)
                            {
                                offsetY = 2;
                            }
                            if (type == 33 || type == 49 || type == 174 || type == 372)
                            {
                                offsetY = -4;
                            }
                            switch (type)
                            {
                                case 3:
                                case 4:
                                case 5:
                                case 24:
                                case 33:
                                case 49:
                                case 61:
                                case 71:
                                case 110:
                                case 174:
                                case 201:
                                case 323:
                                case 324:
                                case 372:
                                    height = 20;
                                    break;
                                case 16:
                                case 17:
                                case 18:
                                case 20:
                                case 26:
                                case 32:
                                case 69:
                                case 72:
                                case 77:
                                case 79:
                                case 80:
                                case 352:
                                    height = 18;
                                    break;
                                case 14:
                                case 15:
                                case 21:
                                case 411:
                                case 441:
                                case 467:
                                case 468:
                                case 469:
                                    if (num8 == 18)
                                    {
                                        height = 18;
                                    }
                                    break;
                                case 172:
                                case 376:
                                    if (num8 % 38 == 18)
                                    {
                                        height = 18;
                                    }
                                    break;
                                case 27:
                                    if (num8 % 74 == 54)
                                    {
                                        height = 18;
                                    }
                                    break;
                                case 137:
                                    height = 18;
                                    break;
                                case 462:
                                    height = 18;
                                    break;
                                case 135:
                                    offsetY = 2;
                                    height = 18;
                                    break;
                                case 378:
                                    offsetY = 2;
                                    break;
                                case 254:
                                    offsetY = 2;
                                    break;
                                case 132:
                                    offsetY = 2;
                                    height = 18;
                                    break;
                                case 405:
                                    height = 16;
                                    if (num8 > 0)
                                    {
                                        height = 18;
                                    }
                                    break;
                                case 406:
                                    height = 16;
                                    if (num8 % 54 >= 36)
                                    {
                                        height = 18;
                                    }
                                    break;
                                default:
                                    height = 16;
                                    break;
                            }
                            if (type == 52)
                            {
                                offsetY -= 2;
                            }
                            if (type == 324)
                            {
                                offsetY = -2;
                            }
                            if (type == 231 || type == 238)
                            {
                                offsetY += 2;
                            }
                            if (type == 207)
                            {
                                offsetY = 2;
                            }
                            width = ((type != 4 && type != 5 && type != 323 && type != 324) ? 16 : 20);
                            if (type == 73 || type == 74 || type == 113)
                            {
                                offsetY -= 12;
                                height = 32;
                            }
                            if (type == 388 || type == 389)
                            {
                                int num9 = TileObjectData.GetTileData(type, num7 / 18).Height * 18 + 4;
                                offsetY = -2;
                                if (num8 == num9 - 20 || num8 == num9 * 2 - 20 || num8 == 0 || num8 == num9)
                                {
                                    height = 18;
                                }
                            }
                            if (type == 410 && num8 == 36)
                            {
                                height = 18;
                            }
                            if (type == 227)
                            {
                                width = 32;
                                height = 38;
                                offsetY = ((num7 != 238) ? (offsetY - 20) : (offsetY - 6));
                            }
                            if (type == 185 || type == 186 || type == 187)
                            {
                                offsetY = 2;
                                switch (type)
                                {
                                    case 185:
                                        if (num8 == 18 && num7 >= 576 && num7 <= 882)
                                        {
                                            Main.tileShine2[185] = true;
                                        }
                                        else
                                        {
                                            Main.tileShine2[185] = false;
                                        }
                                        break;
                                    case 186:
                                        if (num7 >= 864 && num7 <= 1170)
                                        {
                                            Main.tileShine2[186] = true;
                                        }
                                        else
                                        {
                                            Main.tileShine2[186] = false;
                                        }
                                        break;
                                }
                            }
                            if (type == 178 && num8 <= 36)
                            {
                                offsetY = 2;
                            }
                            if (type == 184)
                            {
                                width = 20;
                                if (num8 <= 36)
                                {
                                    offsetY = 2;
                                }
                                else if (num8 <= 108)
                                {
                                    offsetY = -2;
                                }
                            }
                            if (type == 28)
                            {
                                offsetY += 2;
                            }
                            if (type == 81)
                            {
                                offsetY -= 8;
                                height = 26;
                                width = 24;
                            }
                            if (type == 105)
                            {
                                offsetY = 2;
                            }
                            if (type == 124)
                            {
                                height = 18;
                            }
                            if (type == 137)
                            {
                                height = 18;
                            }
                            if (type == 138)
                            {
                                height = 18;
                            }
                            if (type == 139 || type == 142 || type == 143)
                            {
                                offsetY = 2;
                            }
                            TileLoader.SetDrawPositions(j, i, ref width, ref offsetY, ref height);
                            int num10 = 0;
                            if (tile.halfBrick())
                            {
                                num10 = 8;
                            }
                            int frameYOffset = Main.tileFrame[type] * 38;
                            int frameXOffset = 0;
                            if (type == 272)
                            {
                                frameYOffset = 0;
                            }
                            if (type == 106)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                            }
                            if (type >= 300 && type <= 308)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 354)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 355)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 377)
                            {
                                frameYOffset = Main.tileFrame[type] * 38;
                                offsetY = 2;
                            }
                            if (type == 463)
                            {
                                frameYOffset = Main.tileFrame[type] * 72;
                                offsetY = 2;
                            }
                            if (type == 464)
                            {
                                frameYOffset = Main.tileFrame[type] * 72;
                                offsetY = 2;
                            }
                            if (type == 379)
                            {
                                frameYOffset = Main.tileFrame[type] * 90;
                            }
                            if (type == 349)
                            {
                                int num11 = num7 % 36;
                                int num12 = num8 % 54;
                                if (Animation.GetTemporaryFrame(j - num11 / 18, i - num12 / 18, out int frameData))
                                {
                                    num7 = (short)(36 * frameData + num11);
                                }
                            }
                            if (type == 441 || type == 468)
                            {
                                int num13 = num7 % 36;
                                int num14 = num8 % 38;
                                if (Animation.GetTemporaryFrame(j - num13 / 18, i - num14 / 18, out int frameData2))
                                {
                                    num8 = (short)(38 * frameData2 + num14);
                                }
                            }
                            if (type == 390)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                            }
                            if (type == 412)
                            {
                                frameYOffset = 0;
                                offsetY = 2;
                            }
                            if (type == 455)
                            {
                                frameYOffset = 0;
                                offsetY = 2;
                            }
                            if (type == 406)
                            {
                                int num15 = Main.tileFrame[type];
                                if (num8 >= 108)
                                {
                                    num15 = 6 - num8 / 54;
                                }
                                else if (num8 >= 54)
                                {
                                    num15 = Main.tileFrame[type] - 1;
                                }
                                frameYOffset = num15 * 56;
                                frameYOffset += num8 / 54 * 2;
                            }
                            if (type == 452)
                            {
                                int num16 = Main.tileFrame[type];
                                if (num7 >= 54)
                                {
                                    num16 = 0;
                                }
                                frameYOffset = num16 * 54;
                            }
                            if (type == 455)
                            {
                                int num17 = 1 + Main.tileFrame[type];
                                if (!BirthdayParty.PartyIsUp)
                                {
                                    num17 = 0;
                                }
                                frameYOffset = num17 * 54;
                            }
                            if (type == 454)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                            }
                            if (type == 453)
                            {
                                int num18 = Main.tileFrameCounter[type] / 20;
                                int num19 = i - tile.frameY / 18;
                                frameYOffset = (num18 + (num19 + j)) % 3 * 54;
                            }
                            if (type == 456)
                            {
                                int num20 = Main.tileFrameCounter[type] / 20;
                                int num21 = i - tile.frameY / 18;
                                int num22 = j - tile.frameX / 18;
                                frameYOffset = (num20 + (num21 + num22)) % 4 * 54;
                            }
                            if (type == 405)
                            {
                                int num23 = Main.tileFrame[type];
                                if (num7 >= 54)
                                {
                                    num23 = 0;
                                }
                                frameYOffset = num23 * 38;
                            }
                            if (type == 12)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                            }
                            if (type == 96)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                            }
                            if (type == 238)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                            }
                            if (type == 31)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                            }
                            if (type == 215)
                            {
                                frameYOffset = ((num8 >= 36) ? 252 : (Main.tileFrame[type] * 36));
                                offsetY = 2;
                            }
                            if (type == 231)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 243)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 247)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 228)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 244)
                            {
                                offsetY = 2;
                                frameYOffset = ((num7 < 54) ? (Main.tileFrame[type] * 36) : 0);
                            }
                            if (type == 235)
                            {
                                frameYOffset = Main.tileFrame[type] * 18;
                            }
                            if (type == 217 || type == 218)
                            {
                                frameYOffset = Main.tileFrame[type] * 36;
                                offsetY = 2;
                            }
                            if (type == 219 || type == 220)
                            {
                                frameYOffset = Main.tileFrame[type] * 54;
                                offsetY = 2;
                            }
                            if (type == 270 || type == 271)
                            {
                                int num24 = Main.tileFrame[type] + j % 6;
                                if (j % 2 == 0)
                                {
                                    num24 += 3;
                                }
                                if (j % 3 == 0)
                                {
                                    num24 += 3;
                                }
                                if (j % 4 == 0)
                                {
                                    num24 += 3;
                                }
                                while (num24 > 5)
                                {
                                    num24 -= 6;
                                }
                                frameXOffset = num24 * 18;
                                frameYOffset = 0;
                            }
                            switch (type)
                            {
                                case 428:
                                    offsetY += 4;
                                    if (PressurePlateHelper.PressurePlatesPressed.ContainsKey(new Microsoft.Xna.Framework.Point(j, i)))
                                    {
                                        frameXOffset += 18;
                                    }
                                    break;
                                case 442:
                                    width = 20;
                                    height = 20;
                                    switch (num7 / 22)
                                    {
                                        case 1:
                                            offsetY = -4;
                                            break;
                                        case 2:
                                            offsetY = -2;
                                            width = 24;
                                            break;
                                        case 3:
                                            offsetY = -2;
                                            width = 16;
                                            break;
                                    }
                                    break;
                            }
                            if (TileID.Sets.TeamTiles[type])
                            {
                                frameYOffset = ((!TileID.Sets.Platforms[type]) ? (frameYOffset + 90) : frameYOffset);
                            }
                            TileLoader.SetAnimationFrame(type, j, i, ref frameXOffset, ref frameYOffset);
                            if (!TileLoader.PreDraw(j, i, type, spriteBatch))
                            {
                                TileLoader.PostDraw(j, i, type, spriteBatch);
                                continue;
                            }
                            if (type == 373 || type == 374 || type == 375 || type == 461)
                            {
                                int num25 = 60;
                                switch (type)
                                {
                                    case 374:
                                        num25 = 120;
                                        break;
                                    case 375:
                                        num25 = 180;
                                        break;
                                }
                                if (rand.Next(num25 * 2) != 0 || tile.liquid != 0)
                                {
                                    continue;
                                }
                                Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(j * 16, i * 16, 16, 16);
                                rectangle.X -= 34;
                                rectangle.Width += 68;
                                rectangle.Y -= 100;
                                rectangle.Height = 400;
                                bool flag2 = true;
                                for (int k = 0; k < 500; k++)
                                {
                                    if (Main.gore[k].active && ((Main.gore[k].type >= 706 && Main.gore[k].type <= 717) || Main.gore[k].type == 943))
                                    {
                                        Microsoft.Xna.Framework.Rectangle value2 = new Microsoft.Xna.Framework.Rectangle((int)Main.gore[k].position.X, (int)Main.gore[k].position.Y, 16, 16);
                                        if (rectangle.Intersects(value2))
                                        {
                                            flag2 = false;
                                        }
                                    }
                                }
                                if (!flag2)
                                {
                                    continue;
                                }
                                Vector2 position = new Vector2(j * 16, i * 16);
                                int num26 = 706;
                                if (Main.waterStyle > 1)
                                {
                                    num26 = 706 + Main.waterStyle - 1;
                                    if (Main.waterStyle >= 12)
                                    {
                                        num26 = WaterStyleLoader.GetWaterStyle(Main.waterStyle).GetDropletGore();
                                    }
                                }
                                if (type == 374)
                                {
                                    num26 = 716;
                                }
                                if (type == 375)
                                {
                                    num26 = 717;
                                }
                                if (type == 461)
                                {
                                    num26 = 943;
                                }
                                if (num26 != 943 || rand.Next(3) == 0)
                                {
                                    int num27 = Gore.NewGore(position, default(Vector2), num26);
                                    Main.gore[num27].velocity *= 0f;
                                }
                                continue;
                            }
                            if ((type >= 275 && type <= 281) || type == 296 || type == 297 || type == 309 || type == 358 || type == 359 || type == 414 || type == 413)
                            {
                                Main.critterCage = true;
                                int num28 = j - num7 / 18;
                                int num29 = i - num8 / 18;
                                int num30 = num28 / 6 * (num29 / 4);
                                num30 %= Main.cageFrames;
                                if (type == 275 || type == 359)
                                {
                                    frameYOffset = Main.bunnyCageFrame[num30] * 54;
                                }
                                if (type == 276 || type == 414)
                                {
                                    frameYOffset = Main.squirrelCageFrame[num30] * 54;
                                }
                                if (type == 413)
                                {
                                    frameYOffset = Main.squirrelCageFrameOrange[num30] * 54;
                                }
                                if (type == 277)
                                {
                                    frameYOffset = Main.mallardCageFrame[num30] * 54;
                                }
                                if (type == 278)
                                {
                                    frameYOffset = Main.duckCageFrame[num30] * 54;
                                }
                                if (type == 279 || type == 358)
                                {
                                    frameYOffset = Main.birdCageFrame[num30] * 54;
                                }
                                if (type == 280)
                                {
                                    frameYOffset = Main.blueBirdCageFrame[num30] * 54;
                                }
                                if (type == 281)
                                {
                                    frameYOffset = Main.redBirdCageFrame[num30] * 54;
                                }
                                if (type == 296)
                                {
                                    frameYOffset = Main.scorpionCageFrame[0, num30] * 54;
                                }
                                if (type == 297)
                                {
                                    frameYOffset = Main.scorpionCageFrame[0, num30] * 54;
                                }
                                if (type == 309)
                                {
                                    frameYOffset = Main.penguinCageFrame[num30] * 54;
                                }
                            }
                            else if (type == 285 || type == 286 || type == 298 || type == 299 || type == 310 || type == 339 || (type >= 361 && type <= 364) || (type >= 391 && type <= 394))
                            {
                                Main.critterCage = true;
                                int num31 = j - num7 / 18;
                                int num32 = i - num8 / 18;
                                int num33 = num31 / 3 * (num32 / 3);
                                num33 %= Main.cageFrames;
                                if (type == 285)
                                {
                                    frameYOffset = Main.snailCageFrame[num33] * 36;
                                }
                                if (type == 286)
                                {
                                    frameYOffset = Main.snail2CageFrame[num33] * 36;
                                }
                                if (type == 298 || type == 361)
                                {
                                    frameYOffset = Main.frogCageFrame[num33] * 36;
                                }
                                if (type == 299 || type == 363)
                                {
                                    frameYOffset = Main.mouseCageFrame[num33] * 36;
                                }
                                if (type == 310 || type == 364 || type == 391)
                                {
                                    frameYOffset = Main.wormCageFrame[num33] * 36;
                                }
                                if (type == 339 || type == 362)
                                {
                                    frameYOffset = Main.grasshopperCageFrame[num33] * 36;
                                }
                                if (type == 392 || type == 393 || type == 394)
                                {
                                    frameYOffset = Main.slugCageFrame[type - 392, num33] * 36;
                                }
                            }
                            else if (type == 282 || (type >= 288 && type <= 295) || (type >= 316 && type <= 318) || type == 360)
                            {
                                Main.critterCage = true;
                                int num34 = j - num7 / 18;
                                int num35 = i - num8 / 18;
                                int num36 = num34 / 2 * (num35 / 3);
                                num36 %= Main.cageFrames;
                                if (type == 282)
                                {
                                    frameYOffset = Main.fishBowlFrame[num36] * 36;
                                }
                                else if ((type >= 288 && type <= 295) || type == 360)
                                {
                                    int num37 = type - 288;
                                    if (type == 360)
                                    {
                                        num37 = 8;
                                    }
                                    frameYOffset = Main.butterflyCageFrame[num37, num36] * 36;
                                }
                                else if (type >= 316 && type <= 318)
                                {
                                    int num38 = type - 316;
                                    frameYOffset = Main.jellyfishCageFrame[num38, num36] * 36;
                                }
                            }
                            else
                            {
                                switch (type)
                                {
                                    case 207:
                                        if (num8 >= 72)
                                        {
                                            frameYOffset = Main.tileFrame[type];
                                            int num39 = j;
                                            if (num7 % 36 != 0)
                                            {
                                                num39--;
                                            }
                                            frameYOffset += num39 % 6;
                                            if (frameYOffset >= 6)
                                            {
                                                frameYOffset -= 6;
                                            }
                                            frameYOffset *= 72;
                                        }
                                        else
                                        {
                                            frameYOffset = 0;
                                        }
                                        break;
                                    case 410:
                                        if (num8 >= 56)
                                        {
                                            frameYOffset = Main.tileFrame[type];
                                            frameYOffset *= 56;
                                        }
                                        else
                                        {
                                            frameYOffset = 0;
                                        }
                                        break;
                                    case 326:
                                    case 327:
                                    case 328:
                                    case 329:
                                    case 336:
                                    case 340:
                                    case 341:
                                    case 342:
                                    case 343:
                                    case 344:
                                    case 345:
                                    case 351:
                                    case 421:
                                    case 422:
                                    case 458:
                                    case 459:
                                        frameYOffset = Main.tileFrame[type] * 90;
                                        break;
                                }
                            }
                            Texture2D texture2D = null;
                            Microsoft.Xna.Framework.Rectangle value3 = Microsoft.Xna.Framework.Rectangle.Empty;
                            Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Transparent;
                            byte b = (byte)(100f + 150f * Main.martianLight);
                            Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(b, b, b, 0);
                            Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(100, 100, 100, 0);
                            Microsoft.Xna.Framework.Color color4 = new Microsoft.Xna.Framework.Color(150, 100, 50, 0);
                            switch (type)
                            {
                                case 10:
                                    if (num8 / 54 == 32)
                                    {
                                        texture2D = Main.glowMaskTexture[57];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 54, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 11:
                                    {
                                        int num42 = num8 / 54;
                                        if (num42 == 32)
                                        {
                                            texture2D = Main.glowMaskTexture[58];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 54, width, height);
                                            color = color2;
                                        }
                                        if (num42 == 33)
                                        {
                                            texture2D = Main.glowMaskTexture[119];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 54, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 14:
                                    {
                                        int num52 = num7 / 54;
                                        if (num52 == 31)
                                        {
                                            texture2D = Main.glowMaskTexture[67];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color2;
                                        }
                                        if (num52 == 32)
                                        {
                                            texture2D = Main.glowMaskTexture[124];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 15:
                                    {
                                        int num46 = num8 / 40;
                                        if (num46 == 32)
                                        {
                                            texture2D = Main.glowMaskTexture[54];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 40, width, height);
                                            color = color2;
                                        }
                                        if (num46 == 33)
                                        {
                                            texture2D = Main.glowMaskTexture[116];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 40, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 18:
                                    {
                                        int num49 = num7 / 36;
                                        if (num49 == 27)
                                        {
                                            texture2D = Main.glowMaskTexture[69];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color2;
                                        }
                                        if (num49 == 28)
                                        {
                                            texture2D = Main.glowMaskTexture[125];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 19:
                                    {
                                        int num50 = num8 / 18;
                                        if (num50 == 26)
                                        {
                                            texture2D = Main.glowMaskTexture[65];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 18, width, height);
                                            color = color2;
                                        }
                                        if (num50 == 27)
                                        {
                                            texture2D = Main.glowMaskTexture[112];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 18, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 33:
                                    if (num7 / 18 == 0 && num8 / 22 == 26)
                                    {
                                        texture2D = Main.glowMaskTexture[61];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 22, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 34:
                                    if (num7 / 54 == 0 && num8 / 54 == 33)
                                    {
                                        texture2D = Main.glowMaskTexture[55];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 54, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 87:
                                    {
                                        int num53 = num7 / 54;
                                        if (num53 == 26)
                                        {
                                            texture2D = Main.glowMaskTexture[64];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color2;
                                        }
                                        if (num53 == 27)
                                        {
                                            texture2D = Main.glowMaskTexture[121];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 88:
                                    {
                                        int num45 = num7 / 54;
                                        if (num45 == 24)
                                        {
                                            texture2D = Main.glowMaskTexture[59];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color2;
                                        }
                                        if (num45 == 25)
                                        {
                                            texture2D = Main.glowMaskTexture[120];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 89:
                                    {
                                        int num54 = num7 / 54;
                                        if (num54 == 29)
                                        {
                                            texture2D = Main.glowMaskTexture[66];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color2;
                                        }
                                        if (num54 == 30)
                                        {
                                            texture2D = Main.glowMaskTexture[123];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 90:
                                    {
                                        int num48 = num8 / 36;
                                        if (num48 == 27)
                                        {
                                            texture2D = Main.glowMaskTexture[52];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                            color = color2;
                                        }
                                        if (num48 == 28)
                                        {
                                            texture2D = Main.glowMaskTexture[113];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 93:
                                    if (num7 / 54 == 27)
                                    {
                                        texture2D = Main.glowMaskTexture[62];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 54, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 79:
                                    {
                                        int num43 = num8 / 36;
                                        if (num43 == 27)
                                        {
                                            texture2D = Main.glowMaskTexture[53];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                            color = color2;
                                        }
                                        if (num43 == 28)
                                        {
                                            texture2D = Main.glowMaskTexture[114];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 42:
                                    if (num8 / 36 == 33)
                                    {
                                        texture2D = Main.glowMaskTexture[63];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 100:
                                    if (num7 / 36 == 0 && num8 / 36 == 27)
                                    {
                                        texture2D = Main.glowMaskTexture[68];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 36, width, height);
                                        color = color2;
                                    }
                                    break;
                                case 101:
                                    {
                                        int num51 = num7 / 54;
                                        if (num51 == 28)
                                        {
                                            texture2D = Main.glowMaskTexture[60];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color2;
                                        }
                                        if (num51 == 29)
                                        {
                                            texture2D = Main.glowMaskTexture[115];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 54, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 104:
                                    {
                                        int num47 = num7 / 36;
                                        if (num47 == 24)
                                        {
                                            texture2D = Main.glowMaskTexture[51];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color2;
                                        }
                                        if (num47 == 25)
                                        {
                                            texture2D = Main.glowMaskTexture[118];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 184:
                                    if (tile.frameX == 110)
                                    {
                                        texture2D = Main.glowMaskTexture[127];
                                        value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height);
                                        color = color4;
                                    }
                                    break;
                                case 172:
                                    {
                                        int num44 = num8 / 38;
                                        if (num44 == 28)
                                        {
                                            texture2D = Main.glowMaskTexture[88];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 38, width, height);
                                            color = color2;
                                        }
                                        if (num44 == 29)
                                        {
                                            texture2D = Main.glowMaskTexture[122];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 % 38, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 463:
                                    texture2D = Main.glowMaskTexture[243];
                                    value3 = new Microsoft.Xna.Framework.Rectangle(num7, num8 + frameYOffset, width, height);
                                    color = new Microsoft.Xna.Framework.Color(127, 127, 127, 0);
                                    break;
                                case 441:
                                case 468:
                                    {
                                        int num41 = num7 / 36;
                                        if (num41 == 48)
                                        {
                                            texture2D = Main.glowMaskTexture[56];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color2;
                                        }
                                        if (num41 == 49)
                                        {
                                            texture2D = Main.glowMaskTexture[117];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                                case 21:
                                case 467:
                                    {
                                        int num40 = num7 / 36;
                                        if (num40 == 48)
                                        {
                                            texture2D = Main.glowMaskTexture[56];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color2;
                                        }
                                        if (num40 == 49)
                                        {
                                            texture2D = Main.glowMaskTexture[117];
                                            value3 = new Microsoft.Xna.Framework.Rectangle(num7 % 36, num8, width, height);
                                            color = color3;
                                        }
                                        break;
                                    }
                            }
                            Texture2D texture2D2 = null;
                            Microsoft.Xna.Framework.Rectangle empty = Microsoft.Xna.Framework.Rectangle.Empty;
                            Microsoft.Xna.Framework.Color color5 = Microsoft.Xna.Framework.Color.Transparent;
                            if (TileID.Sets.HasOutlines[type] && Collision.InTileBounds(j, i, Main.TileInteractionLX, Main.TileInteractionLY, Main.TileInteractionHX, Main.TileInteractionHY) && Main.SmartInteractTileCoords.Contains(new Microsoft.Xna.Framework.Point(j, i)))
                            {
                                int num55 = (drawColor.R + drawColor.G + drawColor.B) / 3;
                                bool flag3 = false;
                                if (Main.SmartInteractTileCoordsSelected.Contains(new Microsoft.Xna.Framework.Point(j, i)))
                                {
                                    flag3 = true;
                                }
                                if (num55 > 10)
                                {
                                    texture2D2 = Main.highlightMaskTexture[type];
                                    color5 = (flag3 ? new Microsoft.Xna.Framework.Color(num55, num55, num55 / 3, num55) : new Microsoft.Xna.Framework.Color(num55 / 2, num55 / 2, num55 / 2, num55));
                                }
                            }
                            if (Main.player[Main.myPlayer].dangerSense)
                            {
                                bool flag4 = type == 135 || type == 137 || type == 138 || type == 141 || type == 210 || type == 442 || type == 443 || type == 444;
                                if (tile.slope() == 0 && !tile.inActive())
                                {
                                    flag4 = (flag4 || type == 32 || type == 69 || type == 48 || type == 232 || type == 352 || type == 51 || type == 229);
                                    if (!Main.player[Main.myPlayer].fireWalk)
                                    {
                                        flag4 = (flag4 || type == 37 || type == 58 || type == 76);
                                    }
                                    if (!Main.player[Main.myPlayer].iceSkate)
                                    {
                                        flag4 = (flag4 || type == 162);
                                    }
                                }
                                if (flag4 || TileLoader.Dangersense(j, i, type, Main.player[Main.myPlayer]))
                                {
                                    if (drawColor.R < byte.MaxValue)
                                    {
                                        drawColor.R = byte.MaxValue;
                                    }
                                    if (drawColor.G < 50)
                                    {
                                        drawColor.G = 50;
                                    }
                                    if (drawColor.B < 50)
                                    {
                                        drawColor.B = 50;
                                    }
                                    drawColor.A = Main.mouseTextColor;
                                    if (!Main.gamePaused && Main.instance.IsActive && rand.Next(30) == 0)
                                    {
                                        int num56 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 60, 0f, 0f, 100, default(Microsoft.Xna.Framework.Color), 0.3f);
                                        Main.dust[num56].fadeIn = 1f;
                                        Main.dust[num56].velocity *= 0.1f;
                                        Main.dust[num56].noLight = true;
                                        Main.dust[num56].noGravity = true;
                                    }
                                }
                            }
                            if (Main.player[Main.myPlayer].findTreasure)
                            {
                                bool flag5 = false;
                                if (type == 185 && num8 == 18 && num7 >= 576 && num7 <= 882)
                                {
                                    flag5 = true;
                                }
                                if (type == 186 && num7 >= 864 && num7 <= 1170)
                                {
                                    flag5 = true;
                                }
                                if (flag5 || Main.tileSpelunker[type] || (Main.tileAlch[type] && type != 82))
                                {
                                    byte b2 = 200;
                                    byte b3 = 170;
                                    if (drawColor.R < b2)
                                    {
                                        drawColor.R = b2;
                                    }
                                    if (drawColor.G < b3)
                                    {
                                        drawColor.G = b3;
                                    }
                                    drawColor.A = Main.mouseTextColor;
                                    if (!Main.gamePaused && Main.instance.IsActive && rand.Next(60) == 0)
                                    {
                                        int num57 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 204, 0f, 0f, 150, default(Microsoft.Xna.Framework.Color), 0.3f);
                                        Main.dust[num57].fadeIn = 1f;
                                        Main.dust[num57].velocity *= 0.1f;
                                        Main.dust[num57].noLight = true;
                                    }
                                }
                            }
                            if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || rand.Next(4) == 0))
                            {
                                switch (type)
                                {
                                    case 238:
                                        if (rand.Next(10) == 0)
                                        {
                                            int num58 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 168);
                                            Main.dust[num58].noGravity = true;
                                            Main.dust[num58].alpha = 200;
                                        }
                                        break;
                                    case 463:
                                        {
                                            if (num8 == 54 && num7 == 0)
                                            {
                                                for (int l = 0; l < 4; l++)
                                                {
                                                    if (rand.Next(2) != 0)
                                                    {
                                                        Dust obj = Dust.NewDustDirect(new Vector2(j * 16 + 4, i * 16), 36, 8, 16);
                                                        obj.noGravity = true;
                                                        obj.alpha = 140;
                                                        obj.fadeIn = 1.2f;
                                                        obj.velocity = Vector2.Zero;
                                                    }
                                                }
                                            }
                                            if (num8 != 18 || (num7 != 0 && num7 != 36))
                                            {
                                                break;
                                            }
                                            for (int m = 0; m < 1; m++)
                                            {
                                                if (rand.Next(13) == 0)
                                                {
                                                    Dust obj2 = Dust.NewDustDirect(new Vector2(j * 16, i * 16), 8, 8, 274);
                                                    obj2.position = new Vector2(j * 16 + 8, i * 16 + 8);
                                                    Dust dust = obj2;
                                                    dust.position.X = dust.position.X + (float)((num7 == 36) ? 4 : (-4));
                                                    obj2.noGravity = true;
                                                    obj2.alpha = 128;
                                                    obj2.fadeIn = 1.2f;
                                                    obj2.noLight = true;
                                                    obj2.velocity = new Vector2(0f, rand.NextFloatDirection() * 1.2f);
                                                }
                                            }
                                            break;
                                        }
                                }
                                if (type == 139 && tile.frameX == 36 && tile.frameY % 36 == 0 && (int)Main.time % 7 == 0 && rand.Next(3) == 0)
                                {
                                    int num59 = rand.Next(570, 573);
                                    Vector2 position2 = new Vector2(j * 16 + 8, i * 16 - 8);
                                    Vector2 velocity = new Vector2(Main.windSpeed * 2f, -0.5f);
                                    velocity.X *= 1f + (float)rand.Next(-50, 51) * 0.01f;
                                    velocity.Y *= 1f + (float)rand.Next(-50, 51) * 0.01f;
                                    if (num59 == 572)
                                    {
                                        position2.X -= 8f;
                                    }
                                    if (num59 == 571)
                                    {
                                        position2.X -= 4f;
                                    }
                                    Gore.NewGore(position2, velocity, num59, 0.8f);
                                }
                                if (type == 244 && num7 == 18 && num8 == 18 && rand.Next(2) == 0)
                                {
                                    if (rand.Next(500) == 0)
                                    {
                                        Gore.NewGore(new Vector2(j * 16 + 8, i * 16 + 8), default(Vector2), 415, (float)rand.Next(51, 101) * 0.01f);
                                    }
                                    else if (rand.Next(250) == 0)
                                    {
                                        Gore.NewGore(new Vector2(j * 16 + 8, i * 16 + 8), default(Vector2), 414, (float)rand.Next(51, 101) * 0.01f);
                                    }
                                    else if (rand.Next(80) == 0)
                                    {
                                        Gore.NewGore(new Vector2(j * 16 + 8, i * 16 + 8), default(Vector2), 413, (float)rand.Next(51, 101) * 0.01f);
                                    }
                                    else if (rand.Next(10) == 0)
                                    {
                                        Gore.NewGore(new Vector2(j * 16 + 8, i * 16 + 8), default(Vector2), 412, (float)rand.Next(51, 101) * 0.01f);
                                    }
                                    else if (rand.Next(3) == 0)
                                    {
                                        Gore.NewGore(new Vector2(j * 16 + 8, i * 16 + 8), default(Vector2), 411, (float)rand.Next(51, 101) * 0.01f);
                                    }
                                }
                                if (type == 165 && num7 >= 162 && num7 <= 214 && num8 == 72 && rand.Next(60) == 0)
                                {
                                    int num60 = Dust.NewDust(new Vector2(j * 16 + 2, i * 16 + 6), 8, 4, 153);
                                    Main.dust[num60].scale -= (float)rand.Next(3) * 0.1f;
                                    Main.dust[num60].velocity.Y = 0f;
                                    Dust dust2 = Main.dust[num60];
                                    dust2.velocity.X = dust2.velocity.X * 0.05f;
                                    Main.dust[num60].alpha = 100;
                                }
                                if (type == 42 && num7 == 0)
                                {
                                    int num61 = num8 / 36;
                                    int num62 = num8 / 18 % 2;
                                    if (num61 == 7 && num62 == 1)
                                    {
                                        if (rand.Next(50) == 0)
                                        {
                                            int num63 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16 + 4), 8, 8, 58, 0f, 0f, 150);
                                            Main.dust[num63].velocity *= 0.5f;
                                        }
                                        if (rand.Next(100) == 0)
                                        {
                                            int num64 = Gore.NewGore(new Vector2(j * 16 - 2, i * 16 - 4), default(Vector2), rand.Next(16, 18));
                                            Main.gore[num64].scale *= 0.7f;
                                            Main.gore[num64].velocity *= 0.25f;
                                        }
                                    }
                                    else if (num61 == 29 && num62 == 1 && rand.Next(40) == 0)
                                    {
                                        int num65 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16), 8, 8, 59, 0f, 0f, 100);
                                        if (rand.Next(3) != 0)
                                        {
                                            Main.dust[num65].noGravity = true;
                                        }
                                        Main.dust[num65].velocity *= 0.3f;
                                        Dust dust3 = Main.dust[num65];
                                        dust3.velocity.Y = dust3.velocity.Y - 1.5f;
                                    }
                                }
                                if (type == 215 && num8 < 36 && rand.Next(3) == 0 && ((Main.drawToScreen && rand.Next(4) == 0) || !Main.drawToScreen) && num8 == 0)
                                {
                                    int num66 = Dust.NewDust(new Vector2(j * 16 + 2, i * 16 - 4), 4, 8, 31, 0f, 0f, 100);
                                    if (num7 == 0)
                                    {
                                        Dust dust4 = Main.dust[num66];
                                        dust4.position.X = dust4.position.X + (float)rand.Next(8);
                                    }
                                    if (num7 == 36)
                                    {
                                        Dust dust5 = Main.dust[num66];
                                        dust5.position.X = dust5.position.X - (float)rand.Next(8);
                                    }
                                    Main.dust[num66].alpha += rand.Next(100);
                                    Main.dust[num66].velocity *= 0.2f;
                                    Dust dust6 = Main.dust[num66];
                                    dust6.velocity.Y = dust6.velocity.Y - (0.5f + (float)rand.Next(10) * 0.1f);
                                    Main.dust[num66].fadeIn = 0.5f + (float)rand.Next(10) * 0.1f;
                                }
                                if (type == 4 && rand.Next(40) == 0 && num7 < 66)
                                {
                                    int num67 = num8 / 22;
                                    switch (num67)
                                    {
                                        case 0:
                                            num67 = 6;
                                            break;
                                        case 8:
                                            num67 = 75;
                                            break;
                                        case 9:
                                            num67 = 135;
                                            break;
                                        case 10:
                                            num67 = 158;
                                            break;
                                        case 11:
                                            num67 = 169;
                                            break;
                                        case 12:
                                            num67 = 156;
                                            break;
                                        case 13:
                                            num67 = 234;
                                            break;
                                        case 14:
                                            num67 = 66;
                                            break;
                                        default:
                                            num67 = 58 + num67;
                                            break;
                                    }
                                    int num68;
                                    switch (num7)
                                    {
                                        case 22:
                                            num68 = Dust.NewDust(new Vector2(j * 16 + 6, i * 16), 4, 4, num67, 0f, 0f, 100);
                                            break;
                                        case 44:
                                            num68 = Dust.NewDust(new Vector2(j * 16 + 2, i * 16), 4, 4, num67, 0f, 0f, 100);
                                            break;
                                        default:
                                            num68 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16), 4, 4, num67, 0f, 0f, 100);
                                            break;
                                    }
                                    if (rand.Next(3) != 0)
                                    {
                                        Main.dust[num68].noGravity = true;
                                    }
                                    Main.dust[num68].velocity *= 0.3f;
                                    Dust dust7 = Main.dust[num68];
                                    dust7.velocity.Y = dust7.velocity.Y - 1.5f;
                                    if (num67 == 66)
                                    {
                                        Main.dust[num68].color = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                                        Main.dust[num68].noGravity = true;
                                    }
                                }
                                if (type == 93 && rand.Next(40) == 0 && num7 == 0)
                                {
                                    int num69 = num8 / 54;
                                    if (num8 / 18 % 3 == 0)
                                    {
                                        int num70;
                                        switch (num69)
                                        {
                                            case 20:
                                                num70 = 59;
                                                break;
                                            default:
                                                num70 = -1;
                                                break;
                                            case 0:
                                            case 6:
                                            case 7:
                                            case 8:
                                            case 10:
                                            case 14:
                                            case 15:
                                            case 16:
                                                num70 = 6;
                                                break;
                                        }
                                        if (num70 != -1)
                                        {
                                            int num71 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16 + 2), 4, 4, num70, 0f, 0f, 100);
                                            if (rand.Next(3) != 0)
                                            {
                                                Main.dust[num71].noGravity = true;
                                            }
                                            Main.dust[num71].velocity *= 0.3f;
                                            Dust dust8 = Main.dust[num71];
                                            dust8.velocity.Y = dust8.velocity.Y - 1.5f;
                                        }
                                    }
                                }
                                if (type == 100 && rand.Next(40) == 0 && num7 < 36)
                                {
                                    int num72 = num8 / 36;
                                    if (num8 / 18 % 2 == 0)
                                    {
                                        int num73;
                                        switch (num72)
                                        {
                                            case 0:
                                            case 2:
                                            case 5:
                                            case 7:
                                            case 8:
                                            case 10:
                                            case 12:
                                            case 14:
                                            case 15:
                                            case 16:
                                                num73 = 6;
                                                break;
                                            case 20:
                                                num73 = 59;
                                                break;
                                            default:
                                                num73 = -1;
                                                break;
                                        }
                                        if (num73 != -1)
                                        {
                                            Vector2 position3 = (num7 == 0) ? ((rand.Next(3) == 0) ? new Vector2(j * 16 + 4, i * 16 + 2) : new Vector2(j * 16 + 14, i * 16 + 2)) : ((rand.Next(3) == 0) ? new Vector2(j * 16 + 6, i * 16 + 2) : new Vector2(j * 16, i * 16 + 2));
                                            int num74 = Dust.NewDust(position3, 4, 4, num73, 0f, 0f, 100);
                                            if (rand.Next(3) != 0)
                                            {
                                                Main.dust[num74].noGravity = true;
                                            }
                                            Main.dust[num74].velocity *= 0.3f;
                                            Dust dust9 = Main.dust[num74];
                                            dust9.velocity.Y = dust9.velocity.Y - 1.5f;
                                        }
                                    }
                                }
                                if (type == 98 && rand.Next(40) == 0 && num8 == 0 && num7 == 0)
                                {
                                    int num75 = Dust.NewDust(new Vector2(j * 16 + 12, i * 16 + 2), 4, 4, 6, 0f, 0f, 100);
                                    if (rand.Next(3) != 0)
                                    {
                                        Main.dust[num75].noGravity = true;
                                    }
                                    Main.dust[num75].velocity *= 0.3f;
                                    Dust dust10 = Main.dust[num75];
                                    dust10.velocity.Y = dust10.velocity.Y - 1.5f;
                                }
                                if (type == 49 && rand.Next(2) == 0)
                                {
                                    int num76 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16 - 4), 4, 4, 172, 0f, 0f, 100);
                                    if (rand.Next(3) == 0)
                                    {
                                        Main.dust[num76].scale = 0.5f;
                                    }
                                    else
                                    {
                                        Main.dust[num76].scale = 0.9f;
                                        Main.dust[num76].noGravity = true;
                                    }
                                    Main.dust[num76].velocity *= 0.3f;
                                    Dust dust11 = Main.dust[num76];
                                    dust11.velocity.Y = dust11.velocity.Y - 1.5f;
                                }
                                if (type == 372 && rand.Next(2) == 0)
                                {
                                    int num77 = Dust.NewDust(new Vector2(j * 16 + 4, i * 16 - 4), 4, 4, 242, 0f, 0f, 100);
                                    if (rand.Next(3) == 0)
                                    {
                                        Main.dust[num77].scale = 0.5f;
                                    }
                                    else
                                    {
                                        Main.dust[num77].scale = 0.9f;
                                        Main.dust[num77].noGravity = true;
                                    }
                                    Main.dust[num77].velocity *= 0.3f;
                                    Dust dust12 = Main.dust[num77];
                                    dust12.velocity.Y = dust12.velocity.Y - 1.5f;
                                }
                                if (type == 34 && rand.Next(40) == 0 && num7 < 54)
                                {
                                    int num78 = num8 / 54;
                                    int num79 = num7 / 18 % 3;
                                    if (num8 / 18 % 3 == 1 && num79 != 1)
                                    {
                                        int num80;
                                        switch (num78)
                                        {
                                            case 25:
                                                num80 = 59;
                                                break;
                                            default:
                                                num80 = -1;
                                                break;
                                            case 0:
                                            case 1:
                                            case 2:
                                            case 3:
                                            case 4:
                                            case 5:
                                            case 12:
                                            case 13:
                                            case 16:
                                            case 19:
                                            case 21:
                                                num80 = 6;
                                                break;
                                        }
                                        if (num80 != -1)
                                        {
                                            int num81 = Dust.NewDust(new Vector2(j * 16, i * 16 + 2), 14, 6, num80, 0f, 0f, 100);
                                            if (rand.Next(3) != 0)
                                            {
                                                Main.dust[num81].noGravity = true;
                                            }
                                            Main.dust[num81].velocity *= 0.3f;
                                            Dust dust13 = Main.dust[num81];
                                            dust13.velocity.Y = dust13.velocity.Y - 1.5f;
                                        }
                                    }
                                }
                                if (type == 22 && rand.Next(400) == 0)
                                {
                                    Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14);
                                }
                                else if ((type == 23 || type == 24 || type == 32) && rand.Next(500) == 0)
                                {
                                    Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14);
                                }
                                else if (type == 25 && rand.Next(700) == 0)
                                {
                                    Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14);
                                }
                                else if (type == 112 && rand.Next(700) == 0)
                                {
                                    Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14);
                                }
                                else if (type == 31 && rand.Next(20) == 0)
                                {
                                    if (num7 >= 36)
                                    {
                                        int num82 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 5, 0f, 0f, 100);
                                        Main.dust[num82].velocity.Y = 0f;
                                        Dust dust14 = Main.dust[num82];
                                        dust14.velocity.X = dust14.velocity.X * 0.3f;
                                    }
                                    else
                                    {
                                        Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14, 0f, 0f, 100);
                                    }
                                }
                                else if (type == 26 && rand.Next(20) == 0)
                                {
                                    if (num7 >= 54)
                                    {
                                        int num83 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 5, 0f, 0f, 100);
                                        Main.dust[num83].scale = 1.5f;
                                        Main.dust[num83].noGravity = true;
                                        Main.dust[num83].velocity *= 0.75f;
                                    }
                                    else
                                    {
                                        Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14, 0f, 0f, 100);
                                    }
                                }
                                else if ((type == 71 || type == 72) && rand.Next(500) == 0)
                                {
                                    Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 41, 0f, 0f, 250, default(Microsoft.Xna.Framework.Color), 0.8f);
                                }
                                else if ((type == 17 || type == 77 || type == 133) && rand.Next(40) == 0)
                                {
                                    if (num7 == 18 && num8 == 18)
                                    {
                                        int num84 = Dust.NewDust(new Vector2(j * 16 - 4, i * 16 - 6), 8, 6, 6, 0f, 0f, 100);
                                        if (rand.Next(3) != 0)
                                        {
                                            Main.dust[num84].noGravity = true;
                                        }
                                    }
                                }
                                else if (type == 405 && rand.Next(20) == 0)
                                {
                                    if (num7 == 18 && num8 == 18)
                                    {
                                        int num85 = Dust.NewDust(new Vector2(j * 16 - 4, i * 16 - 6), 24, 10, 6, 0f, 0f, 100);
                                        if (rand.Next(5) != 0)
                                        {
                                            Main.dust[num85].noGravity = true;
                                        }
                                    }
                                }
                                else if (type == 452 && num8 == 0 && num7 == 0 && rand.Next(3) == 0)
                                {
                                    Vector2 position4 = new Vector2(j * 16 + 16, i * 16 + 8);
                                    Vector2 velocity2 = new Vector2(0f, 0f);
                                    if (Main.windSpeed < 0f)
                                    {
                                        velocity2.X = 0f - Main.windSpeed;
                                    }
                                    int num86 = Main.tileFrame[type];
                                    int type2 = 907 + num86 / 5;
                                    if (rand.Next(2) == 0)
                                    {
                                        Gore.NewGore(position4, velocity2, type2, rand.NextFloat() * 0.4f + 0.4f);
                                    }
                                }
                                else if (type == 406 && num8 == 54 && num7 == 0 && rand.Next(3) == 0)
                                {
                                    Vector2 position5 = new Vector2(j * 16 + 16, i * 16 + 8);
                                    Vector2 velocity3 = new Vector2(0f, 0f);
                                    if (Main.windSpeed < 0f)
                                    {
                                        velocity3.X = 0f - Main.windSpeed;
                                    }
                                    int type3 = rand.Next(825, 828);
                                    if (rand.Next(4) == 0)
                                    {
                                        Gore.NewGore(position5, velocity3, type3, rand.NextFloat() * 0.2f + 0.2f);
                                    }
                                    else if (rand.Next(2) == 0)
                                    {
                                        Gore.NewGore(position5, velocity3, type3, rand.NextFloat() * 0.3f + 0.3f);
                                    }
                                    else
                                    {
                                        Gore.NewGore(position5, velocity3, type3, rand.NextFloat() * 0.4f + 0.4f);
                                    }
                                }
                                else if (type == 37 && rand.Next(250) == 0)
                                {
                                    int num87 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), rand.Next(3));
                                    if (Main.dust[num87].scale > 1f)
                                    {
                                        Main.dust[num87].noGravity = true;
                                    }
                                }
                                else if ((type == 58 || type == 76) && rand.Next(250) == 0)
                                {
                                    int num88 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), rand.Next(3));
                                    if (Main.dust[num88].scale > 1f)
                                    {
                                        Main.dust[num88].noGravity = true;
                                    }
                                    Main.dust[num88].noLight = true;
                                }
                                else if (type == 61)
                                {
                                    if (num7 == 144)
                                    {
                                        if (rand.Next(60) == 0)
                                        {
                                            int num89 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 44, 0f, 0f, 250, default(Microsoft.Xna.Framework.Color), 0.4f);
                                            Main.dust[num89].fadeIn = 0.7f;
                                        }
                                        drawColor.A = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f);
                                        drawColor.R = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f);
                                        drawColor.B = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f);
                                        drawColor.G = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f);
                                    }
                                }
                                else if (Main.tileShine[type] > 0)
                                {
                                    Main.tileShine[211] = 500;
                                    if (drawColor.R > 20 || drawColor.B > 20 || drawColor.G > 20)
                                    {
                                        int num90 = drawColor.R;
                                        if (drawColor.G > num90)
                                        {
                                            num90 = drawColor.G;
                                        }
                                        if (drawColor.B > num90)
                                        {
                                            num90 = drawColor.B;
                                        }
                                        num90 /= 30;
                                        if (rand.Next(Main.tileShine[type]) < num90 && (type != 21 || (num7 >= 36 && num7 < 180) || (num7 >= 396 && num7 <= 409)) && type != 467)
                                        {
                                            Microsoft.Xna.Framework.Color newColor = Microsoft.Xna.Framework.Color.White;
                                            if (type == 178)
                                            {
                                                switch (num7 / 18)
                                                {
                                                    case 0:
                                                        newColor = new Microsoft.Xna.Framework.Color(255, 0, 255, 255);
                                                        break;
                                                    case 1:
                                                        newColor = new Microsoft.Xna.Framework.Color(255, 255, 0, 255);
                                                        break;
                                                    case 2:
                                                        newColor = new Microsoft.Xna.Framework.Color(0, 0, 255, 255);
                                                        break;
                                                    case 3:
                                                        newColor = new Microsoft.Xna.Framework.Color(0, 255, 0, 255);
                                                        break;
                                                    case 4:
                                                        newColor = new Microsoft.Xna.Framework.Color(255, 0, 0, 255);
                                                        break;
                                                    case 5:
                                                        newColor = new Microsoft.Xna.Framework.Color(255, 255, 255, 255);
                                                        break;
                                                    case 6:
                                                        newColor = new Microsoft.Xna.Framework.Color(255, 255, 0, 255);
                                                        break;
                                                }
                                                int num91 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
                                                Main.dust[num91].velocity *= 0f;
                                            }
                                            else
                                            {
                                                if (type == 63)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(0, 0, 255, 255);
                                                }
                                                if (type == 64)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 0, 0, 255);
                                                }
                                                if (type == 65)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(0, 255, 0, 255);
                                                }
                                                if (type == 66)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 255, 0, 255);
                                                }
                                                if (type == 67)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 0, 255, 255);
                                                }
                                                if (type == 68)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 255, 255, 255);
                                                }
                                                if (type == 12)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 0, 0, 255);
                                                }
                                                if (type == 204)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(255, 0, 0, 255);
                                                }
                                                if (type == 211)
                                                {
                                                    newColor = new Microsoft.Xna.Framework.Color(50, 255, 100, 255);
                                                }
                                                int num92 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
                                                Main.dust[num92].velocity *= 0f;
                                            }
                                        }
                                    }
                                }
                            }
                            if (TileID.Sets.BasicChest[type])
                            {
                                Microsoft.Xna.Framework.Point key = new Microsoft.Xna.Framework.Point(j, i);
                                if (num7 % 36 != 0)
                                {
                                    key.X--;
                                }
                                if (num8 % 36 != 0)
                                {
                                    key.Y--;
                                }
                                if (!dictionary.ContainsKey(key))
                                {
                                    dictionary[key] = Chest.FindChest(key.X, key.Y);
                                }
                                int num93 = num7 / 18;
                                int num94 = num8 / 18;
                                int num95 = num7 / 36;
                                frameXOffset = num93 * 18 - num7;
                                int num96 = num94 * 18;
                                if (dictionary[key] != -1)
                                {
                                    int frame = Main.chest[dictionary[key]].frame;
                                    if (frame == 1)
                                    {
                                        num96 += 38;
                                    }
                                    if (frame == 2)
                                    {
                                        num96 += 76;
                                    }
                                }
                                frameYOffset = num96 - num8;
                                if (num94 != 0)
                                {
                                    height = 18;
                                }
                                if (type == 21 && (num95 == 48 || num95 == 49))
                                {
                                    value3 = new Microsoft.Xna.Framework.Rectangle(16 * (num93 % 2), num8 + frameYOffset, width, height);
                                }
                            }
                            if (type == 378)
                            {
                                Microsoft.Xna.Framework.Point key2 = new Microsoft.Xna.Framework.Point(j, i);
                                if (num7 % 36 != 0)
                                {
                                    key2.X--;
                                }
                                if (num8 % 54 != 0)
                                {
                                    key2.Y -= num8 / 18;
                                }
                                if (!dictionary2.ContainsKey(key2))
                                {
                                    dictionary2[key2] = TETrainingDummy.Find(key2.X, key2.Y);
                                }
                                if (dictionary2[key2] != -1)
                                {
                                    int num97 = ((TETrainingDummy)TileEntity.ByID[dictionary2[key2]]).npc;
                                    if (num97 != -1)
                                    {
                                        frameYOffset = Main.npc[num97].frame.Y / 55 * 54 + num8 - num8;
                                    }
                                }
                            }
                            if (type == 395)
                            {
                                Microsoft.Xna.Framework.Point key3 = new Microsoft.Xna.Framework.Point(j, i);
                                if (num7 % 36 != 0)
                                {
                                    key3.X--;
                                }
                                if (num8 % 36 != 0)
                                {
                                    key3.Y--;
                                }
                                if (!dictionary3.ContainsKey(key3))
                                {
                                    dictionary3[key3] = TEItemFrame.Find(key3.X, key3.Y);
                                    if (dictionary3[key3] != -1)
                                    {
                                        Main.specX[nextSpecialDrawIndex] = key3.X;
                                        Main.specY[nextSpecialDrawIndex] = key3.Y;
                                        nextSpecialDrawIndex++;
                                    }
                                }
                            }
                            if ((type == 269 || type == 128) && num8 / 18 == 2)
                            {
                                if (num7 >= 100)
                                {
                                    bool flag6 = false;
                                    int num98 = Main.tile[j, i - 1].frameX;
                                    if (num98 >= 100)
                                    {
                                        int num99 = 0;
                                        while (num98 >= 100)
                                        {
                                            num99++;
                                            num98 -= 100;
                                        }
                                        switch (num99)
                                        {
                                            case 15:
                                            case 36:
                                            case 41:
                                            case 42:
                                            case 58:
                                            case 59:
                                            case 60:
                                            case 61:
                                            case 62:
                                            case 63:
                                                flag6 = true;
                                                break;
                                        }
                                    }
                                    if (!flag6)
                                    {
                                        Main.specX[nextSpecialDrawIndex] = j;
                                        Main.specY[nextSpecialDrawIndex] = i;
                                        nextSpecialDrawIndex++;
                                    }
                                }
                                if (Main.tile[j, i - 1].frameX >= 100)
                                {
                                    Main.specX[nextSpecialDrawIndex] = j;
                                    Main.specY[nextSpecialDrawIndex] = i - 1;
                                    nextSpecialDrawIndex++;
                                }
                                if (Main.tile[j, i - 2].frameX >= 100)
                                {
                                    Main.specX[nextSpecialDrawIndex] = j;
                                    Main.specY[nextSpecialDrawIndex] = i - 2;
                                    nextSpecialDrawIndex++;
                                }
                            }
                            if (type == 5 && num8 >= 198 && num7 >= 22)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 323 && num7 <= 132 && num7 >= 88)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 412 && num7 == 0 && num8 == 0)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 237 && num7 == 18 && num8 == 0)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 334 && num8 / 18 == 1 && num7 >= 5000)
                            {
                                int num100 = Main.tile[j, i].frameX;
                                int num101 = 0;
                                while (num100 >= 5000)
                                {
                                    num101++;
                                    num100 -= 5000;
                                }
                                if (num101 == 1 || num101 == 4)
                                {
                                    Main.specX[nextSpecialDrawIndex] = j;
                                    Main.specY[nextSpecialDrawIndex] = i;
                                    nextSpecialDrawIndex++;
                                }
                            }
                            if (type == 5 && num8 >= 198 && num7 >= 22)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 323 && num7 <= 132 && num7 >= 88)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 412 && num7 == 0 && num8 == 0)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 237 && num7 == 18 && num8 == 0)
                            {
                                Main.specX[nextSpecialDrawIndex] = j;
                                Main.specY[nextSpecialDrawIndex] = i;
                                nextSpecialDrawIndex++;
                            }
                            if (type == 72 && num7 >= 36)
                            {
                                int num102 = 0;
                                switch (num8)
                                {
                                    case 18:
                                        num102 = 1;
                                        break;
                                    case 36:
                                        num102 = 2;
                                        break;
                                }
                                spriteBatch.Draw(Main.shroomCapTexture, new Vector2(j * 16 - (int)screenPosition.X - 22, i * 16 - (int)screenPosition.Y - 26) + value, new Microsoft.Xna.Framework.Rectangle(num102 * 62, 0, 60, 42), Lighting.GetColor(j, i), 0f, default(Vector2), 1f, spriteEffects, 0f);
                            }
                            TileLoader.DrawEffects(j, i, type, spriteBatch, ref drawColor, ref nextSpecialDrawIndex);
                            if (drawColor.R >= 1 || drawColor.G >= 1 || drawColor.B >= 1)
                            {
                                Tile tile2 = Main.tile[j + 1, i];
                                Tile tile3 = Main.tile[j - 1, i];
                                Tile tile4 = Main.tile[j, i - 1];
                                Tile tile5 = Main.tile[j, i + 1];
                                if (tile2 == null)
                                {
                                    tile2 = new Tile();
                                    Main.tile[j + 1, i] = tile2;
                                }
                                if (tile3 == null)
                                {
                                    tile3 = new Tile();
                                    Main.tile[j - 1, i] = tile3;
                                }
                                if (tile4 == null)
                                {
                                    tile4 = new Tile();
                                    Main.tile[j, i - 1] = tile4;
                                }
                                if (tile5 == null)
                                {
                                    tile5 = new Tile();
                                    Main.tile[j, i + 1] = tile5;
                                }
                                if (solidOnly && flag && !tile.inActive() && !Main.tileSolidTop[type])
                                {
                                    bool flag7 = false;
                                    if (tile.halfBrick())
                                    {
                                        int num103 = 160;
                                        if ((tile3.liquid > num103 || tile2.liquid > num103) && Main.instance.waterfallManager.CheckForWaterfall(j, i))
                                        {
                                            flag7 = true;
                                        }
                                    }
                                    if (!flag7)
                                    {
                                        int num104 = 0;
                                        bool flag8 = false;
                                        bool flag9 = false;
                                        bool flag10 = false;
                                        bool flag11 = false;
                                        int num105 = 0;
                                        bool flag12 = false;
                                        int num106 = tile.slope();
                                        if (tile3.liquid > 0 && num106 != 1 && num106 != 3)
                                        {
                                            flag8 = true;
                                            switch (tile3.liquidType())
                                            {
                                                case 0:
                                                    flag12 = true;
                                                    break;
                                                case 1:
                                                    num105 = 1;
                                                    break;
                                                case 2:
                                                    num105 = 11;
                                                    break;
                                            }
                                            if (tile3.liquid > num104)
                                            {
                                                num104 = tile3.liquid;
                                            }
                                        }
                                        if (tile2.liquid > 0 && num106 != 2 && num106 != 4)
                                        {
                                            flag9 = true;
                                            switch (tile2.liquidType())
                                            {
                                                case 0:
                                                    flag12 = true;
                                                    break;
                                                case 1:
                                                    num105 = 1;
                                                    break;
                                                case 2:
                                                    num105 = 11;
                                                    break;
                                            }
                                            if (tile2.liquid > num104)
                                            {
                                                num104 = tile2.liquid;
                                            }
                                        }
                                        if (tile4.liquid > 0 && num106 != 3 && num106 != 4)
                                        {
                                            flag10 = true;
                                            switch (tile4.liquidType())
                                            {
                                                case 0:
                                                    flag12 = true;
                                                    break;
                                                case 1:
                                                    num105 = 1;
                                                    break;
                                                case 2:
                                                    num105 = 11;
                                                    break;
                                            }
                                        }
                                        if (tile5.liquid > 0 && num106 != 1 && num106 != 2)
                                        {
                                            if (tile5.liquid > 240)
                                            {
                                                flag11 = true;
                                            }
                                            switch (tile5.liquidType())
                                            {
                                                case 0:
                                                    flag12 = true;
                                                    break;
                                                case 1:
                                                    num105 = 1;
                                                    break;
                                                case 2:
                                                    num105 = 11;
                                                    break;
                                            }
                                        }
                                        if (waterStyleOverride != -1)
                                        {
                                            Main.waterStyle = waterStyleOverride;
                                        }
                                        if (num105 == 0)
                                        {
                                            num105 = Main.waterStyle;
                                        }
                                        if ((flag10 || flag11 || flag8 || flag9) && (!flag12 || num105 != 1))
                                        {
                                            Microsoft.Xna.Framework.Color color6 = Lighting.GetColor(j, i);
                                            Vector2 value4 = new Vector2(j * 16, i * 16);
                                            Microsoft.Xna.Framework.Rectangle value5 = new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 16);
                                            if (flag11 && (flag8 || flag9))
                                            {
                                                flag8 = true;
                                                flag9 = true;
                                            }
                                            if ((!flag10 || (!flag8 && !flag9)) && (!flag11 || !flag10))
                                            {
                                                if (flag10)
                                                {
                                                    value5 = new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 4);
                                                    if (tile.halfBrick() || tile.slope() != 0)
                                                    {
                                                        value5 = new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 12);
                                                    }
                                                }
                                                else if (flag11 && !flag8 && !flag9)
                                                {
                                                    value4 = new Vector2(j * 16, i * 16 + 12);
                                                    value5 = new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 4);
                                                }
                                                else
                                                {
                                                    float num107 = 256 - num104;
                                                    num107 /= 32f;
                                                    int y = 4;
                                                    if (tile4.liquid == 0 && !WorldGen.SolidTile(j, i - 1))
                                                    {
                                                        y = 0;
                                                    }
                                                    if ((flag8 && flag9) || tile.halfBrick() || tile.slope() != 0)
                                                    {
                                                        value4 = new Vector2(j * 16, i * 16 + (int)num107 * 2);
                                                        value5 = new Microsoft.Xna.Framework.Rectangle(0, y, 16, 16 - (int)num107 * 2);
                                                    }
                                                    else if (!flag8)
                                                    {
                                                        value4 = new Vector2(j * 16 + 12, i * 16 + (int)num107 * 2);
                                                        value5 = new Microsoft.Xna.Framework.Rectangle(0, y, 4, 16 - (int)num107 * 2);
                                                    }
                                                    else
                                                    {
                                                        value4 = new Vector2(j * 16, i * 16 + (int)num107 * 2);
                                                        value5 = new Microsoft.Xna.Framework.Rectangle(0, y, 4, 16 - (int)num107 * 2);
                                                    }
                                                }
                                            }
                                            float num108 = 0.5f;
                                            switch (num105)
                                            {
                                                case 1:
                                                    num108 = 1f;
                                                    break;
                                                case 11:
                                                    num108 *= 1.7f;
                                                    if (num108 > 1f)
                                                    {
                                                        num108 = 1f;
                                                    }
                                                    break;
                                            }
                                            if ((double)i < Main.worldSurface || num108 > 1f)
                                            {
                                                num108 = 1f;
                                                if (tile4.wall > 0 || tile3.wall > 0 || tile2.wall > 0 || tile5.wall > 0)
                                                {
                                                    num108 = 0.65f;
                                                }
                                                if (tile.wall > 0)
                                                {
                                                    num108 = 0.5f;
                                                }
                                            }
                                            if (tile.halfBrick() && tile4.liquid > 0 && tile.wall > 0)
                                            {
                                                num108 = 0f;
                                            }
                                            float num109 = (float)(int)color6.R * num108;
                                            float num110 = (float)(int)color6.G * num108;
                                            float num111 = (float)(int)color6.B * num108;
                                            float num112 = (float)(int)color6.A * num108;
                                            color6 = new Microsoft.Xna.Framework.Color((byte)num109, (byte)num110, (byte)num111, (byte)num112);
                                            spriteBatch.Draw(Main.liquidTexture[num105], value4 - screenPosition + value, value5, color6, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        }
                                    }
                                }
                                if (type == 314)
                                {
                                    if (tile.inActive())
                                    {
                                        drawColor = tile.actColor(drawColor);
                                    }
                                    else if (Main.tileShine2[type])
                                    {
                                        drawColor = Main.shine(drawColor, type);
                                    }
                                    Minecart.TrackColors(j, i, tile, out int frontColor, out int num113);
                                    Texture2D texture = (!Main.canDrawColorTile(type, frontColor)) ? Main.tileTexture[type] : Main.tileAltTexture[type, frontColor];
                                    Texture2D texture2 = (!Main.canDrawColorTile(type, num113)) ? Main.tileTexture[type] : Main.tileAltTexture[type, num113];
                                    tile.frameNumber();
                                    if (num8 != -1)
                                    {
                                        spriteBatch.Draw(texture2, new Vector2(j * 16 - (int)screenPosition.X, i * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(num8, Main.tileFrame[314]), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    spriteBatch.Draw(texture, new Vector2(j * 16 - (int)screenPosition.X, i * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(num7, Main.tileFrame[314]), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    if (Minecart.DrawLeftDecoration(num8))
                                    {
                                        spriteBatch.Draw(texture2, new Vector2(j * 16 - (int)screenPosition.X, (i + 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(36), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    if (Minecart.DrawLeftDecoration(num7))
                                    {
                                        spriteBatch.Draw(texture, new Vector2(j * 16 - (int)screenPosition.X, (i + 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(36), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    if (Minecart.DrawRightDecoration(num8))
                                    {
                                        spriteBatch.Draw(texture2, new Vector2(j * 16 - (int)screenPosition.X, (i + 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(37, Main.tileFrame[314]), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    if (Minecart.DrawRightDecoration(num7))
                                    {
                                        spriteBatch.Draw(texture, new Vector2(j * 16 - (int)screenPosition.X, (i + 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(37), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    if (Minecart.DrawBumper(num7))
                                    {
                                        spriteBatch.Draw(texture, new Vector2(j * 16 - (int)screenPosition.X, (i - 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(39), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (Minecart.DrawBouncyBumper(num7))
                                    {
                                        spriteBatch.Draw(texture, new Vector2(j * 16 - (int)screenPosition.X, (i - 1) * 16 - (int)screenPosition.Y) + value, Minecart.GetSourceRect(38), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                }
                                else if (type == 51)
                                {
                                    Microsoft.Xna.Framework.Color color7 = Lighting.GetColor(j, i);
                                    float num114 = 0.5f;
                                    float num115 = (float)(int)color7.R * num114;
                                    float num116 = (float)(int)color7.G * num114;
                                    float num117 = (float)(int)color7.B * num114;
                                    float num118 = (float)(int)color7.A * num114;
                                    color7 = new Microsoft.Xna.Framework.Color((byte)num115, (byte)num116, (byte)num117, (byte)num118);
                                    if (Main.canDrawColorTile(j, i))
                                    {
                                        spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), color7, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), color7, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                }
                                else if (type == 171)
                                {
                                    if (num5 > i - num8 && num8 == 7)
                                    {
                                        offsetY -= 16 * num8;
                                        num7 = Main.tile[j, i - num8].frameX;
                                        num8 = Main.tile[j, i - num8].frameY;
                                    }
                                    if (num7 >= 10)
                                    {
                                        int num119 = 0;
                                        if ((num8 & 1) == 1)
                                        {
                                            num119++;
                                        }
                                        if ((num8 & 2) == 2)
                                        {
                                            num119 += 2;
                                        }
                                        if ((num8 & 4) == 4)
                                        {
                                            num119 += 4;
                                        }
                                        int num120 = 0;
                                        if ((num8 & 8) == 8)
                                        {
                                            num120++;
                                        }
                                        if ((num8 & 0x10) == 16)
                                        {
                                            num120 += 2;
                                        }
                                        if ((num8 & 0x20) == 32)
                                        {
                                            num120 += 4;
                                        }
                                        int num121 = 0;
                                        if ((num8 & 0x40) == 64)
                                        {
                                            num121++;
                                        }
                                        if ((num8 & 0x80) == 128)
                                        {
                                            num121 += 2;
                                        }
                                        if ((num8 & 0x100) == 256)
                                        {
                                            num121 += 4;
                                        }
                                        if ((num8 & 0x200) == 512)
                                        {
                                            num121 += 8;
                                        }
                                        int num122 = 0;
                                        if ((num8 & 0x400) == 1024)
                                        {
                                            num122++;
                                        }
                                        if ((num8 & 0x800) == 2048)
                                        {
                                            num122 += 2;
                                        }
                                        if ((num8 & 0x1000) == 4096)
                                        {
                                            num122 += 4;
                                        }
                                        if ((num8 & 0x2000) == 8192)
                                        {
                                            num122 += 8;
                                        }
                                        Microsoft.Xna.Framework.Color color8 = Lighting.GetColor(j + 1, i + 4);
                                        spriteBatch.Draw(Main.xmasTree[0], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 128), color8, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        if (num119 > 0)
                                        {
                                            num119--;
                                            Microsoft.Xna.Framework.Color color9 = color8;
                                            if (num119 != 3)
                                            {
                                                color9 = new Microsoft.Xna.Framework.Color(255, 255, 255, 255);
                                            }
                                            spriteBatch.Draw(Main.xmasTree[3], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(66 * num119, 0, 64, 128), color9, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        }
                                        if (num120 > 0)
                                        {
                                            num120--;
                                            spriteBatch.Draw(Main.xmasTree[1], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(66 * num120, 0, 64, 128), color8, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        }
                                        if (num121 > 0)
                                        {
                                            num121--;
                                            spriteBatch.Draw(Main.xmasTree[2], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(66 * num121, 0, 64, 128), color8, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        }
                                        if (num122 > 0)
                                        {
                                            num122--;
                                            spriteBatch.Draw(Main.xmasTree[4], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(66 * num122, 130 * Main.tileFrame[171], 64, 128), new Microsoft.Xna.Framework.Color(255, 255, 255, 255), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                                        }
                                    }
                                }
                                else if (type == 160 && !tile.halfBrick())
                                {
                                    Microsoft.Xna.Framework.Color color10 = default(Microsoft.Xna.Framework.Color);
                                    color10 = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
                                    if (tile.inActive())
                                    {
                                        color10 = tile.actColor(color10);
                                    }
                                    if (tile.slope() == 0)
                                    {
                                        spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (tile.slope() > 2)
                                    {
                                        if (tile.slope() == 3)
                                        {
                                            for (int n = 0; n < 8; n++)
                                            {
                                                int num123 = 2;
                                                int num124 = n * 2;
                                                int num125 = n * -2;
                                                int num126 = 16 - n * 2;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num124, i * 16 - (int)screenPosition.Y + offsetY + n * num123 + num125) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num124, num8 + 16 - num126, num123, num126), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num124, i * 16 - (int)screenPosition.Y + offsetY + n * num123 + num125) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num124, num8 + 16 - num126, num123, num126), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int num127 = 0; num127 < 8; num127++)
                                            {
                                                int num128 = 2;
                                                int num129 = 16 - num127 * num128 - num128;
                                                int num130 = 16 - num127 * num128;
                                                int num131 = num127 * -2;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num129, i * 16 - (int)screenPosition.Y + offsetY + num127 * num128 + num131) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num129, num8 + 16 - num130, num128, num130), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num129, i * 16 - (int)screenPosition.Y + offsetY + num127 * num128 + num131) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num129, num8 + 16 - num130, num128, num130), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, 16, 2), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, 16, 2), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                    else
                                    {
                                        if (tile.slope() == 1)
                                        {
                                            for (int num132 = 0; num132 < 8; num132++)
                                            {
                                                int num133 = 2;
                                                int num134 = num132 * 2;
                                                int height2 = 14 - num132 * num133;
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num134, i * 16 - (int)screenPosition.Y + offsetY + num132 * num133) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num134, num8, num133, height2), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        if (tile.slope() == 2)
                                        {
                                            for (int num135 = 0; num135 < 8; num135++)
                                            {
                                                int num136 = 2;
                                                int num137 = 16 - num135 * num136 - num136;
                                                int height3 = 14 - num135 * num136;
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num137, i * 16 - (int)screenPosition.Y + offsetY + num135 * num136) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num137, num8, num136, height3), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 14) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + 14, 16, 2), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 14) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + 14, 16, 2), color10, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                }
                                else if (tile.slope() > 0)
                                {
                                    if (tile.inActive())
                                    {
                                        drawColor = tile.actColor(drawColor);
                                    }
                                    else if (Main.tileShine2[type])
                                    {
                                        drawColor = Main.shine(drawColor, type);
                                    }
                                    if (TileID.Sets.Platforms[tile.type])
                                    {
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        if (tile.slope() == 1 && Main.tile[j + 1, i + 1].active() && Main.tile[j + 1, i + 1].slope() != 2 && !Main.tile[j + 1, i + 1].halfBrick() && !TileID.Sets.BlocksStairs[Main.tile[j + 1, i + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[j, i + 1].type])
                                        {
                                            if (TileID.Sets.Platforms[Main.tile[j + 1, i + 1].type] && Main.tile[j + 1, i + 1].slope() == 0)
                                            {
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(324, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(324, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                            else if (Main.canDrawColorTile(j, i))
                                            {
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(198, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                            else
                                            {
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(198, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        else if (tile.slope() == 2 && Main.tile[j - 1, i + 1].active() && Main.tile[j - 1, i + 1].slope() != 1 && !Main.tile[j - 1, i + 1].halfBrick() && !TileID.Sets.BlocksStairs[Main.tile[j - 1, i + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[j, i + 1].type])
                                        {
                                            if (TileID.Sets.Platforms[Main.tile[j - 1, i + 1].type] && Main.tile[j - 1, i + 1].slope() == 0)
                                            {
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(306, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(306, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                            else if (Main.canDrawColorTile(j, i))
                                            {
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(162, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                            else
                                            {
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 16) + value, new Microsoft.Xna.Framework.Rectangle(162, num8, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                    }
                                    else if (TileID.Sets.HasSlopeFrames[tile.type])
                                    {
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, 16, 16), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                    else if (tile.slope() > 2)
                                    {
                                        if (tile.slope() == 3)
                                        {
                                            for (int num138 = 0; num138 < 8; num138++)
                                            {
                                                int num139 = 2;
                                                int num140 = num138 * 2;
                                                int num141 = num138 * -2;
                                                int num142 = 16 - num138 * 2;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num140, i * 16 - (int)screenPosition.Y + offsetY + num138 * num139 + num141) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num140 + frameXOffset, num8 + 16 - num142 + frameYOffset, num139, num142), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num140, i * 16 - (int)screenPosition.Y + offsetY + num138 * num139 + num141) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num140 + frameXOffset, num8 + 16 - num142 + frameYOffset, num139, num142), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int num143 = 0; num143 < 8; num143++)
                                            {
                                                int num144 = 2;
                                                int num145 = 16 - num143 * num144 - num144;
                                                int num146 = 16 - num143 * num144;
                                                int num147 = num143 * -2;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num145, i * 16 - (int)screenPosition.Y + offsetY + num143 * num144 + num147) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num145 + frameXOffset, num8 + 16 - num146 + frameYOffset, num144, num146), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num145, i * 16 - (int)screenPosition.Y + offsetY + num143 * num144 + num147) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num145 + frameXOffset, num8 + 16 - num146 + frameYOffset, num144, num146), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, 16, 2), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, 16, 2), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                    else
                                    {
                                        if (tile.slope() == 1)
                                        {
                                            for (int num148 = 0; num148 < 8; num148++)
                                            {
                                                int num149 = 2;
                                                int num150 = num148 * 2;
                                                int height4 = 14 - num148 * num149;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num150, i * 16 - (int)screenPosition.Y + offsetY + num148 * num149) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num150 + frameXOffset, num8 + frameYOffset, num149, height4), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num150, i * 16 - (int)screenPosition.Y + offsetY + num148 * num149) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num150 + frameXOffset, num8 + frameYOffset, num149, height4), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        if (tile.slope() == 2)
                                        {
                                            for (int num151 = 0; num151 < 8; num151++)
                                            {
                                                int num152 = 2;
                                                int num153 = 16 - num151 * num152 - num152;
                                                int height5 = 14 - num151 * num152;
                                                if (Main.canDrawColorTile(j, i))
                                                {
                                                    spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num153, i * 16 - (int)screenPosition.Y + offsetY + num151 * num152) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num153 + frameXOffset, num8 + frameYOffset, num152, height5), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num153, i * 16 - (int)screenPosition.Y + offsetY + num151 * num152) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num153 + frameXOffset, num8 + frameYOffset, num152, height5), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 14) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + 14 + frameYOffset, 16, 2), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 14) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + 14 + frameYOffset, 16, 2), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                }
                                else if (type == 129)
                                {
                                    Vector2 value6 = new Vector2(0f, 0f);
                                    if (num8 < 36)
                                    {
                                        value6.Y += 2 * (num8 == 0).ToDirectionInt();
                                    }
                                    else
                                    {
                                        value6.X += 2 * (num8 == 36).ToDirectionInt();
                                    }
                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value + value6, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(255, 255, 255, 100), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                }
                                else if (Main.tileAlch[type])
                                {
                                    height = 20;
                                    offsetY = -2;
                                    int num154 = type;
                                    int num155 = num7 / 18;
                                    if (num154 > 82)
                                    {
                                        if (num155 == 0 && Main.dayTime)
                                        {
                                            num154 = 84;
                                        }
                                        if (num155 == 1 && !Main.dayTime)
                                        {
                                            num154 = 84;
                                        }
                                        if (num155 == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
                                        {
                                            num154 = 84;
                                        }
                                        if (num155 == 4 && (Main.raining || Main.cloudAlpha > 0f))
                                        {
                                            num154 = 84;
                                        }
                                        if (num155 == 5 && !Main.raining && Main.time > 40500.0)
                                        {
                                            num154 = 84;
                                        }
                                    }
                                    if (num154 == 84)
                                    {
                                        if (num155 == 0 && rand.Next(100) == 0)
                                        {
                                            int num156 = Dust.NewDust(new Vector2(j * 16, i * 16 - 4), 16, 16, 19, 0f, 0f, 160, default(Microsoft.Xna.Framework.Color), 0.1f);
                                            Dust dust15 = Main.dust[num156];
                                            dust15.velocity.X = dust15.velocity.X / 2f;
                                            Dust dust16 = Main.dust[num156];
                                            dust16.velocity.Y = dust16.velocity.Y / 2f;
                                            Main.dust[num156].noGravity = true;
                                            Main.dust[num156].fadeIn = 1f;
                                        }
                                        if (num155 == 1 && rand.Next(100) == 0)
                                        {
                                            Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 41, 0f, 0f, 250, default(Microsoft.Xna.Framework.Color), 0.8f);
                                        }
                                        if (num155 == 3)
                                        {
                                            if (rand.Next(200) == 0)
                                            {
                                                int num157 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 14, 0f, 0f, 100, default(Microsoft.Xna.Framework.Color), 0.2f);
                                                Main.dust[num157].fadeIn = 1.2f;
                                            }
                                            if (rand.Next(75) == 0)
                                            {
                                                int num158 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 16, 27, 0f, 0f, 100);
                                                Dust dust17 = Main.dust[num158];
                                                dust17.velocity.X = dust17.velocity.X / 2f;
                                                Dust dust18 = Main.dust[num158];
                                                dust18.velocity.Y = dust18.velocity.Y / 2f;
                                            }
                                        }
                                        if (num155 == 4 && rand.Next(150) == 0)
                                        {
                                            int num159 = Dust.NewDust(new Vector2(j * 16, i * 16), 16, 8, 16);
                                            Dust dust19 = Main.dust[num159];
                                            dust19.velocity.X = dust19.velocity.X / 3f;
                                            Dust dust20 = Main.dust[num159];
                                            dust20.velocity.Y = dust20.velocity.Y / 3f;
                                            Dust dust21 = Main.dust[num159];
                                            dust21.velocity.Y = dust21.velocity.Y - 0.7f;
                                            Main.dust[num159].alpha = 50;
                                            Main.dust[num159].scale *= 0.1f;
                                            Main.dust[num159].fadeIn = 0.9f;
                                            Main.dust[num159].noGravity = true;
                                        }
                                        if (num155 == 5)
                                        {
                                            if (rand.Next(40) == 0)
                                            {
                                                int num160 = Dust.NewDust(new Vector2(j * 16, i * 16 - 6), 16, 16, 6, 0f, 0f, 0, default(Microsoft.Xna.Framework.Color), 1.5f);
                                                Dust dust22 = Main.dust[num160];
                                                dust22.velocity.Y = dust22.velocity.Y - 2f;
                                                Main.dust[num160].noGravity = true;
                                            }
                                            drawColor.A = (byte)((int)Main.mouseTextColor / 2);
                                            drawColor.G = Main.mouseTextColor;
                                            drawColor.B = Main.mouseTextColor;
                                        }
                                        if (num155 == 6)
                                        {
                                            if (rand.Next(30) == 0)
                                            {
                                                int num161 = Dust.NewDust(newColor: new Microsoft.Xna.Framework.Color(50, 255, 255, 255), Position: new Vector2(j * 16, i * 16), Width: 16, Height: 16, Type: 43, SpeedX: 0f, SpeedY: 0f, Alpha: 254, Scale: 0.5f);
                                                Main.dust[num161].velocity *= 0f;
                                            }
                                            byte b4 = (byte)((Main.mouseTextColor + drawColor.G * 2) / 3);
                                            byte b5 = (byte)((Main.mouseTextColor + drawColor.B * 2) / 3);
                                            if (b4 > drawColor.G)
                                            {
                                                drawColor.G = b4;
                                            }
                                            if (b5 > drawColor.B)
                                            {
                                                drawColor.B = b5;
                                            }
                                        }
                                    }
                                    if (Main.canDrawColorTile(j, i))
                                    {
                                        spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else
                                    {
                                        Main.instance.LoadTiles(num154);
                                        spriteBatch.Draw(Main.tileTexture[num154], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                }
                                else if (type == 80)
                                {
                                    bool flag13 = false;
                                    bool flag14 = false;
                                    bool flag15 = false;
                                    Texture2D texture2D3 = null;
                                    if (!Main.canDrawColorTile(j, i))
                                    {
                                        int num162 = j;
                                        if (num7 == 36)
                                        {
                                            num162--;
                                        }
                                        if (num7 == 54)
                                        {
                                            num162++;
                                        }
                                        if (num7 == 108)
                                        {
                                            num162 = ((num8 != 18) ? (num162 + 1) : (num162 - 1));
                                        }
                                        int num163 = i;
                                        bool flag16 = false;
                                        if (Main.tile[num162, num163].type == 80 && Main.tile[num162, num163].active())
                                        {
                                            flag16 = true;
                                        }
                                        while (!Main.tile[num162, num163].active() || !Main.tileSolid[Main.tile[num162, num163].type] || !flag16)
                                        {
                                            if (Main.tile[num162, num163].type == 80 && Main.tile[num162, num163].active())
                                            {
                                                flag16 = true;
                                            }
                                            num163++;
                                            if (num163 > i + 20)
                                            {
                                                break;
                                            }
                                        }
                                        if (Main.tile[num162, num163].type == 112)
                                        {
                                            flag13 = true;
                                        }
                                        if (Main.tile[num162, num163].type == 116)
                                        {
                                            flag14 = true;
                                        }
                                        if (Main.tile[num162, num163].type == 234)
                                        {
                                            flag15 = true;
                                        }
                                        texture2D3 = TileLoader.GetCactusTexture(Main.tile[num162, num163].type);
                                    }
                                    if (texture2D3 != null)
                                    {
                                        spriteBatch.Draw(texture2D3, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (flag13)
                                    {
                                        spriteBatch.Draw(Main.evilCactusTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (flag15)
                                    {
                                        spriteBatch.Draw(Main.crimsonCactusTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (flag14)
                                    {
                                        spriteBatch.Draw(Main.goodCactusTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else if (Main.canDrawColorTile(j, i))
                                    {
                                        spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                }
                                else if (type == 272 && !tile.halfBrick() && !Main.tile[j - 1, i].halfBrick() && !Main.tile[j + 1, i].halfBrick())
                                {
                                    int num164 = Main.tileFrame[type];
                                    num164 += j % 2;
                                    num164 += i % 2;
                                    num164 += j % 3;
                                    for (num164 += i % 3; num164 > 1; num164 -= 2)
                                    {
                                    }
                                    num164 *= 90;
                                    if (tile.inActive())
                                    {
                                        drawColor = tile.actColor(drawColor);
                                    }
                                    else if (Main.tileShine2[type])
                                    {
                                        drawColor = Main.shine(drawColor, type);
                                    }
                                    if (Main.canDrawColorTile(j, i))
                                    {
                                        spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + num164, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + num164, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                    }
                                }
                                else
                                {
                                    if (type == 160)
                                    {
                                        drawColor = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
                                    }
                                    if (type != 19 && type != 380 && Main.tileSolid[type] && !TileID.Sets.NotReallySolid[type] && !tile.halfBrick() && (Main.tile[j - 1, i].halfBrick() || Main.tile[j + 1, i].halfBrick()))
                                    {
                                        if (tile.inActive())
                                        {
                                            drawColor = tile.actColor(drawColor);
                                        }
                                        else if (Main.tileShine2[type])
                                        {
                                            drawColor = Main.shine(drawColor, type);
                                        }
                                        if (Main.tile[j - 1, i].halfBrick() && Main.tile[j + 1, i].halfBrick())
                                        {
                                            if (Main.canDrawColorTile(j, i))
                                            {
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(126 + frameXOffset, frameYOffset, 16, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                            else
                                            {
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                if (!Main.tile[j, i - 1].bottomSlope() && Main.tile[j, i - 1].type == type)
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(90 + frameXOffset, frameYOffset, 16, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else
                                                {
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(126 + frameXOffset, frameYOffset, 16, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                            }
                                        }
                                        else if (Main.tile[j - 1, i].halfBrick())
                                        {
                                            if (Main.canDrawColorTile(j, i))
                                            {
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + 4f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + 4 + frameXOffset, frameYOffset + num8, width - 4, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(126 + frameXOffset, frameYOffset, 4, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                            else
                                            {
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + 4f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + 4 + frameXOffset, frameYOffset + num8, width - 4, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(126 + frameXOffset, frameYOffset, 4, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        else if (Main.tile[j + 1, i].halfBrick())
                                        {
                                            if (Main.canDrawColorTile(j, i))
                                            {
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8, width - 4, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + 12f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(138 + frameXOffset, frameYOffset, 4, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                            else
                                            {
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 8) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8 + 8, width, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8, width - 4, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + 12f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(138, 0, 4, 8), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        else if (Main.canDrawColorTile(j, i))
                                        {
                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, frameYOffset + num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                    else if (Lighting.NotRetro && Main.tileSolid[type] && type != 137 && type != 235 && type != 388 && !tile.halfBrick() && !tile.inActive())
                                    {
                                        if (drawColor.R > num || (double)(int)drawColor.G > (double)num * 1.1 || (double)(int)drawColor.B > (double)num * 1.2)
                                        {
                                            Lighting.GetColor9Slice(j, i, ref slices);
                                            bool flag17 = tile.inActive();
                                            bool flag18 = Main.tileShine2[type];
                                            Texture2D texture3 = (!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()];
                                            for (int num165 = 0; num165 < 9; num165++)
                                            {
                                                int num166 = 0;
                                                int num167 = 0;
                                                int width2 = 4;
                                                int height6 = 4;
                                                switch (num165)
                                                {
                                                    case 1:
                                                        width2 = 8;
                                                        num166 = 4;
                                                        break;
                                                    case 2:
                                                        num166 = 12;
                                                        break;
                                                    case 3:
                                                        height6 = 8;
                                                        num167 = 4;
                                                        break;
                                                    case 4:
                                                        width2 = 8;
                                                        height6 = 8;
                                                        num166 = 4;
                                                        num167 = 4;
                                                        break;
                                                    case 5:
                                                        num166 = 12;
                                                        num167 = 4;
                                                        height6 = 8;
                                                        break;
                                                    case 6:
                                                        num167 = 12;
                                                        break;
                                                    case 7:
                                                        width2 = 8;
                                                        height6 = 4;
                                                        num166 = 4;
                                                        num167 = 12;
                                                        break;
                                                    case 8:
                                                        num166 = 12;
                                                        num167 = 12;
                                                        break;
                                                }
                                                Microsoft.Xna.Framework.Color color11 = drawColor;
                                                Microsoft.Xna.Framework.Color color12 = slices[num165];
                                                color11.R = (byte)((drawColor.R + color12.R) / 2);
                                                color11.G = (byte)((drawColor.G + color12.G) / 2);
                                                color11.B = (byte)((drawColor.B + color12.B) / 2);
                                                if (flag17)
                                                {
                                                    color11 = tile.actColor(color11);
                                                }
                                                else if (flag18)
                                                {
                                                    color11 = Main.shine(color11, type);
                                                }
                                                spriteBatch.Draw(texture3, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num166, i * 16 - (int)screenPosition.Y + offsetY + num167) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num166 + frameXOffset, num8 + num167 + frameYOffset, width2, height6), color11, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        else if (drawColor.R > num2 || (double)(int)drawColor.G > (double)num2 * 1.1 || (double)(int)drawColor.B > (double)num2 * 1.2)
                                        {
                                            Lighting.GetColor4Slice(j, i, ref slices);
                                            bool flag19 = tile.inActive();
                                            bool flag20 = Main.tileShine2[type];
                                            Texture2D texture4 = (!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()];
                                            for (int num168 = 0; num168 < 4; num168++)
                                            {
                                                int num169 = 0;
                                                int num170 = 0;
                                                switch (num168)
                                                {
                                                    case 1:
                                                        num169 = 8;
                                                        break;
                                                    case 2:
                                                        num170 = 8;
                                                        break;
                                                    case 3:
                                                        num169 = 8;
                                                        num170 = 8;
                                                        break;
                                                }
                                                Microsoft.Xna.Framework.Color color13 = drawColor;
                                                Microsoft.Xna.Framework.Color color14 = slices[num168];
                                                color13.R = (byte)((drawColor.R + color14.R) / 2);
                                                color13.G = (byte)((drawColor.G + color14.G) / 2);
                                                color13.B = (byte)((drawColor.B + color14.B) / 2);
                                                if (flag19)
                                                {
                                                    color13 = tile.actColor(color13);
                                                }
                                                else if (flag20)
                                                {
                                                    color13 = Main.shine(color13, type);
                                                }
                                                spriteBatch.Draw(texture4, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num169, i * 16 - (int)screenPosition.Y + offsetY + num170) + value, new Microsoft.Xna.Framework.Rectangle(num7 + num169 + frameXOffset, num8 + num170 + frameYOffset, 8, 8), color13, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                            }
                                        }
                                        else
                                        {
                                            if (tile.inActive())
                                            {
                                                drawColor = tile.actColor(drawColor);
                                            }
                                            else if (Main.tileShine2[type])
                                            {
                                                drawColor = Main.shine(drawColor, type);
                                            }
                                            Texture2D texture5 = (!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()];
                                            spriteBatch.Draw(texture5, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                        }
                                    }
                                    else
                                    {
                                        if (Lighting.NotRetro && Main.tileShine2[type])
                                        {
                                            if (type == 21)
                                            {
                                                if (num7 >= 36 && num7 < 178)
                                                {
                                                    drawColor = Main.shine(drawColor, type);
                                                }
                                            }
                                            else if (!tile.inActive())
                                            {
                                                drawColor = Main.shine(drawColor, type);
                                            }
                                        }
                                        if (tile.inActive())
                                        {
                                            drawColor = tile.actColor(drawColor);
                                        }
                                        switch (type)
                                        {
                                            case 128:
                                            case 269:
                                                {
                                                    int num275;
                                                    for (num275 = num7; num275 >= 100; num275 -= 100)
                                                    {
                                                    }
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num275, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    break;
                                                }
                                            case 334:
                                                {
                                                    int num280 = num7;
                                                    int num281 = 0;
                                                    while (num280 >= 5000)
                                                    {
                                                        num280 -= 5000;
                                                        num281++;
                                                    }
                                                    if (num281 != 0)
                                                    {
                                                        num280 = (num281 - 1) * 18;
                                                    }
                                                    spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num280, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    break;
                                                }
                                            case 5:
                                                {
                                                    int num282 = j;
                                                    int num283 = i;
                                                    if (num7 == 66 && num8 <= 45)
                                                    {
                                                        num282++;
                                                    }
                                                    if (num7 == 88 && num8 >= 66 && num8 <= 110)
                                                    {
                                                        num282--;
                                                    }
                                                    if (num7 == 22 && num8 >= 132)
                                                    {
                                                        num282--;
                                                    }
                                                    if (num7 == 44 && num8 >= 132)
                                                    {
                                                        num282++;
                                                    }
                                                    for (; Main.tile[num282, num283].active() && Main.tile[num282, num283].type == 5; num283++)
                                                    {
                                                    }
                                                    int treeVariant = GetTreeVariant(num282, num283);
                                                    Texture2D treeTexture = TileLoader.GetTreeTexture(Main.tile[num282, num283]);
                                                    if (treeTexture != null)
                                                    {
                                                        spriteBatch.Draw(treeTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    else if (treeVariant == -1)
                                                    {
                                                        if (Main.canDrawColorTile(j, i))
                                                        {
                                                            spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                        }
                                                        else
                                                        {
                                                            spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                        }
                                                    }
                                                    else if (Main.canDrawColorTree(j, i, treeVariant))
                                                    {
                                                        spriteBatch.Draw(Main.woodAltTexture[treeVariant, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    else
                                                    {
                                                        spriteBatch.Draw(Main.woodTexture[treeVariant], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    break;
                                                }
                                            case 323:
                                                {
                                                    int num276 = -1;
                                                    int num277 = j;
                                                    int num278;
                                                    for (num278 = i; Main.tile[num277, num278].active() && Main.tile[num277, num278].type == 323; num278++)
                                                    {
                                                    }
                                                    if (Main.tile[num277, num278].active() && Main.tile[num277, num278].type == 53)
                                                    {
                                                        num276 = 0;
                                                    }
                                                    if (Main.tile[num277, num278].active() && Main.tile[num277, num278].type == 234)
                                                    {
                                                        num276 = 1;
                                                    }
                                                    if (Main.tile[num277, num278].active() && Main.tile[num277, num278].type == 116)
                                                    {
                                                        num276 = 2;
                                                    }
                                                    if (Main.tile[num277, num278].active() && Main.tile[num277, num278].type == 112)
                                                    {
                                                        num276 = 3;
                                                    }
                                                    int y2 = 22 * num276;
                                                    int num279 = num8;
                                                    Texture2D palmTreeTexture = TileLoader.GetPalmTreeTexture(Main.tile[num277, num278]);
                                                    if (palmTreeTexture != null)
                                                    {
                                                        spriteBatch.Draw(palmTreeTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num279, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, 0, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    else if (Main.canDrawColorTile(j, i))
                                                    {
                                                        spriteBatch.Draw(Main.tileAltTexture[type, tile.color()], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num279, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, y2, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    else
                                                    {
                                                        spriteBatch.Draw(Main.tileTexture[type], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num279, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, y2, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    break;
                                                }
                                            default:
                                                if (num10 == 8 && (!Main.tile[j, i + 1].active() || !Main.tileSolid[Main.tile[j, i + 1].type] || Main.tile[j, i + 1].halfBrick()))
                                                {
                                                    Texture2D texture6 = (!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()];
                                                    if (TileID.Sets.Platforms[type])
                                                    {
                                                        spriteBatch.Draw(texture6, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + num10) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                    else
                                                    {
                                                        spriteBatch.Draw(texture6, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + num10) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height - num10 - 4), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                        spriteBatch.Draw(texture6, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + 12) + value, new Microsoft.Xna.Framework.Rectangle(144 + frameXOffset, 66 + frameYOffset, width, 4), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                    }
                                                }
                                                else
                                                {
                                                    Texture2D texture7 = (!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()];
                                                    spriteBatch.Draw(texture7, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY + num10) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height - num10), drawColor, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 27)
                                                {
                                                    int num171 = 14;
                                                    spriteBatch.Draw(Main.FlameTexture[num171], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + frameYOffset, width, height), new Microsoft.Xna.Framework.Color(255, 255, 255, 255), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 215 && num8 < 36)
                                                {
                                                    int num172 = 15;
                                                    Microsoft.Xna.Framework.Color color15 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
                                                    if (num7 / 54 == 5)
                                                    {
                                                        color15 = new Microsoft.Xna.Framework.Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
                                                    }
                                                    spriteBatch.Draw(Main.FlameTexture[num172], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + frameYOffset, width, height), color15, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 286)
                                                {
                                                    spriteBatch.Draw(Main.glowSnailTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height), new Microsoft.Xna.Framework.Color(75, 100, 255, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 270)
                                                {
                                                    spriteBatch.Draw(Main.fireflyJarTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 271)
                                                {
                                                    spriteBatch.Draw(Main.lightningbugJarTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 316 || type == 317 || type == 318)
                                                {
                                                    int num173 = j - num7 / 18;
                                                    int num174 = i - num8 / 18;
                                                    int num175 = num173 / 2 * (num174 / 3);
                                                    num175 %= Main.cageFrames;
                                                    spriteBatch.Draw(Main.jellyfishBowlTexture[type - 316], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + Main.jellyfishCageFrame[type - 316, num175] * 36, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 149 && num7 < 54)
                                                {
                                                    spriteBatch.Draw(Main.xmasLightTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 300 || type == 302 || type == 303 || type == 306)
                                                {
                                                    int num176 = 9;
                                                    if (type == 302)
                                                    {
                                                        num176 = 10;
                                                    }
                                                    if (type == 303)
                                                    {
                                                        num176 = 11;
                                                    }
                                                    if (type == 306)
                                                    {
                                                        num176 = 12;
                                                    }
                                                    spriteBatch.Draw(Main.FlameTexture[num176], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8 + frameYOffset, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                else if (Main.tileFlame[type])
                                                {
                                                    ulong seed = (ulong)((long)Main.TileFrameSeed ^ (((long)j << 32) | i));
                                                    int num177 = type;
                                                    int num178 = 0;
                                                    switch (num177)
                                                    {
                                                        case 4:
                                                            num178 = 0;
                                                            break;
                                                        case 33:
                                                        case 174:
                                                            num178 = 1;
                                                            break;
                                                        case 100:
                                                        case 173:
                                                            num178 = 2;
                                                            break;
                                                        case 34:
                                                            num178 = 3;
                                                            break;
                                                        case 93:
                                                            num178 = 4;
                                                            break;
                                                        case 49:
                                                            num178 = 5;
                                                            break;
                                                        case 372:
                                                            num178 = 16;
                                                            break;
                                                        case 98:
                                                            num178 = 6;
                                                            break;
                                                        case 35:
                                                            num178 = 7;
                                                            break;
                                                        case 42:
                                                            num178 = 13;
                                                            break;
                                                    }
                                                    switch (num178)
                                                    {
                                                        case 7:
                                                            {
                                                                for (int num182 = 0; num182 < 4; num182++)
                                                                {
                                                                    float num183 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                    float num184 = (float)Utils.RandomInt(ref seed, -10, 10) * 0.15f;
                                                                    num183 = 0f;
                                                                    num184 = 0f;
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num183, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num184) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                }
                                                                break;
                                                            }
                                                        case 1:
                                                            switch (Main.tile[j, i].frameY / 22)
                                                            {
                                                                case 5:
                                                                case 6:
                                                                case 7:
                                                                case 10:
                                                                    {
                                                                        for (int num266 = 0; num266 < 7; num266++)
                                                                        {
                                                                            float num267 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            float num268 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num267, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num268) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 8:
                                                                    {
                                                                        for (int num272 = 0; num272 < 7; num272++)
                                                                        {
                                                                            float num273 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            float num274 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num273, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num274) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 12:
                                                                    {
                                                                        for (int num260 = 0; num260 < 7; num260++)
                                                                        {
                                                                            float num261 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num262 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num261, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num262) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 14:
                                                                    {
                                                                        for (int num269 = 0; num269 < 8; num269++)
                                                                        {
                                                                            float num270 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num271 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num270, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num271) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 16:
                                                                    {
                                                                        for (int num263 = 0; num263 < 4; num263++)
                                                                        {
                                                                            float num264 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num265 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num264, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num265) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 27:
                                                                case 28:
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                    break;
                                                                default:
                                                                    {
                                                                        for (int num257 = 0; num257 < 7; num257++)
                                                                        {
                                                                            float num258 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num259 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num258, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num259) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        case 2:
                                                            switch (Main.tile[j, i].frameY / 36)
                                                            {
                                                                case 3:
                                                                    {
                                                                        for (int num248 = 0; num248 < 3; num248++)
                                                                        {
                                                                            float num249 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.05f;
                                                                            float num250 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num249, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num250) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 6:
                                                                    {
                                                                        for (int num254 = 0; num254 < 5; num254++)
                                                                        {
                                                                            float num255 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num256 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num255, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num256) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 9:
                                                                    {
                                                                        for (int num242 = 0; num242 < 7; num242++)
                                                                        {
                                                                            float num243 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            float num244 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num243, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num244) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 11:
                                                                    {
                                                                        for (int num251 = 0; num251 < 7; num251++)
                                                                        {
                                                                            float num252 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num253 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num252, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num253) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 13:
                                                                    {
                                                                        for (int num245 = 0; num245 < 8; num245++)
                                                                        {
                                                                            float num246 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num247 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num246, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num247) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 28:
                                                                case 29:
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                    break;
                                                                default:
                                                                    {
                                                                        for (int num239 = 0; num239 < 7; num239++)
                                                                        {
                                                                            float num240 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num241 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num240, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num241) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        case 3:
                                                            switch (Main.tile[j, i].frameY / 54)
                                                            {
                                                                case 8:
                                                                    {
                                                                        for (int num233 = 0; num233 < 7; num233++)
                                                                        {
                                                                            float num234 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            float num235 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num234, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num235) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 9:
                                                                    {
                                                                        for (int num221 = 0; num221 < 3; num221++)
                                                                        {
                                                                            float num222 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.05f;
                                                                            float num223 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num222, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num223) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 11:
                                                                    {
                                                                        for (int num227 = 0; num227 < 7; num227++)
                                                                        {
                                                                            float num228 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            float num229 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num228, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num229) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 15:
                                                                    {
                                                                        for (int num236 = 0; num236 < 7; num236++)
                                                                        {
                                                                            float num237 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num238 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num237, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num238) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 17:
                                                                case 20:
                                                                    {
                                                                        for (int num230 = 0; num230 < 7; num230++)
                                                                        {
                                                                            float num231 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            float num232 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num231, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num232) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 18:
                                                                    {
                                                                        for (int num224 = 0; num224 < 8; num224++)
                                                                        {
                                                                            float num225 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num226 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num225, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num226) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 34:
                                                                case 35:
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                    break;
                                                                default:
                                                                    {
                                                                        for (int num218 = 0; num218 < 7; num218++)
                                                                        {
                                                                            float num219 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num220 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num219, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num220) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        case 4:
                                                            switch (Main.tile[j, i].frameY / 54)
                                                            {
                                                                case 1:
                                                                    {
                                                                        for (int num212 = 0; num212 < 3; num212++)
                                                                        {
                                                                            float num213 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num214 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num213, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num214) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 2:
                                                                case 4:
                                                                    {
                                                                        for (int num198 = 0; num198 < 7; num198++)
                                                                        {
                                                                            float num199 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            float num200 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.075f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num199, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num200) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 3:
                                                                    {
                                                                        for (int num206 = 0; num206 < 7; num206++)
                                                                        {
                                                                            float num207 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
                                                                            float num208 = (float)Utils.RandomInt(ref seed, -20, 1) * 0.35f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num207, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num208) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 5:
                                                                    {
                                                                        for (int num215 = 0; num215 < 7; num215++)
                                                                        {
                                                                            float num216 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            float num217 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.3f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num216, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num217) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 9:
                                                                    {
                                                                        for (int num209 = 0; num209 < 7; num209++)
                                                                        {
                                                                            float num210 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num211 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num210, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num211) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 13:
                                                                    {
                                                                        for (int num203 = 0; num203 < 8; num203++)
                                                                        {
                                                                            float num204 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            float num205 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.1f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num204, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num205) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                                case 12:
                                                                    {
                                                                        float num201 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.01f;
                                                                        float num202 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.01f;
                                                                        spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num201, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num202) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(Utils.RandomInt(ref seed, 90, 111), Utils.RandomInt(ref seed, 90, 111), Utils.RandomInt(ref seed, 90, 111), 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        break;
                                                                    }
                                                                case 28:
                                                                case 29:
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                    break;
                                                                default:
                                                                    {
                                                                        for (int num195 = 0; num195 < 7; num195++)
                                                                        {
                                                                            float num196 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                            float num197 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                            spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num196, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num197) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        case 13:
                                                            {
                                                                int num185 = num8 / 36;
                                                                switch (num185)
                                                                {
                                                                    case 1:
                                                                    case 3:
                                                                    case 6:
                                                                    case 8:
                                                                    case 19:
                                                                    case 27:
                                                                    case 29:
                                                                    case 30:
                                                                    case 31:
                                                                    case 32:
                                                                    case 36:
                                                                        {
                                                                            for (int num192 = 0; num192 < 7; num192++)
                                                                            {
                                                                                float num193 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                                float num194 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                                spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num193, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num194) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(100, 100, 100, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                            }
                                                                            break;
                                                                        }
                                                                    case 2:
                                                                    case 16:
                                                                    case 25:
                                                                        {
                                                                            for (int num189 = 0; num189 < 7; num189++)
                                                                            {
                                                                                float num190 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                                float num191 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.1f;
                                                                                spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num190, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num191) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(50, 50, 50, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                            }
                                                                            break;
                                                                        }
                                                                    default:
                                                                        switch (num185)
                                                                        {
                                                                            case 29:
                                                                                {
                                                                                    for (int num186 = 0; num186 < 7; num186++)
                                                                                    {
                                                                                        float num187 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                                        float num188 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
                                                                                        spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num187, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num188) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(25, 25, 25, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                                    }
                                                                                    break;
                                                                                }
                                                                            case 34:
                                                                            case 35:
                                                                                spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(75, 75, 75, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                                break;
                                                                        }
                                                                        break;
                                                                }
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                for (int num179 = 0; num179 < 7; num179++)
                                                                {
                                                                    Microsoft.Xna.Framework.Color color16 = new Microsoft.Xna.Framework.Color(100, 100, 100, 0);
                                                                    if (num8 / 22 == 14)
                                                                    {
                                                                        color16 = new Microsoft.Xna.Framework.Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
                                                                    }
                                                                    float num180 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
                                                                    float num181 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
                                                                    spriteBatch.Draw(Main.FlameTexture[num178], new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + num180, (float)(i * 16 - (int)screenPosition.Y + offsetY) + num181) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), color16, 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                                if (type == 144)
                                                {
                                                    spriteBatch.Draw(Main.timerTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color(200, 200, 200, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                if (type == 237)
                                                {
                                                    spriteBatch.Draw(Main.sunAltarTexture, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + offsetY) + value, new Microsoft.Xna.Framework.Rectangle(num7, num8, width, height), new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor / 2, (int)Main.mouseTextColor / 2, (int)Main.mouseTextColor / 2, 0), 0f, default(Vector2), 1f, spriteEffects, 0f);
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            if (Main.tileGlowMask[tile.type] != 0)
                            {
                                Texture2D texture8 = Main.glowMaskTexture[Main.tileGlowMask[tile.type]];
                                double num284 = Main.time * 0.08;
                                Microsoft.Xna.Framework.Color color17 = Microsoft.Xna.Framework.Color.White;
                                if (tile.type == 350)
                                {
                                    color17 = new Microsoft.Xna.Framework.Color(new Vector4((float)((double)(0f - (float)Math.Cos(((int)(num284 / 6.283) % 3 == 1) ? num284 : 0.0)) * 0.2 + 0.2)));
                                }
                                if (tile.type == 381)
                                {
                                    color17 = color4;
                                }
                                if (tile.type == 370)
                                {
                                    color17 = color3;
                                }
                                if (tile.type == 390)
                                {
                                    color17 = color3;
                                }
                                if (tile.type == 391)
                                {
                                    color17 = new Microsoft.Xna.Framework.Color(250, 250, 250, 200);
                                }
                                if (tile.type == 209)
                                {
                                    color17 = PortalHelper.GetPortalColor(Main.myPlayer, (tile.frameX >= 288) ? 1 : 0);
                                }
                                if (tile.type == 429 || tile.type == 445)
                                {
                                    texture8 = ((!Main.canDrawColorTile(j, i)) ? Main.tileTexture[type] : Main.tileAltTexture[type, tile.color()]);
                                    frameYOffset = 18;
                                }
                                if (tile.slope() == 0 && !tile.halfBrick())
                                {
                                    spriteBatch.Draw(texture8, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height), color17, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                }
                                else if (tile.halfBrick())
                                {
                                    spriteBatch.Draw(texture8, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, i * 16 - (int)screenPosition.Y + 10) + value, new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset + 10, width, 6), color17, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                }
                                else
                                {
                                    byte b6 = tile.slope();
                                    for (int num285 = 0; num285 < 8; num285++)
                                    {
                                        int num286 = num285 << 1;
                                        Microsoft.Xna.Framework.Rectangle value7 = new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset + num285 * 2, num286, 2);
                                        int num287 = 0;
                                        switch (b6)
                                        {
                                            case 2:
                                                value7.X = 16 - num286;
                                                num287 = 16 - num286;
                                                break;
                                            case 3:
                                                value7.Width = 16 - num286;
                                                break;
                                            case 4:
                                                value7.Width = 14 - num286;
                                                value7.X = num286 + 2;
                                                num287 = num286 + 2;
                                                break;
                                        }
                                        spriteBatch.Draw(texture8, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num287, i * 16 - (int)screenPosition.Y + num285 * 2) + value, value7, color17, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                            if (texture2D != null)
                            {
                                int num288 = 0;
                                int num289 = 0;
                                spriteBatch.Draw(texture2D, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num288, i * 16 - (int)screenPosition.Y + offsetY + num289) + value, value3, color, 0f, default(Vector2), 1f, spriteEffects, 0f);
                            }
                            if (texture2D2 != null)
                            {
                                empty = new Microsoft.Xna.Framework.Rectangle(num7 + frameXOffset, num8 + frameYOffset, width, height);
                                int num290 = 0;
                                int num291 = 0;
                                spriteBatch.Draw(texture2D2, new Vector2((float)(j * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f + (float)num290, i * 16 - (int)screenPosition.Y + offsetY + num291) + value, empty, color5, 0f, default(Vector2), 1f, spriteEffects, 0f);
                            }
                            TileLoader.PostDraw(j, i, type, spriteBatch);
                        }
                    }
                }
            }
            //orig(self, solidOnly, waterStyleOverride);
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

        public override void Unload()
        {
            Unloading = true;
            try
            {
                Main.OnPostDraw -= OnPostDraw;
                On.Terraria.Player.QuickGrapple -= Player_QuickGrapple;
                //On.Terraria.Main.DrawTiles -= Main_DrawTiles;
                if (!Main.dedServ)
                {
                    Main.logoTexture = Main.instance.OurLoad<Texture2D>("Images" + Path.DirectorySeparatorChar.ToString() + "Logo");
                    Main.logo2Texture = Main.instance.OurLoad<Texture2D>("Images" + Path.DirectorySeparatorChar.ToString() + "Logo2");
                }
                IsCalamityLoaded = false;
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
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            //if (Main.gamePaused && Filters.Scene["OrangeScreen"].IsActive()) Filters.Scene["OrangeScreen"].Deactivate();
            //if (!Filters.Scene["OrangeScreen"].IsActive()/* && !Main.gamePaused*/) Filters.Scene.Activate("OrangeScreen", Vector2.Zero);
            //Filters.Scene.Activate("ReallyDark", Vector2.Zero);
            if (!Main.dedServ)
            {
                mimicryFrameCounter++;
                if (mimicryFrameCounter >= 13 * 5)
                {
                    mimicryFrameCounter = 5;
                }
                int texNum = (mimicryFrameCounter / 5);
                Main.itemTexture[ModContent.ItemType<拟态魔能>()] = ModTexturesTable[$"拟态魔能_{texNum.ToString()}"];
                if (AEntrogicConfigClient.Instance.HookMouse)
                {
                    if (HookCursorHotKey.JustPressed)
                    {
                        HookCursor = ModHelper.MouseScreenPos;
                    }
                    const float scaleMax = 1.3f;
                    float scale = 1f + ((float)ModTimer % 100f / 99f * (scaleMax - 1f) * 2f);
                    if (scale >= scaleMax) // scale范围[1.0, scaleMax]
                    {
                        scale = scaleMax - (scale - scaleMax);
                    }
                    spriteBatch.Draw(ModTexturesTable["HookCursor"], HookCursor + new Vector2(-scale * 8f), null, Color.White, 0f, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0f);
                }
                //if (ModHelper.ControlShift && Main.HoverItem.type != 0 && Main.HoverItem.GetGlobalItem<EntrogicItem>().card && Instance.CardInventoryUI.slotActive)
                //{
                //    int index = Instance.CardInventoryUI.HoverOnSlot();
                //    int slotCard = ModHelper.FindFirst(EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType, 0);
                //    if (index != -1)
                //    {
                //        Item i = new Item();
                //        i.SetDefaults(EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[index]);
                //        if (Main.LocalPlayer.ItemSpace(i))
                //        {
                //            Main.cursorOverride = 7;
                //        }
                //    }
                //    else if (slotCard != -1 && CardInventoryGridSlot.AllowPutin(Instance.CardInventoryUI.Grid[slotCard].inventoryItem, Main.HoverItem, slotCard))
                //    {
                //        Main.cursorOverride = 9;
                //    }
                //}
            }
            base.PostDrawInterface(spriteBatch);
        }
        public override void PreUpdateEntities()
        {
            base.PreUpdateEntities();
        }
        public override void MidUpdateTimeWorld()
        {
            ModTimer++;
            CardInventoryUI.visible = Main.playerInventory;
            base.MidUpdateTimeWorld();
        }
        private void OnPostDraw(GameTime gameTime)
        {
            if (Main.dedServ)
            {
                return;
            }
            Main.spriteBatch.SafeBegin();
            if (Main.gameMenu)
            {
                if (DateTime.Now.Month == 3 && DateTime.Now.Day == 20)
                {
                    string text = "[c/ff0000:H][c/ff2b00:a][c/ff5600:p][c/ff8100:p][c/ffa800:y] [c/ffd700:B][c/ffef00:i][c/e8f300:r][c/a6d200:t][c/63b100:h][c/219000:d][c/009021:a][c/00b163:y][c/00d2a6:,] [c/00d2ff:-][c/0090ff:C][c/004dff:y][c/000bff:r][c/1b00e3:i][c/3d00c2:l][c/5e00a1:-][c/7f0080:!]";
                    string seeing = "Happy Birthday, -Cyril-!";
                    Vector2 pos = new Vector2(Main.screenWidth, 0f);
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, text, pos += new Vector2(-Main.fontMouseText.MeasureString(seeing).X, 0f), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                }
            }
            //string textBlock = "NONONONONONONONONONONONO\nNONONONONONONONONONONONO\nNONONONONONONONONONONONO";
            //string text = "[c/FF0000:But] [c/FF2800:E][c/FF5000:n][c/FF7800:t][c/FFA000:r][c/FFC800:o][c/FFF000:g][c/D7FF00:i][c/AFFF00:c][c/87FF00:!!!]";
            //string seeing = "But Entrogic!!!";
            //Vector2 pos = new Vector2(Main.screenWidth / 2f, 100f);
            //ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, textBlock, pos -= new Vector2(Main.fontMouseText.MeasureString(textBlock).X / 2f * 1.5f, 40f), Color.Red, 0f, Vector2.Zero, new Vector2(1.5f));
            //ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, text, pos -= new Vector2(-Main.fontMouseText.MeasureString(seeing).X, -120f), Color.White, 0f, Vector2.Zero, new Vector2(2f));
            //string basicStr = "NO"; // Better with ■
            //Vector2 size = Main.fontMouseText.MeasureString(basicStr);
            //for (float i = 0; i <= Main.screenWidth + size.X; i += size.X + 2f)
            //{
            //    for (float j = 0; j <= Main.screenHeight + size.Y; j += size.Y - 8f)
            //    {
            //        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, basicStr, new Vector2(i, j), Color.Red, 0f, Vector2.Zero, new Vector2(1));
            //    }
            //}
            Main.spriteBatch.SafeEnd();
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
        public override void UpdateUI(GameTime gameTime)
        {
            EntrogicPlayer ePlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            if (BookUIE != null && ePlayer.IsActiveBook)
                BookUIE.Update(gameTime);
            //if (BookPageUIE != null && Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().ActiveBook)
            //    BookPageUIE.Update(gameTime);
            if (CardUIE != null && ePlayer.IsActiveCard)
                CardUIE.Update(gameTime);
            if (CardInventoryUIE != null && CardInventoryUI.visible)
                CardInventoryUIE.Update(gameTime);
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (Main.gameMenu)
            {
                return;
            }
            if (AEntrogicConfigClient.Instance.SuperZoom >= 10)
            {
                Transform.Zoom = new Vector2(((float)AEntrogicConfigClient.Instance.SuperZoom) / 100f);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Book UI",
                    delegate
                    {
                        BookUIE.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                //layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                //    "Entrogic: Book Page UI",
                //    delegate
                //    {
                //        BookPageUIE.Draw(Main.spriteBatch, new GameTime());
                //        return true;
                //    },
                //    InterfaceScaleType.UI)
                //);
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card UI",
                    delegate
                    {
                        CardUIE.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (InventoryIndex != -1)
            {
                layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card Inventory UI",
                    delegate
                    {
                        if (CardInventoryUI.visible)
                        {
                            CardInventoryUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            Player player = Main.LocalPlayer;
            if (Main.myPlayer == -1 || Main.gameMenu || !player.active)
            {
                return;
            }
            bool magicStorm = EntrogicWorld.magicStorm;
            if (magicStorm && player.ZoneOverworldHeight)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/MagicStorm");
                priority = MusicPriority.Environment;
            }
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EntrogicModMessageType msgType = (EntrogicModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case EntrogicModMessageType.SendMagicStormRequest:
                    EntrogicWorld.magicStorm = reader.ReadBoolean();
                    break;
                case EntrogicModMessageType.NPCSpawnOnPlayerAction:
                    int num = (int)reader.ReadByte();
                    int num2 = (int)reader.ReadInt16();
                    if (!NPC.AnyNPCs(num2))
                    {
                        NPC.SpawnOnPlayer(num, num2);
                    }
                    break;
                case EntrogicModMessageType.EntrogicPlayerSyncPlayer:
                    byte playernumber = reader.ReadByte();
                    EntrogicPlayer entrogicPlayer = Main.player[playernumber].GetModPlayer<EntrogicPlayer>();
                    entrogicPlayer.IsActiveBook = reader.ReadBoolean();
                    byte cardNumber = reader.ReadByte();
                    entrogicPlayer.CardType[cardNumber] = reader.ReadInt32();
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    break;
                case EntrogicModMessageType.SendBookOpenRequest:
                    playernumber = reader.ReadByte();
                    entrogicPlayer = Main.player[playernumber].GetModPlayer<EntrogicPlayer>();
                    entrogicPlayer.IsActiveBook = reader.ReadBoolean();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)EntrogicModMessageType.SendBookOpenRequest);
                        packet.Write(playernumber);
                        packet.Write(entrogicPlayer.IsActiveBook);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case EntrogicModMessageType.MerchantPlayerSyncPlayer:
                    playernumber = reader.ReadByte();
                    CardMerchantQuest cardMerchantQuest = Main.player[playernumber].GetModPlayer<CardMerchantQuest>();
                    cardMerchantQuest.Complete = reader.ReadString();
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    break;
                case EntrogicModMessageType.SendCompletedCardMerchantMissionRequest:
                    playernumber = reader.ReadByte();
                    cardMerchantQuest = Main.player[playernumber].GetModPlayer<CardMerchantQuest>();
                    cardMerchantQuest.Complete = reader.ReadString();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)EntrogicModMessageType.SendCompletedCardMerchantMissionRequest);
                        packet.Write(playernumber);
                        packet.Write(cardMerchantQuest.Complete);
                        packet.Send(-1, playernumber);
                    }
                    break;
                default:
                    Logger.WarnFormat("Entrogic: Unknown Message type: {0}", msgType);
                    break;
            }
        }
        internal enum EntrogicModMessageType : byte
        {
            SendMagicStormRequest,
            NPCSpawnOnPlayerAction,
            EntrogicPlayerSyncPlayer,
            SendBookOpenRequest,
            MerchantPlayerSyncPlayer,
            SendCompletedCardMerchantMissionRequest
        }
        public static void DebugModeNewText(string message, bool debug = false)
        {
            if (Main.netMode == 0 && debug)
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
                    return tile.type == TileID.Dirt || tile.type == TileID.SnowBlock || tile.type == TileID.Mud || tile.type == TileID.Grass || tile.type == TileID.JungleGrass || tile.type == TileID.HallowedGrass || tile.type == TileID.FleshGrass || tile.type == TileID.CorruptGrass || tile.type == TileID.MushroomGrass || tile.type == TileID.Sand || tile.type == TileID.Silt || tile.type == TileID.Ebonsand || tile.type == TileID.Crimsand || tile.type == TileID.Pearlsand || tile.type == TileID.ClayBlock || tile.type == TileID.Ash || tile.type == TileID.Cloud || tile.type == TileID.RainCloud || tile.type == TileID.SnowCloud || tile.type == TileID.Slush;
                }
            }
            return false;
        }
        /// <summary>
        /// 当前Item是否存在
        /// </summary>
        public static bool ItemSafe(Item item)
        {
            if (item != null && item.active && item.type != 0)
            {
                return true;
            }
            return false;
        }
        internal static void Explode(Vector2 position, Vector2 size, int damage, bool friendly = false, int goreTimes = 1, bool useSomke = true)
        {
            Main.PlaySound(SoundID.Item14, position);
            position.X = position.X - (float)(size.X / 2);
            position.Y = position.Y - (float)(size.Y / 2);
            Rectangle hitbox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            damage = Main.DamageVar(damage);
            foreach (NPC npc in Main.npc)
            {
                if (npc != null && npc.type != 0 && npc.friendly != friendly && npc.getRect().Intersects(hitbox))
                {
                    npc.StrikeNPC(damage, 3f, (npc.Center.X - hitbox.Center.X) > 0 ? 1 : -1);
                    if (Main.dedServ)
                    {
                        NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI);
                    }
                }
            }
            if (!friendly)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && player.getRect().Intersects(hitbox))
                    {
                        player.Hurt(PlayerDeathReason.ByOther(3), damage, (player.Center.X - hitbox.Center.X) > 0 ? 1 : -1);
                        if (Main.dedServ)
                        {
                            NetMessage.SendData(MessageID.PlayerHurtV2, -1, -1, null, player.whoAmI);
                        }
                    }
                }
            }
            if (!useSomke)
                return;
            for (int num762 = 0; num762 < 20; num762++)
            {
                int num763 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num763];
                dust.velocity *= 1.4f;
            }
            for (int num764 = 0; num764 < 10; num764++)
            {
                int num765 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 6, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num765].noGravity = true;
                Dust dust = Main.dust[num765];
                dust.velocity *= 5f;
                num765 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust = Main.dust[num765];
                dust.velocity *= 3f;
            }
            for (int i = 0; i < goreTimes; i++)
            {
                int num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                Gore gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore138 = Main.gore[num766];
                gore138.velocity.X = gore138.velocity.X + 1f;
                Gore gore139 = Main.gore[num766];
                gore139.velocity.Y = gore139.velocity.Y + 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore140 = Main.gore[num766];
                gore140.velocity.X = gore140.velocity.X - 1f;
                Gore gore141 = Main.gore[num766];
                gore141.velocity.Y = gore141.velocity.Y + 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore142 = Main.gore[num766];
                gore142.velocity.X = gore142.velocity.X + 1f;
                Gore gore143 = Main.gore[num766];
                gore143.velocity.Y = gore143.velocity.Y - 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore144 = Main.gore[num766];
                gore144.velocity.X = gore144.velocity.X - 1f;
                Gore gore145 = Main.gore[num766];
                gore145.velocity.Y = gore145.velocity.Y - 1f;
            }
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.AdaTitBar"), new int[]
            {
            ItemID.AdamantiteBar,
            ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("Entrogic:AdamantiteBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.RCAV"), new int[]

            {
            ItemID.RottenChunk,
            ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("Entrogic:RCAV", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.CriDemBar"), new int[]

            {
            ItemID.CrimtaneBar,
            ItemID.DemoniteBar
            });
            RecipeGroup.RegisterGroup("Entrogic:DemBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Mods.Entrogic.GolPlaBar"), new int[]

            {
            ItemID.PlatinumBar,
            ItemID.GoldBar
            });
            RecipeGroup.RegisterGroup("Entrogic:GoldBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("Mods.Entrogic.Emblems"), new int[]
            {
            ItemID.RangerEmblem,
            ItemID.SorcererEmblem,
            ItemID.SummonerEmblem,
            ItemID.WarriorEmblem
            });
            RecipeGroup.RegisterGroup("Entrogic:FourEmblem", group);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddRecipeGroup("Entrogic:FourEmblem");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.RangerEmblem);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddRecipeGroup("Entrogic:FourEmblem");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.SorcererEmblem);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddRecipeGroup("Entrogic:FourEmblem");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.SummonerEmblem);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddRecipeGroup("Entrogic:FourEmblem");
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.WarriorEmblem);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.BugNet);
            recipe.AddRecipeGroup("Entrogic:GoldBar", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(ItemID.GoldenBugNet);
            recipe.AddRecipe();
        }
    }
}