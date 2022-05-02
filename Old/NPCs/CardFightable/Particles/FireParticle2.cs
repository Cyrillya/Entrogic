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
    public class FireParticle2 : Particle
    {
        public FireParticle2() : base()
        {
            _texture = (Texture2D)ModContent.GetTexture(typeof(FireParticle2).FullName.Replace('.', '/') + $"_{Main.rand.Next(3)}");
        }
        public override void Update(GameTime gameTime)
        {
            Velocity.Y = Velocity.Y * 0.94f;
            Velocity.X = Velocity.X * 0.94f;
            _rotation += Velocity.X * 0.5f;
            Velocity *= 0.92f;
            Scale -= 0.045f;

            base.Update(gameTime);
        }
        public override Color GetColor(Color newColor)
        {
            return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 255 - Alpha);
        }
    }
}
