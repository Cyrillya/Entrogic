using Entrogic.Content.Miscellaneous.Items.Materials;
using Entrogic.Content.Symbiosis;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Melee.Swords
{
    public class CastIronSword : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public override void SetDefaults() {
            Item.damage = 43;
            Item.knockBack = 4f;
            Item.crit += 29;
            Item.rare = ItemRarityID.Orange;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.2f;
            Item.width = 48;
            Item.height = 48;
            Item.maxStack = 1;
            Item.useTurn = true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GelOfLife>(), 7)
                .AddIngredient(ModContent.ItemType<CastIronBar>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if (Main.rand.Next(4) == 0) {
                target.AddBuff(31, 180);
            }
        }
    }
}