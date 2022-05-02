using Terraria.ID;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class BurningWoodenSword : ModItem
    {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("“十分的烫手”\n点燃你的敌人");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 8;
            Item.knockBack = 2f;
            Item.crit += 9;
            Item.rare = ItemRarityID.White;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.4f;
            Item.width = 40;
            Item.height = 34;
            Item.maxStack = 1;
            Item.autoReuse = false;
            Item.useTurn = true;
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