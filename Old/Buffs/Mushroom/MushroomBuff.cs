using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Buffs.Mushroom
{
    public abstract class MushroomBuff : ModBuff
    {
        public sealed override void SetDefaults()
        {
            Description.SetDefault("“Who's that Mushroom！”");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
            canBeCleared = true;
        }
    }
}
