using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Entrogic.Buffs.Enemies
{
    /// <summary>
    /// 溶解 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/8 18:59:28
    /// </summary>
    public class 溶解 : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0) npc.lifeRegen = 0;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            mpr mpr = player.GetModPlayer<mpr>();
            if (player.lifeRegenTime > 0)
                player.lifeRegenTime = 0;
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;
            mpr.Timer++;
            if (mpr.Timer % 60 == 0)
            {
                int sec = mpr.Timer / 60;
                int SHITAmount = sec == 1 ? 1 : sec == 2 ? 1 : sec == 3 ? 2 : sec == 4 ? 3 : sec == 5 ? 5 : sec == 6 ? 8 : sec == 7 ? 13 : sec == 8 ? 21 : sec == 9 ? 34 : sec == 10 ? 55 : sec == 11 ? 89 : sec == 12 ? 144 : 233;
                player.statLife -= SHITAmount;
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.LifeRegen, SHITAmount, false, true);
                if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + Language.GetTextValue("Mods.Entrogic.Instrumen")), 10.0, 0, false);
                }
            }
        }
    }
    public class mpr : ModPlayer
    {
        public int Timer = 0;
        public override void PostUpdateBuffs()
        {
            if (!player.HasBuff(BuffType<溶解>()))
                Timer = 0;
        }
    }
}
