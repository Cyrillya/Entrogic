using Entrogic.Content.Items.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords.TwoHandSword
{
    internal class TwoHandSword_Proj3 : TwoHandSwordBase
    {
        public override void SetDefaults() {
            base.SetDefaults();
            LocalAI_RotateFactor_MAX = 20;
            AI_CHARGE_MAX = 20;
        }

        public override void Channeling() {
            base.Channeling();

            Player player = Main.player[Projectile.owner];
            player.direction = -(Main.MouseWorld.X < player.Center.X ? -1 : 1); // 武器特效，玩家背向鼠标
            player.itemRotation = (new Vector2(0.2f * player.direction, -0.2f)).ToRotation() * player.direction;
            Projectile.rotation = MathHelper.ToRadians(LocalAI_RotateFactor - 30);
            Projectile.spriteDirection = player.direction;
        }

        public override void Unchanneling() {
            base.Unchanneling();
            Projectile.rotation += MathHelper.ToRadians(5f);
        }

        public override void SummonProjectile(out int type, out Vector2 offset, ref int direction, out float damageMult) {
            base.SummonProjectile(out _, out _, ref direction, out _);

            Player player = Main.player[Projectile.owner];
            player.direction = -player.direction;
            direction = player.direction;
            type = ModContent.ProjectileType<SwordCombo3>();
            offset = new Vector2(0 * direction, 80);
            damageMult = MathHelper.Lerp(1f, 3f, AI_Charge / AI_CHARGE_MAX);
        }

        public override void ComboController(Player player) {
            base.ComboController(player);
            player.itemRotation = (new Vector2(0.2f * player.direction, 0.8f)).ToRotation() * player.direction;
            player.GetModPlayer<TwoHandSwordPlayer>().ComboMode = 0;
            player.GetModPlayer<TwoHandSwordPlayer>().ComboTimer = 0;
        }
    }
    internal class SwordCombo3 : ProjectileBase
    {
        public override void SetDefaults() {
            Projectile.width = 279;
            Projectile.height = 148;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 16;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
        }

        public override bool ShouldUpdatePosition() {
            return false;
        }

        public override void AI() {
            base.AI();
            if (Projectile.ai[0] == 0) {
                Projectile.ai[0] = 1;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-130 * Projectile.spriteDirection, 0), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-70 * Projectile.spriteDirection, 30), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-40 * Projectile.spriteDirection, 40), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(30 * Projectile.spriteDirection, 60), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(130 * Projectile.spriteDirection, 80), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        private float _frame = 1;
        private Rectangle FindFrame(int height, int width) {
            if (!Main.player[Projectile.owner].channel) {
                _frame += 0.35f;
                if (_frame > 4) _frame = 4;
            }
            return new Rectangle(width * (int)((_frame - 1) % 4), height * (int)((_frame - 1) / 4), width, height);
        }

        public override bool PreDraw(ref Color lightColor) {
            short frameYCount = 1; short frameXCount = 4;
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(tex.Value, Projectile.position - Main.screenPosition, FindFrame(tex.Height() / frameYCount, tex.Width() / frameXCount), Color.White, 0f, Vector2.Zero, Projectile.scale, spriteEffects, 0);

            //ModHelper.DrawBorderedRect(spriteBatch, new Color(0, 0, 0, 0), Color.Red, (Projectile.Hitbox.Location.ToVector2() - Main.screenPosition).Floor(), Projectile.Hitbox.Size(), 2);
            return false;
        }

        public override bool? CanDamage() {
            return false;
        }
    }
}