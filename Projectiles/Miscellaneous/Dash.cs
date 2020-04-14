using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Miscellaneous
{
    public class Dash : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 20;
            projectile.height = 42;
            projectile.penetrate = -1;
        }
        
        public float UpdateCount
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }
        
        public float DashCount
        {
            get
            {
                return projectile.ai[0] - 20f;
            }
        }
        
        public float DashEffect
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }
        public bool timerOn = false;
        float num2 = 0;
        float num3 = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!timerOn)
            {
                num3 = projectile.velocity.X;
                if (num3 < 0) num3 *= -1;
                num2 = num3 / 2f;
                timerOn = true;
            }
            if (player.dead || !player.active)
            {
                projectile.timeLeft = 0;
                return;
            }
            if (this.UpdateCount == 0f)
            {
                int num = 0;
                while ((float)num < 120f)
                {
                    Vector2 value = Collision.TileCollision(projectile.position, projectile.velocity / 2f, projectile.width, projectile.height, true, true, (int)player.gravDir);
                    if (value == Vector2.Zero)
                    {
                        break;
                    }
                    projectile.position += value / 2f;
                    num++;
                }
                this.dashStep = (projectile.Center - player.Center) / 15f;
                projectile.velocity = Vector2.Zero;
            }
            if (this.UpdateCount >= 0f)
            {
                if (this.UpdateCount == 0f)
                {
                    this.dashStep = (projectile.Center - player.Center) / 15f;
                }
                player.position += Collision.TileCollision(player.position, this.dashStep / 2f, player.width, player.height, true, true, (int)player.gravDir);
                player.velocity = Collision.TileCollision(player.position, this.dashStep * 0.8f, player.width, player.height, true, true, (int)player.gravDir);
                player.immune = true;
                player.immuneTime = Math.Max(player.immuneTime, 2);
                player.immuneNoBlink = true;
                player.fallStart = (int)(player.position.Y / 16f);
                player.fallStart2 = player.fallStart;
                if (this.dashStep.X > 0f)
                {
                    player.direction = 1;
                }
                if (this.dashStep.X < 0f)
                {
                    player.direction = -1;
                }
                if (this.UpdateCount >= num2)
                {
                    projectile.timeLeft = 0;
                }
            }
            else
            {
                player.velocity *= 0.99f;
            }
            UpdateCount += 1f;
            if (DashEffect == 1)
            {
                if (UpdateCount % 2f == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float speed = i * 10 - 5;
                        Projectile.NewProjectile(player.position, new Vector2(0f, speed * 8f), mod.ProjectileType("GodBeamFri"), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }
            }
        }
        
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.velocity = this.dashStep / 15f;
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        
        public const float dashStepCount = 15f;
        
        public const float dashStepDelay = 0f;
        
        public Vector2 dashStep;
    }
}
