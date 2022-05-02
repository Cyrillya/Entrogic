using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;
using Terraria.ID;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class Arsonist : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.damage = 56;
            Item.width = 42;
            Item.height = 54;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = RarityLevelID.EarlyHM;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Arsonists>();
            Item.shootSpeed = 6f;
            Item.DamageType = DamageClass.Melee;
            Item.noUseGraphic = false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.Next(35) == 0) {
                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.InfernoFork);
                dust.noGravity = true;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotateRandom(MathHelper.ToRadians(5));
        }
    }
}