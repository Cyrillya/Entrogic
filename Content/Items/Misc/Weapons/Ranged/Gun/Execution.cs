using Entrogic.Content.Items.ContyElemental;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class Execution : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Execution");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "就地处决");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void SetDefaults() {
            Item.DefaultToRangedWeapon(ProjectileID.GelBalloon, AmmoID.Bullet, 50, 20f);
            Item.scale = 1.2f;
            Item.damage = 167;
            Item.width = 32;
            Item.height = 20;
            Item.knockBack = 18;
            Item.value = Item.sellPrice(gold: 1, silver: 80);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item40;
            Item.crit = 21;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            if (type == ProjectileID.Bullet) {
                type = ProjectileID.BulletHighVelocity;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoulofContamination>(), 7)
                .AddIngredient(ItemID.HallowedBar, 18)
                .AddIngredient(ItemID.Shotgun)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
