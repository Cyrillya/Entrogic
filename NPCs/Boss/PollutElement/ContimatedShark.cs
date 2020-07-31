using CalamityMod.Projectiles.Ranged;

using Terraria;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Terraria.ID;
using System.Windows.Forms;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class ContimatedShark : ModProjectile
    {
        public override string Texture { get { return "Entrogic/NPCs/Boss/PollutElement/PolluShark"; } }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.Size = new Vector2(30, 120);
            projectile.penetrate = 6;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 60;
            projectile.alpha = 255;
        }
        private int Timer
        {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public override void AI()
        {
            Timer++;
            if (Timer == 1)
            {
                int proj = Projectile.NewProjectile(projectile.Center, new Vector2(0f, 1f), ProjectileType<EffectRay>(), 0, 0f, projectile.owner);
                Main.projectile[proj].ai[0] = 12f;
                Main.projectile[proj].ai[1] = projectile.whoAmI;
                if (Main.dedServ)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                }
            }
            projectile.rotation = MathHelper.ToRadians(270f);
            if (Timer < 10)
            {
                projectile.alpha -= 255 / 10 + 1;
                projectile.alpha = Math.Max(0, projectile.alpha);
                return;
            }
            if (Timer < 15) return;
            projectile.velocity.Y += 2.33f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 24)
            {
                projectile.frameCounter = 0;
            }
            projectile.frame = projectile.frameCounter / 6;
        }
        public override bool ShouldUpdatePosition()
        {
            return Timer >= 15;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.DrawShadow(spriteBatch, lightColor);
            return false;
        }
    }
}
