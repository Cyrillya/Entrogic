using Entrogic.Items.Materials;
using Entrogic.Items.VoluGels;
using Entrogic.Tiles;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class GelCrossbowArrowMachineGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("凝胶弩箭机枪");
            Tooltip.SetDefault("“没错，一把射箭的机枪”\n" +
                "小心枪口过热");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 5;
            item.DamageType = DamageClass.Ranged;
            item.useTime = 5;
            item.useAnimation = 5;
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
            item.shootSpeed = 20f;
            item.useAmmo = AmmoID.Arrow;
            item.width = 58;
            item.height = 22;
            item.crit = 15;
            item.UseSound = SoundID.Item39;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
        //public float Timer = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float rotation = MathHelper.ToRadians(2);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            int numbersProjectiles = 1 + Main.rand.Next(3); // [1, 3] shots
            for (int i = 0; i < numbersProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-6, 6 + 1)));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        /*public override bool UseItem(Player player)
        {
            Timer++;
            if (Timer == 118)
            {
                Timer = 0;
            }
            return true;
        }*/
        public override void AddRecipes()
        {
<<<<<<< HEAD
            CreateRecipe()
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddIngredient(ItemID.Gel, 30)
                .AddIngredient(ItemType<CastIronBar>(), 5)
                .AddIngredient(ItemType<GelOfLife>(), 7)
                .AddRecipeGroup("IronBar", 10)
                .AddTile(TileType<MagicDiversionPlatformTile>())
                .Register();
=======
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Gel, 30);
            recipe.AddIngredient(ItemType<CastIronBar>(), 5);
            recipe.AddIngredient(ItemType<GelOfLife>(), 7);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddTile(TileType<MagicDiversionPlatformTile>());
            recipe.SetResult(this);
            recipe.AddRecipe();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
    }
}