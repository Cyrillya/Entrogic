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
    public class FallenStar : CardFightBullet
    {
        private Vector2 TargetPosition; // UI位置
        public FallenStar(Vector2 targetPos)
        {
            TargetPosition = targetPos;
            _texture = (Texture2D)ModContent.GetTexture("Entrogic/Projectiles/Arcane/FallenStar");
            LifeSpan = 5.5f;
            IsFriendly = true;
            Size = new Vector2(8, 8);
            IsPanelBullet = true;
        }
        public override Color GetAlpha(Color drawColor)
        {
            return base.GetAlpha(drawColor) * 0.5f;
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            //if (LifeSpan > 4.96 && LifeSpan <= 5.1f)
            //    Velocity += -ModHelper.GetFromToVector(CardGameUI.ToUIPos(Center), TargetPosition) * 0.4f;
            //else if (LifeSpan <= 5.1f)
                Velocity += ModHelper.GetFromToVector(CardGameUI.ToUIPos(Center), TargetPosition) * 0.9f;
            _rotation = MathHelper.ToRadians(45f) + ModHelper.GetFromToRadians(CardGameUI.ToUIPos(Center), TargetPosition);

            if (Vector2.Distance(CardGameUI.ToUIPos(Center), TargetPosition) < 20f)
            {
                IsRemoved = true;
                CardFightableNPC fightNPC = (CardFightableNPC)attackNPC.modNPC;
                fightNPC.CardGameHealth -= 120 / 5; // 总伤害120,，因为一射5个
            }
            base.Update(gameTime, attackPlayer, attackNPC);
        }
        public override void Kill(Player attackPlayer)
        {
            Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f + Main.screenPosition;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, (int)screenCenter.X, (int)screenCenter.Y + -30);
            Size.X = 20;
            Size.Y = 20;
            Position.X = CardGameUI.ToUIPos(Center).X - Size.X / 2f;
            Position.Y = CardGameUI.ToUIPos(Center).Y - Size.Y / 2f;
            for (int i = 0; i < 23; i++)
            {
                FireParticle particle = new FireParticle()
                {
                    Position = Position,
                    UIPosition = UIPosition,
                    Alpha = 100,
                    IsPanelParticle = true
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
                    Position = Position,
                    UIPosition = UIPosition,
                    Alpha = 100,
                    IsPanelParticle = true
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
                StarParticle particle = new StarParticle()
                {
                    Position = Position,
                    UIPosition = UIPosition,
                    Alpha = Main.rand.Next(140, 201),
                    IsPanelParticle = true
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
