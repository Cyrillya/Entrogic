namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Bow
{
    public class CuteLittleBow : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.damage = 43;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.LightPurple;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 13;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 58;
            Item.height = 22;
            Item.crit = 24;
            Item.UseSound = SoundID.Item39;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Main.rand.Next(3) == 0) {
                int numbersProjectiles = 1 + Main.rand.Next(1); 
                for (int i = 0; i < numbersProjectiles; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
            }
            float numberProjectiles = 2 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(2);
            position += Vector2.Normalize(velocity) * 45f;
            for (int i = 0; i < numberProjectiles; i++) {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            return true;
        }
        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.SoulofLight, 6)
            //    .AddIngredient(ItemID.Wire, 12)
            //    .AddIngredient(ItemType<CuteWidget>(), 1)
            //    .AddRecipeGroup("IronBar", 4)
            //    .AddTile(TileID.MythrilAnvil)
            //    .Register();
        }
    }
}