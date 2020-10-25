using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Enemies
{
    public class DirtHostile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = false;
            projectile.aiStyle = -1;
            projectile.scale = 1.2f;
        }
        public override void AI()
        {
            projectile.rotation++;
            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 0);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
            for (int num590 = 0; num590 < 5; num590++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 0);
            }
            if (timeLeft > 10)
            {
                int num832 = -1;
                int num833 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                int num834 = (int)(projectile.position.Y + (float)(projectile.width / 2)) / 16;
                int num835 = 0;
                int num836 = 2;
                if (Main.tile[num833, num834].halfBrick() && projectile.velocity.Y > 0f && Math.Abs(projectile.velocity.Y) > Math.Abs(projectile.velocity.X))
                {
                    num834--;
                }
                if (!Main.tile[num833, num834].active() && num835 >= 0)
                {
                    bool flag5 = false;
                    if (num834 < Main.maxTilesY - 2 && Main.tile[num833, num834 + 1] != null && Main.tile[num833, num834 + 1].active() && Main.tile[num833, num834 + 1].type == 314)
                    {
                        flag5 = true;
                    }
                    if (!flag5)
                    {
                        WorldGen.PlaceTile(num833, num834, num835, mute: false, forced: true);
                    }
                    if (!flag5 && Main.tile[num833, num834].active() && Main.tile[num833, num834].type == num835)
                    {
                        if (Main.tile[num833, num834 + 1].halfBrick() || Main.tile[num833, num834 + 1].slope() != 0)
                        {
                            WorldGen.SlopeTile(num833, num834 + 1);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, num833, num834 + 1);
                            }
                        }
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, num833, num834, num835);
                        }
                    }
                    else if (num836 > 0)
                    {
                        num832 = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, num836);
                    }
                }
                else if (num836 > 0)
                {
                    num832 = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, num836);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && num832 >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num832, 1f);
                }
            }
        }
    }
}
