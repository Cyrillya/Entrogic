using Entrogic.Content.Items.Misc.Materials;
using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class CuteLittleSword : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 51;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 1, 50);
            Item.rare = RarityLevelID.EarlyHM;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<CuteBlade>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.scale = 1.2f;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.Wire, 12)
                .AddRecipeGroup("IronBar", 4)
                .AddIngredient(ModContent.ItemType<CuteWidget>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        public override bool? UseItem(Player player) {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI)
                player.statDefense = (int)((float)player.statDefense * 1.3f);
            return base.UseItem(player);
        }
    }
}