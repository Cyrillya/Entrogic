using Entrogic.Items.Materials;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 就地处决 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void SetDefaults()
        {
            item.scale = 1.2f;
            item.damage = 167;
            item.DamageType = DamageClass.Ranged;
            item.width = 32;
            item.height = 20;
            item.useTime = 50;
            item.useAnimation = 50;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noMelee = true;
            item.knockBack = 18;
            item.value = Item.sellPrice(gold:1, silver:80);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item40;
            item.autoReuse = true;
            item.crit += 21;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet) type = ProjectileID.BulletHighVelocity;
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<EntrogicPlayer>().ProjectieHasArmorPenetration = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<SoulofContamination>(), 7)
                .AddIngredient(ItemID.HallowedBar, 18)
                .AddIngredient(ItemID.Shotgun)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            damage += target.defense / 2;
        }
    }
}
