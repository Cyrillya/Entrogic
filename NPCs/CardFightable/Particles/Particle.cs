using Terraria;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace Entrogic.NPCs.CardFightable.Particles
{
    public class Particle : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 UIPosition; // UI的左上角
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Size;
        public float Scale;
        public int Alpha;
        protected float _rotation;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;
        public Color Color = Color.White;

        public bool IsPanelParticle;
        public bool IsRemoved = false;
        public Particle()
        {
        }

        public virtual void Setup(Vector2 size, float scale, Player attackPlayer)
        {
            Position.X += (float)Main.rand.Next((int)size.X - 4) + 4f;
            Position.Y += (float)Main.rand.Next((int)size.Y - 4) + 4f;
            Velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
            Velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
            Scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
            Scale *= scale;
            if (Velocity.HasNaNs())
            {
                Velocity = Vector2.Zero;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity;
            if (Scale < 0.1f)
            {
                IsRemoved = true;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position + UIPosition, null, GetColor(Color), _rotation, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public virtual Color GetColor(Color newColor)
        {
            return newColor;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
