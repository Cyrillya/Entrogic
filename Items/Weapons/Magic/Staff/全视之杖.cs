using Entrogic.Items.VoluGels;

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
            item.DamageType = DamageClass.Magic;
            item.mana = 10;
            item.width = 66;
            item.height = 22;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.Shoot;
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
            CreateRecipe()
                .AddIngredient(ItemID.MagicMissile, 1)//海蓝法杖
                .AddIngredient(ItemType<GelOfLife>() ,7)
                .AddIngredient(ItemID.Lens, 8)//晶状体
                .AddTile(TileType<Tiles.MagicDiversionPlatformTile>())
                .Register();
        }
    }
}