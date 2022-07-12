using Terraria.Audio;

namespace Entrogic.Content.Projectiles.Elementals
{
    public class EarthElemental_Proj : ProjectileBase
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Dirt");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "泥土");
        }

        public override void SetDefaults() {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = false;
            Projectile.aiStyle = -1;
            Projectile.scale = 1.2f;
        }

        public override void AI() {
            Projectile.rotation++;
            Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int num590 = 0; num590 < 5; num590++) {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt);
            }
            if (timeLeft > 10) {
                int num832 = -1;
                int num833 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                int num834 = (int)(Projectile.position.Y + (float)(Projectile.width / 2)) / 16;
                int num835 = 0;
                int num836 = 2;
                if (Main.tile[num833, num834].IsHalfBlock && Projectile.velocity.Y > 0f && Math.Abs(Projectile.velocity.Y) > Math.Abs(Projectile.velocity.X)) {
                    num834--;
                }
                if (!Main.tile[num833, num834].HasUnactuatedTile && num835 >= 0) {
                    bool flag5 = false;
                    if (num834 < Main.maxTilesY - 2 && Main.tile[num833, num834 + 1] != null && Main.tile[num833, num834 + 1].HasUnactuatedTile && Main.tile[num833, num834 + 1].TileType == 314) {
                        flag5 = true;
                    }
                    if (!flag5) {
                        WorldGen.PlaceTile(num833, num834, num835, mute: false, forced: true);
                    }
                    if (!flag5 && Main.tile[num833, num834].HasUnactuatedTile && Main.tile[num833, num834].TileType == num835) {
                        if (Main.tile[num833, num834 + 1].IsHalfBlock || Main.tile[num833, num834 + 1].Slope != 0) {
                            WorldGen.SlopeTile(num833, num834 + 1);
                            if (Main.netMode == NetmodeID.Server) {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, num833, num834 + 1);
                            }
                        }
                        if (Main.netMode != NetmodeID.SinglePlayer) {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, num833, num834, num835);
                        }
                    }
                    else if (num836 > 0) {
                        num832 = Item.NewItem(Projectile.GetSource_DropAsItem(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, num836);
                    }
                }
                else if (num836 > 0) {
                    num832 = Item.NewItem(Projectile.GetSource_DropAsItem(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, num836);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && num832 >= 0) {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num832, 1f);
                }
            }
        }
    }
}
