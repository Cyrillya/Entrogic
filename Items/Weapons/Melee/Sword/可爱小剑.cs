using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class 可爱小剑 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/7BFFFF:轻盈小巧的一把短剑]\n" +
                "[c/7BFFFF:“使用时可以格挡部分伤害”]");
        }

        public override void SetDefaults()
        {
            item.damage = 51;           //The damage of your weapon 武器的伤害
            item.width = 34;            //Weapon's texture's width 武器材质的宽度
            item.height = 34;           //Weapon's texture's height 武器材质的高度
            item.useTime = 6;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 6;         //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
            item.useStyle = 5;
            item.knockBack = 4;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.value = Item.sellPrice(0, 1, 50);
            item.rare = 6;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13   //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = false;
            item.channel = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("可爱剑光");
            item.shootSpeed = 20f;
            item.noUseGraphic = true;
            item.melee = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.Wire, 12);
            //recipe.AddIngredient(null, "CuteWidget", 1);
            recipe.AddRecipeGroup("IronBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool UseItem(Player player)
        {
            player.statDefense = (int)((float)player.statDefense * 1.3f);
            return true;
        }
    }
}