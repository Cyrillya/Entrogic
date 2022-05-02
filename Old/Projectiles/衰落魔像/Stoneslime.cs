using Entrogic.Items.AntaGolem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.衰落魔像
{
    public class Stoneslime : Minion
    {
        public float dust;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stoneslime");
            Main.projFrames[projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.CountsAsHoming[projectile.type] = true;
        }
        
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.minionSlots = 1f;
            projectile.alpha = 25;
            projectile.aiStyle = 26;
            projectile.timeLeft = 18000;
            projectile.penetrate = -1;
            projectile.DamageType = DamageClass.Summon;
            aiType = ProjectileID.BabySlime;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            weaponType = ItemType<StoneSlimeStaff>();
        }
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.dead)
            {
                modPlayer.HasStoneSlime = false;
            }
            if (modPlayer.HasStoneSlime)
            {
                projectile.timeLeft = 2;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }
        public int timer = 0;
        public override void Behavior()
        {
            if (dust == 0f)
            {
                int num = 16;
                for (int i = 0; i < num; i++)
                {
                    Vector2 vector = Vector2.Normalize(projectile.velocity) * new Vector2((float)projectile.width / 2f, (float)projectile.height) * 0.75f;
                    vector = Utils.RotatedBy(vector, (double)((float)(i - (num / 2 - 1)) * 6.28318548f / (float)num), default(Vector2)) + projectile.Center;
                    Vector2 vector2 = vector - projectile.Center;
                    int num2 = Dust.NewDust(vector + vector2, 0, 0, 1, vector2.X * 1f, vector2.Y * 1f, 32, default(Color), 1.1f);
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                    Main.dust[num2].velocity = vector2;
                }
                dust += 1f;
            }
            timer++;
            if (timer >= 30 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPCC = new Vector2(-1f);
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(Main.projectile, false) && Main.npc[i].active && !Main.npc[i].friendly)
                    {
                        if (NPCC.Y == -1 || NPCC.Y > projectile.Distance(Main.npc[i].position))
                        {
                            NPCC.X = i;
                            NPCC.Y = projectile.Distance(Main.npc[i].position);
                        }
                    }
                }
                if (NPCC.X != -1 && Main.npc[(int)NPCC.X].active && NPCC.Y < 600)
                {
                    Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num11 = Main.npc[(int)NPCC.X].position.X + (float)Main.npc[(int)NPCC.X].width * 0.5f - vector3.X;
                    float num12 = Main.npc[(int)NPCC.X].position.Y - vector3.Y;
                    float num13 = (float)Math.Sqrt((double)(num11 * num11 + num12 * num12));
                    num12 = Main.npc[(int)NPCC.X].position.Y - vector3.Y - (float)Main.rand.Next(0, 180);
                    num13 = (float)Math.Sqrt((double)(num11 * num11 + num12 * num12));
                    num13 = 7f / num13;
                    num11 *= num13;
                    num12 *= num13;
                    Projectile.NewProjectile(vector3.X, vector3.Y, num11, num12, ProjectileType<GravelFriendly>(), projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                    timer = 0;
                }

            }
        }
        public Vector2 NPCC = new Vector2(-1f);
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D = (Texture2D)Terraria.GameContent.TextureAssets.Projectile[projectile.type];
            int num = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Height() / Main.projFrames[projectile.type];
            int y = num * projectile.frame;
            Main.spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), projectile.GetAlpha(lightColor), projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.GetGlobalNPC<减速>().stone = true;
            }
        }
    }
    public class 减速 : GlobalNPC
    {
        public bool stone = false;
        public int stoneTimer = 0;
        public override bool PreAI(NPC npc)
        {
            if (stone && !npc.boss)
            {
                stoneTimer++;
                if (stoneTimer >= 60)
                {
                    stone = false;
                    stoneTimer = 0;
                }
                npc.velocity.X = 0f;
                return false;
            }
            else return true;
        }
        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (stone && !npc.boss) return new Color(50, 50, 50);
            return null;
        }
        public override bool InstancePerEntity { get { return true; } }
    }
    public class GravelFriendly : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Projectiles/Enemies/Gravel"; } }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.Size = new Vector2(8, 8);
            projectile.scale *= 1.25f;
            projectile.penetrate = -1;
            projectile.alpha = 80;
            projectile.aiStyle = 1;
            projectile.timeLeft = 300;
            projectile.DamageType = DamageClass.Summon;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
        }
        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
            {
                int num70 = Dust.NewDust(projectile.position - projectile.velocity * 3f, projectile.width, projectile.height, 1, 0f, 0f, 80);
                Dust dust = Main.dust[num70];
                dust.scale *= 1.2f;
                dust.velocity *= 0.3f;
                dust = Main.dust[num70];
                dust.velocity += projectile.velocity * 0.3f;
                Main.dust[num70].noGravity = true;
            }
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item17, projectile.position);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 1, 0, 0, 0);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.GetGlobalNPC<减速>().stone = true;
            }
        }
    }
}
