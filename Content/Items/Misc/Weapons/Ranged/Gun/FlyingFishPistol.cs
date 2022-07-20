namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class FlyingFishPistol : WeaponGun
    {
        public override Vector2? HoldoutOffset() => new Vector2(-6, -8);

        public override void ModifyGunDefaults(out int singleShotTime, out float shotVelocity, out bool autoReuse) {
            singleShotTime = 16;
            shotVelocity = 10f;
            autoReuse = false;

            Item.width = 52;
            Item.height = 32;
            Item.knockBack = 4;
            Item.damage = 49;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.UseSound = SoundAssets.Pistol;
            RecoilPower = 16;
            BarrelLength = 50;
            ShootDustDegree = 30;
            DustCount = 6;
        }

        public override void AddRecipes() {
            //CreateRecipe()
            //    .AddIngredient(ItemID.ZephyrFish, 1)//飞鱼宠物
            //    .AddIngredient(ItemType<GelOfLife>(), 3)
            //    .AddIngredient(ItemID.IllegalGunParts, 1)//非法枪械部件
            //    .AddIngredient(ItemID.Handgun,1)
            //    .AddTile(TileType<MagicDiversionPlatformTile>())
            //    .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            velocity.RotateRandom(MathHelper.ToRadians(5));
        }
    }
}