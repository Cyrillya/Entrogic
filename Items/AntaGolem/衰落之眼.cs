using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Items.AntaGolem
{
    public class 衰落之眼 :ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“空洞的眼眶...你感觉它在盯着你”");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.MedusaHead);
            item.damage = 34;
            item.shoot = mod.ProjectileType("AthanasyEye");
            item.value = Item.sellPrice(0, 3, 50);
        }
    }
    public class AthanasyEye : ModProjectile
    {
        public override string Texture { get { return "Entrogic/Items/AntaGolem/衰落之眼"; } }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.MedusaHead);
            projectile.aiStyle = -1;
        }
        public override bool CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 zero = Vector2.Zero;
            zero.X = (float)player.direction * 6f;
            zero.Y = player.gravDir * -14f;
            projectile.ai[0] += 1f;
            int num909 = 0;
            if (projectile.ai[0] >= 60f)
            {
                int num3 = num909;
                num909 = num3 + 1;
            }
            if (projectile.ai[0] >= 180f)
            {
                int num3 = num909;
                num909 = num3 + 1;
            }
            if (projectile.ai[0] >= 240f)
            {
                projectile.Kill();
                return;
            }
            bool flag41 = false;
            if (projectile.ai[0] == 60f || projectile.ai[0] == 180f)
            {
                flag41 = true;
            }
            bool flag42 = projectile.ai[0] >= 180f;
            Vector2 center11 = player.Center;
            Vector2 vector107 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - center11;
            if (player.gravDir == -1f)
            {
                vector107.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - center11.Y;
            }
            Vector2 vector108 = new Vector2((float)Math.Sign((vector107.X == 0f) ? ((float)player.direction) : vector107.X), 0f);
            if (vector108.X != projectile.velocity.X || vector108.Y != projectile.velocity.Y)
            {
                projectile.netUpdate = true;
            }
            projectile.velocity = vector108;
            if (projectile.soundDelay <= 0 && !flag42)
            {
                projectile.soundDelay = 10;
                projectile.soundDelay *= 2;
                if (projectile.ai[0] != 1f)
                {
                    Main.PlaySound(SoundID.Item15, projectile.position);
                }
            }
            if (projectile.ai[0] == 181f)
            {
                Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 17, 1f, 0f);
            }
            if (projectile.ai[0] > 10f && !flag42)
            {
                Vector2 vector109 = projectile.Center + new Vector2((float)(player.direction * 2), player.gravDir * 5f);
                float scaleFactor9 = MathHelper.Lerp(30f, 10f, (projectile.ai[0] - 10f) / 180f);
                float num910 = Main.rand.NextFloat() * 6.28318548f;
                for (float num911 = 0f; num911 < 1f; num911 += 1f)
                {
                    Vector2 value46 = Vector2.UnitY.RotatedBy((double)(num911 / 1f * 6.28318548f + num910), default(Vector2));
                    Dust dust104 = Main.dust[Dust.NewDust(vector109, 0, 0, DustID.Stone, 0f, 0f, 0, default(Color), 1f)];
                    dust104.position = vector109 + value46 * scaleFactor9;
                    dust104.noGravity = true;
                    dust104.customData = player;
                    dust104.velocity = value46 * -2f;
                }
            }
            if (projectile.ai[0] > 180f && projectile.ai[0] <= 182f)
            {
                Vector2 vector110 = projectile.Center + new Vector2((float)(player.direction * 2), player.gravDir * 5f);
                float scaleFactor10 = MathHelper.Lerp(20f, 30f, (projectile.ai[0] - 180f) / 182f);
                Main.rand.NextFloat();
                for (float num912 = 0f; num912 < 10f; num912 += 1f)
                {
                    Vector2 value47 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
                    Dust dust105 = Main.dust[Dust.NewDust(vector110, 0, 0, DustID.Stone, 0f, 0f, 0, default(Color), 1f)];
                    dust105.position = vector110 + value47 * scaleFactor10;
                    dust105.noGravity = true;
                    dust105.customData = player;
                    dust105.velocity = value47 * 4f;
                    dust105.scale = 0.5f + Main.rand.NextFloat();
                }
            }
            if (Main.myPlayer == projectile.owner)
            {
                bool flag43 = !flag41 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
                bool flag44 = player.channel && flag43;
                if ((!flag42 && !flag44) || projectile.ai[0] == 180f)
                {
                    Vector2 vector111 = player.Center + new Vector2((float)(player.direction * 4), player.gravDir * 2f);
                    int num913 = projectile.damage * (1 + num909);
                    vector111 = projectile.Center;
                    int num914 = 0;
                    float num915 = 0f;
                    int num3;
                    for (int num916 = 0; num916 < 200; num916 = num3 + 1)
                    {
                        NPC npc9 = Main.npc[num916];
                        if (npc9.active && projectile.Distance(npc9.Center) < 500f && npc9.CanBeChasedBy(this, false) && Collision.CanHitLine(npc9.position, npc9.width, npc9.height, vector111, 0, 0))
                        {
                            Vector2 vector112 = npc9.Center - vector111;
                            num915 += vector112.ToRotation();
                            num3 = num914;
                            num914 = num3 + 1;
                            Projectile p = Main.projectile[Projectile.NewProjectile(vector111.X, vector111.Y, vector112.X, vector112.Y, mod.ProjectileType("衰落之眼"), 0, 0f, projectile.owner, (float)projectile.whoAmI, 0f)];
                            p.Center = npc9.Center;
                            p.damage = num913;
                            p.Damage();
                            p.damage = 0;
                            p.Center = vector111;
                            projectile.ai[0] = 180f;
                        }
                        num3 = num916;
                    }
                    if (num914 != 0)
                    {
                        num915 /= (float)num914;
                    }
                    else
                    {
                        num915 = ((player.direction == 1) ? 0f : 3.14159274f);
                    }
                    for (int num918 = 0; num918 < 6; num918 = num3 + 1)
                    {
                        Vector2 vector113 = Vector2.Zero;
                        if (Main.rand.Next(4) != 0)
                        {
                            vector113 = Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)num915, default(Vector2)) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
                        }
                        else
                        {
                            vector113 = Vector2.UnitX.RotatedByRandom(6.2831854820251465) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
                        }
                        Projectile.NewProjectile(vector111.X, vector111.Y, vector113.X, vector113.Y, mod.ProjectileType("衰落之眼"), 0, 0f, projectile.owner, (float)projectile.whoAmI, 0f);
                        num3 = num918;
                    }
                    projectile.ai[0] = 180f;
                    projectile.netUpdate = true;
                }
            }
            projectile.rotation = ((player.gravDir == 1f) ? 0f : 3.14159274f);
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            Vector2 vector114 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector114.X = (float)player.bodyFrame.Width - vector114.X - vector114.X;
            }
            vector114 -= (player.bodyFrame.Size() - new Vector2((float)player.width, 42f)) / 2f;
            projectile.Center = (player.position + vector114 + zero - projectile.velocity).Floor();
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
    }
}
