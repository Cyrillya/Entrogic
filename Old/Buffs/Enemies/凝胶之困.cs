using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace Entrogic.Buffs.Enemies
{
    public class 凝胶之困 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("凝胶之困");
            Description.SetDefault("“这些粘粘的东西让你难以行动”");
            Main.debuff[Type] = true; 
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed -= 1.4f;
            player.maxRunSpeed -= 1f;
        }
    }
}
