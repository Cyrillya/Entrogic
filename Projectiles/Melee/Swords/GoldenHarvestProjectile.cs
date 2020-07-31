using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Entrogic.Items.Weapons.Melee.Sword;

namespace Entrogic.Projectiles.Melee.Swords
{
    public class GoldenHarvestProjectile : ModProjectile
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
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (crit && target != null)
            { 
                target.GetGlobalNPC<MoreMoney>().moneyPoint50++;
                target.GetGlobalNPC<MoreMoney>().ExtraMoney += Main.rand.Next(1,5000); // 每次有1铜币到50银币不等的额外钱币
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
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
            projectile.direction = player.direction;
            player.heldProj = projectile.whoAmI;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.position += projectile.rotation.ToRotationVector2() * 80f;
            if(projectile.velocity.X > 0) // 向右攻击需要微调位置
            {
                projectile.position += new Vector2(12f, 12f);
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
}
