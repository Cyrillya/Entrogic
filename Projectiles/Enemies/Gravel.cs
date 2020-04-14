using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Enemies
{
    public class Gravel : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.alpha = 32;
            projectile.width = 6;
            projectile.height = 6;
            projectile.aiStyle = 1;
            projectile.scale *= 1.25f;
            projectile.hostile = true;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (projectile.alpha == 0 && Main.rand.Next(3) == 0)
            {
                int num70 = Dust.NewDust(projectile.position - projectile.velocity * 3f, projectile.width, projectile.height, 1, 0f, 0f, 50);
                Dust dust = Main.dust[num70];
                dust.scale *= 1.2f;
                dust.velocity *= 0.3f;
                dust = Main.dust[num70];
                dust.velocity += projectile.velocity * 0.3f;
                Main.dust[num70].noGravity = true;
            }
            projectile.alpha -= 50;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                Main.PlaySound(SoundID.Item17, projectile.position);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 1, 0, 0, 0);
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(8) == 0)
            {
                target.AddBuff(BuffID.Stoned, 90, true);
            }
        }
    }
}
