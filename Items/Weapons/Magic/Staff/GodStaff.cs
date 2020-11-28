
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class GodStaff : ModItem
    {
        public override void SetStaticDefaults() 
        { 
            Item.staff[item.type] = true;
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item20;
            item.Size = new Vector2(74, 76);
            item.useTime = item.useAnimation = 30;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 5);
            item.rare = RareID.LV6;
            item.autoReuse = item.channel = true;
            item.damage = 63;
            item.crit += 4;
            item.knockBack = 7f;
            item.shoot = ProjectileType<Projectiles.Magic.Staff.GodCrystal>();
            item.shootSpeed = 20f;
            item.noMelee = true;
            item.mana = 20;
            item.DamageType = DamageClass.Magic;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.shootSpeed = 12f;
            item.knockBack = 2f;
            position = Main.MouseWorld;
            if (player.altFunctionUse != 2)
            {
                damage = (int)(damage * 1.4f);
                Projectile.NewProjectile(position.X, position.Y, speedY, speedX, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, -speedY, -speedX, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, speedY, -speedX, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, -speedY, speedX, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, -speedX, -speedY, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, -speedX, speedY, type, damage, knockBack, Main.myPlayer);
                Projectile.NewProjectile(position.X, position.Y, speedX, -speedY, type, damage, knockBack, Main.myPlayer);
            }
            else
            {
                type = ProjectileType<Projectiles.Magic.Staff.FC>();
                Projectile.NewProjectile(position.X, position.Y, 0f, 0f, type, damage, knockBack, Main.myPlayer);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(Mod, "CBar", 6)
                .AddIngredient(ItemID.MagicalHarp, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}