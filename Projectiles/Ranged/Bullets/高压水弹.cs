using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Ranged.Bullets
{
    public class 高压水弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("高压水弹");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.velocity *= 15f;
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 100;
            projectile.alpha = 4;
            projectile.light = 0.25f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.Bullet;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(103, 240, false);
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.timeLeft < 91)
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 101, 0f, 0f, 50, default(Color), 0.85f);
                    dust.noGravity = true;
                    dust.scale = (float)Main.rand.Next(90, 110) * 0.014f;
                    dust.position = projectile.Center - projectile.velocity * i / 3f;
                    dust.velocity *= 0.45f;
                    dust.fadeIn = 0.1f;
                    projectile.velocity.Y += 0.05f;
                }
        }
    }
}




