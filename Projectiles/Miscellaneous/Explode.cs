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
    public class Explode : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("时空裂纹");
        }
        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height =100;
            projectile.scale = 1f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 63;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.alpha = 150;
            projectile.light = 1f;
            projectile.aiStyle = -1;
            Main.projFrames[projectile.type] = 7;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 52;
            Main.player[projectile.owner].armorPenetration += target.defense;
        }
        public override void AI()
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0;
            projectile.rotation = 0;
            projectile.frameCounter++;
            if (projectile.frameCounter % 2==0) projectile.frame++;
            if (projectile.frame == 7) projectile.Kill();
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 3f);
                dust.noGravity = true;
            }
        }
        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{            
        //    target.AddBuff(mod.BuffType(""), damage);
        //}
    }
    public class 破防 : ModPlayer
    {
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.type == ProjectileType<Explode>()) player.armorPenetration += 2147483647 / 2 + 2;
        }
    }
}