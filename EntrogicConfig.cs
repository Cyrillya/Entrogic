using Entrogic.Items.Weapons.Card;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace Entrogic
{
    // This file contains 2 real ModConfigs (and also a bunch of fake ModConfigs showcasing various ideas). One is set to ConfigScope.ServerSide and the other ConfigScope.ClientSide
    // ModConfigs contain Public Fields and Properties that represent the choices available to the user. 
    // Those Fields or Properties will be presented to users in the Config menu.
    // DONT use static members anywhere in this class (except for a variable named Instance, see below), tModLoader maintains several instances of ModConfig classes which will not work well with static properties or fields.

    /// <summary>
    /// This config operates on a per-client basis. 
    /// These parameters are local to this computer and are NOT synced from the server.
    /// </summary>
    [Label("Entrogic 个人设置")]
    public class AEntrogicConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static AEntrogicConfigClient Instance;

        [DefaultValue(false)]
        [Label("Show more useful informations in tooltip")]
        public bool ShowUsefulInformations;

        [DefaultValue(false)]
        [Label("使用高速枪械时屏幕抖动")]
        public bool GunEffect;

        [DefaultValue(false)]
        [Label("启用钩爪指针")]
        public bool HookMouse;

        [DefaultValue(false)]
        [Label("启用癫痫模式")]
        public bool ThatsCrazy;
    }
    [Label("服务器设置")]
    public class BEntrogicConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return false;
        }

        public static BEntrogicConfigServer Instance;

        [DefaultValue(true)]
        [Label("(卡牌)多人模式强制开荒")]
        [ReloadRequired]
        public bool ClearNewPlayersCard;
    }
    [Label("卡牌设置")]
    public class CEntrogicCardConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static CEntrogicCardConfig Instance;

        [Header("$Mods.Entrogic.Common.RandomCardImage")]

        [DefaultValue(false)]
        [Label("手牌栏以环绕中心排列")]
        public bool HandSlotSpecial;

        [DefaultValue(false)]
        [Label("自动补牌")]
        public bool AutoUseCard;

    }
    [Label("Debug Settings")]
    public class DEntrogicDebugClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static DEntrogicDebugClient Instance;

        [DefaultValue(false)]
        [Label("启用作者模式")]
        public bool AuthorMode;

        [Range(0, 216000)]
        [DefaultValue(1150)]
        [Label("Change custom wash delay time")]
        public int CardWashDelayTime;

        [Range(0, 216000)]
        [DefaultValue(600)]
        [Label("Change custom pass delay time")]
        public int CardPassDelayTime;

    }
}