using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.FriendlyProjectiles
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

        private bool Smashed = false;

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (CurrentAIState == AIState.Dropping && Main.myPlayer == Projectile.owner && !Smashed) {
                Smashed = true;
                var position = Projectile.Center;
                Projectile.NewProjectile(Main.LocalPlayer.GetSource_ItemUse(Main.LocalPlayer.HeldItem),
                    position, Vector2.Zero, ModContent.ProjectileType<RockFlailImpact>(),
                    Projectile.damage, 2f, Main.myPlayer);
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void ShootProjectile() {
            Projectile.NewProjectile(Main.LocalPlayer.GetSource_ItemUse(Main.LocalPlayer.HeldItem),
                Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GolemTornado>(),
                (int)(Projectile.damage * 0.7f), 2f, Main.myPlayer);
        }

        public override bool ModifyRotation(ref bool freeRotation) {
            freeRotation = false;
            return base.ModifyRotation(ref freeRotation);
        }
    }
}
