using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class GoldenHarvest : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.UseSound = SoundID.Item1;
            Item.Size = new Vector2(168, 84);
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 1, 24, 0);
            Item.rare = RarityLevelID.LatePHM;
            Item.damage = 36;
            Item.crit = -2;
            Item.knockBack = 7f;
            Item.shoot = ModContent.ProjectileType<GoldenHarvestProjectile>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
        }
    }
}
