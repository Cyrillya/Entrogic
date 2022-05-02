namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Bow
{
    public class Galekylin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("不朽圣火的力量，好好利用吧");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 89;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.crit += 26;
            Item.height = 64;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Tsunami, 1)
                .AddIngredient(ItemID.DD2PhoenixBow, 1)
                .AddIngredient(ItemID.FragmentVortex, 20)
                .AddIngredient(ItemID.FragmentSolar, 20)
                .AddIngredient(ItemID.LunarBar, 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool CanConsumeAmmo(Player player) => Main.rand.NextFloat() >= .33f;

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2PhoenixBowShot, 250, knockback, player.whoAmI);

            float numberProjectiles = 3 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(3);
            position += Vector2.Normalize(velocity) * 45f;
            for (int i = 0; i < numberProjectiles; i++) {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // 如果只有1个弹丸，请注意除以0。
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }

            return false;
        }
    }
}