namespace Entrogic.Content.Projectiles.Athanasy.Weapons
{
    public class RockFlailDebris : ProjectileBase
    {
        internal static int VanillaDebrisType { get; private set; }

        public override void Load() {
            VanillaDebrisType = ProjectileID.DeerclopsRangedProjectile;
        }

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 3;
            NPCs.Enemies.Athanasy.Athanasy.DebrisType = Type;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(VanillaDebrisType);
            AIType = VanillaDebrisType;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 150;
            Projectile.localAI[0] = 1; // 去掉粒子
        }

        public override void AI() {
            if (Projectile.timeLeft < 10f) {
                Projectile.Opacity = Projectile.timeLeft / 10f;
            }
        }

        private Asset<Texture2D> cachedTexture;
        private int cachedType;

        public override bool PreDraw(ref Color lightColor) {
            Main.instance.LoadProjectile(VanillaDebrisType);
            // 偷天换日
            cachedTexture = TextureAssets.Projectile[VanillaDebrisType];
            cachedType = Type;
            TextureAssets.Projectile[VanillaDebrisType] = TextureAssets.Projectile[Type];
            Projectile.type = VanillaDebrisType;
            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor) {
            if (cachedType != -1) {
                TextureAssets.Projectile[VanillaDebrisType] = cachedTexture;
                Projectile.type = cachedType;
                cachedType = -1;
            }
        }
    }
}
