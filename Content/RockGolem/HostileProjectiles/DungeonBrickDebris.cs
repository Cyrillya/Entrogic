using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.RockGolem.HostileProjectiles
{
    public class DungeonBrickDebris : ProjectileBase
    {
        internal static int VanillaDebrisType { get; private set; }

        public override void Load() {
            VanillaDebrisType = ProjectileID.DeerclopsRangedProjectile;
        }

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 3;
            RockGolem.Enemies.Athanasy.DebrisType = Type;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(VanillaDebrisType);
            AIType = VanillaDebrisType;
            Projectile.scale = 1.2f;
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
