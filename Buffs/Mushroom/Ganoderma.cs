using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Mushroom
{
    public class Ganoderma : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("灵芝");
            Description.SetDefault("“Who is that Mushroom！”");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
            canBeCleared = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int num = player.statLifeMax;
            player.statLifeMax2 += (int)((double)num * 0.3);
            player.lifeRegen += 9;
        }
    }
}
