using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Dusts
{
    public class SnowStormyDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 10, 10);
        }

        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.87f;
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.1f;
            dust.scale -= 0.08f;
            if (dust.scale <= 0.1f)
            {
                dust.active = false;
            }
            Lighting.AddLight(dust.position, Color.AntiqueWhite.ToVector3());
            return false;
        }
    }
}
