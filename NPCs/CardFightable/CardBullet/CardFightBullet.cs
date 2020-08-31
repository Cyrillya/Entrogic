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
        public Vector2 Center
        {
            get => Position + new Vector2(_texture.Width, _texture.Height) / 2f;
            set => Position = value - new Vector2(_texture.Width, _texture.Height) / 2f;
        }
        protected float Scale = 1f;
        public byte Direction;
        public byte SpriteDirection;
        protected float _rotation;
        protected float drawOffsetY;
        protected float gfxOffY;
        protected Vector2 HitboxOffset;
        public Rectangle Hitbox => new Rectangle((int)(Position.X + HitboxOffset.X), (int)(Position.Y + HitboxOffset.Y), (int)Size.X, (int)Size.Y);

        public int Damage = 0;
        public Color Color = Color.White;
        public float LifeSpan = 0f;

        public bool IsPanelBullet;
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
            spriteBatch.Draw(_texture, 
                Position + new Vector2(_texture.Width, _texture.Height) / 2f + new Vector2(0f, drawOffsetY) + PlaygroundPos, 
                null, 
                GetAlpha(Color), 
                _rotation, 
                _texture.Size() * 0.5f,
                Scale,
                SpriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f);
        }

        public virtual Color GetAlpha(Color drawColor)
        {
            return drawColor;
        }

        public virtual void Kill(Player attackPlayer)
        {

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
            return this.Hitbox.Right + this.Velocity.X > bullet.Hitbox.Left &&
              this.Hitbox.Left < bullet.Hitbox.Left &&
              this.Hitbox.Bottom > bullet.Hitbox.Top &&
              this.Hitbox.Top < bullet.Hitbox.Bottom;
        }

        protected bool IsTouchingRight(CardFightBullet bullet)
        {
            return this.Hitbox.Left + this.Velocity.X < bullet.Hitbox.Right &&
              this.Hitbox.Right > bullet.Hitbox.Right &&
              this.Hitbox.Bottom > bullet.Hitbox.Top &&
              this.Hitbox.Top < bullet.Hitbox.Bottom;
        }

        protected bool IsTouchingTop(CardFightBullet bullet)
        {
            return this.Hitbox.Bottom + this.Velocity.Y > bullet.Hitbox.Top &&
              this.Hitbox.Top < bullet.Hitbox.Top &&
              this.Hitbox.Right > bullet.Hitbox.Left &&
              this.Hitbox.Left < bullet.Hitbox.Right;
        }

        protected bool IsTouchingBottom(CardFightBullet bullet)
        {
            return this.Hitbox.Top + this.Velocity.Y < bullet.Hitbox.Bottom &&
              this.Hitbox.Bottom > bullet.Hitbox.Bottom &&
              this.Hitbox.Right > bullet.Hitbox.Left &&
              this.Hitbox.Left < bullet.Hitbox.Right;
        }

        protected bool GetCollided(CardFightBullet bullet)
        {
            return IsTouchingTop(bullet) || IsTouchingBottom(bullet) || IsTouchingLeft(bullet) || IsTouchingRight(bullet);
        }
        #endregion
    }
}
