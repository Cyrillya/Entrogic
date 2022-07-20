using Entrogic.Common.Globals.Players;
using Terraria.Graphics.CameraModifiers;

namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class WeaponGun : ItemBase
    {
        public abstract void ModifyGunDefaults(out int singleShotTime, out float shotVelocity, out bool autoReuse);

        public override void SetStaticDefaults() => SacrificeTotal = 1;

        public sealed override void SetDefaults() {
            ModifyGunDefaults(out int singleShotTime, out float shotVelocity, out bool autoReuse);
            Item.DefaultToRangedWeapon(10, AmmoID.Bullet, singleShotTime, shotVelocity, autoReuse);
            Item.holdStyle = ItemHoldStyleID.HoldFront;
        }

        protected int DustCount = 6;
        protected int RecoilPower;
        protected int BarrelLength;
        protected int ShootDustDegree;

        public override bool? UseItem(Player player) {
            player.GetModPlayer<GunPlayer>().RecoilDegree += RecoilPower;
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                ModNetHandler.PlayerData.SendRecoilDegree(-1, -1, (short)player.GetModPlayer<GunPlayer>().RecoilDegree);
            }

            var directionVector = player.MountedCenter.DirectionTo(Main.MouseWorld);

            PunchCameraModifier modifier = new(player.Center, directionVector, RecoilPower * 0.05f, 5f, 8, 600f, "Entrogic: Pistol");
            Main.instance.CameraModifiers.Add(modifier);

            for (int i = 0; i < DustCount; i++) {
                Vector2 offset = Vector2.Zero;
                ItemLoader.HoldoutOffset(player.gravDir, Type, ref offset);
                offset.RotatedBy(player.itemRotation);
                offset.Y -= 4f;

                var d = Dust.NewDustPerfect(player.Center + directionVector * BarrelLength + offset,
                                    DustID.FireworksRGB,
                                    directionVector.RotatedByRandom(MathHelper.ToRadians(ShootDustDegree)) * Main.rand.NextFloat(8, 13),
                                    100,
                                    new(187, 125, 70));
                d.noGravity = true;
            }

            return base.UseItem(player);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame) {
            float offsetRadians = MathHelper.ToRadians(player.GetModPlayer<GunPlayer>().RecoilDegree);
            offsetRadians = -offsetRadians * player.gravDir * player.direction;
            SetItemDirection(player, heldItemFrame, offsetRadians);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame) {
            float offsetRadians = MathHelper.ToRadians(player.GetModPlayer<GunPlayer>().RecoilDegree);
            offsetRadians = -offsetRadians * player.gravDir;
            SetItemDirection(player, heldItemFrame, offsetRadians);
        }

        public static void SetItemDirection(Player player, Rectangle heldItemFrame, float offsetRadians) {
            var pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            var mouseWorld = player.GetModPlayer<SyncedDataPlayer>().MouseWorld;
            var vectorToMouse = mouseWorld - pointPoisition;
            if (player.gravDir == -1f) // 原版算出来的
                vectorToMouse.Y = Main.screenPosition.Y * 2f + Main.screenHeight - mouseWorld.Y - pointPoisition.Y;

            int direction = Math.Sign(mouseWorld.X - player.MountedCenter.X);
            player.direction = direction;

            player.itemRotation = (float)Math.Atan2(vectorToMouse.X * direction, vectorToMouse.Y * direction) + player.fullRotation - 1.57f;
            if (player.gravDir == 1f) { // 修正，我也不知道原理
                player.itemRotation = 6.28f - player.itemRotation;
            }
            player.itemRotation += offsetRadians;

            player.itemLocation.X = player.position.X + player.width * 0.5f - heldItemFrame.Width * 0.5f - player.direction * 2;
            player.itemLocation.Y = player.MountedCenter.Y - heldItemFrame.Height * 0.5f;

            float armRotation = player.itemRotation - 1.57f * player.direction;
            if (player.gravDir == -1f) { // 修正，我也不知道原理
                armRotation = 3.14f - armRotation;
            }
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotation);
            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Quarter, armRotation);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Vector2 offset = Vector2.Zero;
            ItemLoader.HoldoutOffset(player.gravDir, Type, ref offset);
            position += offset;
        }
    }
}
