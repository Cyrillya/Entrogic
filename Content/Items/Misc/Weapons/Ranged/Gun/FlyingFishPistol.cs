using Terraria;
using Terraria.ID;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class FlyingFishPistol : ModItem
    {
        public override Vector2? HoldoutOffset() => new Vector2(-7, 0);

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flying Fish Pistol");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "飞鱼手枪");
            Tooltip.SetDefault("“什么残忍的人才会把枪管插到一只可怜的飞鱼嘴里？”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 49;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 54;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            //CreateRecipe()
            //    .AddIngredient(ItemID.ZephyrFish, 1)//飞鱼宠物
            //    .AddIngredient(ItemType<GelOfLife>(), 3)
            //    .AddIngredient(ItemID.IllegalGunParts, 1)//非法枪械部件
            //    .AddIngredient(ItemID.Handgun,1)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .1f;

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotateRandom(MathHelper.ToRadians(2));
        }
    }
}