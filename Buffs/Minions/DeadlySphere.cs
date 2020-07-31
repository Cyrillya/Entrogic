using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Minions
{
    public class DeadlySphere : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Deadly Sphere V2");
            Description.SetDefault("The Deadly Sphere will fight for you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "DeadlySphere");
            Description.AddTranslation(GameCulture.Chinese, "一个毫无瑕疵的球体将为你作战");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DeadlySphere>()] > 0)
            {
                modPlayer.HasDeadlySphere = true;
            }
            if (!modPlayer.HasDeadlySphere)
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