using Entrogic.NPCs.CardFightable.Particles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable.CardBullet
{
    public class MushroomCannon : CardFightBullet
    {
        private int Timer = -40;
        private Vector2 DirectedPos;
        private Vector2 DirectedVelocity;

        private float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }
        Vector2 RotationBase => Position + new Vector2(_texture.Width / 2f, 0f);
        float RotateAddtion => MathHelper.ToRadians(-9f);
        float RotateDrawOffset => MathHelper.ToRadians(-90f);
        public MushroomCannon()
        {
            _texture = (Texture2D)ModContent.GetTexture(typeof(MushroomCannon).FullName.Replace('.', '/'));
            LifeSpan = 12f;
            IsFriendly = false;
            Damage = 0;
            Size = new Vector2(_texture.Width, _texture.Height);
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(attackPlayer);
            Timer++;
            if (Timer > 0)
            {
                // (0 - 40tick 定位玩家)
                // (41 - 59tick 空档)
                // (60tick 发射)
                // (61 - 71tick 回位)
                // 1.2s一个周期，也就是72tick一个周期
                if (Timer % 72 == 60) // 发射！
                {
                    // 角度大于逆时针45°的会有Bug，这里直接暴力修复...
                    float rotOff = Rotation > MathHelper.ToRadians(45f) ? -RotateAddtion : 0f;
                    // 我不知道的Bug
                    Vector2 ShootBase = RotationBase + (Rotation + rotOff).ToRotationVector2() * 40f;
                    MushroomCannonball bullet = new MushroomCannonball()
                    {
                        Velocity = (Rotation).ToRotationVector2() * 8f,
                        Position = ShootBase,
                        UIPosition = this.UIPosition
                    };
                    clientModPlayer._bullets.Add(bullet);

                    Position -= ModHelper.GetFromToVector(Position + Size / 2f, DirectedPos) * 10f;
                }
                // 延迟0.5s，也就是30tick
                else if (Timer % 72 <= 40)
                {
                    // 为了平滑转向
                    DirectedVelocity = 0.1f * (clientModPlayer.CardGamePlayerCenter - DirectedPos);
                    DirectedPos += DirectedVelocity;
                    // 其实按照我们现在这个场地，这个应该是从0-180°的范围再缩小一点点
                    Rotation = ModHelper.GetFromToRadians(RotationBase, DirectedPos);
                }
                else if (Timer % 72 >= 61)
                {
                    Position += ModHelper.GetFromToVector(Position + Size / 2f, DirectedPos) * 1f;
                }

                Velocity = Vector2.Zero;
            }
            else
            {
                DirectedPos = clientModPlayer.CardGamePlayerCenter;
                Rotation = ModHelper.GetFromToRadians(RotationBase, DirectedPos);
            }
            //Velocity = Vector2.Zero;
            //_rotation = ModHelper.GetFromToRadius(Position + Size / 2f, clientModPlayer.CardGamePlayerCenter) + MathHelper.ToRadians(180f);

            base.Update(gameTime, attackPlayer, attackNPC);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // 一些小Bug，暴力修复了
            float rotOff = Rotation > MathHelper.ToRadians(45f) ? RotateAddtion : 0f;
            spriteBatch.Draw(_texture,
                Position + new Vector2(_texture.Width / 2f, 0f) + PlaygroundPos,
                null,
                GetAlpha(Color),
                Rotation + RotateDrawOffset + rotOff, // 因为显示有一些十分奇怪的问题，所以这里直接逆时针偏移一下
                new Vector2(_texture.Width / 2f, 0f),
                Scale,
                SpriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f);
        }
    }
}
