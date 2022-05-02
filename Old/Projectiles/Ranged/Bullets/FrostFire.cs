using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace Entrogic.Projectiles.Ranged.Bullets
{
    public class FrostFire : ModProjectile
    {
<<<<<<< HEAD
        public override string Texture => "Entrogic/Assets/Images/Block";
=======
        public override string Texture => "Entrogic/Images/Block";
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Flames);
            projectile.aiStyle = -1;
            projectile.timeLeft = 80;
        }
        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, MyDustId.IceTorch, 0f, 0f, 0, default, Main.rand.Next(3, 7));
            dust.noGravity = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.Frostburn, Main.rand.Next(120, 211));
        }
        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.Frostburn, Main.rand.Next(120, 211));
        }
    }
}
