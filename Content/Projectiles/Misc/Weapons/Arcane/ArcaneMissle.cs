namespace Entrogic.Content.Projectiles.Misc.Weapons.Arcane
{
    public class ArcaneMissle : ArcaneProjectile
    {
        public override Color? GetAlpha(Color lightColor) {
            return ((Color)base.GetAlpha(lightColor)).MultiplyRGB(Color.Pink);
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("°ÂÊõ·Éµ¯");     //The English name of the Projectile
            ProjectileID.Sets.TrailCacheLength[Type] = 20;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Type] = 0;        //The recording mode
        }

        public override void ArcaneDefaults() {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 10;
            Projectile.light = 3f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = -1;
        }

        public override void Kill(int timeLeft) {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10);
            for (int i = 0; i < 22; i++) {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 3f;
                if (Main.rand.Next(2) == 0) {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, Math.Min(15, 2 + (int)Projectile.oldVelocity.Length()));
            return false;
        }

        public override void AI() {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, MyDustId.PurpleLingering, 0f, 0f, 100, default(Color), 2f);
            dust.noGravity = true;
            NPC target = null;
            NPC bossTarget = null;
            float distanceMax = 800f;
            float distanceMaxBoss = 1000f;
            foreach (NPC npc in Main.npc) {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.type != NPCID.TargetDummy) {
                    float currentDistance = Vector2.Distance(npc.Center, Main.MouseWorld);
                    if (!npc.boss) {
                        if (currentDistance < distanceMax) {
                            distanceMax = currentDistance;
                            target = npc;
                        }
                    } else {
                        if (currentDistance < distanceMaxBoss) {
                            distanceMaxBoss = currentDistance;
                            bossTarget = npc;
                        }
                    }
                }
            }
            if (bossTarget != null) {
                var targetPos = (bossTarget == null) ? Vector2.Zero : bossTarget.Center;
                float targetR = (targetPos - Projectile.Center).ToRotation();
                float selfR = Projectile.velocity.ToRotation();
                float dif = MathHelper.WrapAngle(targetR - selfR);
                float r = selfR + dif * 0.3f;
                Projectile.velocity = Projectile.velocity.Length() * r.ToRotationVector2();
            } else if (target != null) {
                var targetPos = (target == null) ? Vector2.Zero : target.Center;
                float targetR = (targetPos - Projectile.Center).ToRotation();
                float selfR = Projectile.velocity.ToRotation();
                float dif = MathHelper.WrapAngle(targetR - selfR);
                float r = selfR + dif * 0.3f;
                Projectile.velocity = Projectile.velocity.Length() * r.ToRotationVector2();
            }
        }
    }
}