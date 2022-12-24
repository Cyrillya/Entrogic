using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Corrupted.TartagliaEnemy
{
    public class PoisonedArrowHostile : ProjectileBase
    {
        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
            AIType = ProjectileID.WoodenArrowHostile;
        }

        public override void AI() {
            base.AI();
            Projectile.localAI[1]++;
        }

        public override bool PreDraw(ref Color lightColor) {
            
            return Projectile.localAI[1] > 3;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
            base.ModifyHitPlayer(target, ref damage, ref crit);
            target.AddBuff(BuffID.Poisoned, 60 * 10);
            target.noKnockback = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            base.OnHitPlayer(target, damage, crit);
            target.immuneTime = 0;
            target.immune = false;
            Projectile.Kill();
        }
    }
}
