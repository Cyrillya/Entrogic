using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;

namespace Entrogic.NPCs.CardFightable.CardBullet
{
    // 由曾经做的一个MonoGame小游戏复制而来
    public class CardFightBullet : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 UIPosition; // UI的左上角
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 RotateDirection;
        public Vector2 Size;
        public byte Direction;
        public byte SpriteDirection;
        protected float _rotation;
        protected Vector2 RectangleOffset;
        public Rectangle Rectangle => new Rectangle((int)(Position.X + RectangleOffset.X), (int)(Position.Y + RectangleOffset.Y), (int)(Size.X + RectangleOffset.X), (int)(Size.Y + RectangleOffset.Y));

        public int Damage = 0;
        public Color Color = Color.White;
        public float LifeSpan = 0f;

        public bool IsRemoved = false;
        public bool IsFriendly = true;
        public bool CanDamage = true;
        protected Vector2 PlaygroundPos => new Vector2(114f, 86f) + UIPosition;
        protected Vector2 PlaygroundSize => new Vector2(358f, 220f);
        public CardFightBullet()
        {
        }

        public virtual void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            LifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeSpan <= 0f)
                IsRemoved = true;

            Position += Velocity;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position + PlaygroundPos, null, Color, _rotation, Vector2.Zero, 1f, SpriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual int GetDamage(int damage)
        {
            return damage;
        }

        public virtual void Initialize()
        {
        }

        [Obsolete]
        private static CardFightBullet SpawnBullet(Vector2 position, Vector2 panelPosition, string name)
        {
            if (!Entrogic.cfBullets.TryGetValue(name, out CardFightBullet value))
            {
                return null;
            }
            value.UIPosition = panelPosition;
            value.Position = position;
            return value;
        }
        [Obsolete]
        public static T SpawnBullet<T>(Vector2 position, Vector2 panelPosition) where T : CardFightBullet
        {
            return (T)SpawnBullet(position, panelPosition, typeof(T).Name);
        }
        #region Colloision
        protected bool IsTouchingLeft(CardFightBullet bullet)
        {
            return this.Rectangle.Right + this.Velocity.X > bullet.Rectangle.Left &&
              this.Rectangle.Left < bullet.Rectangle.Left &&
              this.Rectangle.Bottom > bullet.Rectangle.Top &&
              this.Rectangle.Top < bullet.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(CardFightBullet bullet)
        {
            return this.Rectangle.Left + this.Velocity.X < bullet.Rectangle.Right &&
              this.Rectangle.Right > bullet.Rectangle.Right &&
              this.Rectangle.Bottom > bullet.Rectangle.Top &&
              this.Rectangle.Top < bullet.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(CardFightBullet bullet)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > bullet.Rectangle.Top &&
              this.Rectangle.Top < bullet.Rectangle.Top &&
              this.Rectangle.Right > bullet.Rectangle.Left &&
              this.Rectangle.Left < bullet.Rectangle.Right;
        }

        protected bool IsTouchingBottom(CardFightBullet bullet)
        {
            return this.Rectangle.Top + this.Velocity.Y < bullet.Rectangle.Bottom &&
              this.Rectangle.Bottom > bullet.Rectangle.Bottom &&
              this.Rectangle.Right > bullet.Rectangle.Left &&
              this.Rectangle.Left < bullet.Rectangle.Right;
        }

        protected bool GetCollided(CardFightBullet bullet)
        {
            return IsTouchingTop(bullet) || IsTouchingBottom(bullet) || IsTouchingLeft(bullet) || IsTouchingRight(bullet);
        }
        #endregion
    }
}
