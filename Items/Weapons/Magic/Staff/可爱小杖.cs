using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class 可爱小杖 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/7BFFFF:嗯..挥起来很快，所以释放法术自然也快]");
        }

        public override void SetDefaults()
        {
            item.damage = 44;
            item.magic = true;
            item.mana = 3;
            item.width = 28;
            item.height = 28;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 1, 50);
            item.rare = 6;
            item.UseSound = SoundID.Item35;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("可爱魔法流");
            item.shootSpeed = 14f;
            item.crit = 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.Wire, 12);
            recipe.AddIngredient(null, "CuteWidget", 1);
            recipe.AddRecipeGroup("IronBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}