using Terraria.Audio;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords
{
    public class GoldenHarvestProjectile : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Type] = 7;

        public override void SetDefaults() {
            Projectile.width = 168;
            Projectile.height = 84;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1f;
            Projectile.alpha = 35;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox) {
            base.ModifyDamageHitbox(ref hitbox);
            hitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y + 18, 168, 40);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (crit && target.lifeMax >= 10 && target.lifeMax <= 100000 && !target.friendly && target.value > 10) {
                // 金史莱姆那抄来的假钱
                int num27 = 7;
                float num28 = 1.1f;
                int num29 = 10;
                Color newColor6 = default(Color);
                if (target.life <= 0) {
                    num28 = 1.5f;
                    num27 = 40;
                    for (int num30 = 0; num30 < 8; num30++) {
                        int num31 = Gore.NewGore(new Vector2(target.position.X, target.Center.Y - 10f), Vector2.Zero, 1218);
                        Main.gore[num31].velocity = new Vector2((float)Main.rand.Next(1, 10) * 0.3f * 2.5f * (float)hitDirection, 0f - (3f + (float)Main.rand.Next(4) * 0.3f));
                    }
                }
                else {
                    for (int num32 = 0; num32 < 3; num32++) {
                        int num33 = Gore.NewGore(new Vector2(target.position.X, target.Center.Y - 10f), Vector2.Zero, 1218);
                        Main.gore[num33].velocity = new Vector2((float)Main.rand.Next(1, 10) * 0.3f * 2f * (float)hitDirection, 0f - (2.5f + (float)Main.rand.Next(4) * 0.3f));
                    }
                }

                for (int num34 = 0; num34 < num27; num34++) {
                    int num35 = Dust.NewDust(target.position, target.width, target.height, num29, 2 * hitDirection, -1f, 80, newColor6, num28);
                    if (Main.rand.Next(3) != 0)
                        Main.dust[num35].noGravity = true;
                }
                // 每次有1铜币到5银币不等的额外钱币
                int copperCoin = Main.rand.Next(1, 100);
                int silverCoin = Main.rand.Next(0, 5);
                Item.NewItem(target.getRect(), ItemID.CopperCoin, copperCoin);
                if (silverCoin > 0)
                    Item.NewItem(target.getRect(), ItemID.SilverCoin, silverCoin);
                // 暴击设为三倍伤害，由于暴击伤害加成在此之后计算，所以是*1.5
                damage = (int)(damage * 1.5f);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI() {
            Projectile.alpha = 64;
            Player player = Main.player[Projectile.owner];

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 3) {
                Projectile.frame++;
                Projectile.ai[0] = 0;
            }
            if (Projectile.frame == 8) {
                Projectile.Kill();
            }
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0) {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 21;
            }
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.position += Projectile.rotation.ToRotationVector2() * 80f;
            if (Projectile.spriteDirection == -1) {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
}
