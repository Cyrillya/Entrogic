namespace Entrogic.Content.NPCs.BaseTypes
{
    public abstract class NPCBase : ModNPC
    {
        protected int State {
            get { return (int)NPC.ai[0]; }
            set { NPC.ai[0] = value; }
        }
        protected float Timer {
            get { return NPC.ai[1]; }
            set { NPC.ai[1] = value; }
        }
        protected virtual void SwitchState(int state) {
            State = state;
        }
    }
}
