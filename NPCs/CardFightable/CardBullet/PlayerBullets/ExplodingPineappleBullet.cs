using Entrogic.NPCs.CardFightable.Particles;
using Entrogic.UI.CardGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable.CardBullet.PlayerBullets
{
    public class ExplodingPineappleBullet : CardFightBullet
    {
        private Vector2 TargetPosition; // UI位置
        public ExplodingPineappleBullet(Vector2 targetPos)
        {
            TargetPosition = targetPos;
            _texture = ModContent.GetTexture("Entrogic/Projectiles/Arcane/Pineapple");
            LifeSpan = 6f;
            IsFriendly = true;
            Size = new Vector2(_texture.Width, _texture.Height);
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            for (int i = 0; i < 3; i++)
            {
                FireParticle particle = new FireParticle()
                {
                    Position = CardGameUI.ToUIPos(Position) + new Vector2(0f, -10f),
                    Velocity = Vector2.Zero,
                    UIPosition = UIPosition,
                    Alpha = 100
                };
                particle.Setup(Size, 1f, attackPlayer);
                particle.Velocity *= 0.85f;
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            Velocity.Y -= 0.1f;

            if (Vector2.Distance(CardGameUI.ToUIPos(Position + Size / 2f), TargetPosition + new Vector2(0, 12f)) < 10f)
            {
                IsRemoved = true;
                CardFightableNPC fightNPC = (CardFightableNPC)attackNPC.modNPC;
                fightNPC.CardGameHealth -= 46;
            }

            base.Update(gameTime, attackPlayer, attackNPC);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Kill(Player attackPlayer)
        {
            Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f + Main.screenPosition;
            Main.PlaySound(SoundID.Item, (int)screenCenter.X, (int)screenCenter.Y + -30, 14, 1f, 0f);
            Position.X = CardGameUI.ToUIPos(Position).X + (float)(Size.X / 2);
            Position.Y = CardGameUI.ToUIPos(Position).Y + (float)(Size.Y / 2);
            Size.X = 20;
            Size.Y = 20;
            Position.X -= (float)(Size.X / 2);
            Position.Y -= (float)(Size.Y / 2);
            for (int i = 0; i < 23; i++)
            {
                FireParticle particle = new FireParticle()
                {
                    Position = new Vector2(Position.X, Position.Y),
                    UIPosition = UIPosition,
                    Alpha = 100
                };
                particle.Setup(Size, 1f, attackPlayer);
                if (Main.rand.Next(2) == 0)
                {
                    particle.Scale = 0.5f;
                }
                particle.Velocity *= 2f;
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            for (int j = 0; j < 20; j++)
            {
                FireParticle2 particle = new FireParticle2()
                {
                    Position = new Vector2(Position.X, Position.Y),
                    UIPosition = UIPosition,
                    Alpha = 100
                };
                particle.Setup(Size, 1f, attackPlayer);
                particle.Velocity *= 2f;
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            for (int k = 0; k < Main.rand.Next(5, 7); k++)
            {
                float scaleFactor = 0.33f;
                if (k % 3 == 1)
                {
                    scaleFactor = 0.66f;
                }
                if (k % 3 == 1)
                {
                    scaleFactor = 1f;
                }
                ExplodeParticle particle = new ExplodeParticle()
                {
                    Position = new Vector2(Position.X + (float)(Size.X / 2) - 24f, Position.Y + (float)(Size.Y / 2) - 24f),
                    UIPosition = UIPosition,
                    Alpha = Main.rand.Next(140, 201)
                };
                particle.Setup(Size, Main.rand.NextFloat() * 0.5f + 1f, attackPlayer);
                particle.Velocity *= scaleFactor;
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);

                particle = new ExplodeParticle()
                {
                    Position = new Vector2(Position.X + (float)(Size.X / 2) - 24f, Position.Y + (float)(Size.Y / 2) - 24f),
                    UIPosition = UIPosition,
                    Alpha = Main.rand.Next(140, 201)
                };
                particle.Setup(Size, Main.rand.NextFloat() * 0.5f + 1f, attackPlayer);
                particle.Velocity *= scaleFactor;
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            //for (float rad = 0.0f; rad < 2 * 3.141f; rad += (float)Main.rand.Next(8, 14) / 10)
            //{
            //    Vector2 vec = new Vector2(120f, 0f);
            //    Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * 16f;
            //    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-10, 11), Main.rand.Next(-10, 11), ProjectileType<PineapplePieces>(), 21, 0f, projectile.owner, 0f, 0f);
            //}
            base.Kill(attackPlayer);
        }
    }
}
