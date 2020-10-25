using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Magic.小刀
{
    public class 琥珀小刀 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("琥珀小刀");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 87, 0f, 0f, 90, default(Color), 0.75f);
                Main.dust[num].velocity *= 1.5f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.25f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[num].noGravity = true;
                }
            }
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.DamageType = DamageClass.Magic;
            projectile.timeLeft = 180;
            projectile.alpha = 0;
            projectile.light = 0f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[projectile.type].Width() * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw((Texture2D)Terraria.GameContent.TextureAssets.Projectile[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void AI()
        {
            projectile.rotation += 0.1f;
            projectile.velocity.Y += 0.3f;
            if (projectile.timeLeft < 177)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 15, 0f, 0f, 90, default(Color), 0.25f);
                dust.noGravity = true;
            }
        }
    }

}




