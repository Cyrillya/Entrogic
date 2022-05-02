using static Terraria.ModLoader.ModContent;

namespace Entrogic.Content.Projectiles.ContyElemental.Hostile
{
    public class ContaShark : ProjectileBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Contaminated Shark");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染鲨");

            Main.projFrames[Type] = 4;
            base.SetStaticDefaults();
        }
        public override void SetDefaults() {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.Size = new Vector2(30, 120);
            Projectile.penetrate = 6;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.alpha = 255;
        }
        private int Timer {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        public override void AI() {
            Timer++;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 24) {
                Projectile.frameCounter = 0;
            }
            Projectile.frame = Projectile.frameCounter / 6;
            if (Timer == 1 && Main.netMode != NetmodeID.MultiplayerClient) {
                int proj = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(0f, 1f), ProjectileType<EffectRay>(), 0, 0f, Projectile.owner);
                Main.projectile[proj].ai[0] = 12f;
                Main.projectile[proj].ai[1] = Projectile.whoAmI;
                if (Main.dedServ) {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                }
            }
            Projectile.rotation = MathHelper.ToRadians(270f);
            if (Timer < 10) {
                Projectile.alpha -= 255 / 10 + 1;
                Projectile.alpha = Math.Max(0, Projectile.alpha);
                return;
            }
            if (Timer < 15) return;
            Projectile.velocity.Y += 2.33f;
        }
        public override bool ShouldUpdatePosition() {
            return Timer >= 15;
        }
        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor);
            return false;
        }
    }
}
