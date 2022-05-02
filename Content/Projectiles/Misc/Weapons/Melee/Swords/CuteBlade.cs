namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class CuteBlade : ModProjectile
    {
        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 23;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Arkhalis);
            Projectile.damage = 51;
            Projectile.aiStyle = -1;
            Projectile.knockBack = 6f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 70;
            Projectile.Size = new Vector2(90f, 84f);
        }

        public override void AI() {
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0) {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 12;
            }
            Projectile.timeLeft = 2;
            if (!Main.player[Projectile.owner].channel)
                Projectile.Kill();
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 1) {
                Projectile.frame++;
                Projectile.ai[0] = 0;
            }
            if (Projectile.frame >= 23)
                Projectile.frame = 0;
            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            Projectile.Center = Main.player[Projectile.owner].Center;
            Main.player[Projectile.owner].itemTime = 6;
            Main.player[Projectile.owner].itemAnimation = 6;
            if (Main.MouseWorld.X > Projectile.Center.X)
                Main.player[Projectile.owner].direction = 1;
            else
                Main.player[Projectile.owner].direction = -1;
            for (int i = 0; i < 2; i++) {
                if (Main.rand.NextBool(2)) {
                    int dust = Dust.NewDust(Projectile.position + new Vector2(Main.rand.Next(18, 31) * Main.player[Projectile.owner].direction, 10f), Projectile.width, Projectile.height - 10, MyDustId.LightCyanParticle1, 8.6f * Main.player[Projectile.owner].direction, 0, 160, default(Color), 1.2f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override bool ShouldUpdatePosition() {
            return false;
        }
    }
}
