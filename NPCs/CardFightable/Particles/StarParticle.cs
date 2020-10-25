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
    public class StarParticle : Particle
    {
        public StarParticle() : base()
        {
            _texture = (Texture2D)ModContent.GetTexture(typeof(StarParticle).FullName.Replace('.', '/') + $"_{Main.rand.Next(2)}");
        }
        public override void Update(GameTime gameTime)
        {
            Velocity.Y = Velocity.Y * 0.98f;
            Velocity.X = Velocity.X * 0.98f;
            Alpha--;
            Scale -= 0.007f;
            if ((double)Scale < 0.05 || Alpha < 0)
            {
                IsRemoved = true;
            }
            _rotation += 0.03f * Velocity.Length();

            base.Update(gameTime);
        }
        public override void Setup(Vector2 size, float scale, Player attackPlayer)
        {
            Velocity.Y -= (float)Main.rand.Next(-20, 21) * 0.1f;
            Velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
            //_rotation = Main.rand.NextFloat() * 3.1415f;
            Scale = 1f * scale;
        }
        public override Color GetColor(Color newColor)
        {
            return new Color(Alpha, Alpha, Alpha, Alpha);
        }
    }
}
