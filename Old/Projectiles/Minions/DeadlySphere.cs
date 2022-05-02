using System;
using Entrogic.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles.Minions
{
    public class DeadlySphere : Minion
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DeadlySphere");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
            projectile.extraUpdates = 2;
            projectile.minionSlots = 2f;
            projectile.timeLeft = 18000;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft *= 5;
            projectile.DamageType = DamageClass.Summon;
            weaponType = ItemType<DeadlySphereStaff>();
        }
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.dead)
            {
                modPlayer.HasDeadlySphere = false;
            }
            if (modPlayer.HasDeadlySphere)
            {
                projectile.timeLeft = 2;
            }
        }
        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            /*if (projectile.localAI[0] == 0f)
            {
                int num = 100;
                for (int i = 0; i < num; i++)
                {
                    int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 16f), projectile.width, projectile.height - 16, 244, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num2].velocity *= 2f;
                    Main.dust[num2].scale *= 1.15f;
                }
                projectile.localAI[0] += 1f;
            }*/
            if ((double)Math.Abs(projectile.velocity.X) > 0.1)
            {
                projectile.spriteDirection = -projectile.direction;
            }
            float num3 = 800f;
            float num4 = 1200f;
            float num5 = 3000f;
            float num6 = 150f;
            float num7 = (float)Main.rand.Next(90, 111) * 0.01f;
            num7 *= Main.essScale;
            Lighting.AddLight(projectile.Center, 1.2f * num7, 0.8f * num7, 0f * num7);
            float num8 = 0.05f;
            for (int j = 0; j < 1000; j++)
            {
                bool flag2 = Main.projectile[j].type == ProjectileType<DeadlySphere>();
                if (j != projectile.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == projectile.owner && flag2 && Math.Abs(projectile.position.X - Main.projectile[j].position.X) + Math.Abs(projectile.position.Y - Main.projectile[j].position.Y) < (float)projectile.width)
                {
                    if (projectile.position.X < Main.projectile[j].position.X)
                    {
                        projectile.velocity.X = projectile.velocity.X - num8;
                    }
                    else
                    {
                        projectile.velocity.X = projectile.velocity.X + num8;
                    }
                    if (projectile.position.Y < Main.projectile[j].position.Y)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - num8;
                    }
                    else
                    {
                        projectile.velocity.Y = projectile.velocity.Y + num8;
                    }
                }
            }
            bool flag3 = false;
            if (projectile.ai[0] == 2f)
            {
                projectile.ai[1] += 1f;
                projectile.extraUpdates = 4;
                projectile.frameCounter++;
                if (projectile.frameCounter > 3)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame > 3)
                {
                    projectile.frame = 1;
                }
                if (projectile.ai[1] > 60f)
                {
                    projectile.ai[1] = 1f;
                    projectile.ai[0] = 0f;
                    projectile.extraUpdates = 2;
                    projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                }
                else
                {
                    flag3 = true;
                }
            }
            if (flag3)
            {
                return;
            }
            Vector2 vector = projectile.position;
            bool flag4 = false;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(projectile, false))
                {
                    float num9 = Vector2.Distance(npc.Center, projectile.Center);
                    if ((Vector2.Distance(projectile.Center, vector) > num9 && num9 < num3) || !flag4)
                    {
                        num3 = num9;
                        vector = npc.Center;
                        flag4 = true;
                    }
                }
            }
            float num10 = num4;
            if (flag4)
            {
                num10 = num5;
            }
            if (Vector2.Distance(player.Center, projectile.Center) > num10)
            {
                projectile.ai[0] = 1f;
                projectile.netUpdate = true;
            }
            if (flag4 && projectile.ai[0] == 0f)
            {
                Vector2 vector2 = vector - projectile.Center;
                float num11 = vector2.Length();
                vector2.Normalize();
                if (num11 > 200f)
                {
                    float scaleFactor = 8f;
                    vector2 *= scaleFactor;
                    projectile.velocity = (projectile.velocity * 40f + vector2) / 41f;
                }
                else
                {
                    float num12 = 4f;
                    vector2 *= -num12;
                    projectile.velocity = (projectile.velocity * 40f + vector2) / 41f;
                }
            }
            else
            {
                bool flag5 = false;
                if (!flag5)
                {
                    flag5 = projectile.ai[0] == 1f;
                }
                float num13 = 6f;
                if (flag5)
                {
                    num13 = 15f;
                }
                Vector2 center = projectile.Center;
                Vector2 vector3 = player.Center - center + new Vector2(0f, -60f);
                float num14 = vector3.Length();
                if (num14 > 200f && num13 < 8f)
                {
                    num13 = 8f;
                }
                if (num14 < num6 && flag5 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                }
                if (num14 > 2000f)
                {
                    projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                    projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2);
                    projectile.netUpdate = true;
                }
                if (num14 > 70f)
                {
                    vector3.Normalize();
                    vector3 *= num13;
                    projectile.velocity = (projectile.velocity * 40f + vector3) / 41f;
                }
                else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                {
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
            }
            if (projectile.frame > 0)
            {
                projectile.frame = 0;
            }
            if (projectile.ai[1] > 0f)
            {
                projectile.ai[1] += (float)Main.rand.Next(1, 4);
            }
            if (projectile.ai[1] > 40f)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 0f && projectile.ai[1] == 0f && flag4 && num3 < 500f)
            {
                projectile.ai[1] += 1f;
                if (Main.myPlayer == projectile.owner)
                {
                    projectile.ai[0] = 2f;
                    Vector2 value = vector - projectile.Center;
                    value.Normalize();
                    projectile.velocity = value * 8f;
                    projectile.netUpdate = true;
                }
                //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<能量体>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (projectile.ai[0] == 2f)
            {
                return new Color?(new Color(255, 255, 255, 255));
            }
            return null;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return projectile.ai[0] == 2f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)((double)damage * (60f - projectile.ai[1]) * 0.025);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(186, 300, false);
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,6, 0, 0, 100, default(Color), 2.5f);   
                d.noGravity = true;
                d.velocity *= 3;
            }
        }
    }
}