using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class Arsonists : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.alpha = 50;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() / 255f);
            projectile.rotation += 0.2f;
            if (projectile.timeLeft < 297)
            {
                // 火焰粒子特效
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height
                    , 174, 0f, 0f, 90, default(Color), 2f);
                // 粒子特效不受重力
                dust.noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            projectile.ProjectileExplode();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.DrawShadow(spriteBatch, lightColor);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }

}




