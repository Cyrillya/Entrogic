using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs
{
    public abstract class FSM_NPC : ModNPC
    {
        protected int State
        {
            get { return (int)npc.ai[0]; }
            set { npc.ai[0] = value; }
        }
        protected int Timer
        {
            get { return (int)npc.ai[1]; }
            set { npc.ai[1] = value; }
        }
        protected virtual void SwitchState(int state)
        {
            State = state;
        }
    }
}