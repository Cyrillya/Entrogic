using Entrogic.Content.Projectiles.Misc.Weapons.Ranged.Thrown;
using Terraria.ID;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Thrown
{
    public class LeafbladeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“飞花摘叶皆可伤人”\n" +
                "早已失传的武功");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            Item.crit += 19;
            Item.consumable = true;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddRecipeGroup("Wood", 1)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Grass);
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotatedByRandom(MathHelper.ToRadians(5));
        }
    }
}
