using Entrogic.Common.WorldGeneration;
using Entrogic.Content.NPCs.Enemies.Athanasy;
using Terraria.Audio;

namespace Entrogic.Content.Projectiles.Athanasy
{
    public class TitansOrderProj : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
        }

        public ref float Timer => ref Projectile.ai[0];

        public override void AI() {
            Timer++;
            if (Timer < 100f) {
                Projectile.velocity = Vector2.Zero;
                return;
            }

            Projectile.velocity = new(0f, 0.9f);
            Projectile.scale -= 0.02f;

            if (Timer == 150f) {
                Projectile.hide = true;
                Projectile.netUpdate = true;
                AthanasySystem.TileRunner.Run(AthanasySystem.ActuateTiles(false, 10, 90));
            }

            if (Timer >= 150f) {
                Projectile.velocity = Vector2.Zero;
            }

            if (Timer == 280f && Main.netMode != NetmodeID.MultiplayerClient) {
                Projectile.netUpdate = true;
                var tilePosition = Projectile.position.ToTileCoordinates16();
                var spawnPosition = new Point((int)Projectile.Center.X, (int)Projectile.Center.Y);
                var rect = ImmortalGolemRoom.BossZone;
                bool hasLightedCampfire = false;
                for (int k = rect.Left; k <= rect.Right; k++) {
                    for (int l = rect.Top; l <= rect.Bottom; l++) {
                        var t = Main.tile[k, l];
                        if (t.TileType == TileID.Campfire && t.TileFrameY == 18 && t.TileFrameX % 54 == 18) {
                            hasLightedCampfire = true;
                            var tileWorldCoord = new Vector2(k, l).ToWorldCoordinates(autoAddY: 0);
                            NPC.NewNPC(new EntitySource_TileInteraction(Main.player[Projectile.owner], tilePosition.X, tilePosition.Y),
                                       spawnPosition.X, spawnPosition.Y,
                                       ModContent.NPCType<StoneFairyCritter>(),
                                       ai2: tileWorldCoord.X,
                                       ai3: tileWorldCoord.Y);
                        }
                    }
                }
                // 不浪费大家时间了
                if (!hasLightedCampfire) {
                    Timer = 900f;
                }
            }

            if (Timer >= 900f && Timer % 40 == 0) {
                var spawnPosition = ImmortalGolemRoom.BossZone.Center.ToWorldCoordinates();

                if (Main.netMode != NetmodeID.Server) {
                    float factor = Utils.GetLerpValue(1080f, 900f, Timer);
                    float distanceFromCenter =  120 * factor + 10;

                    for (float r = 0; r <= 6.28f; r += 6.28f / 50f) {
                        Vector2 position = Vector2.UnitY.RotatedBy(r) * distanceFromCenter + spawnPosition;
                        Vector2 velocity = position.DirectionTo(spawnPosition) * 6f;
                        var d = Dust.NewDustPerfect(position, MyDustID.BlueWhiteBubble, velocity, 180, default, 1.6f);
                        d.fadeIn = 0.1f + (1f - factor) * 0.6f;
                        d.noGravity = true;
                    }
                }
            }

            if (Timer == 1080f) {
                Projectile.Kill();
                var spawnPosition = ImmortalGolemRoom.BossZone.Center.ToWorldCoordinates();
                SoundEngine.PlaySound(SoundID.Roar, spawnPosition);
                spawnPosition.Y += 120 / 2; // height/2
                NPC.SpawnBoss((int)spawnPosition.X, (int)spawnPosition.Y, ModContent.NPCType<NPCs.Enemies.Athanasy.Athanasy>(), Main.myPlayer);
            }
        }
    }
}
