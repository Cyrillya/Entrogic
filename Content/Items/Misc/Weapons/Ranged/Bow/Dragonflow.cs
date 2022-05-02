using Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Bows;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Bow
{
    public class Dragonflow : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = RarityLevelID.EarlyHM;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DragonflowProj>();
            Item.shootSpeed = 15;
            Item.useAmmo = AmmoID.Arrow;
            Item.width = 58;
            Item.height = 22;
            Item.crit = -4;
            Item.UseSound = SoundID.Item39;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override Vector2? HoldoutOffset() => new Vector2(-8f, 0f);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (type == ProjectileID.WoodenArrowFriendly) { type = Item.shoot; damage += 10; }
            // 改自Player.cs中的海啸射击源码(1.4.0.5)
            int arrows = 3;
            velocity.Normalize();
            velocity *= 40f;
            bool flag4 = Collision.CanHit(position, 0, 0, position + velocity, 0, 0);
            for (int i = 0; i < arrows; i++) {
                float angleUnit = MathHelper.ToRadians(Math.Abs(player.velocity.Y) >= 12f ? 40f : Math.Abs(player.velocity.Y) / 12f * 20f + 20f);
                float degree = (float)i - ((float)arrows - 1f) / 2f;
                Vector2 offsetVec = velocity.RotatedBy((double)(angleUnit * degree), default(Vector2));
                if (!flag4) {
                    offsetVec -= velocity;
                }
                var proj = Projectile.NewProjectileDirect(source, position + offsetVec, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                proj.noDropItem = true;
            }
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .Register();
        }
    }
}
