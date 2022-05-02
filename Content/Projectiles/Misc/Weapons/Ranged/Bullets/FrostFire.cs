namespace Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Bullets
{
    public class FrostFire : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Flames);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 80;
        }

        public override void AI() {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, MyDustID.IceTorch, 0f, 0f, 0, default, Main.rand.Next(3, 7));
            dust.noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => target.AddBuff(BuffID.Frostburn, Main.rand.Next(120, 211));

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => target.AddBuff(BuffID.Frostburn, Main.rand.Next(120, 211));
    }
}
