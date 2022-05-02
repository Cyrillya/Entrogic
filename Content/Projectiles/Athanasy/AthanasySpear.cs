namespace Entrogic.Content.Projectiles.Athanasy
{
    internal class AthanasySpear : ModProjectile
    {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
            Main.projFrames[Type] = 2;
            DisplayName.SetDefault("Athanasy Spear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "魔像之矛");
        }

        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.PoisonDart);
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 550;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.hide = true;
            DrawOffsetX = -168;
            DrawOriginOffsetX = 84;
            DrawOriginOffsetY = -12;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
            behindNPCsAndTiles.Add(index);
        }

        public override bool ShouldUpdatePosition() {
            return Projectile.timeLeft <= 300 || Projectile.timeLeft >= 540;
        }

        public override void AI() {
            base.AI();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
        }

        public override void PostDraw(Color lightColor) {
            base.PostDraw(lightColor);

            //var tex = TextureAssets.Projectile[Projectile.type];
            //int num136 = 0;
            //int num137 = 0;
            //float num138 = (float)(tex.Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
            //ProjectileLoader.DrawOffset(Projectile, ref num137, ref num136, ref num138);

            //var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + num138 + (float)num137, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            //var posTile = (pos + Main.screenPosition).ToTileCoordinates();
            //for (int i = posTile.X - 30; i <= posTile.X + 30; i++) {
            //    for (int j = posTile.Y - 30; j <= posTile.Y + 30; j++) {
            //        if (!WorldGen.InWorld(i, j)) continue;

            //        var t = Framing.GetTileSafely(i, j);

            //        if (Lighting.Brightness(i, j) < 0.01f)
            //            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(i << 4, j << 4) - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.Black);
            //    }
            //}
        }

        public override bool PreDraw(ref Color lightColor) {
            var tex = TextureAssets.Projectile[Projectile.type];
            var t = tex.Value;
            int frameHeight = tex.Height() / Main.projFrames[Projectile.type];

            Color alpha13 = Projectile.GetAlpha(lightColor);
            int num136 = 0;
            int num137 = 0;
            float num138 = (float)(tex.Width() - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
            ProjectileLoader.DrawOffset(Projectile, ref num137, ref num136, ref num138);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;

            var rect = new Rectangle(0, frameHeight * Projectile.frame, tex.Width(), frameHeight - 1);

            var origin = new Vector2(num138, Projectile.height / 2 + num136);
            var pos = new Vector2(Projectile.position.X - Main.screenPosition.X + num138 + (float)num137, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);

            Main.EntitySpriteDraw(t, pos, rect, alpha13, Projectile.rotation, origin, Projectile.scale, effects, 0);
            // 从巨鹿Debris里抄来的描边
            float scale23 = Utils.Remap(Projectile.ai[0], 0f, 30f, 1f, 0f);
            Color color84 = Projectile.GetAlpha(Color.White) * scale23 * scale23 * 0.3f;
            color84.A = 0;
            for (int num327 = 0; num327 < 4; num327++) {
                Main.EntitySpriteDraw(t, pos + Projectile.rotation.ToRotationVector2().RotatedBy((float)Math.PI / 2f * (float)num327) * 2f * Projectile.scale, rect, color84, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }
            return true;
        }
    }
}
