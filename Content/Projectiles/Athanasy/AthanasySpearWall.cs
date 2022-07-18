using Terraria.Graphics.Shaders;

namespace Entrogic.Content.Projectiles.Athanasy
{
    public class AthanasySpearWall : ProjectileBase
    {
        public override string Texture => "Entrogic/Content/Projectiles/Athanasy/AthanasySpearSmall";

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = -1; // 自己有一套残影Update
            NPCs.Enemies.Athanasy.Athanasy.WallSpearType = Type;
        }

        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.PoisonDart);
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 550;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.hide = true;
            DrawOffsetX = -70;
            DrawOriginOffsetX = 34;
            DrawOriginOffsetY = -4;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
            if (Timer <= 60)
                behindNPCsAndTiles.Add(index);
        }

        public ref float Timer => ref Projectile.ai[0];
        public float rotation;
        public int targetPlayer = -1;
        public float recordedYOffset;

        public override void AI() {
            Timer++;
            if (Timer <= 60)
                Projectile.hide = true;
            else
                Projectile.hide = false;

            if (Timer == 1) {
                Projectile.netUpdate = true;
                rotation = Projectile.velocity.ToRotation();
            }

            if (Timer <= 5 && !Main.dedServ) {
                float factor = Utils.GetLerpValue(1, 5, Timer);
                Projectile.Opacity = MathHelper.Lerp(0f, 1f, factor);
            }

            if (Timer <= 55f) {
                Projectile.velocity = Vector2.Zero;
                if (Timer <= 30f) {
                    Projectile.velocity = rotation.ToRotationVector2() * 1.5f;
                }
            }
            else {
                Projectile.velocity = rotation.ToRotationVector2() * 10f;
                if (Timer <= 62) {
                    float factor = Utils.GetLerpValue(55, 62, Timer);
                    Projectile.velocity = rotation.ToRotationVector2() * MathHelper.Lerp(1f, 10f, factor);
                }
                if (!Main.dedServ && Timer == 56f) {
                    for (int i = 0; i < 8; i++) {
                        float radians = 6.28f / 8f * i;
                        var velocity = Vector2.One.RotatedBy(radians) * 1.4f;
                        var d = Dust.NewDustPerfect(Projectile.Center + rotation.ToRotationVector2() * 8f, DustID.DungeonWater, velocity, 150, Scale: 1.3f);
                        d.fadeIn = 0.8f;
                        d.noGravity = true;
                    }
                }
            }

            if (!Projectile.tileCollide && Timer >= 90f) {
                Projectile.tileCollide = true;
            }

            Projectile.rotation = rotation;

            // 射出的时候才开始录残影
            if (Timer >= 62f && !Main.dedServ) {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--) {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldPos[0] = Projectile.position;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            // 乐，用完DrawBehind之后lightColor直接无了
            if (Timer <= 60)
                lightColor = Lighting.GetColor(Projectile.position.ToTileCoordinates());

            var tex = TextureAssets.Projectile[Type];
            var t = tex.Value;
            int frameHeight = tex.Height() / Main.projFrames[Type];

            Color drawColor = Projectile.GetAlpha(lightColor);
            int drawOriginOffsetY = 0;
            int drawOffsetX = 0;
            float drawOriginOffsetX = (float)(tex.Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
            ProjectileLoader.DrawOffset(Projectile, ref drawOffsetX, ref drawOriginOffsetY, ref drawOriginOffsetX);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;

            var rect = new Rectangle(0, frameHeight * Projectile.frame, tex.Width(), frameHeight - 1);

            var origin = new Vector2(drawOriginOffsetX, Projectile.height / 2 + drawOriginOffsetY);
            var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            // 残影
            if (Timer > 50f) {
                for (int i = 0; i < Projectile.oldPos.Length; i++) {
                    var posShadow = new Vector2(Projectile.oldPos[i].X - Main.screenPosition.X + drawOriginOffsetX + (float)drawOffsetX, Projectile.oldPos[i].Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);
                    float factor = Utils.GetLerpValue(0, Projectile.oldPos.Length, i); // 透明度插值
                    float opacity = MathHelper.Lerp(0.5f, 0f, factor);
                    Main.EntitySpriteDraw(t, posShadow, rect, drawColor * opacity, Projectile.rotation, origin, Projectile.scale, effects, 0);
                }
            }

            if (Timer >= 55 && Timer <= 70) {
                float factor = Utils.GetLerpValue(55, 70, Timer);
                Color color = new(150, 130, 255, 50);

                color *= MathHelper.Lerp(0.7f, 0f, factor);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.ColorOnlyDye), Projectile, null);

                for (float i = 0f; i < 1f; i += 0.4f) {
                    float radians = i * MathHelper.TwoPi;

                    Main.EntitySpriteDraw(t, pos + new Vector2(0f, 8f).RotatedBy(radians), null, color, Projectile.rotation, origin, Projectile.scale, effects, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Main.EntitySpriteDraw(t, pos, rect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return true;
        }

        public override void Kill(int timeLeft) {
            Projectile.velocity *= -0.3f;
            for (int i = 0; i < 15; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X, Projectile.velocity.Y, 120, Scale: Main.rand.NextFloat(1f, 1.4f));
            }
        }
    }
}
