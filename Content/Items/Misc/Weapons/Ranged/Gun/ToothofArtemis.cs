using Entrogic.Content.Projectiles.Misc.Weapons.Arcane;
using Terraria;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class ToothofArtemis : ItemBase
    {
        public override Vector2? HoldoutOffset() => new Vector2(-7, 0);

        public override void SetStaticDefaults() {
            SacrificeTotal = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.ClockworkAssaultRifle);
            Item.DefaultToRangedWeapon(ProjectileID.GelBalloon, AmmoID.Bullet, 28, 6f, true);
            Item.damage = 34;
            Item.width = 64;
            Item.height = 36;
            Item.useTime = 7;
            Item.value = 140000;
            Item.reuseDelay = 10;
            Item.rare = RarityLevelID.MiddleHM;
        }

        // 仅在第一发消耗弹药，而且有15%几率不消耗
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .15f && player.itemAnimation == player.itemAnimationMax;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            position += Vector2.Normalize(velocity) * 16f; // 往枪口位置靠
            if (player.itemAnimation == Item.useAnimation - Item.useTime * 3) { // 第四下发射奥数飞弹
                int numberProjectiles = 2 + Main.rand.Next(2); // 2 or 3 shots
                for (int i = 0; i < numberProjectiles; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(9));
                    var p = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item),
                                                           position,
                                                           perturbedSpeed,
                                                           ModContent.ProjectileType<ArcaneMissle>(),
                                                           player.GetWeaponDamage(Item),
                                                           Item.knockBack,
                                                           player.whoAmI);
                    p.DamageType = Item.DamageType;
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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