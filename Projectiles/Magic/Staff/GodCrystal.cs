using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Entrogic.Projectiles.Magic.Staff
{
    public class GodCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = 3;
            projectile.scale = 1f;
            projectile.alpha = 60;
            projectile.timeLeft = 600;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
            d.noGravity = true;
            Dust d2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.PurpleShortFx, 0, 0, 100);
            d2.noGravity = true;
            if (projectile.ai[0] == 0f)
            {
                Effect();
                projectile.ai[0] = 1f;
            }
            projectile.alpha += 3;
            if (projectile.timeLeft <= 20) projectile.alpha += 6;
            if (projectile.timeLeft > 20 && (projectile.alpha >= 150 || projectile.alpha < 60)) projectile.alpha = 60;
            if (projectile.alpha >= 255) projectile.Kill();
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 220 / 255f, 29 / 255f, 183 / 255f);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
        }
        public override void Kill(int timeLeft)
        {
            Effect();
        }
        public void Effect()
        {
            float num = 16f;
            int num2 = 0;
            while ((float)num2 < num)
            {
                int num3 = MyDustId.TransparentPurple;
                if (num2 > 8) num3 = MyDustId.PurpleShortFx;
                Color color = default(Color);
                if (num2 <= 8) color = Color.Pink;
                Vector2 vector12 = Vector2.UnitX * 0f;
                vector12 += -Vector2.UnitY.RotatedBy((double)((float)num2 * (6.28318548f / num)), default(Vector2)) * new Vector2(1f, 4f);
                vector12 = vector12.RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
                int num5 = Dust.NewDust(projectile.Center, 0, 0, num3, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num5].scale = 1.5f;
                Main.dust[num5].noGravity = true;
                Main.dust[num5].position = projectile.Center + vector12;
                Main.dust[num5].velocity = projectile.velocity * 0f + vector12.SafeNormalize(Vector2.UnitY) * 1f;
                int num4 = num2;
                num2 = num4 + 1;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                float sx = 0;
                float sy = 0;
                if (-oldVelocity.X >= 0) sx = -1;
                else sx = 1;
                if (-oldVelocity.Y >= 0) sy = -1;
                else sy = 1;
                sx = sx * 10f;
                sy = sy * 10f;
                int j = Projectile.NewProjectile(projectile.position.X + sx, projectile.position.Y + sy, -oldVelocity.X, -oldVelocity.Y, projectile.type, projectile.damage, projectile.knockBack);
                Main.projectile[j].penetrate = projectile.penetrate;
                projectile.position -= new Vector2(sx,sy);

                Main.PlaySound(SoundID.Item10, projectile.position);
            }
            return false;
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
    public class FC : ModProjectile
    {
        public override string Texture => "Entrogic/Images/Block";
        public override void SetDefaults()
        {
            projectile.Size = new Vector2(64);
            projectile.tileCollide = false;
            projectile.penetrate = projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.hide = projectile.friendly = true;
        }
        public override bool CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            for (int i = 0; i < Main.rand.Next(5, 11); i++)
            {
                Vector2 r = new Vector2(Main.rand.Next(580, 800), Main.rand.Next(580, 800));
                r = r - new Vector2(Main.rand.Next(570), Main.rand.Next(570));
                if (Main.rand.NextBool(2)) r.X = -r.X;
                if (Main.rand.NextBool(2)) r.Y = -r.Y;
                Vector2 v = new Vector2(12f, 12f);
                Vector2 vec = projectile.Center + r - projectile.Center;
                Vector2 finalVec = (vec.ToRotation() + MathHelper.Pi).ToRotationVector2() * v;
                int ci =Projectile.NewProjectile(projectile.Center + r, finalVec, ProjectileType<NFC>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                Projectile c = Main.projectile[ci];
                c.localAI[0] = projectile.position.X;
                c.localAI[1] = projectile.position.Y;
            }
            projectile.Kill();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
    public class NFC : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Projectiles/Magic/Staff/GodCrystal"; } }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileType<GodCrystal>());
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            Vector2 OLU = new Vector2(projectile.localAI[0], projectile.localAI[1]);
            Vector2 ORD = OLU + new Vector2(64);
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.TransparentPurple, 0, 0, 100, Color.Pink, 1f);
            d.noGravity = true;
            Dust d2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.PurpleShortFx, 0, 0, 100);
            d2.noGravity = true;
            if (projectile.ai[0] == 0f)
            {
                Effect();
                projectile.ai[0] = 1f;
            }
            projectile.alpha = 64;
            Lighting.AddLight((int)((projectile.position.X + (projectile.width / 2)) / 16f), (int)((projectile.position.Y + (projectile.height / 2)) / 16f), 220 / 255f, 29 / 255f, 183 / 255f);
            if (projectile.Center.X >= OLU.X && projectile.Center.Y >= OLU.Y &&
                projectile.Center.X <= ORD.X && projectile.Center.Y <= ORD.Y) projectile.ai[0] = 2f;
            if (projectile.ai[0] == 2f)
            {
                projectile.ai[1] = 60f;
                projectile.ai[0] = 3f;
            }
            else if (projectile.ai[0] == 3f)
            {
                projectile.ai[1] += 7f;
                projectile.alpha = (int)projectile.ai[1];
                if (projectile.ai[1] >= 255f) projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
        }
        public void Effect()
        {
            float num = 16f;
            int num2 = 0;
            while ((float)num2 < num)
            {
                int num3 = MyDustId.TransparentPurple;
                if (num2 > 8) num3 = MyDustId.PurpleShortFx;
                Color color = default(Color);
                if (num2 <= 8) color = Color.Pink;
                Vector2 vector12 = Vector2.UnitX * 0f;
                vector12 += -Vector2.UnitY.RotatedBy((double)((float)num2 * (6.28318548f / num)), default(Vector2)) * new Vector2(1f, 4f);
                vector12 = vector12.RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
                int num104 = Dust.NewDust(projectile.Center, 0, 0, num3, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num104].scale = 1.5f;
                Main.dust[num104].noGravity = true;
                Main.dust[num104].position = projectile.Center + vector12;
                Main.dust[num104].velocity = projectile.velocity * 0f + vector12.SafeNormalize(Vector2.UnitY) * 1f;
                int num4 = num2;
                num2 = num4 + 1;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
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
