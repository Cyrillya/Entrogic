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
    public class ArcaneMissleBullet : CardFightBullet
    {
        private Vector2 TargetPosition; // UI位置
        internal float InvisibleSpan;
        public ArcaneMissleBullet(Vector2 targetPos, float invisibleSpan = 0f)
        {
            TargetPosition = targetPos;
            _texture = ModContent.GetTexture("Entrogic/Projectiles/Arcane/ArcaneMissle");
            LifeSpan = 6f;
            IsFriendly = true;
            Size = new Vector2(_texture.Width, _texture.Height);
            InvisibleSpan = invisibleSpan;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            InvisibleSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InvisibleSpan > 0f) return;
            for (int extraUpdates = 0; extraUpdates < 2; extraUpdates++)
            {
                for (int i = 0; i < 2; i++)
                {
                    ArcaneMissleParticle particle = new ArcaneMissleParticle()
                    {
                        Position = CardGameUI.ToUIPos(Position),
                        Velocity = Vector2.Zero,
                        UIPosition = UIPosition
                    };
                    particle.Setup(Size, 2f, attackPlayer);
                    attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
                }
                // 计算朝向目标的向量
                Vector2 targetVec = TargetPosition - CardGameUI.ToUIPos(Position + Size / 2f);
                targetVec.Normalize();
                // 目标向量是朝向目标的大小为15的向量
                targetVec *= 8f;
                // 朝向npc的单位向量*15 + 一些(?)偏移量
                Velocity = (Velocity * 8f + targetVec) / 9f;

                if (Vector2.Distance(CardGameUI.ToUIPos(Position), TargetPosition) < 10f)
                {
                    IsRemoved = true;
                    CardFightableNPC fightNPC = (CardFightableNPC)attackNPC.modNPC;
                    fightNPC.CardGameHealth -= 46;
                }

                base.Update(gameTime, attackPlayer, attackNPC);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InvisibleSpan > 0f) return;
            base.Draw(spriteBatch);
        }
        public override void Kill(Player attackPlayer)
        {
            Main.PlaySound(SoundID.Item10);
            for (int i = 0; i < 22; i++)
            {
                ArcaneMissleParticle particle = new ArcaneMissleParticle()
                {
                    Position = CardGameUI.ToUIPos(Position),
                    Velocity = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)),
                    UIPosition = UIPosition
                };
                particle.Setup(Size, 2f, attackPlayer);
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            base.Kill(attackPlayer);
        }
    }
}
