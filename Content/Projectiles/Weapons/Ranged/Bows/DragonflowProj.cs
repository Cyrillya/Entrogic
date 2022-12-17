namespace Entrogic.Content.Projectiles.Weapons.Ranged.Bows
{
    public class DragonflowProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Type] = 0;        //The recording mode
        }

        public override void SetDefaults() {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 50;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() / 255f);
        }

        public override void Kill(int timeLeft) {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.InfernoFriendlyBlast, Projectile.damage, Projectile.knockBack);
            proj.netUpdate = true;
            proj.DamageType = DamageClass.Ranged;
            proj.timeLeft = 2;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => Main.player[Projectile.owner].GetArmorPenetration(Projectile.DamageType) += 10;

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, Math.Min(10, 2 + (int)Projectile.oldVelocity.Length()), 0.8f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire, 300);
    }
}
