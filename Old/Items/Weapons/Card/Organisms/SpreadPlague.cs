using Entrogic.Projectiles.Arcane;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class SpreadPlague : ModCard
    {
        public override void PreCreated()
        {
            said = "但是我们的世界没有口罩！";
            rare = CardRareID.WeakNuclear;
            tooltip = "投掷出易传染的瘟疫";
            costMana = 1;
            series = CardSeriesID.Organism;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得10次施展魔法“散布瘟疫”的机会]";
        }
        public override void CardDefaults()
        {
            item.shootSpeed = 1.2f;
            item.rare = RareID.LV7;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 20);
            item.shoot = ProjectileType<Plague>();
            item.crit += 30;
            item.damage = 11;
            item.knockBack = 5f;
            attackCardRemainingTimes = 10;
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            for (float rad = -MathHelper.PiOver4 / 2f; rad <= MathHelper.PiOver4 / 2f; rad += MathHelper.PiOver4 / 2f / 6f)
            {
                Vector2 vec = (new Vector2(speedX, speedY).ToRotation() + rad).ToRotationVector2() * speed * 0.6f;
                Projectile.NewProjectile(position, vec, type, damage, knockBack, player.whoAmI);
            }
        }
    }
}
