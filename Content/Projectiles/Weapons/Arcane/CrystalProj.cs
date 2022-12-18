namespace Entrogic.Content.Projectiles.Weapons.Arcane;

public class CrystalProj : ProjectileBase
{
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 18;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }

    public override void SetDefaults() {
        Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
        Projectile.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.aiStyle = -1;
        Projectile.Opacity = 0.7f;
    }

    public override void AI() {
        Projectile.ai[0]++;

        if (Projectile.ai[0] >= 10) {
            Projectile.velocity.Y += 0.211f;
            Projectile.velocity *= 0.985f;
        }

        Projectile.Opacity = 1f;
        if (Projectile.ai[0] <= 5f) {
            Projectile.Opacity = Projectile.ai[0] / 3f;
        }

        Projectile.rotation += Projectile.velocity.Length() * 0.05f;
    }

    // 反弹
    public override bool OnTileCollide(Vector2 oldVelocity) {
        Projectile.ai[1]++;
        if (Projectile.ai[1] >= 3) {
            Projectile.Kill();
            return false;
        }

        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

        if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) {
            Projectile.velocity.X = -oldVelocity.X;
        }

        if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
            Projectile.velocity.Y = -oldVelocity.Y;
        }

        return false;
    }

    public override bool PreDraw(ref Color lightColor) {
        int trailLength = Projectile.oldPos.Length;
        var color = new Color(50, 100, 255, 100) * 0.4f;

        for (int i = 0; i < trailLength - 4; i++) {
            if (Projectile.oldPos[i] == Vector2.Zero) continue;

            float factor = TrUtils.GetLerpValue(trailLength - 1, 0, i) * 0.8f;
            var center = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
            var roundTex = ModHelper.GetTexture("Misc/RoundSmall").Value;
            Main.EntitySpriteDraw(roundTex, center, null, color, 0f, roundTex.Size() / 2f, factor, SpriteEffects.None,
                0);

            if (!(Main.gfxQuality >= 0.5f)) continue;

            center = Vector2.Lerp(center, Projectile.oldPos[i + 1] - Main.screenPosition + Projectile.Size / 2f, .5f);
            Main.EntitySpriteDraw(roundTex, center, null, color, 0f, roundTex.Size() / 2f, factor, SpriteEffects.None,
                0);
        }

        // for (int i = 0; i < trailLength - 1; i++) {
        //     if (Projectile.oldPos[i] == Vector2.Zero) continue;
        //
        //     var tex = TextureAssets.Projectile[Type].Value;
        //     var center = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
        //     float factor = TrUtils.GetLerpValue(trailLength - 1, 0, i);
        //     float opacity = factor * 0.4f;
        //     float scale = factor * 0.9f;
        //
        //     Main.spriteBatch.Draw(tex, center, null, Color.White * opacity, Projectile.oldRot[i],
        //         Projectile.Size / 2f, Projectile.scale * scale, SpriteEffects.None, 0);
        // }

        var tex = ModContent.Request<Texture2D>(Texture).Value;
        var texGlow = ModContent.Request<Texture2D>(Texture + "_Glowy").Value;

        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor),
            Projectile.rotation, tex.Size() / 2f, Projectile.scale, 0, 0f);

        float glowFactor = Main.cursorScale * 0.7f;
        var glowColor = Color.Lerp(Color.Transparent, new Color(15, 50, 255, 0), glowFactor);
        Main.spriteBatch.Draw(texGlow, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation,
            texGlow.Size() / 2f, Projectile.scale, 0, 0f);

        return false;
    }
}