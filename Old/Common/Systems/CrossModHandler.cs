using Entrogic.Items.AntaGolem;
using Entrogic.Items.Miscellaneous.Placeable.Trophy;
using Entrogic.Items.PollutElement;
using Entrogic.Items.PollutElement.Armor;
using Entrogic.Items.VoluGels;
using Entrogic.NPCs;
using Entrogic.NPCs.Boss.AntaGolem;
using Entrogic.NPCs.Boss.PollutElement;
using Entrogic.NPCs.Boss.凝胶Java盾;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Common
{
    public class CrossModHandler : ModSystem
    {
        public static bool IsCalamityModRevengenceMode => (bool)LoadedMods["CalamityMod"].Call("GetDifficultyActive", "revengeance");
        public static bool IsCalamityModDeathMode => (bool)LoadedMods["CalamityMod"].Call("GetDifficultyActive", "death");
        internal static Dictionary<string, Mod> LoadedMods;
        private string[] mods;
        public override void Load()
        {
            mods = new string[]
            {
                "Fargowiltas", // Fargowiltas
                "BossChecklist", // Boss Checklist
                "FKBossHealthBar", // Boss Health Bar
                "CalamityMod", // Calamity Mod
            };
            LoadedMods = new Dictionary<string, Mod>();
            foreach (string mod in mods)
            {
                LoadedMods.Add(mod, null);
            }

            if (ModLoaded("FKBossHealthBar"))
            {
                Mod yabhb = LoadedMods["FKBossHealthBar"];
                #region Wlta yabhb
                yabhb.Call("RegisterCustomHealthBar",
                    NPCType<Embryo>(),
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
                    NPCType<Volutio>(),
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
        }
        public override void PostSetupContent()
        {
            try
            {
                LoadedMods = new Dictionary<string, Mod>();

                foreach (string mod in mods)
                {
                    ModLoader.TryGetMod(mod, out Mod loadedMod);
                    LoadedMods.Add(mod, loadedMod);
                }
            }
            catch (Exception e)
            {
                Entrogic.Instance.Logger.Error("Fargowiltas PostSetupContent Error: " + e.StackTrace + e.Message);
            }
            if (ModLoaded("BossChecklist"))
            {
                Mod bossChecklist = LoadedMods["BossChecklist"];
                bossChecklist.Call(
            "AddBoss",
            2.7f,
            new List<int>() { NPCType<Volutio>(), NPCType<Embryo>() },// Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.Volutio", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedGelSymbiosis),
            ItemType<GelCultureFlask>(), // Summon Items
            new List<int> { ItemType<VolutioMask>(), ItemType<VTrophy>() }, // Collections
            new List<int> { ItemType<VolutioTreasureBag>(), ItemType<GelAnkh>(), ItemType<GelOfLife>() }, // Normal Loots
            "$Mods.Entrogic.BossSpawnInfo.GelSymb", // Spawn Info
            "", // Despawn Info
            "Entrogic/Images/GelSym_Textures");// 这里切记别用ModTexturesTable，不然服务器会炸

                bossChecklist.Call(
            "AddBoss",
            5.7f,
            NPCType<Antanasy>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.Antanasy", // Boss Name
            (Func<bool>)(() => EntrogicWorld.downedAthanasy),
            ItemType<TitansOrder>(),
            new List<int> { ItemType<AthanasyMask>(), ItemType<AthanasyTrophy>() },
            new List<int> { ItemType<AthanasyTreasureBag>(), ItemType<RockSpear>(), ItemType<RockShotgun>(), ItemType<EyeofImmortal>(), ItemType<StoneSlimeStaff>() },
            "$Mods.Entrogic.BossSpawnInfo.Athanasy");

                bossChecklist.Call(
            "AddBoss",
            6.8f,
            NPCType<PollutionElemental>(), // Boss ID
            this, // Mod
            "$Mods.Entrogic.NPCName.PollutionElemental", // Boss Name
            (Func<bool>)(() => EntrogicWorld.IsDownedPollutionElemental),
            ItemType<ContaminatedLiquor>(),
            new List<int> { ItemType<PollutionElementalMask>(), ItemType<PETrophy>() },
            new List<int> { ItemType<ContaminatedElementalTreasureBag>(),  ItemType<BottleofStorm>(), ItemType<WaterElementalStaff>(), ItemType<ContaminatedLongbow>(), ItemType<ContaminatedCurrent>(),
                ItemType<HelmetofContamination>(), ItemType<HeadgearofContamination>(), ItemType<MaskofContamination>(), ItemType<BreastplateofContamination>(), ItemType<GreavesofContamination>() },
            "$Mods.Entrogic.BossSpawnInfo.PollutionElement",
            " 在大海中继续沉睡...");
            }
            if (ModLoaded("Fargowiltas"))
            {
                Mod fargos = LoadedMods["Fargowiltas"];
                // AddSummon, order or value in terms of vanilla bosses, your mod internal name, summon  
                // item internal name, inline method for retrieving downed value, price to sell for in copper
                fargos.Call("AddSummon", 2.7f, "Entrogic", "EmbryoCultureFlask", (Func<bool>)(() => EntrogicWorld.downedGelSymbiosis), new Money(gold: 9).ToInt());
                fargos.Call("AddSummon", 5.7f, "Entrogic", "TitansOrder", (Func<bool>)(() => EntrogicWorld.downedAthanasy), new Money(gold: 18).ToInt());
                fargos.Call("AddSummon", 6.8f, "Entrogic", "ContaminatedLiquor", (Func<bool>)(() => EntrogicWorld.IsDownedPollutionElemental), new Money(gold: 32).ToInt());
            }
        }
        public override void Unload()
        {
            mods = null;
            LoadedMods = null;
        }
        public static bool ModLoaded(string modName)
        {
            ModLoader.TryGetMod(modName, out Mod mod);

            return mod != null;
        }
    }
}
