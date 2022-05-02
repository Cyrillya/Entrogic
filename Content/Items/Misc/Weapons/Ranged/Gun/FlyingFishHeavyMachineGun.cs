namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class FlyingFishHeavyMachineGun : ModItem
    {
        public override Vector2? HoldoutOffset() => new Vector2(-9, 0);

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flying Fish Heavy Machine Gun");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "飞鱼重机枪");
            Tooltip.SetDefault("“人的成长就是战胜自己不成熟的过去”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 66;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.IllegalGunParts, 2)//非法枪械部件
            //    .AddIngredient(Mod, "飞鱼冲锋枪", 1)//飞鱼冲锋枪
            //    .AddIngredient(ItemID.FragmentVortex, 18)//星璇碎片
            //    .AddIngredient(ItemID.LunarBar, 20)//夜明锭
            //    .AddIngredient(Mod, "碳钢枪械部件", 1)//碳钢枪械部件
            //    .AddTile(TileID.LunarCraftingStation)
            //    .Register();
        }

        public override bool CanConsumeAmmo(Player player) => Main.rand.NextFloat() >= .80f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int numberProjectiles = 1 + Main.rand.Next(2);
            for (int i = 0; i < numberProjectiles; i++) {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(5)), ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}