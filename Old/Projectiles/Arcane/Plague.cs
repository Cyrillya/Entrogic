using Entrogic.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Arcane
{
    public class Plague : ArcaneProjectile
    {
        public override string Texture => "Entrogic/Projectiles/Arcane/ArcaneMissle";
        public const int maxTimeLeft = 240;
        public override void ArcaneDefaults()
        {
            projectile.width = 28;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = maxTimeLeft;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Buffs.Weapons.Plague>(), Main.rand.Next(3, 9) * 60, false);
            target.GetGlobalNPC<EntrogicNPC>().buffOwner = projectile.owner;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<Buffs.Weapons.Plague>(), Main.rand.Next(3, 9) * 60, false);
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, DustType<PlagueFog>(), 0f, 0f, 0, default);
                        Main.dust[dust].velocity += projectile.velocity * 0.25f;
                    }
                }
            }
        }
        public override void AI()
        {
            if (projectile.timeLeft < maxTimeLeft - 5 && Main.rand.NextBool(4))
            {
                for (int k = 0; k < 2; k++)
                {
                    int dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, DustType<PlagueFog>(), 0f, 0f, 0, default);
                    Main.dust[dust].velocity += projectile.velocity * 0.25f;
                }
            }
        }
    }
}
