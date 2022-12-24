using Entrogic.Content.Miscellaneous.Projectiles.Melee.Swords;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items.Weapons.Melee.Swords
{
    public class SkylineItem : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("黎明的利刃将永夜撕裂");
        }

        public override void SetDefaults() {
            Item.damage = 79;
            Item.width = 42;
            Item.height = 54;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 40);
            Item.rare = RarityLevelID.LateMoon;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Skyline>();
            Item.shootSpeed = 13f;
            Item.DamageType = DamageClass.Melee;
            Item.noUseGraphic = false;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 18)
                .AddIngredient(ItemID.FragmentSolar, 15)
                .AddIngredient(ItemID.MagicDagger, 1)
                .AddIngredient(ItemID.ShadowFlameKnife, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.Next(35) == 0) {
                //Emit dusts when swing the sword
                //使用时出现粒子效果
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotateRandom(MathHelper.ToRadians(5));
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.Daybreak, 300);
        }
    }
}
//啊 啊 啊 啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊
//野 兽 咆 哮（半恼）