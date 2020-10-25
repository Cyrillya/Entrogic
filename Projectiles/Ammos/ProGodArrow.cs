using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Entrogic.Projectiles.Ammos
{
    public class ProGodArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }
        public override void SetDefaults()
        {
            projectile.alpha = 255;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 1800;
            projectile.ignoreWater = true;
            projectile.damage = 18;
            projectile.knockBack = 3;
            projectile.tileCollide = true;
            aiType = ProjectileID.Bullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
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
        public override void OnHitPvp(Player player, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                player.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Main.myPlayer];
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,
                MyDustId.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
                d.noGravity = true;
            }
        }

        public override void AI()
        {
            if (projectile.timeLeft < 1792)
            {
                projectile.alpha = 35;
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
                d.noGravity = true;
                Dust d2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.PurpleShortFx, 0, 0, 100);
                d2.noGravity = true;
            }
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 220 / 255f, 29 / 255f, 183 / 255f);
        }
    }
}
