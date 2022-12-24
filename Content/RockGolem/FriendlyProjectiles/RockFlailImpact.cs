using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.FriendlyProjectiles
{
    public class RockFlailImpact : ProjectileBase
	{
        public override void SetStaticDefaults() {
			Main.projFrames[Type] = 10;
        }

        public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 36;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
			DrawOffsetX = -60;
			DrawOriginOffsetY = -30;
        }

        public override void AI() {
			Projectile.velocity.Y += 0.233f;
			if (Projectile.velocity.Y >= 6f) {
				Projectile.velocity.Y = 6f;
            }

			Projectile.ai[0]++;
			Projectile.frame = (int)Projectile.ai[0] / 5;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.Kill();
            }
			if (Projectile.ai[0] == 5 && Main.myPlayer == Projectile.owner) {
				int count = Main.rand.Next(6, 10 + 1);
				for (int i = 0; i < count; i++) {
					var source = Main.LocalPlayer.GetSource_ItemUse(Main.LocalPlayer.HeldItem);
					var velocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-10, -6));
					int frame = 3 + Main.rand.Next(3);
					int damage = (int)(Projectile.damage * 1.5f);
					Projectile.NewProjectile(source, Projectile.Bottom, velocity, ModContent.ProjectileType<RockFlailDebris>(), damage, Projectile.knockBack, Main.myPlayer, 0f, frame);
				}
			}
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
        }

        public override bool? CanDamage() => false;
    }
}
