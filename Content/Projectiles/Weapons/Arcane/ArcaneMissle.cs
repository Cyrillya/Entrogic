namespace Entrogic.Content.Projectiles.Weapons.Arcane
{
    public class ArcaneMissle : ProjectileBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("°ÂÊõ·Éµ¯");
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults() {
            Projectile.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
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
        }

        public override void Kill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item10);
            for (int i = 0; i < 8; i++) {
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
            float trailLength = ProjectileID.Sets.TrailCacheLength[Type];
            var color = new Color(255, 200, 255) * 0.6f;
            
            for (int i = 0; i < trailLength - 1; i++) {
                float factor = TrUtils.GetLerpValue(trailLength - 1, 0, i) * 0.6f;
                var pos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                var tex = ModHelper.GetTexture("Misc/RoundSmall").Value;
                Main.EntitySpriteDraw(tex, pos, null, color, 0f, tex.Size() / 2f, factor, SpriteEffects.None, 0);

                if (!(Main.gfxQuality >= 0.5f)) continue;
                
                pos = Vector2.Lerp(pos, Projectile.oldPos[i + 1] - Main.screenPosition + Projectile.Size / 2f, .5f);
                Main.EntitySpriteDraw(tex, pos, null, color, 0f, tex.Size() / 2f, factor, SpriteEffects.None, 0);
            }

            //Projectile.DrawShadow(lightColor, Math.Min(15, 2 + (int)Projectile.oldVelocity.Length()));
            return true;
        }

        public override void AI() {
            if (Main.netMode != NetmodeID.Server && Main.rand.NextBool(2)) {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, MyDustID.PurpleLingering, 0f, 0f, 100, default(Color), 1.5f);
                d.noGravity = true;
                d.fadeIn = 1.1f;
                d.velocity += Projectile.velocity;
                if (d.dustIndex != 6000) {
                    d = Dust.CloneDust(d);
                    d.scale /= 2f;
                    d.fadeIn *= 0.85f;
                    d.color = new Color(255, 255, 255, 255);
                    d.velocity *= 0.6f;
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
                LerpToTarget(bossTarget.Center);
            }
            else if (target != null) {
                LerpToTarget(target.Center);
            }
        }

        private void LerpToTarget(Vector2 targetPosition) {
                float targetR = (targetPosition - Projectile.Center).ToRotation();
                float selfR = Projectile.velocity.ToRotation();
                float dif = MathHelper.WrapAngle(targetR - selfR);
                float r = selfR + dif * 0.1f;
                Projectile.velocity = Projectile.velocity.Length() * r.ToRotationVector2();
        }
    }
}