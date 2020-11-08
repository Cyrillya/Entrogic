using Entrogic.Projectiles.Ammos;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class Galekylin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("不朽圣火的力量，好好利用吧");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            item.damage = 89;
            item.DamageType = DamageClass.Ranged;
            item.width = 32;
            item.crit += 26;
            item.height = 64;
            item.useTime = 25;
            item.useAnimation = 25;
            
            item.useStyle = ItemUseStyleID.Shoot;

            item.noMelee = true;
            item.knockBack = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 16f;

            item.useAmmo = AmmoID.Arrow;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Tsunami, 1)
                .AddIngredient(ItemID.DD2PhoenixBow, 1)
                .AddIngredient(ItemID.FragmentVortex, 20)
                .AddIngredient(ItemID.FragmentSolar, 20)
                .AddIngredient(ItemID.LunarBar, 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.DD2PhoenixBowShot, 250, knockBack, player.whoAmI);
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ProjectileType<凤凰箭>();
            }

            float numberProjectiles = 3 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(3);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // 如果只有1个弹丸，请注意除以0。
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }

            return false;
        }
    }
}