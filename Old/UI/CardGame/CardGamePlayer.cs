﻿using Entrogic.NPCs.CardFightable.CardBullet;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

using Terraria;
<<<<<<< HEAD
using Terraria.Audio;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.UI.CardGame
{
    public class CardGamePlayer : CardFightBullet
    {
        private KeyboardState _currentKey;
        private KeyboardState _previousKey;

        public bool controlUp;
        public bool controlDown;
        public bool controlLeft;
        public bool controlRight;
        internal float ImmuneTime;
        private int ImmuneFlashTimer;
        private float WalkSpeed = 2f;
        public CardGamePlayer()
        {
<<<<<<< HEAD
            _texture = (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer_Icon_Default"];
            LifeSpan = 0.2f;
            IsFriendly = true;
            HitboxOffset = new Vector2((int)(_texture.Width / 2f - 1f), (int)(_texture.Height / 2f - 1f));
=======
            _texture = Entrogic.ModTexturesTable["CardFightPlayer_Icon_Default"];
            LifeSpan = 0.2f;
            IsFriendly = true;
            RectangleOffset = new Vector2((int)(_texture.Width / 2f - 1f), (int)(_texture.Height / 2f - 1f));
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            Size = Vector2.One * 2f;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(attackPlayer);
            LifeSpan = 0.5f;
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            Velocity = Vector2.Zero;
            SetKey(_currentKey, _previousKey);
            SetWalk();

            if (ImmuneTime <= 0f)
                foreach (var bullet in clientModPlayer._bullets)
                {
                    if (bullet == this)
<<<<<<< HEAD
                    {
                        continue; 
                    }

                    if (!bullet.IsFriendly && bullet.GetDamage(bullet.Damage) > 0 && Hitbox.Intersects(bullet.Hitbox))
                    {
                        SoundEngine.PlaySound(ResourceLoader.CGHurt);
                        clientModPlayer.CardGamePlayerHealth -= bullet.GetDamage(bullet.Damage);
                        ImmuneTime = 1f;
                        break;
                    }
                }
=======
                        continue;

                    if (GetCollided(bullet) && !bullet.IsFriendly)
                    {
                        clientModPlayer.CardGamePlayerHealth -= bullet.GetDamage(bullet.Damage);
                        ImmuneTime = 1f;
                        Main.PlaySound(Entrogic.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/CGHurt"));
                    }
                }

>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            ImmuneTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            ImmuneTime = Math.Max(ImmuneTime, 0f);
            if (clientModPlayer.CardGamePlayerTurn) IsRemoved = true; // 玩家局就不需要你了
            base.Update(gameTime, attackPlayer, attackNPC);

            Position.X = Math.Max(Position.X, 0);
            Position.X = Math.Min(Position.X, PlaygroundSize.X - _texture.Width);
            Position.Y = Math.Max(Position.Y, 0);
            Position.Y = Math.Min(Position.Y, PlaygroundSize.Y - _texture.Height);
<<<<<<< HEAD
            clientModPlayer.CardGamePlayerCenter = Center;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            ImmuneFlashTimer++;
            if (ImmuneTime <= 0f || ImmuneFlashTimer % 20 <= 10) // 20t一周期，显示与隐藏时间五五开
                base.Draw(spriteBatch);
        }
        private void SetKey(KeyboardState _currentKey, KeyboardState _previousKey)
        {
            controlUp = controlDown = controlLeft = controlRight = false;
            if (_currentKey.IsKeyDown(Keys.W) || _currentKey.IsKeyDown(Keys.Up)) controlUp = true;
            if (_currentKey.IsKeyDown(Keys.S) || _currentKey.IsKeyDown(Keys.Down)) controlDown = true;
            if (_currentKey.IsKeyDown(Keys.A) || _currentKey.IsKeyDown(Keys.Left)) controlLeft = true;
            if (_currentKey.IsKeyDown(Keys.D) || _currentKey.IsKeyDown(Keys.Right)) controlRight = true;
        }
        private void SetWalk()
        {
            if (controlUp)
            {
                Velocity.Y -= WalkSpeed;
            }
            if (controlDown)
            {
                Velocity.Y += WalkSpeed;
            }
            if (controlLeft)
            {
                Velocity.X -= WalkSpeed;
            }
            if (controlRight)
            {
                Velocity.X += WalkSpeed;
            }
        }
    }
}
