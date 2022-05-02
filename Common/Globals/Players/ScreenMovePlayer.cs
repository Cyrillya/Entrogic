namespace Entrogic.Common.Globals.Players
{
    public class ScreenMovePlayer : ModPlayer
    {
        public Vector2 ScreenMoveBegin;
        public Vector2 ScreenMoveTarget;
        public int ScreenMoveTime;
        private int ScreenMoveTimer;

        public float ScreenShakeRange;
        public int ScreenShakeTime;
        private int ScreenShakeTimer;

        public static Vector2 ScreenCenter {
            get => new(Main.screenPosition.X + Main.screenWidth * 0.5f, Main.screenPosition.Y + Main.screenHeight * 0.5f);
            set => Main.screenPosition = new Vector2(value.X - Main.screenWidth * 0.5f, value.Y - Main.screenHeight * 0.5f);
        }

        public override void ModifyScreenPosition() {
            if (ScreenMoveTime != 0 && ScreenMoveBegin != Vector2.Zero && ScreenMoveTarget != Vector2.Zero) {
                ScreenMoveTimer++;
                ScreenCenter = Vector2.SmoothStep(ScreenMoveBegin, ScreenMoveTarget, (float)ScreenMoveTimer / (float)ScreenMoveTime).Floor();
                if (ScreenMoveTimer >= ScreenMoveTime) {
                    ScreenMoveBegin = Vector2.Zero;
                    ScreenMoveTarget = Vector2.Zero;
                    ScreenMoveTime = 0;
                    ScreenMoveTimer = 0;
                }
            }
            if (ScreenShakeTime != 0 && ScreenShakeRange != 0) {
                ScreenShakeTimer++;
                Main.screenPosition += new Vector2(Main.rand.NextFloat() * ScreenShakeRange, Main.rand.NextFloat() * ScreenShakeRange);
                if (ScreenShakeTimer >= ScreenShakeTime) {
                    ScreenShakeRange = 0;
                    ScreenShakeTime = 0;
                    ScreenShakeTimer = 0;
                }
            }

            base.ModifyScreenPosition();
        }
    }
}
