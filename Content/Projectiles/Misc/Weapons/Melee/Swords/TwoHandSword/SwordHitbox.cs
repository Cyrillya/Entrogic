namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords.TwoHandSword
{
    internal class SwordHitbox : ProjectileBase
    {
        public override string Texture => ResourceManager.Blank;

        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.width = 100;
            Projectile.height = 100;
            //Projectile.hide = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(ref Color lightColor) {
            //ModHelper.DrawBorderedRect(spriteBatch, new Color(0, 0, 0, 0), Color.Red, (Projectile.Hitbox.Location.ToVector2() - Main.screenPosition).Floor(), Projectile.Hitbox.Size(), 2);
            return base.PreDraw(ref lightColor);
        }
    }
}
