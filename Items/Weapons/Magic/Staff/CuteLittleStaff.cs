using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class CuteLittleStaff : ModItem
    {
<<<<<<< HEAD:Items/Weapons/Magic/Staff/CuteLittleStaff.cs
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Magic/Staff/可爱小杖.cs
        public override void SetDefaults()
        {
            item.damage = 44;
            item.DamageType = DamageClass.Magic;
            item.mana = 3;
            item.width = 28;
            item.height = 28;
            item.useTime = 12;
            item.useAnimation = 12;
<<<<<<< HEAD:Items/Weapons/Magic/Staff/CuteLittleStaff.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Magic/Staff/可爱小杖.cs
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