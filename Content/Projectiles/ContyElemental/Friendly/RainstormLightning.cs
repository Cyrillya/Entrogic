namespace Entrogic.Content.Projectiles.ContyElemental.Friendly;

// Mostly from Fargo's Soul Mod. I'll delete it if it is not allowed
public class RainstormLightning : ModProjectile
{
    public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.CultistBossLightningOrbArc}";

    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 30;
        ProjectileID.Sets.TrailingMode[Type] = 1;
    }

    private float _colorlerp;
    private bool _playedSound;

    public override void SetDefaults() {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.scale = 0.65f;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.alpha = 100;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 120;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 1;
    }

    public override void AI() {
        Lighting.AddLight(Projectile.Center, .3f, .3f, .8f);
        _colorlerp += 0.15f;
        Projectile.localAI[0]++;

        if (!_playedSound) {
            SoundEngine.PlaySound(SoundID.Item122 with {
                Volume = 0.5f,
                Pitch = -0.5f
            }, Projectile.Center);

            _playedSound = true;
        }

        if (Main.rand.NextBool(6)) {
            if (!Main.rand.NextBool(5))
                return;
            int index3 = Dust.NewDust(Projectile.Center + Projectile.velocity.RotatedBy(1.57079637050629, new Vector2()) * ((float)Main.rand.NextDouble() - 0.5f) * (float)Projectile.width - Vector2.One * 4f, 8, 8, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);
            Dust dust = Main.dust[index3];
            dust.velocity *= 0.5f;
            Main.dust[index3].velocity.Y = -Math.Abs(Main.dust[index3].velocity.Y);
        }

        float num3 = Projectile.velocity.Length(); //take length of initial velocity
        Vector2 spinningpoint = Vector2.UnitX.RotatedBy(Projectile.ai[0]) * num3; //create a base velocity to modify for actual velocity of Projectile
        Vector2 rotationVector2 = spinningpoint.RotatedBy(Projectile.ai[1] * (Math.Floor(Math.Sin((Projectile.localAI[0] - MathHelper.Pi / 4) / 2)) + 0.5f) * MathHelper.Pi / 4); //math thing for zigzag pattern
        Projectile.velocity = rotationVector2;
        Projectile.rotation = Projectile.velocity.ToRotation() + 1.570796f;
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
        for (int index = 0; index < Projectile.oldPos.Length && ((double)Projectile.oldPos[index].X != 0.0 || (double)Projectile.oldPos[index].Y != 0.0); ++index) {
            Rectangle myRect = projHitbox;
            myRect.X = (int)Projectile.oldPos[index].X;
            myRect.Y = (int)Projectile.oldPos[index].Y;
            if (myRect.Intersects(targetHitbox))
                return true;
        }
        return false;
    }

    public override void Kill(int timeLeft) {
        float num2 = (float)(Projectile.rotation + 1.57079637050629 + (Main.rand.NextBool(2)? -1.0 : 1.0) * 1.57079637050629);
        float num3 = (float)(Main.rand.NextDouble() * 2.0 + 2.0);
        Vector2 vector2 = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
        for (int i = 0; i < Projectile.oldPos.Length; i++) {
            int index = Dust.NewDust(Projectile.oldPos[i], 0, 0, DustID.BlueFairy, vector2.X, vector2.Y, 0, new Color(), 1f);
            Main.dust[index].noGravity = true;
            Main.dust[index].scale = 1.7f;
        }
    }

    public override Color? GetAlpha(Color lightColor) {
        return Color.Lerp(new Color(71,167,193), new Color(122,201,204), 0.5f + (float)Math.Sin(_colorlerp) / 2);
    }

    public override bool PreDraw(ref Color lightColor) {
        var tex = TextureAssets.Projectile[Type].Value;
        var rectangle = tex.Bounds;
        var origin = rectangle.Size() / 2f;
        var color = new Color(122, 201, 204);
        for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Type]; i++) {
            if (Projectile.oldPos[i] == Vector2.Zero || Projectile.oldPos[i - 1] == Projectile.oldPos[i])
                continue;
            var offset = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
            int length = (int)offset.Length();
            float scale = Projectile.scale * (float)Math.Sin((i * 0.5f) / MathHelper.Pi);
            offset.Normalize();
            const int step = 4;
            for (int j = 0; j < length; j += step) {
                var pos = Projectile.oldPos[i] + offset * j;
                Main.EntitySpriteDraw(tex, pos + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), rectangle, color, Projectile.rotation, origin, scale + 0.15f, SpriteEffects.FlipHorizontally, 0);
            }
        }
        return false;
    }

    public override void PostDraw(Color lightColor) {
        var tex = TextureAssets.Projectile[Type].Value;
        var rectangle = tex.Bounds;
        var origin = rectangle.Size() / 2f;
        var color = Projectile.GetAlpha(lightColor);
        for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Type]; i++) {
            if (Projectile.oldPos[i] == Vector2.Zero || Projectile.oldPos[i - 1] == Projectile.oldPos[i])
                continue;
            var offset = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
            int length = (int)offset.Length();
            float scale = Projectile.scale * (float)Math.Sin(i * 0.5f / MathHelper.Pi);
            offset.Normalize();
            const int step = 4;
            for (int j = 0; j < length; j += step) {
                var pos = Projectile.oldPos[i] + offset * j;
                Main.EntitySpriteDraw(tex, pos + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), rectangle, color, Projectile.rotation, origin, scale + 0.2f, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Projectile.velocity = Vector2.Zero;
        return false;
    }
}