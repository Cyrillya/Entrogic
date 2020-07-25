
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class BetrayerofDarkFlame : ModCard
    {
        public override void PreCreated()
        {
            rare = CardRareID.Electromagnetism;
            said = "背叛者的惩戒会伴你左右...";
            tooltip = "获得15次施展魔法“诅咒灵体”的机会";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        //public override string AnotherMessages()
        //{
        //    return "“你无法逃避”\r\n" +
        //           "“这是你的命运”\r\n" +
        //           "“接受它罢，这是你最后的自我救赎”\r\n" +
        //           "—————————————————\r\n" +
        //           "[c/EE4000:可怖的力量...同时也会焚烧你的生命]";
        //}
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 12;
            item.shootSpeed = 15f;
            item.damage = 8;
            item.knockBack = 4f;
            item.rare = RareID.LV5;
            item.crit += 20;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 15);
            item.shoot = ProjectileType<Projectiles.Arcane.CursedSpirit>();
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Vector2 vec = Main.MouseWorld - player.Center;
            for (float rad = 0.0f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / (float)Main.rand.Next(5, 6))
            {
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * speed;
                Projectile.NewProjectile(position, finalVec, type, damage, knockBack, player.whoAmI);
            }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return EntrogicWorld.beArrivedAtUnderworld;
        }
    }
    public class BetrayerofDarkFlame_Strong : ModCard
    {
        public override void PreCreated()
        {
            rare = CardRareID.StrongNuclear;
            said = "背叛者的惩戒会伴你前后左右...";
            tooltip = "获得16次施展魔法“诅咒灵体”的机会";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        //public override string AnotherMessages()
        //{
        //    return "“你可以逃避”\r\n" +
        //           "“这不是你的命运”\r\n" +
        //           "“别接受它，这是你刚开始的造福他人”\r\n" +
        //           "—————————————————\r\n" +
        //           "[c/EE4000:牛逼的力量...同时也会焚烧你的生命]";
        //}
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 16;
            item.shootSpeed = 18f;
            item.damage = 46;
            item.knockBack = 4f;
            item.rare = RareID.LV8;
            item.crit += 20;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 15);
            item.shoot = ProjectileType<Projectiles.Arcane.CursedSpirit>();
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Vector2 vec = Main.MouseWorld - player.Center;
            for (float rad = 0.0f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / (float)Main.rand.Next(6, 8))
            {
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * speed;
                Projectile proj = Main.projectile[Projectile.NewProjectile(position, finalVec, type, damage, knockBack, player.whoAmI)];
                proj.tileCollide = false;
            }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return EntrogicWorld.beArrivedAtUnderworld && NPC.downedPlantBoss;
        }
    }
}

