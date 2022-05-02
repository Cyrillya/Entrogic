using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Content.Projectiles.Enemies
{
    public class LavaCannonHostile : ProjectileBase
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults() {
            base.SetDefaults();
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.hide = false;
            Projectile.Opacity = 0.75f;
            Projectile.width = 20;
            Projectile.height = 20;
        }

        public override void Kill(int timeLeft) {
            base.Kill(timeLeft);
            // Play explosion sound
            SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            Projectile.Resize(120, 120);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++) {
                int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            // 喷溅的火焰
            for (int g = 0; g < Main.rand.Next(4, 7); g++) {
                var pos = new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f);
                float divisor = Main.masterMode ? 6f : (Main.expertMode ? 4f : 2f);
                int damage = (int)(Projectile.damage / divisor);
                Projectile.NewProjectile(Projectile.GetSource_Death(), pos, new Vector2(Main.rand.NextFloat(-13, 14), Main.rand.NextFloat(-8, -16)), ModContent.ProjectileType<LavaCannonFlame>(), damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void AI() {
            base.AI();
            Projectile.localAI[1]++;
            Projectile.rotation = Projectile.velocity.ToRotation();

            const int frameDelay = 5;
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter % (Main.projFrames[Type] * frameDelay) / frameDelay;
        }

        public override bool PreDraw(ref Color lightColor) {
            DrawOffsetX = -40;
            DrawOriginOffsetY = -20;
            if (Math.Abs(Projectile.rotation) >= 1.57f) {
                DrawOffsetX = -10;
                DrawOriginOffsetY = -20;
            }
            return true;
        }

        public override void PostDraw(Color lightColor) {
            base.PostDraw(lightColor);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            base.ModifyDamageHitbox(ref hitbox);
            hitbox.X -= 60;
            hitbox.Y -= 60;
            hitbox.Width += 60;
            hitbox.Height += 60;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
            base.ModifyHitPlayer(target, ref damage, ref crit);
            Projectile.timeLeft = 0;
        }
    }
}
