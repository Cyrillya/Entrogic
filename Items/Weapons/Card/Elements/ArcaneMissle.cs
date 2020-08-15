
using Entrogic.NPCs.CardFightable.CardBullet;
using Entrogic.NPCs.CardFightable.CardBullet.PlayerBullets;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class ArcaneMissle : ModCard
    {
        public override void PreCreated()
        {
            said = "用凝聚的奥术力量痛击你的敌人！";
            rare = CardRareID.Gravitation;
            tooltip = "召唤不稳定的的的奥术导弹";
            costMana = 1;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 20;
            item.rare = RareID.LV2;
            item.shoot = ProjectileType<Projectiles.Arcane.ArcaneMissle>();
            item.damage = 8;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 17;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得20次施展魔法“奥术飞弹”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
        }
        public override void CardGameAttack(Player attackPlayer, NPC attackNPC, Vector2 playerDrawPosition, Vector2 NPCDrawPosition, Vector2 panelPos)
        {
            for (int i = 0; i < 3; i++)
            {
                EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(attackPlayer);
                CardFightBullet bullet = new ArcaneMissleBullet(NPCDrawPosition + new Vector2(0, -10f), i * 0.2f)
                {
                    Velocity = new Vector2(Main.rand.Next(-8, 9) + (Main.rand.NextBool(2) ? 6f : -6f), Main.rand.Next(-8, 9) + (Main.rand.NextBool(2) ? 6f : -6f)),
                    Position = playerDrawPosition + new Vector2(0F, -6F),
                    UIPosition = panelPos
                };
                clientModPlayer._bullets.Add(bullet); 
            }
            base.CardGameAttack(attackPlayer, attackNPC, playerDrawPosition, NPCDrawPosition, panelPos);
        }
    }
    public class ArcaneMissle_Electromagnetism : ModCard
    {
        public override void PreCreated()
        {
            said = "用凝聚的强大奥术力量痛击你的敌人！";
            rare = CardRareID.Electromagnetism;
            tooltip = "召唤多个不稳定的的的奥术导弹";
            costMana = 1;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 25;
            item.rare = RareID.LV5;
            item.shoot = ProjectileType<Projectiles.Arcane.ArcaneMissle>();
            item.damage = 28;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 21;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得25次施展魔法“奥术飞弹”(强化版)的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            for (int i = 0; i < Main.rand.Next(3, 4 + 1); i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
                Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI)];
                proj.tileCollide = false;
                proj.timeLeft += 120;
            }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return Main.hardMode;
        }
    }
}

