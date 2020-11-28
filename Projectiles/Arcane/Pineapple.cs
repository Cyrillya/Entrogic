using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Arcane
{
    public class Pineapple : ArcaneProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void ArcaneDefaults()
        {
            projectile.width = 28;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 210;
            projectile.alpha = 0;
            projectile.light = 1.25f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.HellfireArrow;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 120, false);
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                //Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<MiniRocketExplosion>(), base.projectile.damage, 0f, base.projectile.owner, 0f, 0f);
            }
<<<<<<< HEAD:Projectiles/Arcane/Pineapple.cs
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14, 1f, 0f);
=======
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14, 1f, 0f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Projectiles/Arcane/菠萝.cs
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 20;
            projectile.height = 20;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for (int i = 0; i < 23; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 174, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num].velocity *= 1f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.45f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 20; j++)
            {
                int num3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.25f);
                Main.dust[num3].velocity *= 2f;
                Main.dust[num3].noGravity = true;
            }
            for (int k = 0; k < 3; k++)
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
                num3 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore3 = Main.gore[num3];
                gore3.velocity.X = gore3.velocity.X - 1f;
                Gore gore4 = Main.gore[num3];
                gore4.velocity.Y = gore4.velocity.Y + 1f;
            }
            for (float rad = 0.0f; rad < 2 * 3.141f; rad += (float)Main.rand.Next(8, 14) / 10)
            {
                Vector2 vec = new Vector2(120f, 0f);
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * 16f;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-10, 11), Main.rand.Next(-10, 11), ProjectileType<PineapplePieces>(), 21, 0f, projectile.owner, 0f, 0f);
            }
        }
        public override void AI()
        {
            projectile.velocity.Y += 0.04f;
            if (projectile.timeLeft < 207)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 174, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 0.85f;
                dust.fadeIn = 0.5f;
            }
        }
    }
}






