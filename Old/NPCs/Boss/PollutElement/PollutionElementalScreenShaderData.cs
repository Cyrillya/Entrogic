using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class PollutionElementalScreenShaderData : ScreenShaderData
    {
        private int pollutionElementalIndex;

        public PollutionElementalScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdatePollutionElementalIndex()
        {
            int pollutionElementalType = NPCType<PollutionElemental>();
            if (pollutionElementalIndex >= 0 && Main.npc[pollutionElementalIndex].active && Main.npc[pollutionElementalIndex].type == pollutionElementalType)
            {
                return;
            }
            pollutionElementalIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == pollutionElementalType)
                {
                    pollutionElementalIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdatePollutionElementalIndex();
            if (pollutionElementalIndex != -1)
            {
                UseTargetPosition(Main.npc[pollutionElementalIndex].Center);
            }
            base.Apply();
        }
    }
}
