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

namespace Entrogic
{
    public class Entrogic : Mod
    {
        #region Fields
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

        internal static Dictionary<string, Texture2D> ModTexturesTable = new Dictionary<string, Texture2D>();
        [Obsolete]
        internal static Dictionary<string, CardFightBullet> cfBullets = new Dictionary<string, CardFightBullet>();
        private int foolTexts = 0;
        internal static bool Unloading = false;
        internal static readonly string ModFolder;
        internal static bool IsDev = false;
        internal static int ModTimer; 
        internal static Entrogic Instance;
        public static DynamicSpriteFont PixelFont { get { return Instance.GetFont("Fonts/JOJOHOTXiangSubeta"); } }
        internal BookUI BookUI { get; private set; }
        private UserInterface BookUIE;
        internal CardUI CardUI { get; private set; }
        private UserInterface CardUIE;
        internal CardInventoryUI CardInventoryUI { get; private set; }
        private UserInterface CardInventoryUIE;
        internal CardGameUI CardGameUI { get; private set; }
        private UserInterface CardGameUIE;

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
            Main.OnPostDraw += Main_OnPostDraw;
            Main.OnPreDraw += Main_OnPreDraw;
            IL.Terraria.Main.DrawMenu += UpdateExtraWorldDiff;

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call(
            "AddBoss",
            2.7f,
            new List<int>() { ModContent.NPCType<Volutio>(), ModContent.NPCType<Embryo>() },// Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.嘉沃顿", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedGelSymbiosis),
            ModContent.ItemType<GelCultureFlask>(), // Summon Items
            new List<int> { ModContent.ItemType<VolutioMask>(), ModContent.ItemType<VTrophy>() }, // Collections
            new List<int> { ModContent.ItemType<GelAnkh>(), ModContent.ItemType<GelOfLife>() }, // Normal Loots
            "$Mods.Entrogic.BossSpawnInfo.GelSymb", // Spawn Info
            "", // Despawn Info
            "Entrogic/Images/GelSym_Textures");// 这里切记别用ModTexturesTable，不然服务器会炸

                bossChecklist.Call(
            "AddBoss",
            5.7f,
            ModContent.NPCType<Antanasy>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.Antanasy", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedAthanasy),
            ModContent.ItemType<TitansOrder>(),
            new List<int> { ModContent.ItemType<AthanasyMask>(), ModContent.ItemType<AthanasyTrophy>() },
            new List<int> { ModContent.ItemType<RockSpear>(), ModContent.ItemType<RockShotgun>(), ModContent.ItemType<EyeofImmortal>(), ModContent.ItemType<StoneSlimeStaff>() },
            "$Mods.Entrogic.BossSpawnInfo.Athanasy");

                bossChecklist.Call(
            "AddBoss",
            10.3f,
            ModContent.NPCType<PollutionElemental>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.污染之灵", // Boss Name
            (Func<bool>)(() => EntrogicWorld.IsDownedPollutionElemental),
            ModContent.ItemType<ContaminatedLiquor>(),
            new List<int> { ModContent.ItemType<PollutionElementalMask>(), ModContent.ItemType<PETrophy>() },
            new List<int> { ModContent.ItemType<BottleofStorm>(), ModContent.ItemType<WaterElementalStaff>(), ModContent.ItemType<ContaminatedLongbow>(), ModContent.ItemType<ContaminatedCurrent>(),
                ModContent.ItemType<HelmetofContamination>(), ModContent.ItemType<HeadgearofContamination>(), ModContent.ItemType<MaskofContamination>(), ModContent.ItemType<BreastplateofContamination>(), ModContent.ItemType<GreavesofContamination>() },
            "$Mods.Entrogic.BossSpawnInfo.PollutionElement",
            " 在大海中继续沉睡...");
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
                    ModContent.NPCType<Embryo>(),
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
                    ModContent.NPCType<Volutio>(),
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

                AddEquipTexture(new PollutionElementalMask1(), null, EquipType.Head, "PollutionElementalMask1", "Entrogic/Items/PollutElement/PollutionElementalMask1_Head");
                AddEquipTexture(new PollutionElementalMask2(), null, EquipType.Head, "PollutionElementalMask2", "Entrogic/Items/PollutElement/PollutionElementalMask2_Head");
                AddEquipTexture(new PollutionElementalMask3(), null, EquipType.Head, "PollutionElementalMask3", "Entrogic/Items/PollutElement/PollutionElementalMask3_Head");
                AddEquipTexture(new PollutionElementalMask4(), null, EquipType.Head, "PollutionElementalMask4", "Entrogic/Items/PollutElement/PollutionElementalMask4_Head");
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
                GameShaders.Misc["ExampleMod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/ExampleEffectDeath")), "DeathAnimation").UseImage("Images/Misc/Perlin");
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

                CardGameUI = new CardGameUI();
                CardGameUI.Activate();
                CardGameUIE = new UserInterface();
                CardGameUIE.SetState(CardGameUI);
                /*SinsBar.visible = true;
                Sinsbar = new SinsBar();
                Sinsbar.Activate();
                SinsBarInterface = new UserInterface();
                SinsBarInterface.SetState(Sinsbar);*/
            }
            Buildings.Cache("Buildings/CardShrine0.ebuiding", "Buildings/CardShrine1.ebuiding");
            #region Armor Translates
            Translation.RegisterTranslation("mspeed", GameCulture.Chinese, "移动速度", " movement speed");
            Translation.RegisterTranslation("and", GameCulture.Chinese, "与", " and");
            Translation.RegisterTranslation("csc", GameCulture.Chinese, "暴击率", " critical strike chance");
            Translation.RegisterTranslation("knockback", GameCulture.Chinese, "击退", " knockback");
            Translation.RegisterTranslation("damage", GameCulture.Chinese, "伤害", " damage");
            Translation.RegisterTranslation("cntca", GameCulture.Chinese, "的几率不消耗弹药", " chance not to consume ammo");
            Translation.RegisterTranslation("immb", GameCulture.Chinese, "最大魔力值增加", "Increases maximum mana by ");
            Translation.RegisterTranslation("rmub", GameCulture.Chinese, "魔力消耗减少", "Reduces mana usage by ");
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

        public override void Unload()
        {
            Unloading = true;
            try
            {
                Main.OnPostDraw -= Main_OnPostDraw;
                Main.OnPreDraw -= Main_OnPreDraw;
                ModTexturesTable.Clear();
                cfBullets.Clear();
                PassHotkey = null;
                WashHotkey = null;
                HookCursorHotKey = null;
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
            CardInventoryUI.IsActive = Main.playerInventory;
            base.MidUpdateTimeWorld();
        }
        private void Main_OnPostDraw(GameTime gameTime)
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
            //Main.debuff = Debuffs;
        }
        private bool[] Debuffs = Main.debuff;
        private void Main_OnPreDraw(GameTime obj)
        {
            //Debuffs = Main.debuff;
            //for(int i = 0; i < Main.debuff.Length; i++)
            //{
            //    Main.debuff[i] = false;
            //}
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
            if (BookUIE != null && BookUI.IsActive)
                BookUIE.Update(gameTime);
            //if (BookPageUIE != null && Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().ActiveBook)
            //    BookPageUIE.Update(gameTime);
            if (CardUIE != null && CardUI.IsActive)
                CardUIE.Update(gameTime);
            if (CardInventoryUIE != null && CardInventoryUI.IsActive)
                CardInventoryUIE.Update(gameTime);
            if (CardGameUIE != null && ePlayer.CardGameActive)
                CardGameUIE.Update(gameTime);
            //if (Main.netMode != NetmodeID.MultiplayerClient)
            //{
            int texNum = mimicryFrameCounter / 5;
            Main.itemTexture[ModContent.ItemType<拟态魔能>()] = ModTexturesTable[$"拟态魔能_{texNum.ToString()}"];
            //}
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (Main.gameMenu)
            {
                return;
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            EntrogicPlayer ePlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Book UI",
                    delegate
                    {
                        if (BookUI.IsActive)
                        {
                            BookUIE.Draw(Main.spriteBatch, new GameTime());
                        }
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
                        if (CardUI.IsActive)
                        {
                            CardUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card Game UI",
                    delegate
                    {
                        if (ePlayer.CardGameActive)
                        {
                            CardGameUIE.Draw(Main.spriteBatch, new GameTime());
                        }
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
                        if (CardInventoryUI.IsActive)
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
                case EntrogicModMessageType.MerchantPlayerSyncPlayer:
                    {
                        int playernumber = reader.ReadByte();
                        CardMerchantQuest cardMerchantQuest = Main.player[playernumber].GetModPlayer<CardMerchantQuest>();
                        cardMerchantQuest.Complete = reader.ReadString();
                        break;
                    }
                case EntrogicModMessageType.SendCompletedCardMerchantMissionRequest:
                    {
                        int playernumber = reader.ReadByte();
                        CardMerchantQuest cardMerchantQuest = Main.player[playernumber].GetModPlayer<CardMerchantQuest>();
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
            NPCSpawnOnPlayerAction,
            SyncBookBubbleInfo,
            SyncCardGamingInfos,
            MerchantPlayerSyncPlayer,
            SendCompletedCardMerchantMissionRequest
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
            if (item != null && item.active && item.type != ItemID.None)
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
                if (npc != null && npc.type != NPCID.None && npc.friendly != friendly && npc.getRect().Intersects(hitbox))
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