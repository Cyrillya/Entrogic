using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Entrogic.NPCs.Boss.AntaGolem.Projectiles
{
    public class 石子传送门 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }
        int maxTimeLeft = 0;
        public override void SetDefaults()
        {
            projectile.Size = new Vector2(128, 128);
            projectile.aiStyle = -1;
            projectile.scale = 0f;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 900;
            projectile.tileCollide = false;
        }
        float rad = -MathHelper.PiOver2;
        int Timer;
        public override void AI()
        {
            if (maxTimeLeft == 0)
                maxTimeLeft = projectile.timeLeft;
            if (projectile.timeLeft > (maxTimeLeft - 20))
            {
                projectile.scale += 0.05f;
            }
            if (projectile.timeLeft <= 20)
            {
                projectile.scale -= 0.05f;
                if (projectile.scale <= 0f)
                {
                    projectile.Kill();
                }
            }
            projectile.Size = new Vector2(128, 128) * projectile.scale;
            float rotation = MathHelper.TwoPi / 90;
            rad += rotation;
            if (rad > MathHelper.TwoPi)
                projectile.timeLeft = 20;
            NPC owner = Main.npc[(int)projectile.ai[0]];
            if (owner.active && owner.type == NPCType<Antanasy>())
            {
                Vector2 vec = new Vector2(0f, -0.001f);
                vec = Vector2.Normalize(vec);
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * 128f;
                projectile.Center = owner.Center + finalVec;
                Vector2 Center = projectile.Center;
                Timer++;
                if (Timer%20==0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    vec = (Main.player[owner.target].Center - Center).ToRotation().ToRotationVector2() * 20;
                    int proj = Projectile.NewProjectile(Center, vec, ProjectileType<魔像飞弹>(), (int)projectile.ai[1], 0f, Main.myPlayer);
                    Main.projectile[proj].scale = 1.5f;
                    projectile.netUpdate = true;
                }
            }
            else
                projectile.Kill();

            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}
