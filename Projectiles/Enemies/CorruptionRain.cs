using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Enemies
{
    public class CorruptionRain : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.RainNimbus);
            if (!Main.hardMode)
                projectile.damage = 25;
            else if (!NPC.downedPlantBoss && Main.hardMode)
                projectile.damage = 40;
            else if (!NPC.downedMoonlord && NPC.downedPlantBoss && Main.hardMode)
                projectile.damage = 60;
            else if (NPC.downedMoonlord && NPC.downedPlantBoss && Main.hardMode)
                projectile.damage = 100;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 90);
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.CursedInferno, 150);
            }
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(BuffID.CursedInferno, 240);
            }
        }
    }
}