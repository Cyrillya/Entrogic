using Entrogic.Content.Dusts;

namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class CollapseLaser : ProjectileBase
    {
        public override void SetDefaults() {
            Projectile.Size = new(8);
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.penetrate = 8;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            // 越穿透伤害越低
            float factor = Utils.GetLerpValue(0f, 8f, Projectile.penetrate, true);
            factor = MathHelper.Lerp(0.2f, 1f, factor);
            damage = (int)(damage * factor);
        }

        public override void AI() {
            if (Projectile.timeLeft > 290)
                return;
            var d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BubbleCopy>(), Vector2.Zero, 180, default, 1.4f);
            d.fadeIn = 1.2f;
            d.noGravity = true;
        }

        public override void Kill(int timeLeft) {
            if (Main.netMode == NetmodeID.Server)
                return;
            for (int i = 0; i < 15; i++) {
                var position = Utils.RandomVector2(Main.rand, -60f, 60f) + Projectile.Center;
                var velocity = Projectile.Center.DirectionTo(position) * Main.rand.NextFloat(2.3f, 4.4f);
                var d = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, velocity, 100, Color.SkyBlue);
                d.noGravity = true;
            }
        }
    }
}
