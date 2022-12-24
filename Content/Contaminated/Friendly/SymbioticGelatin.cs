using Entrogic.Content.Contaminated.Buffs;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers;

namespace Entrogic.Content.Contaminated.Friendly
{
    public class SymbioticGelatin : Minion
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Symbiotic Gelatin");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "共生明胶");

            Main.projFrames[Type] = 5;
            Main.projPet[Type] = true; // Denotes that this projectile is a pet or minion
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void MinionDefaults(out int buffType) {
            buffType = ModContent.BuffType<SymbioticGelatinBuff>();
            SearchDistance = 960f;

            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 80;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 0.33f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.localNPCHitCooldown = 15;
            Projectile.usesLocalNPCImmunity = true;
        }

        protected float Timer {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        public override void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 60f;

            // Teleport to player if distance is too big
            vectorToIdlePosition = ModHelper.GetFromToVector(Projectile.Center, idlePosition);
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2200f) {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.05f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++) {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
                    if (Projectile.position.X < other.position.X) {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y) {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        public override void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
            base.Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

            if (foundTarget) {
                Timer++;
                if (Timer >= Main.rand.Next(31, 40) || Timer == 1f) {
                    float speedMuiltpiler = 15f;
                    if (distanceFromTarget >= 720) {
                        speedMuiltpiler = 22f;
                    }
                    Projectile.velocity = (ModHelper.GetFromToRadians(Projectile.Center, targetCenter) + MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f))).ToRotationVector2() * speedMuiltpiler;
                    Timer = 1f;
                }
                else {
                    Projectile.velocity *= 0.98f;
                }
            }
            else {
                float limitSpeed = 1.8f;
                if (distanceToIdlePosition > 70f) {
                    Projectile.velocity = (Projectile.velocity * 40f + Vector2.Normalize(vectorToIdlePosition) * 15f) / 41f;
                }
                else if (Projectile.velocity.X >= -limitSpeed && Projectile.velocity.X <= limitSpeed
                    && Projectile.velocity.Y >= -limitSpeed && Projectile.velocity.Y <= limitSpeed) {
                    Projectile.velocity *= 1.4f;
                }
            }
        }

        public override void Visuals() {
            base.Visuals();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            Projectile.localAI[0] += 0.2f;
            Projectile.frame = (int)Projectile.localAI[0] % 5;
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, 7);
            return false;
        }
    }
}
