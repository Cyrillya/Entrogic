using Entrogic.Content.Items.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Projectiles.Weapons.Melee.Swords.TwoHandSword
{
    internal class TwoHandSword_Proj2 : TwoHandSwordBase
    {
        public override void SetDefaults() {
            base.SetDefaults();
            LocalAI_RotateFactor_MAX = 60;
            AI_CHARGE_MAX = 24;
        }

        public override void Channeling() {
            base.Channeling();

            Player player = Main.player[Projectile.owner];
            player.itemRotation = (new Vector2(0.2f * player.direction, 0.8f)).ToRotation() * player.direction;
            Projectile.rotation = MathHelper.ToRadians(50 + LocalAI_RotateFactor);
        }

        public override void Unchanneling() {
            base.Unchanneling();
            Projectile.rotation -= MathHelper.ToRadians(8f);
        }

        public override void SummonProjectile(out int type, out Vector2 offset, ref int direction, out float damageMult) {
            base.SummonProjectile(out _, out _, ref direction, out _);

            type = ModContent.ProjectileType<SwordCombo2>();
            offset = new Vector2(100 * direction, 0);
            damageMult = MathHelper.Lerp(0.8f, 2f, AI_Charge / AI_CHARGE_MAX);
        }

        public override void ComboController(Player player) {
            base.ComboController(player);
            player.itemRotation = (new Vector2(0.2f * player.direction, -0.8f)).ToRotation() * player.direction;

            player.GetModPlayer<TwoHandSwordPlayer>().ComboMode = 0;
            player.GetModPlayer<TwoHandSwordPlayer>().ComboTimer = 0;
            if (AI_Charge == AI_CHARGE_MAX) {
                player.GetModPlayer<TwoHandSwordPlayer>().ComboMode = 2;
                player.GetModPlayer<TwoHandSwordPlayer>().ComboTimer = 60;
            }
        }
    }
    internal class SwordCombo2 : ProjectileBase
    {
        public override void SetDefaults() {
            Projectile.width = 171;
            Projectile.height = 194;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 15;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
        }

        public override void AI() {
            base.AI();
            if (Projectile.ai[0] == 0) {
                Projectile.ai[0] = 1;
                Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(-50 * Projectile.spriteDirection, -180), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                p.height = 500;
                Projectile p2 = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(20 * Projectile.spriteDirection, -120), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                p2.height = 400;
                Projectile p3 = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(100 * Projectile.spriteDirection, -40), Vector2.Zero, ModContent.ProjectileType<SwordHitbox>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                p3.height = 300;
            }
        }

        public override bool ShouldUpdatePosition() {
            return false;
        }

        private float _frame = 1;
        private Rectangle FindFrame(int height, int width) {
            if (!Main.player[Projectile.owner].channel) {
                _frame += 0.5f;
                if (_frame > 5) _frame = 5;
            }
            return new Rectangle(width * (int)((_frame - 1) % 4), height * (int)((_frame - 1) / 4), width, height);
        }

        public override bool PreDraw(ref Color lightColor) {
            short frameYCount = 2; short frameXCount = 4;
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(tex.Value, Projectile.position - Main.screenPosition, FindFrame(tex.Height() / frameYCount, tex.Width() / frameXCount), Color.White, 0f, Vector2.Zero, new Vector2(Projectile.scale), spriteEffects, 0);

            //ModHelper.DrawBorderedRect(spriteBatch, new Color(0, 0, 0, 0), Color.Red, (Projectile.Hitbox.Location.ToVector2() - Main.screenPosition).Floor(), Projectile.Hitbox.Size(), 2);
            return false;
        }

        public override bool? CanDamage() {
            return false;
        }
    }
}