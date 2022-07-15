global using Entrogic.Common;
global using Entrogic.Common.ModSystems;
global using Entrogic.Common.Netcodes;
global using Entrogic.Common.Statics;
global using Entrogic.Content.DamageClasses;
global using Entrogic.Content.Projectiles.BaseTypes;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using ReLogic.Graphics;
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using Terraria;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.GameContent.Creative;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;

namespace Entrogic
{
    public class Entrogic : Mod
    {
        public static Entrogic Instance;

        public Entrogic() {
            Instance = this;
        }

        public override string Name => base.Name;

        public override Version Version => base.Version;

        public override uint ExtraPlayerBuffSlots => 88;

        public void LoggerWarn(string warnMsg) {
            Logger.Warn(warnMsg);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) => ModNetHandler.HandlePacket(reader, whoAmI);
    }
}