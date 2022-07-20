namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class BurningWoodenSword : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.WoodenSword);
            Item.damage = 8;
            Item.knockBack = 2f;
            Item.crit = 9;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.scale = 1.4f;
            Item.width = 40;
            Item.height = 34;
            Item.useTurn = false;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ItemID.WoodenSword, 1)//木剑
                .AddIngredient(ItemID.Torch, 8)//火把
                .AddTile(TileID.WorkBenches)//工作台
                .Register();

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) => target.AddBuff(24, 240);

        public override void MeleeEffects(Player player, Rectangle hitbox) => Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Torch, 0, 0, 90, default(Color), 0.65f);
    }
}