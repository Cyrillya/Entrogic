using Entrogic.Content.NPCs.Enemies.Athanasy;

namespace Entrogic.Common.ModSystems
{
    public class ModIntegrationSystem : ModSystem
    {
        public override void PostSetupContent() {
            DoPhaseIndicatorIntegration();
        }

        private static void DoPhaseIndicatorIntegration() {
            if (Main.dedServ || !ModLoader.TryGetMod("PhaseIndicator", out Mod phaseIndicator))
                return;

            phaseIndicator.Call("PhaseIndicator",
                                ModContent.NPCType<Athanasy>(),
                                (NPC npc, float difficulty) => difficulty >= 2 ? 0.5f : 0.4f);
        }
    }
}
