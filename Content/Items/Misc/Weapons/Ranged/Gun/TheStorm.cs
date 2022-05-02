using Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Bullets;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class TheStorm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Storm");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暴风雪");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = RarityLevelID.MiddleHM;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SnowStormy>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Snowball;
            Item.width = 58;
            Item.height = 22;
            Item.crit = 24;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0f);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            if (type == ProjectileID.SnowBallFriendly) { type = Item.shoot; damage += 17; }
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

            // 改自Player.cs中的海啸射击源码(1.4.0.5)
            int arrows = 2;
            velocity.Normalize();
            velocity *= 40f;
            bool flag4 = Collision.CanHit(position, 0, 0, position + velocity, 0, 0);
            for (int i = 0; i < arrows; i++) {
                float angleUnit = MathHelper.ToRadians(20f);
                float degree = (float)i - ((float)arrows - 1f) / 2f;
                Vector2 offsetVec = velocity.RotatedBy((double)(angleUnit * degree), default(Vector2));
                if (!flag4) {
                    offsetVec -= velocity;
                }
                int proj = Projectile.NewProjectile(source, position + offsetVec, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                Main.projectile[proj].noDropItem = true;
            }
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SnowBlock, 100)
                .AddIngredient(ItemID.SoulofLight, 12)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient(ItemID.Wire, 10)
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
