namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class FlyingFishSubmachineGun : WeaponGun
    {
        public override Vector2? HoldoutOffset() => new Vector2(-6, -8);

        public override void ModifyGunDefaults(out int singleShotTime, out float shotVelocity, out bool autoReuse) {
            singleShotTime = 6;
            shotVelocity = 12f;
            autoReuse = true;

            Item.width = 62;
            Item.height = 32;
            Item.knockBack = 3.2f;
            Item.damage = 22;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.UseSound = SoundAssets.SubmachineGun;
            RecoilPower = 6;
            BarrelLength = 54;
            ShootDustDegree = 30;
            DustCount = 4;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            if (type == ProjectileID.Bullet) {
                type = ProjectileID.BulletHighVelocity;
            }
            velocity = velocity.RotateRandom(MathHelper.ToRadians(3));
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
    }
}