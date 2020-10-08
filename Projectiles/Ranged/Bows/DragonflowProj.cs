using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Ranged.Bows
{
    public class DragonflowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.alpha = 50;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 3;
            projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() / 255f);
        }
        public override void Kill(int timeLeft)
        {
            Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ProjectileID.InfernoFriendlyBlast, projectile.damage, projectile.knockBack);
            proj.netUpdate = true;
            proj.ranged = true;
            proj.timeLeft = 2;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Main.player[projectile.whoAmI].armorPenetration += 10;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = Main.projectileTexture[projectile.type];
            int frameHeight = t.Height / Main.projFrames[projectile.type];
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            if (projectile.localAI[0] < 0) effects = effects | SpriteEffects.FlipVertically;
            Vector2 origin = new Vector2(t.Width / 2, frameHeight / 2);

            int length = Math.Min(10, 2 + (int)projectile.oldVelocity.Length());

            for (int i = length; i >= 0; i--)
            {
                Vector2 drawPos = projectile.Center - Main.screenPosition - projectile.oldVelocity * i * 0.5f;
                float trailOpacity = projectile.Opacity - 0.05f - 0.95f / length * i;
                if (i != 0) trailOpacity *= 0.8f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t,
                        drawPos.ToPoint().ToVector2(),
                        new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight),
                        //new Color(1f * colMod, 0.7f * colMod, 0.4f, 0.8f) * trailOpacity,
                        new Color(1f, 0.8f, 0.6f, 0.8f) * trailOpacity, // 不受环境影响
                        projectile.rotation,
                        origin,
                        projectile.scale * (1f + 0.02f * i),
                        effects,
                        0);
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.whoAmI].armorPenetration -= 10;
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
