using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 飞鱼重机枪 : ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“人的成长就是战胜自己不成熟的过去”");
        }

        public override void SetDefaults()
        {
            item.damage = 66;
            item.ranged = true;
            item.width = 64;
            item.height = 32;
            item.useTime = 5;
            item.useAnimation = 5;

            // 下一条是使用方式，用于定义物品使用时的时动画
            // 1 代表挥动，也就是剑类武器！
            // 2 代表像药水一样喝下去(所以我们可以吞剑吞书吞回旋镖甚至吞剑了？)
            // 3 咸鱼突刺！(像同志短剑一样刺出去)
            // 4 神，赐予我力量吧！！(将物品提起，像使用生命水晶时那样的)
            // 5 手持枪、弓、法杖类武器的动作(这里就是这个)
            item.useStyle = 5;

            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 10;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;

            // 决定枪射出点什么(item.shoot)和射出抛射物的速度(item.shootSpeed)
            // 这里原作者让枪射出净化粉末，并且以 （16像素 / 帧） 的速度射出去 
            item.shoot = 10;
            item.shootSpeed = 5f;

            //需要使用的特定弹药(此处为所有子弹)
            //[重要]如果设置了消耗什么弹药，那么之前shoot设置的值就会被弹药物品的属性所覆盖
            //也就是说,他现在不会射出净化粉末...
            //同时也就是说，你到底射出的是什么就由弹药决定了！
            item.useAmmo = AmmoID.Bullet;
        }

        //添加一个酷毙的合成配方！
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(324, 2);//非法枪械部件
            recipe.AddIngredient(null, "飞鱼冲锋枪", 1);//飞鱼冲锋枪
            recipe.AddIngredient(3456, 18);//星璇碎片
            recipe.AddIngredient(3467, 20);//夜明锭
            recipe.AddIngredient(null, "碳钢枪械部件", 1);//碳钢枪械部件
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .80f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet) // 或者使用ProjectileID.WoodenArrowFriendly
            {
                type = 242; // 或者使用ProjectileID.FireArrow;
            }
            {

                int numberProjectiles = 1 + Main.rand.Next(2); // 4 or 5 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 242, damage, knockBack, player.whoAmI);
                }
            }
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
            }
            return true;
        }
    }
}