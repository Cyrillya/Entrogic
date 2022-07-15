namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class CuteBlade : BladeProjectile
    {
        public override void SelectTrailTextures(out Texture2D mainColor, out Texture2D mainShape, out Texture2D maskColor) {
            base.SelectTrailTextures(out _, out mainShape, out maskColor);
            mainColor = TextureManager.Cyromap2.Value;
            // 以rgb中b值来检测
            EffectManager.BladeTrail.Value.Parameters["uDetectMode"].SetValue(2);
        }

        public override void SetDefaults() {
            TrailColor = Color.AliceBlue;
            ReadyDegree = 4;
            EndDegree = 4;
            StartDegree = -90;
            FinalDegree = 70;
            ReadyTimePercent = 0.15f;
            FinalTimePercent = 0.15f;
            base.SetDefaults();
        }

        public override void GetBladeDrawStats(ref Color lightColor, out Color bladeColor, out BlendState blendState) {
            base.GetBladeDrawStats(ref lightColor, out _, out blendState);
            bladeColor = Color.AliceBlue * 0.8f;
        }
    }
}
