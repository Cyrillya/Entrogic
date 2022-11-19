namespace Entrogic.Content.Projectiles.Misc.Weapons.Arcane
{
    public class ArcaneMissle : ArcaneProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("°ÂÊõ·Éµ¯");
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
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
            SoundEngine.PlaySound(SoundID.Item10);
            for (int i = 0; i < 16; i++) {
                var d = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 100, default(Color), 2f);
                d.noGravity = true;
                d.velocity *= 3f;
                if (d.dustIndex != 6000 && d.dustIndex < Main.maxDustToDraw * 0.4f) {
                    d = Dust.CloneDust(d);
                    d.scale /= 2f;
                    d.fadeIn = 0.35f + (float)Main.rand.Next(10) * 0.1f;
                    d.color = new Color(255, 255, 255, 255);
                    d.velocity *= 0.6f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.DrawShadow(lightColor, Math.Min(15, 2 + (int)Projectile.oldVelocity.Length()));
            return false;
        }

        public override void AI() {
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 2; i++) {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, MyDustID.PurpleLingering, 0f, 0f, 100, default(Color), 2f);
                    d.noGravity = true;
                    d.fadeIn = 1.3f;
                    d.velocity += Projectile.velocity;
                    d.scale *= 1.2f;
                    if (d.dustIndex != 6000) {
                        d = Dust.CloneDust(d);
                        d.scale /= 2f;
                        d.fadeIn *= 0.85f;
                        d.color = new Color(255, 255, 255, 255);
                        d.velocity *= 0.6f;
                    }
                }
            }

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
                    }
                    else {
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
            }
            else if (target != null) {
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