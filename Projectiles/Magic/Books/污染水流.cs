using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Entrogic.Projectiles.Magic.Books
{
    public class 污染水流 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.damage = 60;
            projectile.tileCollide = false;
            projectile.timeLeft = 90;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.hide = true;
            projectile.extraUpdates = 100;
        }
        public override void AI()
        {
            NPC target = null;
            float distanceMax = 400f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && Collision.CanHit(projectile.Center, 1, 1, npc.position, npc.width, npc.height))
                {
                    float currentDistance = Vector2.Distance(npc.Center, projectile.Center);
                    if (currentDistance < distanceMax)
                    {
                        distanceMax = currentDistance;
                        target = npc;
                    }
                }
            }
            projectile.ai[0]++;
            if (target != null && projectile.ai[0]%3==0)
            {
                Vector2 toTarget = target.Center - projectile.Center;
                toTarget.Normalize();
                toTarget *= 6f;
                toTarget = toTarget.RotatedBy(Main.rand.NextFloatDirection() * 0.3f);
                Projectile.NewProjectile(projectile.Center + projectile.velocity * 5f, toTarget, ProjectileType<污染水滴>(), 25, 0f, projectile.owner, target.whoAmI);
            }
        }
    }
}
