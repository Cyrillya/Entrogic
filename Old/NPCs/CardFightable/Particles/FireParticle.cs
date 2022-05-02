using Terraria;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Entrogic.NPCs.CardFightable.Particles
{
    public class FireParticle : Particle
    {
        public FireParticle() : base()
        {
            _texture = (Texture2D)ModContent.GetTexture(typeof(FireParticle).FullName.Replace('.', '/') + $"_{Main.rand.Next(3)}");
        }
        public override void Update(GameTime gameTime)
        {
            Scale -= 0.05f;
            float num63 = Scale * 1f;
            if (num63 > 0.6f)
            {
                num63 = 0.6f;
            }

            base.Update(gameTime);
        }
        public override void Setup(Vector2 size, float scale, Player attackPlayer)
        {
            base.Setup(size, scale, attackPlayer);
            Scale *= 0.8f;
        }
        public override Color GetColor(Color newColor)
        {
            return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
        }
    }
}
