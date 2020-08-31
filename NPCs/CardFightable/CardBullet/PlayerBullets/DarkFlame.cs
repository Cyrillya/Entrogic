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
    public class DarkFlame : CardFightBullet
    {
        private Vector2 TargetPosition; // UI位置
        internal float InvisibleSpan;
        public DarkFlame(Vector2 targetPos, float invisibleSpan = 0f)
        {
            TargetPosition = targetPos;
            _texture = ModContent.GetTexture("Entrogic/Projectiles/Arcane/CursedSpirit");
            LifeSpan = 6f;
            IsFriendly = true;
            Size = new Vector2(_texture.Width, _texture.Height);
            InvisibleSpan = invisibleSpan;
            IsPanelBullet = true;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            Velocity *= 1.1f;
            for (int i = 0; i < 2; i++)
            {
                ArcaneMissleParticle particle = new ArcaneMissleParticle()
                {
                    Position = CardGameUI.ToUIPos(Position),
                    Velocity = Vector2.Zero,
                    UIPosition = UIPosition,
                    IsPanelParticle = true
                };
                particle.Setup(Size, 1f, attackPlayer);
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }

            if (Vector2.Distance(CardGameUI.ToUIPos(Position), TargetPosition) < 36f)
            {
                IsRemoved = true;
                CardFightableNPC fightNPC = (CardFightableNPC)attackNPC.modNPC;
                fightNPC.CardGameHealth -= 120 / 6; // 总伤害120,，因为一射6个
            }

            base.Update(gameTime, attackPlayer, attackNPC);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // 无贴图
            //base.Draw(spriteBatch);
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
                    UIPosition = UIPosition,
                    IsPanelParticle = true
                };
                particle.Setup(Size, 2f, attackPlayer);
                attackPlayer.GetModPlayer<EntrogicPlayer>()._particles.Add(particle);
            }
            base.Kill(attackPlayer);
        }
    }
}
