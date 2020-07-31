using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles
{
    public class 机械导弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("机械导弹");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 62;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            int num = (int)Player.FindClosest(projectile.Center, 1, 1);
            float num2 = projectile.Distance(Main.player[num].Center);
            if (num2 > 48f && projectile.timeLeft > 120)
            {
                float scaleFactor = projectile.velocity.Length();
                Vector2 vector = Main.player[num].Center - projectile.Center;
                vector.Normalize();
                vector *= scaleFactor;
                projectile.velocity = (projectile.velocity * 24f + vector) / 25f;
                projectile.velocity.Normalize();
                projectile.velocity *= scaleFactor;
            }
            else if (num2 <= 48f)
            {
                projectile.Kill();
            }
            for (int num6 = 0; num6 < 2; num6++)
            {
                float num7 = 0f;
                float num8 = 0f;
                if (num6 == 1)
                {
                    num7 = projectile.velocity.X * 0.5f;
                    num8 = projectile.velocity.Y * 0.5f;
                }
                int num9 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num7, projectile.position.Y + 3f + num8) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 6, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num9].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                Main.dust[num9].velocity *= 0.2f;
                Main.dust[num9].noGravity = true;
                num9 = Dust.NewDust(new Vector2(projectile.position.X + 3f + num7, projectile.position.Y + 3f + num8) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, 31, 0f, 0f, 100, default(Color), 0.5f);
                Main.dust[num9].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[num9].velocity *= 0.05f;
            }
        }

        public override void Kill(int timeLeft)
        {
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 128;
            projectile.height = 128;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
            for (int num5 = 0; num5 < 30; num5++)
            {
                int num6 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num6].velocity *= 1.4f;
            }
            for (int num7 = 0; num7 < 20; num7++)
            {
                int num8 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3.5f);
                Main.dust[num8].noGravity = true;
                Main.dust[num8].velocity *= 7f;
                num8 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num8].velocity *= 3f;
            }
            for (int num9 = 0; num9 < 2; num9++)
            {
                float scaleFactor9 = 0.4f;
                if (num9 == 1)
                {
                    scaleFactor9 = 0.8f;
                }
                int num10 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num10].velocity *= scaleFactor9;
                Gore expr_1347F_cp_0 = Main.gore[num10];
                expr_1347F_cp_0.velocity.X = expr_1347F_cp_0.velocity.X + 1f;
                Gore expr_1349F_cp_0 = Main.gore[num10];
                expr_1349F_cp_0.velocity.Y = expr_1349F_cp_0.velocity.Y + 1f;
                num10 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num10].velocity *= scaleFactor9;
                Gore expr_13522_cp_0 = Main.gore[num10];
                expr_13522_cp_0.velocity.X = expr_13522_cp_0.velocity.X - 1f;
                Gore expr_13542_cp_0 = Main.gore[num10];
                expr_13542_cp_0.velocity.Y = expr_13542_cp_0.velocity.Y + 1f;
                num10 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num10].velocity *= scaleFactor9;
                Gore expr_135C5_cp_0 = Main.gore[num10];
                expr_135C5_cp_0.velocity.X = expr_135C5_cp_0.velocity.X + 1f;
                Gore expr_135E5_cp_0 = Main.gore[num10];
                expr_135E5_cp_0.velocity.Y = expr_135E5_cp_0.velocity.Y - 1f;
                num10 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num10].velocity *= scaleFactor9;
                Gore expr_13668_cp_0 = Main.gore[num10];
                expr_13668_cp_0.velocity.X = expr_13668_cp_0.velocity.X - 1f;
                Gore expr_13688_cp_0 = Main.gore[num10];
                expr_13688_cp_0.velocity.Y = expr_13688_cp_0.velocity.Y - 1f;
            }
        }
    }
}