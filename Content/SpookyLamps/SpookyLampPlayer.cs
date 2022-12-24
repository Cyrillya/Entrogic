namespace Entrogic.Content.SpookyLamps
{
    internal class SpookyLampPlayer : ModPlayer
    {
        public override void UpdateDead() {
            base.UpdateDead();
            SpookyLampHandler.HandleLamps(Player, false);
        }

        public override void PlayerDisconnect(Player player) {
            base.PlayerDisconnect(player);
            SpookyLampHandler.HandleLamps(Player, false);
        }
    }
}
