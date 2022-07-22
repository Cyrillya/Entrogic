using Terraria.ModLoader.Core;
using Hook = Entrogic.Common.Hooks.NPCs.IModifyCollisionData;

namespace Entrogic.Common.Hooks.NPCs
{
    public interface IModifyCollisionData
    {
        bool ModifyCollisionData(NPC npc, Rectangle victimHitbox, ref float damageMultiplier, ref Rectangle npcHitbox);

        public static readonly HookList<GlobalNPC> Hook = NPCLoader.AddModHook(new HookList<GlobalNPC>(typeof(Hook).GetMethod(nameof(ModifyCollisionData))));

        public static bool Invoke(NPC npc, Rectangle victimHitbox, ref float damageMultiplier, ref Rectangle npcHitbox) {
            bool result = true;

            foreach (Hook g in Hook.Enumerate(npc)) {
                result &= g.ModifyCollisionData(npc, victimHitbox, ref damageMultiplier, ref npcHitbox);
            }


            if (result && npc.ModNPC is Hook) {
                result = (npc.ModNPC as Hook).ModifyCollisionData(npc, victimHitbox, ref damageMultiplier, ref npcHitbox);
            }

            return result;
        }
    }
    public class InvokeModifyCollisionData : ILoadable
    {
        public void Load(Mod mod) {
            On.Terraria.NPC.GetMeleeCollisionData += (On.Terraria.NPC.orig_GetMeleeCollisionData orig, Rectangle victimHitbox, int enemyIndex, ref int specialHitSetter, ref float damageMultiplier, ref Rectangle npcRect) => {
                if (Hook.Invoke(Main.npc[enemyIndex], victimHitbox, ref damageMultiplier, ref npcRect)) {
                    orig.Invoke(victimHitbox, enemyIndex, ref specialHitSetter, ref damageMultiplier, ref npcRect);
                }
            };
        }

        public void Unload() { }
    }
}
