using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Entrogic.Projectiles.Minions
{
    /// <summary>
    /// 运动水元素 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/9 21:22:45
    /// </summary>
    public class 运动水元素 : Minion
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
            projectile.minion = true;
            projectile.minionSlots = 0.33f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            weaponType = ItemType<Items.PollutElement.WaterElementalStaff>();
        }
        public int timer = 0;
        protected float Timer
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            EntrogicPlayer modPlayer = player.GetModPlayer<EntrogicPlayer>();
            NPC target = null;
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

            Timer++;
            float FindArea = 100f * 16f;
            if (Timer >= Main.rand.Next(41, 77) || Timer == 1f)
            {
                NPC closestNPC = null;
                float distance = FindArea;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.Distance(player.Center) <= FindArea && npc.active && !npc.friendly && npc.type != NPCID.TargetDummy && !npc.dontTakeDamage)
                    {
                        if (npc.boss)
                            target = npc;
                        else
                        {
                            if (npc.Distance(player.Center) < distance)
                            {
                                distance = npc.Distance(player.Center);
                                closestNPC = npc;
                            }
                        }

                    }
                }
                if (target == null)
                {
                    target = closestNPC;
                }

                if (target != null)
                {
                    projectile.velocity = (target.Center - projectile.Center).ToRotation().ToRotationVector2() * 15f;
                    projectile.velocity *= 0.99f;
                }
                else
                {
                    projectile.velocity = (player.Center - projectile.Center).ToRotation().ToRotationVector2() * 4f;
                    projectile.velocity *= 0.99f;
                }
                Timer = 1f;
            }
            if (projectile.Distance(player.Center) > FindArea)
                projectile.Center = player.Center;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
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
