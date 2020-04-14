using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Mushroom
{
    public class Boletus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("牛肝菌");
            Description.SetDefault("“Who is that Mushroom！”");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
            canBeCleared = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += 0.35f;
        }
    }
}
