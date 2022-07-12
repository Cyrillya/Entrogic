using Terraria;
namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class FlyingFishSubmachineGun : ModItem
    {
        public override Vector2? HoldoutOffset() => new Vector2(-7, 0);

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flying Fish Submachine Gun");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "飞鱼冲锋枪");
            Tooltip.SetDefault("“来自东亚弧形群岛的美食”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 32;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.SoulofMight, 18)
            //    .AddIngredient(ItemID.SoulofFright, 18)
            //    .AddIngredient(ItemID.SoulofSight, 18)
            //    .AddIngredient(null,"飞鱼手枪", 1)
            //    .AddIngredient(ItemID.HallowedBar, 20)
            //    .AddIngredient(null, "碳钢枪械部件", 1)//碳钢枪械部件
            //    .AddTile(TileID.MythrilAnvil)
            //    .Register();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .66f;

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            if (type == ProjectileID.Bullet) {
                type = ProjectileID.BulletHighVelocity;
            }
            velocity.RotateRandom(MathHelper.ToRadians(3));
        }
    }
}