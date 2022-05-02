
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Miscellaneous
{
    public class Arcana : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.timeLeft = 60 * 60; // 1min
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override bool PreAI()
        {
            projectile.velocity *= 0.93f;
            projectile.alpha = Math.Max(0, (int)(255 - projectile.ai[1]));
            projectile.ai[1] += 2f;
            Lighting.AddLight(projectile.Center, -0.8f, -0.8f, 1.2f);
            Dust d = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, MyDustId.BlueMagic, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 150, Color.LightBlue, Main.rand.NextFloat(0.8f, 1.3f))];
            d.noLight = true;
            if (projectile.alpha <= 0)
                foreach (Player player in Main.player)
                {
                    if (player.Distance(projectile.Center) > 100)
                    {
                        continue;
                    }
                    Vector2 vectorItemToPlayer = player.Center - projectile.Center;
                    Vector2 movement = -vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.09f;
                    projectile.velocity = projectile.velocity + movement;
                    projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height);
                    if (player.Distance(projectile.Center) < projectile.width / 2f)
                    {
                        Item.NewItem(projectile.Center, ItemType<Items.Materials.拟态魔能>(), Main.rand.Next(1, 5 + 1));
                        projectile.Kill();
                    }
                }
            if (Main.dayTime && projectile.ai[0] != 1)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, MyDustId.BlueMagic, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 150, Color.LightBlue, Main.rand.NextFloat(1f, 1.5f));
                }
                projectile.Kill();
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

            int length = Math.Min(120, 2 + (int)projectile.oldVelocity.Length());

            for (int i = length; i >= 0; i--)
            {
                Vector2 drawPos = projectile.Center - Main.screenPosition - projectile.oldVelocity * i * 1.2f;
                float trailOpacity = projectile.Opacity - 0.05f - 0.95f / length * i * 0.3f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t, drawPos.ToPoint().ToVector2(), new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight), new Color(1f * colMod, 1f * colMod, 1f, 2f) * trailOpacity * 1.2f,
                        projectile.rotation, origin, projectile.scale * (1f - 0.02f * i), effects, 0);
                }
            }
            return false;
        }
    }
}
