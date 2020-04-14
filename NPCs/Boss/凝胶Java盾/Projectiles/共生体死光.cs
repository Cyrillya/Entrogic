using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.Enums;


namespace Entrogic.NPCs.Boss.凝胶Java盾.Projectiles
{
    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class 共生体死光 : ModProjectile
    {
        public Color gelcolor = new Color(255, 98, 71, 137);
        // The maximum charge value
        private const float MaxChargeValue = 50f;
        //The distance charge particle from the player center
        private const float MoveDistance = 60f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.alpha = 100;
            projectile.scale = 0.5f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)),
                projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MoveDistance);
            DrawLaserOut(spriteBatch, mod.GetTexture("NPCs/Boss/凝胶Java盾/Projectiles/激光外层"), (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)),
                projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, new Color(50, 50, 50, 50), (int)MoveDistance);
            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 36, 46, 34), i < transDist ? Color.Transparent : c, r,
                    new Vector2(46 * .5f, 34 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 46, 32), Color.White, r, new Vector2(46 * .5f, 32 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 70, 46, 28), Color.White, r, new Vector2(46 * .5f, 28 * .5f), scale, 0, 0);
            #endregion
        }
        public void DrawLaserOut(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = color;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 36, 46, 33), i < transDist ? color : c, r,
                    new Vector2(46 * .5f, 33 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 46, 32), color, r, new Vector2(46 * .5f, 32 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 69, 46, 29), color, r, new Vector2(46 * .5f, 29 * .5f), scale, 0, 0);
            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)),
                (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)) + unit * Distance, 22, ref point);
        }

        // The AI of the projectile
        public override void AI()
        {
            //Vector2 rotatPos = new Vector2(Main.npc[(int)projectile.ai[1]].localAI[0], Main.npc[(int)projectile.ai[1]].localAI[1]);

            #region Set projectile position
            //Vector2 diff = rotatPos - (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f));
            Vector2 diff = new Vector2(-Main.npc[(int)projectile.ai[1]].localAI[0], Main.npc[(int)projectile.ai[1]].localAI[1]);
            projectile.velocity = diff;
            projectile.direction = Main.npc[(int)projectile.ai[1]].localAI[0] > Main.npc[(int)projectile.ai[1]].position.X ? 1 : -1;
            projectile.netUpdate = true;
            projectile.position = (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)) + projectile.velocity * MoveDistance;
            projectile.timeLeft = 150;
            int dir = projectile.direction;
            #endregion

            #region Charging process
            projectile.localAI[1]++;
            if (projectile.localAI[1] >= 180 || !Main.npc[(int)projectile.ai[1]].active)
                projectile.Kill();
            else
            {
                Vector2 offset = projectile.velocity;
                offset *= MoveDistance - 20;
                Vector2 pos = (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)) + offset - new Vector2(10, 10);
                Vector2 dustVelocity = Vector2.UnitX * 18f;
                dustVelocity = dustVelocity.RotatedBy(projectile.rotation - 1.57f, default(Vector2));
                Vector2 spawnPos = projectile.Center + dustVelocity;
                for (int k = 0; k < 7; k++)
                {
                    Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - (7 * 2));
                    Dust dust = Main.dust[Dust.NewDust(pos, 40, 40, MyDustId.GreyPebble, projectile.velocity.X / 2f,
                        projectile.velocity.Y / 2f, 0, gelcolor, 1f)];
                    dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - 7 * 2f) / 10f;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(10, 20) * 0.05f;
                }
            }
            #endregion

            #region Set laser tail position and dusts
            Vector2 start = (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f));
            Vector2 unit = projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)) + projectile.velocity * Distance;
                if (!Collision.CanHit((Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 100f)), 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
            }

            Vector2 dustPos = (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 80f)) + projectile.velocity * Distance;
            //Imported dust code from source because I'm lazy
            for (int i = 0; i < 2; ++i)
            {
                float num1 = projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 20, 20, MyDustId.GreyPebble, dustVel.X, dustVel.Y, 0, gelcolor, 1f)];
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.noLight = false;
                dust = Dust.NewDustDirect((Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 80f)), 30, 30, MyDustId.GreyPebble,
                    -unit.X * Distance, -unit.Y * Distance);
                dust.color = gelcolor;
                dust.fadeIn = 0f;
                dust.noLight = false;
                dust.noGravity = true;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 offset = projectile.velocity.RotatedBy(1.57f, new Vector2()) * ((float)Main.rand.NextDouble() - 0.5f) *
                                 projectile.width;
                Dust dust = Main.dust[
                    Dust.NewDust(dustPos + offset - Vector2.One * 4f, 38, 38, MyDustId.GreyPebble, 0.0f, 0.0f, 30, gelcolor, 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                dust.noGravity = true;

                unit = dustPos - (Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 80f));
                unit.Normalize();
                dust = Main.dust[
                    Dust.NewDust((Main.npc[(int)projectile.ai[1]].Center + new Vector2(0f, 80f)) + 55 * unit, 38, 38, MyDustId.GreyPebble, 0.0f, 0.0f, 30, gelcolor, 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                dust.noGravity = true;
                dust.noLight = false;
            }
            #endregion

            //Add lights
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<Buffs.Enemies.溶解>(), Main.rand.Next(90, 151) * (Main.expertMode ? (int)Main.expertDebuffTime : 1));
        }
    }
}
