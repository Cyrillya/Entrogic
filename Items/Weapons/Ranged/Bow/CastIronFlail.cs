using Entrogic.Items.Materials;
using Entrogic.Items.VoluGels;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class CastIronFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("铸铁连弩");
            Tooltip.SetDefault("“精美的连弩....宛如一件艺术品”\n" +
                "有几率多射出一发箭");
        }

        public override void SetDefaults()
        {
            item.damage = 21;
            item.DamageType = DamageClass.Ranged;
            item.useTime = 10;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noMelee = true; 
            item.knockBack = 4;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder; 
            item.shootSpeed = 13;
            item.useAmmo = AmmoID.Arrow;
            item.reuseDelay = 25;
            item.width = 58;
            item.height = 22;
            item.crit = 11;
            item.UseSound = SoundID.Item5;
            item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(5) == 0)
            {
                int numberProjectiles = 1 + Main.rand.Next(2); // 4 or 5 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<GelOfLife>(), 7)
                .AddIngredient(ItemType<CastIronBar>(), 10)
                .AddTile(TileID.Anvils);
        }
    }
}