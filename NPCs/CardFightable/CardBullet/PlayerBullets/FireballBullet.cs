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
    public class FireballBullet : CardFightBullet
    {
        private Vector2 TargetPosition; // UI位置
        private bool State2;
        private float ai;
        private float localAI;
        public FireballBullet(Vector2 targetPos)
        {
            TargetPosition = targetPos;
            _texture = (Texture2D)Entrogic.ModTexturesTable["Block"];
            LifeSpan = 5f * 2f;
            IsFriendly = true;
            Size = new Vector2(12f, 12f);
            Damage = 100;
            IsPanelBullet = true;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            for (int extraUpdates = 0; extraUpdates < 2; extraUpdates++)
            {
                if (!State2)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        FireParticle particle = new FireParticle()
                        {
                            Position = CardGameUI.ToUIPos(Position) + new Vector2(0f, -10f),
                            Velocity = Vector2.Zero,
                            UIPosition = UIPosition,
                            Alpha = 100,
                            IsPanelParticle = true
                        };
                        particle.Setup(Size, 1.2f, attackPlayer);
                        particle.Velocity *= 0.5f * 1.1f;
                        attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
                    }
                    Velocity += new Vector2(0f, -0.8f);
                }
                else
                {
                    if (localAI == 0f)
                    {
                        Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f + Main.screenPosition;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item74, (int)screenCenter.X, (int)screenCenter.Y + -30);
                        localAI += 1f;
                    }
                    ai += 1f;
                    ai += 3f;
                    float num466 = 25f;
                    if (ai > 180f)
                    {
                        num466 -= (ai - 180f) / 2f;
                    }
                    num466 *= 0.7f;
                    int num467 = 0;
                    while ((float)num467 < num466)
                    {
                        float num468 = (float)Main.rand.Next(-10, 11);
                        float num469 = (float)Main.rand.Next(-10, 11);
                        float num470 = (float)Main.rand.Next(3, 9);
                        float num471 = (float)Math.Sqrt((double)(num468 * num468 + num469 * num469));
                        num471 = num470 / num471;
                        num468 *= num471;
                        num469 *= num471;

                        FireParticle particle = new FireParticle()
                        {
                            Position = CardGameUI.ToUIPos(Position) + new Vector2(0f, -10f),
                            Velocity = Vector2.Zero,
                            UIPosition = UIPosition,
                            Alpha = 100,
                            IsPanelParticle = true
                        };
                        particle.Setup(Size, 1.5f, attackPlayer);
                        particle.Position = CardGameUI.ToUIPos(Position) + Size / 2f;
                        particle.Position += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                        particle.Velocity = new Vector2(num468, num469) * 0.75f;
                        attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);

                        int num3 = num467;
                        num467 = num3 + 1;
                    }
                    Velocity = Vector2.Zero;
                }

                base.Update(gameTime, attackPlayer, attackNPC);

                if (!State2 && Vector2.Distance(CardGameUI.ToUIPos(Position + Size / 2f), TargetPosition + new Vector2(0, 12f)) < 10f)
                {
                    State2 = true;
                    CardFightableNPC fightNPC = (CardFightableNPC)attackNPC.modNPC;
                    fightNPC.CardGameHealth -= GetDamage(Damage);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Kill(Player attackPlayer)
        {
            base.Kill(attackPlayer);
        }
    }
}
