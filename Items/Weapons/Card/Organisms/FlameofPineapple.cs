
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class FlameofPineapple : ModCard
    {
        public override void PreCreated()
        {
            said = "菠———萝———哥———";
            rare = CardRareID.StrongNuclear;
            tooltip = "投掷出燃烧的菠萝";
            costMana = 2;
            series = CardSeriesID.Organism;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得18次施展魔法“爆炸菠萝”的机会]";
        }
        public override void CardDefaults()
        {
            item.shootSpeed = 8f;
            item.rare = RareID.LV7;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 20);
            item.shoot = ProjectileType<Projectiles.Arcane.Pineapple>();
            item.crit += 30;
            item.damage = 17;
            item.knockBack = 5f;
            attackCardRemainingTimes = 18;
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Vector2 vec = new Vector2(speedX, speedY);
            Projectile.NewProjectile(position, vec, type, damage, knockBack, player.whoAmI);
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return NPC.downedQueenBee;
        }
    }
}

