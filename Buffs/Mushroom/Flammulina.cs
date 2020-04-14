using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Buffs.Mushroom
{
    public class Flammulina : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("金针菇");
            Description.SetDefault("“Who is that Mushroom！”");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
            canBeCleared = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeSpeed += 0.2f;
            player.moveSpeed += 0.06f;
        }
    }
}
