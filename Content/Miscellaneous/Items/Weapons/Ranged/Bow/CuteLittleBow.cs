using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Ranged.Bow
{
    public class CuteLittleBow : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public override void SetDefaults() {
            Item.DefaultToBow(20, 13f, true);
            Item.rare = RarityLevelID.EarlyHM;
            Item.damage = 43;
            Item.knockBack = 4;
            Item.width = 58;
            Item.height = 22;
            Item.crit = 24;
            Item.UseSound = SoundID.Item39;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Main.rand.NextBool(3)) {
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