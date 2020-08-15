using Entrogic.NPCs.CardFightable.Particles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable.CardBullet
{
    public class MushroomCannonball : CardFightBullet
    {
        private int collideCount;
        private int collideDelay;
        public MushroomCannonball()
        {
            _texture = ModContent.GetTexture(typeof(MushroomCannonball).FullName.Replace('.', '/'));
            LifeSpan = 6f;
            IsFriendly = false;
            Damage = 60;
            Size = new Vector2(_texture.Width, _texture.Height);
            collideDelay = 15;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            base.Update(gameTime, attackPlayer, attackNPC);
            _rotation = Velocity.ToRotation();

            collideDelay--;
            if (collideCount >= 3 || collideDelay > 0)
                return;
            if (Position.X <= 0f - 5f) 
            { 
                Velocity.X = -Velocity.X; 
                collideCount++;
                collideDelay = 3;
            } // 撞左边墙
            if (Position.Y <= 0f - 5f)
            {
                Velocity.Y = -Velocity.Y;
                collideCount++;
                collideDelay = 3;
            } // 撞上边墙
            if (Position.X >= PlaygroundSize.X - _texture.Width + 5f)
            {
                Velocity.X = -Velocity.X;
                collideCount++;
                collideDelay = 3;
            } // 撞右边墙
            if (Position.Y >= PlaygroundSize.Y - _texture.Height + 5f)
            {
                Velocity.Y = -Velocity.Y;
                collideCount++;
                collideDelay = 3;
            } // 撞下边墙
        }
    }
}
