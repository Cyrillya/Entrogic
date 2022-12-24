using Entrogic.Content.Contaminated.Weapons;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Friendly
{
    public class ContyCurrentProj2 : ProjectileBase
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Main.projFrames[Type] = 4;
            DisplayName.SetDefault("Electric Current");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "电流");
        }

        public override void SetDefaults() {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = false;
            _weapon = ModContent.ItemType<ContyCurrent>();
        }

        public override bool? CanDamage() => false;

        private int _weapon;
        public override void AI() {
            Player player = Main.player[Projectile.owner];
            // 发出蓝光
            Lighting.AddLight(Projectile.position, 49 / 255f, 94 / 255f, 227 / 255f);
            if (!Main.dedServ) {
                Projectile.localAI[0] += 0.15f;
                Projectile.localAI[0] %= Main.projFrames[Type];
                Projectile.frame = (int)Projectile.localAI[0];
            }
            // 玩家死亡或玩家没有手持电流，射弹消失
            if (Projectile.ai[0] == 0f && (player.dead || !player.active || player.HeldItem.type != _weapon)) {
                Projectile.ai[0] = 1f;
                Projectile.timeLeft = 60;
            }
            if (Projectile.Distance(player.Center) >= 16 * 100) {
                Projectile.Center = player.Center;
                Projectile.netUpdate = true;
            }

            // 取自骷髅王头的悬停
            Projectile.rotation = Projectile.velocity.X / 15f;
            float accelerationY = 0.06f;
            float maxSpeedY = 2f;
            float accelerationX = 0.11f;
            float maxSpeedX = 3.5f;

            // X轴速度
            if (Projectile.Center.X > player.Center.X) {
                if (Projectile.velocity.X > 0f)
                    Projectile.velocity.X *= 0.98f;

                Projectile.velocity.X -= accelerationX;
                if (Projectile.velocity.X > maxSpeedX)
                    Projectile.velocity.X = maxSpeedX;
            }

            if (Projectile.Center.X < player.Center.X) {
                if (Projectile.velocity.X < 0f)
                    Projectile.velocity.X *= 0.98f;

                Projectile.velocity.X += accelerationX;
                if (Projectile.velocity.X < 0f - maxSpeedX)
                    Projectile.velocity.X = 0f - maxSpeedX;
            }

            // Y轴速度
            if (Projectile.timeLeft < 60) {
                Projectile.velocity.Y -= 0.7f;
            }
            else {
                if (Projectile.Center.Y > player.Center.Y - 150f) {
                    if (Projectile.velocity.Y > 0f)
                        Projectile.velocity.Y *= 0.98f;

                    Projectile.velocity.Y -= accelerationY;
                    if (Projectile.velocity.Y > maxSpeedY)
                        Projectile.velocity.Y = maxSpeedY;
                }
                else if (Projectile.Center.Y < player.Center.Y - 150f) {
                    if (Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y *= 0.98f;

                    Projectile.velocity.Y += accelerationY;
                    if (Projectile.velocity.Y < 0f - maxSpeedY)
                        Projectile.velocity.Y = 0f - maxSpeedY;
                }
            }
        }
    }
}
