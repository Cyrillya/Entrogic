using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Dusts
{
    public class PlagueFog : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.velocity *= 0.5f;
            dust.velocity.Y -= 0.5f;
            dust.alpha = 100;
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 38, 40, 38);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.1f;
            dust.alpha += 3;
            dust.scale -= 0.03f;
            if (dust.alpha >= 255)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
