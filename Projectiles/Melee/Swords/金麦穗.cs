using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Items.Weapons.Melee.Sword;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class 金麦穗 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 168;
            projectile.height = 84;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.scale = 1f;
            projectile.alpha = 35;
        }
        public int num = 0;
        public override bool? CanHitNPC(NPC target)
        {
            return (num != 1 || num != 0 || num != 7);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (crit && target != null)
            { target.GetGlobalNPC<MoreMoney>().moneyPoint50++; }
        }
        public override void AI()
        {
            projectile.alpha = 64;
            Player player = Main.player[projectile.owner];

            Main.projFrames[projectile.type] = 7;
            projectile.ai[0]++;
            if (projectile.ai[0] == 3)
            {
                num = projectile.frame + 1;
                projectile.frame = num;
                projectile.ai[0] = 0;
            }
            if (num == 8)
            {
                projectile.Kill();
            }
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 21;
            }
            Vector2 vector21 = projectile.Center + projectile.velocity * 3f;
            projectile.velocity.X = (player.direction == -1) ? -0.1f : 0.1f;
            projectile.position.X = ((player.direction == -1) ? player.position.X - (projectile.width / 2) : player.position.X + (projectile.width / 2)) - 64;
            projectile.position.Y = player.Center.Y - 36f;
            projectile.rotation = 0f;
            projectile.spriteDirection = projectile.direction;
            player.itemTime = 3;
            player.itemAnimation = 3;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
        }
    }
}
