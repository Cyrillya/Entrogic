using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Magic
{
    public class Binary : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.damage = 61;
            projectile.magic = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 260;
            projectile.scale = 1.6f;
            projectile.tileCollide = false;
            projectile.extraUpdates = 2;
            projectile.friendly = true;
            Main.projFrames[projectile.type] = 2;
        }
        public int n;
        public override void AI()
        {
            Player p = Main.player[Main.myPlayer];
            projectile.velocity.X = 0f;
            projectile.velocity.Y = 0f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                    return;
                }
            }
            n++;
            if (n == 4)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.GreenGems, 0f, 0f, 165, default(Color), 1.4f);
                dust.noGravity = true;
                n = 0;
            }
            projectile.alpha = 255 - projectile.timeLeft;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            crit = false;
            int recent = -1;
            for (int i = 99; i >= 0; i--)
            {
                CombatText ctToCheck = Main.combatText[i];
                if (ctToCheck.lifeTime == 60 || ctToCheck.lifeTime == 120)
                {
                    if (ctToCheck.alpha == 1f)
                    {
                        if (ctToCheck.color == CombatText.DamagedHostile || ctToCheck.color == CombatText.DamagedHostileCrit)
                        {
                            recent = i;
                            break;
                        }
                    }
                }
            }
            if (recent != -1)
            {
                string d2 = Convert.ToString(damage, 2);
                CombatText text = Main.combatText[recent];
                text.color = new Color(30, 215, 90);
                text.text = d2;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.position - Main.screenPosition, new Rectangle(0, tex.Height / Main.projFrames[projectile.type] * projectile.frame, tex.Width, tex.Height / Main.projFrames[projectile.type]),
                projectile.GetAlpha(lightColor), projectile.rotation, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
