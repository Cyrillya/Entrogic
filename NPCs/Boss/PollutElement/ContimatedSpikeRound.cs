using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.Boss.PollutElement
{
    public class ContimatedSpikeRound : ModProjectile
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
            projectile.timeLeft = 300;
            projectile.scale = 1.3f;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
