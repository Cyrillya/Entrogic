using Entrogic.Content.Items.BaseTypes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.Athanasy
{
    public class RockShotgun : ItemBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rock Shotgun");
            DisplayName.TranslationChinese("岩石猎枪");
            Tooltip.SetDefault("It has many hieroglyphs engraved on it...\n\"Use with caution\"");
            Tooltip.TranslationChinese("它上面刻有许多象形文字...\n“谨慎使用”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -4);
        }
        public override void SetDefaults()
        {
            Item.scale = 1.2f;
            Item.damage = 111;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 20;
            Item.useTime = Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = false;
            Item.crit += 29;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            type = ProjectileID.BulletHighVelocity;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}
