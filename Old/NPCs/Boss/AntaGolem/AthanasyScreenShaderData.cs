using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.AntaGolem
{
    public class AthanasyScreenShaderData : ScreenShaderData
    {
        private int athanasyIndex;

        public AthanasyScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdateAthanasyIndex()
        {
            int athanasyType = NPCType<Antanasy>();
            if (athanasyIndex >= 0 && Main.npc[athanasyIndex].active && Main.npc[athanasyIndex].type == athanasyType)
            {
                return;
            }
            athanasyIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == athanasyType)
                {
                    athanasyIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdateAthanasyIndex();
            if (athanasyIndex != -1)
            {
                UseTargetPosition(Main.npc[athanasyIndex].Center);
            }
            base.Apply();
        }
    }
}
