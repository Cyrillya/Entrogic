using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class CuteLittleStaff : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 44;
            item.DamageType = DamageClass.Magic;
            item.mana = 3;
            item.width = 28;
            item.height = 28;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = ItemUseStyleID.Swing;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 1, 50);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item35;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.Magic.Staff.可爱魔法流>();
            item.shootSpeed = 14f;
            item.crit = 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.Wire, 12)
                .AddIngredient(Mod, "CuteWidget", 1)
                .AddRecipeGroup("IronBar", 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}