using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class 全视之杖 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("远古邪神之权杖");
        }

        public override void SetDefaults()
        {
            item.damage = 19;
            item.magic = true;
            item.mana = 10;
            item.width = 66;
            item.height = 22;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 10000;
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.Magic.Staff.着魔眼珠>();
            item.shootSpeed = 14f;
            item.crit = 19;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MagicMissile, 1);//海蓝法杖
            recipe.AddIngredient(null,"GelOfLife" ,7);
            recipe.AddIngredient(ItemID.Lens, 8);//晶状体
            recipe.AddTile(TileType<Tiles.MagicDiversionPlatformTile>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}