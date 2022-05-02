using Entrogic.Items.Materials;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class CuteLittleBow : ModItem
    {
<<<<<<< HEAD
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public override void SetDefaults()
        {
            item.damage = 43;
            item.DamageType = DamageClass.Ranged;
            item.useTime = 20;
            item.useAnimation = 20;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.noMelee = true; 
            item.knockBack = 4;
            item.rare = ItemRarityID.LightPurple;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder; 
            item.shootSpeed = 13;
            item.useAmmo = AmmoID.Arrow;
            item.width = 58;
            item.height = 22;
            item.crit = 24;
            item.UseSound = SoundID.Item39;
            item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(3) == 0)
            {
                int numbersProjectiles = 1 + Main.rand.Next(1); // 4 or 5 shots
                for (int i = 0; i < numbersProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
            }
            float numberProjectiles = 1 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(2);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // 如果只有1个弹丸，请注意除以0。
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.Wire, 12)
                .AddIngredient(ItemType<CuteWidget>(), 1)
                .AddRecipeGroup("IronBar", 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}