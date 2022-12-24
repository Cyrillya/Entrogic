using Entrogic.Content.Miscellaneous.Projectiles.Ranged.Thrown;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Ranged.Thrown
{
    public class LeafbladeItem : ItemBase
    {
        public override void SetStaticDefaults() => SacrificeTotal = 100;

        public override void SetDefaults()
        {
            Item.damage = 5;       
            Item.width = 28;         
            Item.height = 22;          
            Item.useTime = 12;          
            Item.useAnimation = 12;     
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;         
            Item.value = Item.sellPrice(copper: 5);
            Item.rare = ItemRarityID.Green;              
            Item.UseSound = SoundID.Item1;      
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Leafblade>();
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = true;
            Item.crit = 19;
            Item.consumable = true;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddRecipeGroup(RecipeGroupID.Wood, 1)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
        }
    }
}
