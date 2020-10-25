using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Arcane
{
    public class PineapplePieces : ArcaneProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("菠萝碎片");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void ArcaneDefaults()
        {
            projectile.width = 12;
            projectile.height = 10;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 70;
            projectile.alpha = 0;
            projectile.light = 1.25f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180, false);
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                //Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<MiniRocketExplosion>(), base.projectile.damage, 0f, base.projectile.owner, 0f, 0f);
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14, 1f, 0f);
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 15;
            projectile.height = 15;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int i = 0; i < 6; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 174, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num].velocity *= 1f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.35f;
                }
            }
            for (int j = 0; j < 5; j++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 0.85f);
                Main.dust[num].velocity *= 1f;
                Main.dust[num].noGravity = true;
            }
            for (int k = 0; k < 2; k++)
            {
                float scaleFactor = 0.33f;
                if (k == 1)
                {
                    scaleFactor = 0.66f;
                }
                if (k == 2)
                {
                    scaleFactor = 1f;
                }
                int num3 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore = Main.gore[num3];
                gore.velocity.X = gore.velocity.X + 1f;
                Gore gore2 = Main.gore[num3];
                gore2.velocity.Y = gore2.velocity.Y + 1f;
            }
        }
        public override void AI()
        {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 174, 0f, 0f, 100, default(Color), 0.6f);
                dust.velocity *= 1f;
                dust.fadeIn = 0.25f;
        }
    }
}




 