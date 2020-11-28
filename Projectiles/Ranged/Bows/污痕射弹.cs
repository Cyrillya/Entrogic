using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Ranged.Bows
{
    public class 污痕射弹 : ModProjectile
    {
<<<<<<< HEAD
        public override string Texture => "Entrogic/Assets/Images/Block";
=======
        public override string Texture => "Entrogic/Images/Block";
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.Size = new Vector2(12, 12);
            projectile.penetrate = 10;
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.timeLeft = 450;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, MyDustId.White, projectile.velocity.X, projectile.velocity.Y, 50, new Color(56, 114, 80), 1.5f);
                Main.dust[num].noGravity = true;
                Dust dust = Main.dust[num];
                dust.velocity *= 0.3f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //If collide with tile, reduce the penetrate.
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, projectile.position);
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = (Texture2D)Terraria.GameContent.TextureAssets.Projectile[projectile.type];
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
                if (i != 0) trailOpacity /= 2f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t, drawPos.ToPoint().ToVector2(), new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight), new Color(1f * colMod, 1f * colMod, 1f, 0.5f) * trailOpacity,
                        projectile.rotation, origin, projectile.scale * (1f + 0.02f * i), effects, 0);
                }
            }
            return false;
        }
    }
}
