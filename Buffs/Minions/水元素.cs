using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Buffs.Minions
{
    /// <summary>
    /// 水元素 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/9 21:28:52
    /// </summary>
    public class 水元素 :ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("运动水元素");
            Description.SetDefault("不稳定的水元素将为你而战");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("运动水元素")] > 0)
            {
                modPlayer.HasMovementWaterElemental = true;
            }
            if (!modPlayer.HasMovementWaterElemental)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
