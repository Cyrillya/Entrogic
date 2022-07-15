using Terraria;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Audio;
using Entrogic.Common.ModSystems;
using Entrogic.Content.NPCs.Enemies.ContyElemental;

namespace Entrogic.Content.Projectiles.ContyElemental.Hostile
{
    public class ShootingLine : ModProjectile
    {
        public override string Texture => TextureManager.Blank;

        private List<float> _rotates = new();

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 50;
        }

        public override void AI()
        {
            // 污染之灵
            NPC elemental = Main.npc[(int)Projectile.ai[1]];
            // 保险
            if (elemental == null || elemental.type != NPCType<ContaminatedElemental>() || elemental.active == false)
            {
                Projectile.active = false;
                Projectile.Kill();
                Projectile.netUpdate = true;
                return;
            }
            Player player = Main.player[elemental.target];
            Projectile.Center = elemental.Center;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 15 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // 射击
                foreach (var rotation in _rotates)
                {
                    Vector2 vec = rotation.ToRotationVector2();
                    int bullet = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vec * 0.9f, ProjectileType<ContamintedSpike>(), (elemental.ModNPC as ContaminatedElemental).GetSpikeDamage(), 2f);
                    if (Main.dedServ)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, bullet);
                    }
                    SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
                }
                Projectile.active = false;
                Projectile.Kill();
            }
            if (Projectile.ai[0] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // 选取
                float startRadius = ModHelper.GetFromToRadians(Projectile.Center, player.Center);
                float endRadius = ModHelper.GetFromToRadians(Projectile.Center, player.Center) + MathHelper.TwoPi;
                for (float rotation = startRadius; rotation < endRadius; rotation += MathHelper.TwoPi / 12f)
                {
                    _rotates.Add(rotation);

                    // 特效
                    Vector2 vec = rotation.ToRotationVector2();
                    int proj = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, vec, ProjectileType<EffectRay>(), 0, 0f, Projectile.owner);
                    Main.projectile[proj].ai[1] = Projectile.whoAmI;
                    if (Main.dedServ)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                    }
                }
            }
        }
    }
}
