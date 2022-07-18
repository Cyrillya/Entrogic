namespace Entrogic.Common.Globals.Players
{
    public class SyncedDataPlayer : ModPlayer
    {
        private Vector2 _lastMouseWorld;
        internal Vector2 MouseWorld;

        public override void PostUpdate() {
            if (Main.myPlayer != Player.whoAmI) {
                return;
            }

            MouseWorld = Main.MouseWorld;

            if (_lastMouseWorld != MouseWorld && Main.netMode != NetmodeID.SinglePlayer) {
                ModNetHandler.PlayerData.SendMouseWold(-1, -1, MouseWorld);
            }

            _lastMouseWorld = MouseWorld;
        }
    }
}
