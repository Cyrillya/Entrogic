using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Projectiles.Minions
{
    /// <summary>
    /// 运动水元素 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/9 21:22:45
    /// </summary>
    public class WaterElemental : Minion
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("运动水元素");
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 80;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.DamageType = DamageClass.Summon;
            projectile.minionSlots = 0.33f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.localNPCHitCooldown = 15;
            projectile.usesLocalNPCImmunity = true;
            weaponType = ItemType<Items.PollutElement.WaterElementalStaff>();
        }
        public int timer = 0;
        protected float Timer
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        protected int TargetMode // 0为追踪玩家最近，1为追踪射弹最近
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }
        private NPC target = null;
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            if (player.dead)
            {
                modPlayer.HasMovementWaterElemental = false;
            }
            if (modPlayer.HasMovementWaterElemental)
            {
                projectile.timeLeft = 2;
            }
            if (player == null)
                return;
            if (target != null && !target.active)
                target = null;

            Timer++;
            float FindArea = 60f * 16f;
            if (Timer >= Main.rand.Next(31, 40) || Timer == 1f)
            {
                NPC closestNPC = null;
                Vector2 targetBaseCenter = TargetMode == 0 ? player.Center : projectile.Center;
                float distance = FindArea;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.Distance(targetBaseCenter) <= FindArea && npc.active && !npc.friendly && npc.type != NPCID.TargetDummy && !npc.dontTakeDamage)
                    {
                        if (npc.boss)
                            target = npc;
                        else
                        {
                            if (npc.Distance(targetBaseCenter) < distance)
                            {
                                distance = npc.Distance(targetBaseCenter);
                                closestNPC = npc;
                            }
                        }

                    }
                }
                if (target == null)
                {
                    target = closestNPC;
                }
                if (player.HasMinionAttackTargetNPC && player.MinionAttackTargetNPC >= 0 && player.MinionAttackTargetNPC < 255 && Main.npc[player.MinionAttackTargetNPC].active && Main.npc[player.MinionAttackTargetNPC].Distance(player.Center) <= FindArea + 20f * 16f) 
                    target = Main.npc[player.MinionAttackTargetNPC];

                if (target != null)
                {
                    projectile.velocity = ((target.Center - projectile.Center).ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f))).ToRotationVector2() * 15f;
                    projectile.velocity *= 0.99f;
                }
                Timer = 1f;
            }
            else
            {
                projectile.velocity *= 0.98f;
            }
            if (target == null) // 抄原版的都是
            {
                float num628 = 0.05f;
                int num3;
                for (int num629 = 0; num629 < 1000; num629 = num3 + 1)
                {
                    bool flag22 = Main.projectile[num629].type == projectile.type;
                    if (num629 != projectile.whoAmI && Main.projectile[num629].active && Main.projectile[num629].owner == projectile.owner && flag22 && Math.Abs(projectile.position.X - Main.projectile[num629].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num629].position.Y) < (float)projectile.width)
                    {
                        if (projectile.position.X < Main.projectile[num629].position.X)
                        {
                            projectile.velocity.X = projectile.velocity.X - num628;
                        }
                        else
                        {
                            projectile.velocity.X = projectile.velocity.X + num628;
                        }
                        if (projectile.position.Y < Main.projectile[num629].position.Y)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - num628;
                        }
                        else
                        {
                            projectile.velocity.Y = projectile.velocity.Y + num628;
                        }
                    }
                    num3 = num629;
                }
                Vector2 DirectionVector = player.Center + new Vector2(0f, -60f) - projectile.Center;
                float plrDistance = DirectionVector.Length();
                if (plrDistance > 2200f)
                {
                    projectile.Center = player.Center;
                    projectile.netUpdate = true;
                }
                float limitSpeed = 1.8f;
                if (plrDistance > 70f)
                {
                    DirectionVector.Normalize();
                    DirectionVector *= 15f;
                    projectile.velocity = (projectile.velocity * 40f + DirectionVector) / 41f;
                }
                else if (projectile.velocity.X >= -limitSpeed && projectile.velocity.X <= limitSpeed
                    && projectile.velocity.Y >= -limitSpeed && projectile.velocity.Y <= limitSpeed)
                {
                    projectile.velocity *= 1.4f;
                }
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = (Texture2D)Terraria.GameContent.TextureAssets.Projectile[projectile.type];
            int frameHeight = t.Height / Main.projFrames[projectile.type];
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            if (projectile.localAI[0] < 0) effects = effects | SpriteEffects.FlipVertically;
            Vector2 origin = new Vector2(t.Width / 2, frameHeight / 2);

            int length = 7;
            for (int i = length; i >= 1; i--)
            {
                Vector2 drawPos = projectile.Center - Main.screenPosition - projectile.oldVelocity * i * 0.5f;
                float trailOpacity = projectile.Opacity - 0.05f - 0.95f / length * i;
                if (i != 0) trailOpacity /= 2f;
                if (trailOpacity > 0f)
                {
                    float colMod = 0.4f + 0.6f * trailOpacity;
                    spriteBatch.Draw(t,
                        drawPos.ToPoint().ToVector2(),
                        new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight),
                        new Color(1f * colMod, 1f * colMod, 1f, 0.5f) * trailOpacity,
                        projectile.rotation,
                        origin,
                        projectile.scale * (1f + 0.02f * i),
                        effects,
                        0);
                }
            }
            spriteBatch.Draw(t,
                (projectile.Center - Main.screenPosition).NoShake(),
                new Rectangle(0, frameHeight * projectile.frame, t.Width, frameHeight),
                projectile.GetAlpha(lightColor),
                projectile.rotation,
                origin,
                projectile.scale,
                effects,
                0);
            return false;
        }
        public override void PostAI()
        {
            projectile.tileCollide = false;
            int countTime = 5 * projectile.extraUpdates;
            timer++;
            if (timer <= countTime)
                projectile.frame = 0;
            else if (timer <= countTime * 2)
                projectile.frame = 1;
            else if (timer <= countTime * 3)
                projectile.frame = 2;
            else if (timer <= countTime * 4)
                projectile.frame = 3;
            else if (timer <= countTime * 5)
                projectile.frame = 4;
            if (timer >= countTime * 5)
                timer = 0;
        }
    }
}
