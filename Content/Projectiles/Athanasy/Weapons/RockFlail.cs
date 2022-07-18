namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class RockFlail : Flail
    {
        public override void SetFlailDefaults() {
            LaunchTimeLimit = 20;
            LaunchSpeed = 16f;
            MaxLaunchLength = 450f;
            RetractAcceleration = 2f;
            MaxRetractSpeed = 18f;
            ForcedRetractAcceleration = 4f;
            MaxForcedRetractSpeed = 22f;

            Projectile.width = 36;
            Projectile.height = 36;
            DrawOriginOffsetY = 10;
            DrawOffsetX = -10;
        }

        public override void ThrowProjectile() {
            base.ThrowProjectile();
        }

        public override bool ModifyRotation(ref bool freeRotation) {
            freeRotation = false;
            return base.ModifyRotation(ref freeRotation);
        }
    }
}
