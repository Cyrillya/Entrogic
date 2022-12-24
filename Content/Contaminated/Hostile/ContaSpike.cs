using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Hostile
{
    public class ContaSpike : ProjectileBase
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Contaminated Spike");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染尖刺");
        }


        public override void Load() {
            base.Load();
            On.Terraria.Main.DrawProj += Main_DrawProj;
        }

        private void Main_DrawProj(On.Terraria.Main.orig_DrawProj orig, Main self, int i) {
            if (Main.projectile[i].type == Type || Main.projectile[i].type == ModContent.ProjectileType<ContamintedSpike>()) {

                int type = Main.projectile[i].type;
                Main.projectile[i].type = ProjectileID.QueenBeeStinger;
                var ProjectileTexture = TextureAssets.Projectile[ProjectileID.QueenBeeStinger];
                TextureAssets.Projectile[ProjectileID.QueenBeeStinger] = TextureAssets.Projectile[type];

                orig(self, i);

                Main.projectile[i].type = type;
                TextureAssets.Projectile[ProjectileID.QueenBeeStinger] = ProjectileTexture;

                return;
            }
            orig(self, i);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.QueenBeeStinger);
            Projectile.aiStyle = -1;
            //aiType = ProjectileID.QueenBeeStinger;

            //Projectile.hostile = true;
            //Projectile.friendly = false;
            //Projectile.Size = new Vector2(12, 12);
            //Projectile.penetrate = 6;
            //Projectile.aiStyle = -1;
            //Projectile.ignoreWater = false;
            //Projectile.tileCollide = false;
            //Projectile.timeLeft = 300;
            //Projectile.scale = 1.3f;
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Color dustColor = new(120, 120, 120);
            if (Main.rand.Next(2) == 0)
                Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, 0f, 0f, 0, dustColor, Projectile.scale).noGravity = true;

            //if (Projectile.localAI[0] == 0f) {
            //    Projectile.localAI[0] = 1f;
            //    for (int num96 = 0; num96 < 20; num96++) {
            //        Dust dust3 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 56, 0f, 0f, 0, default(Color), 1.3f);
            //        dust3.noGravity = true;
            //        dust3.velocity += Projectile.velocity * 0.75f;
            //    }

            //    for (int num97 = 0; num97 < 10; num97++) {
            //        Dust dust4 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 56, 0f, 0f, 0, default(Color), 1.3f);
            //        dust4.noGravity = true;
            //        dust4.velocity *= 2f;
            //    }
            //}
        }
    }
}
