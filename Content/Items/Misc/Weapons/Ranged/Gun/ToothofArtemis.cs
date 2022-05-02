using Entrogic.Content.Projectiles.Misc.Weapons.Arcane;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    // 阿荻米斯之牙
    public class ToothofArtemis : ModItem
    {
        public override Vector2? HoldoutOffset() {
            return new Vector2(-7, 0);
        }
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("汇聚了泰拉大陆上最顶尖的技术（迫真）\n“还处于试验阶段”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 34;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 36;
            Item.useTime = 7;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = 140000;
            Item.rare = RarityLevelID.MiddleHM;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 6f;
            Item.reuseDelay = 20;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Player player) => Main.rand.NextFloat() >= .15f;

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Main.rand.Next(5) == 0) {
                int numberProjectiles = 2 + Main.rand.Next(2); // 2 or 3 shots
                for (int i = 0; i < numberProjectiles; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(9));
                    Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<ArcaneMissle>(), damage, knockback, player.whoAmI);
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotateRandom(MathHelper.ToRadians(5));
        }


        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.Megashark, 1)//巨兽鲨
            //    .AddRecipeGroup("Entrogic:AdamantiteBar", 12)
            //    .AddIngredient(ItemType<碳钢枪械部件>(), 1)
            //    .AddTile(TileID.MythrilAnvil)
            //    .Register();
        }
    }
}