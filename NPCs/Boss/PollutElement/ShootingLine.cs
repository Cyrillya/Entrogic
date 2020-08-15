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
    public class ShootingLine : ModProjectile
    {
        private List<float> rotates = new List<float>();
        public override string Texture => "Entrogic/Images/Block";
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.damage = 0;
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 50;
        }
        public override void AI()
        {
            // 污染之灵
            NPC elemental = Main.npc[(int)projectile.ai[1]];
            // 保险
            if (elemental == null || elemental.type != NPCType<PollutionElemental>() || elemental.active == false)
            {
                projectile.active = false;
                projectile.Kill();
                projectile.netUpdate = true;
                return;
            }
            Player player = Main.player[elemental.target];
            projectile.Center = elemental.Center;
            projectile.ai[0]++;
            if (projectile.ai[0] == 15 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // 射击
                foreach (var rotation in rotates)
                {
                    Vector2 vec = rotation.ToRotationVector2();
                    int bullet = Projectile.NewProjectile(projectile.Center, vec * 0.9f, ProjectileType<ContimatedSpike>(), 40, 2f);
                    if (Main.dedServ)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, bullet);
                    }
                }
                projectile.active = false;
                projectile.Kill();
            }
            if (projectile.ai[0] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // 选取
                float startRadius = ModHelper.GetFromToRadians(projectile.Center, player.Center);
                float endRadius = ModHelper.GetFromToRadians(projectile.Center, player.Center) + MathHelper.TwoPi;
                for (float rotation = startRadius; rotation < endRadius; rotation += MathHelper.TwoPi / 12f)
                {
                    rotates.Add(rotation);

                    // 特效
                    Vector2 vec = rotation.ToRotationVector2();
                    int proj = Projectile.NewProjectile(projectile.Center, vec, ProjectileType<EffectRay>(), 0, 0f, projectile.owner);
                    Main.projectile[proj].ai[1] = projectile.whoAmI;
                    if (Main.dedServ)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                    }
                }
            }
        }
    }
    public class ContimatedSpike : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Projectiles/污染尖刺"; } }
        public override void SetDefaults()
        {
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.Size = new Vector2(12, 12);
            projectile.penetrate = 6;
            projectile.aiStyle = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 450;
            projectile.scale = 1.6f;
        }
        public override void AI()
        {
            projectile.velocity *= 1.3f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.DrawShadow(spriteBatch, lightColor, 6);
            return false;
        }
    }
}
