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
    public class ArcaneMissleParticle : Particle
    {
        public ArcaneMissleParticle() : base()
        {
            _texture = ModContent.GetTexture($"Entrogic/NPCs/CardFightable/Particles/ArcaneMissleParticle_{Main.rand.Next(3)}");
        }
        public override void Update(GameTime gameTime)
        {
            Velocity.Y = Velocity.Y * 0.94f;
            Velocity.X = Velocity.X * 0.94f;
            float num97 = Scale * 0.8f;
            if (num97 > 1f)
            {
                num97 = 1f;
            }
            _rotation += Velocity.X * 0.5f;
            Velocity *= 0.92f;
            Scale -= 0.045f;

            base.Update(gameTime);
            if (Scale < 0.2f)
            {
                IsRemoved = true;
            }
        }
        public override Color GetColor(Color newColor)
        {
            return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
        }
    }
}
