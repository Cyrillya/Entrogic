using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.FriendlyProjectiles
{
    public class RockSpearFriendly : ProjectileBase
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults() {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -70;
            DrawOriginOffsetX = 34;
            DrawOriginOffsetY = -4;
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool PreDraw(ref Color lightColor) {
            var tex = TextureAssets.Projectile[Type];
            var t = tex.Value;
            int frameHeight = tex.Height() / Main.projFrames[Type];

            Color drawColor = Projectile.GetAlpha(lightColor);
            int drawOriginOffsetY = 0;
            int drawOffsetX = 0;
            float drawOriginOffsetX = (tex.Width() - Projectile.width) * 0.5f + Projectile.width * 0.5f;
            ProjectileLoader.DrawOffset(Projectile, ref drawOffsetX, ref drawOriginOffsetY, ref drawOriginOffsetX);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;

            var rect = new Rectangle(0, frameHeight * Projectile.frame, tex.Width(), frameHeight - 1);

            var origin = new Vector2(drawOriginOffsetX, Projectile.height / 2 + drawOriginOffsetY);
            var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            // 残影
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                var posShadow = new Vector2(Projectile.oldPos[i].X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.oldPos[i].Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);
                float factor = Utils.GetLerpValue(0, Projectile.oldPos.Length, i); // 透明度插值
                float opacity = MathHelper.Lerp(0.5f, 0f, factor);
                Main.EntitySpriteDraw(t, posShadow, rect, drawColor * opacity, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            Main.EntitySpriteDraw(t, pos, rect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return true;
        }

        public override void Kill(int timeLeft) {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
