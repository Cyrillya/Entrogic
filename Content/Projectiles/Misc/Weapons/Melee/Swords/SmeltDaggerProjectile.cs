using Terraria.ID;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class SmeltDaggerProjectile : ModProjectile
    {
        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 24;
        }

        public override void SetDefaults() {
            Projectile.width = 76;
            Projectile.height = 76;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.2f;
            Projectile.alpha = 128;
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
            if (Projectile.frame >= 24)
                Projectile.frame = 0;
            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            Projectile.Center = Main.player[Projectile.owner].Center + Projectile.Size * ((Projectile.scale - 1f) * 0.5f);
            Main.player[Projectile.owner].itemTime = 6;
            Main.player[Projectile.owner].itemAnimation = 6;
            if (Main.MouseWorld.X > Projectile.Center.X)
                Main.player[Projectile.owner].direction = 1;
            else
                Main.player[Projectile.owner].direction = -1;
            if (Main.rand.NextBool(3)) {
                int dust = Dust.NewDust(Projectile.position + new Vector2(Main.rand.Next(18, 31) * Main.player[Projectile.owner].direction, 10f), Projectile.width, Projectile.height - 10, DustID.Torch, 8.6f * Main.player[Projectile.owner].direction, 0, 160, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
            }
            Lighting.AddLight((int)((Projectile.position.X + (Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (Projectile.height / 2)) / 16f), 254 / 255f, 133 / 255f, 70 / 255f);
        }
        public override bool ShouldUpdatePosition() {
            return false;
        }
    }
}
