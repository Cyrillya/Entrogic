using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Ranged.Bow
{
    public class Galekylin : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public override void SetDefaults() {
            Item.DefaultToBow(25, 16f, true);
            Item.damage = 127;
            Item.crit = 26;
            Item.width = 32;
            Item.height = 64;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Tsunami, 1)
                .AddIngredient(ItemID.DD2PhoenixBow, 1)
                .AddIngredient(ItemID.FragmentVortex, 20)
                .AddIngredient(ItemID.FragmentSolar, 20)
                .AddIngredient(ItemID.LunarBar, 18)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .33f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
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