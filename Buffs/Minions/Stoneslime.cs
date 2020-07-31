using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace Entrogic.Buffs.Minions
{
    public class Stoneslime : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stoneslime");
            Description.SetDefault("The stoneslime will fight for you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "岩石史莱姆");
            Description.AddTranslation(GameCulture.Chinese, "岩石史莱姆将为你而战");
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.衰落魔像.Stoneslime>()] > 0)
            {
                modPlayer.HasStoneSlime = true;
            }
            if (!modPlayer.HasStoneSlime)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
