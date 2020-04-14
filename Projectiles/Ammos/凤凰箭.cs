using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Ammos
{
    public class 凤凰箭 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("凤凰箭");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 28;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 0;
            projectile.light = 1.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.HellfireArrow;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 300, false);
        }
        public override void Kill(int timeLeft)
        {
            if (base.projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, 0f, 0f, base.mod.ProjectileType("MiniRocketExplosion"), base.projectile.damage, 0f, base.projectile.owner, 0f, 0f);
            }
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 14, 1f, 0f);
            base.projectile.position.X = base.projectile.position.X + (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y + (float)(base.projectile.height / 2);
            base.projectile.width = 20;
            base.projectile.height = 20;
            base.projectile.position.X = base.projectile.position.X - (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y - (float)(base.projectile.height / 2);
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num].velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 30; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 5f;
                num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num2].velocity *= 2f;
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
                int num3 = Gore.NewGore(new Vector2(base.projectile.position.X + (float)(base.projectile.width / 2) - 24f, base.projectile.position.Y + (float)(base.projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore = Main.gore[num3];
                gore.velocity.X = gore.velocity.X + 1f;
                Gore gore2 = Main.gore[num3];
                gore2.velocity.Y = gore2.velocity.Y + 1f;
                num3 = Gore.NewGore(new Vector2(base.projectile.position.X + (float)(base.projectile.width / 2) - 24f, base.projectile.position.Y + (float)(base.projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore3 = Main.gore[num3];
                gore3.velocity.X = gore3.velocity.X - 1f;
                Gore gore4 = Main.gore[num3];
                gore4.velocity.Y = gore4.velocity.Y + 1f;
                num3 = Gore.NewGore(new Vector2(base.projectile.position.X + (float)(base.projectile.width / 2) - 24f, base.projectile.position.Y + (float)(base.projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore5 = Main.gore[num3];
                gore5.velocity.X = gore5.velocity.X + 1f;
                Gore gore6 = Main.gore[num3];
                gore6.velocity.Y = gore6.velocity.Y - 1f;
                num3 = Gore.NewGore(new Vector2(base.projectile.position.X + (float)(base.projectile.width / 2) - 24f, base.projectile.position.Y + (float)(base.projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num3].velocity *= scaleFactor;
                Gore gore7 = Main.gore[num3];
                gore7.velocity.X = gore7.velocity.X - 1f;
                Gore gore8 = Main.gore[num3];
                gore8.velocity.Y = gore8.velocity.Y - 1f;
            }
        }
        public override void AI()
        {
            if (projectile.timeLeft < 597)
            {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height
                    , 6, 0f, 0f, 100, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
    }

}




