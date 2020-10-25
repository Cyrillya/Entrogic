using Entrogic.Items.VoluGels;
using Entrogic.Tiles;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 飞鱼手枪 : ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“什么残忍的人才会把枪管插到一只可怜的飞鱼嘴里？”");
        }

        public override void SetDefaults()
        {
            item.damage = 49;
            item.DamageType = DamageClass.Ranged;
            item.width = 54;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.Shoot;

            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = ProjectileID.WaterGun;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ZephyrFish, 1)//飞鱼宠物
                .AddIngredient(ItemType<GelOfLife>(), 3)
                .AddIngredient(ItemID.IllegalGunParts, 1)//非法枪械部件
                .AddIngredient(ItemID.Handgun,1)
                .AddTile(TileType<MagicDiversionPlatformTile>())
                .Register();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .1f;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2));
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
                return true;
            }
        }
    }
}