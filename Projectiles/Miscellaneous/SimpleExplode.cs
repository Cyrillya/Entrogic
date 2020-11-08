using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;


namespace Entrogic.Projectiles.Miscellaneous
{
    public class SimpleExplode : ModProjectile
    {
        public override string Texture => "Entrogic/Assets/Images/Block";
        public bool useSmoke;
        public int goreTimes;
        public override void SetDefaults()
        {
            projectile.timeLeft = 1;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.light = 1f;
            projectile.aiStyle = -1;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            base.ModifyDamageHitbox(ref hitbox);
            hitbox = projectile.getRect();

            //int gore = Gore.NewGore(hitbox.TopLeft(), Vector2.Zero, Main.rand.Next(61, 64));
            //Main.gore[gore].velocity = Vector2.Zero;
            //gore = Gore.NewGore(hitbox.TopRight(), Vector2.Zero, Main.rand.Next(61, 64));
            //Main.gore[gore].velocity = Vector2.Zero;
            //gore = Gore.NewGore(hitbox.BottomLeft(), Vector2.Zero, Main.rand.Next(61, 64));
            //Main.gore[gore].velocity = Vector2.Zero;
            //gore = Gore.NewGore(hitbox.BottomRight(), Vector2.Zero, Main.rand.Next(61, 64));
            //Main.gore[gore].velocity = Vector2.Zero;
            //gore = Gore.NewGore(hitbox.Center(), Vector2.Zero, Main.rand.Next(61, 64));
            //Main.gore[gore].velocity = Vector2.Zero;

            hitbox.X += 1 * 16;
            hitbox.Y += 1 * 16;
        }
        public override void AI()
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0;
            projectile.rotation = 0;
            Vector2 position = projectile.position;
            Vector2 size = projectile.Size;
            Rectangle hitbox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            if (!useSmoke)
                return;
            for (int num762 = 0; num762 < 20; num762++)
            {
                int num763 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num763];
                dust.velocity *= 1.4f;
            }
            for (int num764 = 0; num764 < 10; num764++)
            {
                int num765 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 6, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num765].noGravity = true;
                Dust dust = Main.dust[num765];
                dust.velocity *= 5f;
                num765 = Dust.NewDust(new Vector2(position.X, position.Y), (int)size.X, (int)size.Y, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust = Main.dust[num765];
                dust.velocity *= 3f;
            }
            size *= 0.7f;
            for (int i = 0; i < goreTimes; i++)
            {
                int num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                Gore gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore138 = Main.gore[num766];
                gore138.velocity.X = gore138.velocity.X + 1f;
                Gore gore139 = Main.gore[num766];
                gore139.velocity.Y = gore139.velocity.Y + 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore140 = Main.gore[num766];
                gore140.velocity.X = gore140.velocity.X - 1f;
                Gore gore141 = Main.gore[num766];
                gore141.velocity.Y = gore141.velocity.Y + 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore142 = Main.gore[num766];
                gore142.velocity.X = gore142.velocity.X + 1f;
                Gore gore143 = Main.gore[num766];
                gore143.velocity.Y = gore143.velocity.Y - 1f;
                num766 = Gore.NewGore(new Vector2(hitbox.Center.X, hitbox.Center.Y) + new Vector2(Main.rand.NextFloat(-size.X, size.X), Main.rand.NextFloat(-size.Y, size.Y)), default(Vector2), Main.rand.Next(61, 64));
                gore = Main.gore[num766];
                gore.velocity *= 0.4f;
                Gore gore144 = Main.gore[num766];
                gore144.velocity.X = gore144.velocity.X - 1f;
                Gore gore145 = Main.gore[num766];
                gore145.velocity.Y = gore145.velocity.Y - 1f;
            }
        }
    }
}