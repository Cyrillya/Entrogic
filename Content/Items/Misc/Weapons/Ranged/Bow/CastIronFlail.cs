namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Bow
{
    public class CastIronFlail : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public override void SetDefaults() {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 13;
            Item.useAmmo = AmmoID.Arrow;
            Item.reuseDelay = 25;
            Item.width = 58;
            Item.height = 22;
            Item.crit = 11;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

            if (Main.rand.Next(2) == 0) {
                int numberProjectiles = 1 + Main.rand.Next(3);
                for (int i = 0; i < numberProjectiles; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemType<GelOfLife>(), 7)
            //    .AddIngredient(ItemType<CastIronBar>(), 10)
            //    .AddTile(TileID.Anvils);
        }
    }
}