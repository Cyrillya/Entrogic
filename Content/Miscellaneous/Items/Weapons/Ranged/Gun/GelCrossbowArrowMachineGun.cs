namespace Entrogic.Content.Miscellaneous.Items.Weapons.Ranged.Gun
{
    public class GelCrossbowArrowMachineGun : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("凝胶弩箭机枪");
            Tooltip.SetDefault("“没错，一把射箭的机枪”\n" +
                "小心枪口过热");
        }

        public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

        public override void SetDefaults() {
            Item.channel = true;
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.LightPurple;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 58;
            Item.height = 22;
            Item.crit = 15;
            Item.UseSound = SoundID.Item39;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            float rotation = MathHelper.ToRadians(2);
            position += Vector2.Normalize(velocity) * 45f;
            int numbersProjectiles = 1 + Main.rand.Next(3); // [1, 3] shots
            for (int i = 0; i < numbersProjectiles; i++) {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-6, 6 + 1)));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.IllegalGunParts, 1)
            //    .AddIngredient(ItemID.Gel, 30)
            //    .AddIngredient(ItemType<CastIronBar>(), 5)
            //    .AddIngredient(ItemType<GelOfLife>(), 7)
            //    .AddRecipeGroup("IronBar", 10)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }
    }
}