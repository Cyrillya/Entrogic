using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Buffs.Miscellaneous;
using System.Collections.Generic;

namespace Entrogic.Buffs.Weapons
{
    public class Plague : ModBuff
    {
        public override void SetDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = true;
        }

        public int Timer = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            Timer++;
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (player.buffType[i] == BuffType<Antibody>())
                {
                    player.DelBuff(buffIndex);
                    buffIndex--;
                    return;
                }
            }
            if (player.lifeRegenTime > 0)
                player.lifeRegenTime = 0;
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            if (Main.rand.NextBool(20))
                foreach (Player target in Main.player)
                {
                    if (target.active && !target.GetModPlayer<EntrogicPlayer>().HasAntibody && target.whoAmI != player.whoAmI && player.team == target.team && player.Distance(target.Center) <= 26f * 16f && !target.buffImmune[Type])
                    {
                        target.AddBuff(Type, Main.rand.Next(5, 10 + 1) * 60);
                    }
                }

            if (/*Timer % 10 == 0 && */Main.rand.NextBool(28 * 3))
            {
                int amount = (int)(player.statLifeMax2 / 240f) + 4;
                amount *= 3;
                player.statLife -= amount;
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.LifeRegen, amount, false, true);

                float antiPossibility = 0.11f;
                antiPossibility += player.statLife / player.statLifeMax2 * 0.19f;
                if (Main.rand.NextFloat() < antiPossibility * 2f)
                    player.AddBuff(BuffType<Antibody>(), Main.rand.Next(3, 6) * 60);
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Timer++;
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffType[i] == BuffType<Antibody>())
                {
                    npc.DelBuff(buffIndex);
                    buffIndex--;
                    return;
                }
            }
            if (npc.type != NPCID.TargetDummy && npc.GetGlobalNPC<EntrogicNPC>().buffOwner != -1)
                Main.player[npc.GetGlobalNPC<EntrogicNPC>().buffOwner].GetModPlayer<EntrogicPlayer>().PlagueNPCAmount++;
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            if (Main.rand.NextBool(20))
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.whoAmI != npc.whoAmI && !target.friendly && npc.Distance(target.Center) <= 16f * 16f && !target.buffImmune[Type] && !(target.type != NPCID.TargetDummy && npc.type == NPCID.TargetDummy))
                    {
                        target.AddBuff(Type, Main.rand.Next(4, 9 + 1) * 60);
                        target.GetGlobalNPC<EntrogicNPC>().buffOwner = npc.GetGlobalNPC<EntrogicNPC>().buffOwner;
                    }
                }

            if (/*Timer % 10 == 0 && */Main.rand.NextBool(34 * 3) && !npc.dontTakeDamage)
            {
                int amount = (int)(npc.lifeMax / 390f) + (Main.hardMode ? 6 : 3);
                if (npc.boss)
                    amount = (Main.hardMode ? 9 : 4) + (int)(npc.lifeMax / 620f);
                amount *= 3;
                if (npc.type != NPCID.TargetDummy)
                    npc.life -= amount;
                int life = CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), CombatText.LifeRegenNegative, amount, false, true);
                if (Main.netMode == NetmodeID.Server && life != 100)
                {
                    CombatText combatText = Main.combatText[life];
                    NetMessage.SendData(MessageID.CombatTextInt, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, amount);
                }
                float antiPossibility = 0.07f;
                antiPossibility += npc.life / npc.lifeMax * 0.17f;
                if (npc.boss)
                    antiPossibility *= 1.8f;
                if (Main.rand.NextFloat() < antiPossibility * 2f)
                    npc.AddBuff(BuffType<Antibody>(), Main.rand.Next(4, 9) * 60);
                // 处理NPC死亡
                if (npc.life > 0 || npc.immortal)
                {
                    return;
                }
                npc.life = 1;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.StrikeNPCNoInteraction(9999, 0f, 0);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, 9999f);
                    }
                }
            }
        }
    }
}
