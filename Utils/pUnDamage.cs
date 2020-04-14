using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Entrogic
{
    /// <summary>
    /// pUnDamage 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/2/25 0:24:46
    /// </summary>
    public class pUnDamage : ModPlayer
    {
        public int pTimer = 1800;
        public bool pEffect = false;
        public bool pEffectEnable = false;
        public int pCounter = 0;
        public override void ResetEffects()
        {
            pEffectEnable = false;
        }
        public override void UpdateDead()
        {
            pEffectEnable = false;
        }
        
        public float fRingScale = 1f;
        public float flameRingRot = 0f;
        public static readonly PlayerLayer MiscEffects = new PlayerLayer("Entrogic", "PollutionEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("Entrogic");
            pUnDamage modPlayer = drawPlayer.GetModPlayer<pUnDamage>();
            if (modPlayer.pEffect && modPlayer.pEffectEnable)
            {
                Texture2D texture = mod.GetTexture("Texture/污染光圈");
                int frameSize = texture.Height / 3;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y);

                float num = 0.1f;
                float num2 = 0.9f;
                modPlayer.fRingScale += 0.004f;
                float flameRingScale;
                if (modPlayer.fRingScale < 1f)
                {
                    flameRingScale = modPlayer.fRingScale;
                }
                else
                {
                    modPlayer.fRingScale = 0.8f;
                    flameRingScale = modPlayer.fRingScale;
                }
                modPlayer.flameRingRot += 0.05f;
                if (modPlayer.flameRingRot > 6.28318548f)
                {
                    modPlayer.flameRingRot -= 6.28318548f;
                }
                if (modPlayer.flameRingRot < -6.28318548f)
                {
                    modPlayer.flameRingRot += 6.28318548f;
                }
                float num3 = flameRingScale + num;
                if (num3 > 1f)
                {
                    num3 -= num * 2f;
                }
                float num4 = MathHelper.Lerp(0.8f, 0f, Math.Abs(num3 - num2) * 10f);
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, frameSize), new Color(num4, num4, num4, num4 / 2f), modPlayer.flameRingRot, new Vector2(texture.Width / 2f, frameSize / 2f), num3 * 0.6f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
                modPlayer.fRingScale = 1f;
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            MiscEffects.visible = true;
            layers.Add(MiscEffects);
        }
        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            //float scale = 0.0f;
            //scale = MathHelper.Clamp(0.0f, 0.0f, 1.0f);
            //drawInfo.eyeWhiteColor = Color.Multiply(drawInfo.eyeWhiteColor, scale);
            //drawInfo.eyeColor = Color.Multiply(drawInfo.eyeColor, scale);
            //drawInfo.hairColor = Color.Multiply(drawInfo.hairColor, scale);
            //drawInfo.faceColor = Color.Multiply(drawInfo.faceColor, scale);
            //drawInfo.bodyColor = Color.Multiply(drawInfo.bodyColor, scale);
            //drawInfo.legColor = Color.Multiply(drawInfo.legColor, scale);
            //drawInfo.shirtColor = Color.Multiply(drawInfo.shirtColor, scale);
            //drawInfo.underShirtColor = Color.Multiply(drawInfo.underShirtColor, scale);
            //drawInfo.pantsColor = Color.Multiply(drawInfo.pantsColor, scale);
            //drawInfo.shoeColor = Color.Multiply(drawInfo.shoeColor, scale);
            //drawInfo.headGlowMaskColor = Color.Multiply(drawInfo.headGlowMaskColor, scale);
            //drawInfo.bodyGlowMaskColor = Color.Multiply(drawInfo.bodyGlowMaskColor, scale);
            //drawInfo.legGlowMaskColor = Color.Multiply(drawInfo.legGlowMaskColor, scale);
            //drawInfo.armGlowMaskColor = Color.Multiply(drawInfo.armGlowMaskColor, scale);
            //drawInfo.upperArmorColor = Color.Multiply(drawInfo.upperArmorColor, scale);
            //drawInfo.lowerArmorColor = Color.Multiply(drawInfo.lowerArmorColor, scale);
            //drawInfo.middleArmorColor = Color.Multiply(drawInfo.middleArmorColor, scale);
        }
        public int num = 0;
        public override void PostUpdate()
        {
            if (pEffectEnable)
            {
                pTimer++;
                if (pTimer >= 1800) pEffect = true;
                else pEffect = false;
            }
            if (!pEffectEnable)
            {
                pEffect = false;
            }
            if (pEffect && (!player.immune || player.immuneTime <= 2))
            {
                Rectangle rectangle4 = new Rectangle((int)((double)player.position.X + (double)player.velocity.X * 0.5 - 4.0), (int)((double)player.position.Y + (double)player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
                for (int l = 0; l < 200; l++)
                {
                    if ((Main.npc[l].active && !Main.npc[l].dontTakeDamage && !Main.npc[l].friendly && Main.npc[l].immune[player.whoAmI] <= 0 && Main.npc[l].damage > 0))
                    {
                        NPC npc4 = Main.npc[l];
                        Rectangle rect4 = npc4.getRect();
                        if (((rectangle4.Intersects(rect4) && (npc4.noTileCollide || player.CanHit(npc4)))) && player.whoAmI == Main.myPlayer)
                        {
                            player.immune = true;
                            player.immuneTime = 60;
                            if (player.longInvince)
                            {
                                player.immuneTime += 40;
                            }
                            for (int m = 0; m < player.hurtCooldowns.Length; m++)
                            {
                                player.hurtCooldowns[m] = player.immuneTime;
                            }
                            if (player.whoAmI == Main.myPlayer)
                            {
                                NetMessage.SendData(MessageID.Dodge, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                            }
                            SetEffect();
                        }
                    }
                }
                for (int l = 0; l < 1000; l++)
                {
                    if (Main.projectile[l].active && !Main.projectile[l].friendly && Main.projectile[l].hostile && Main.projectile[l].damage > 0)
                    {
                        Projectile projectile = Main.projectile[l];
                        Rectangle rect5 = projectile.getRect();
                        if (rectangle4.Intersects(rect5) && player.whoAmI == Main.myPlayer)
                        {
                            player.immune = true;
                            player.immuneTime = 60;
                            if (player.longInvince)
                            {
                                player.immuneTime += 40;
                            }
                            for (int m = 0; m < player.hurtCooldowns.Length; m++)
                            {
                                player.hurtCooldowns[m] = player.immuneTime;
                            }
                            if (player.whoAmI == Main.myPlayer)
                            {
                                NetMessage.SendData(MessageID.Dodge, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                            }
                            SetEffect();
                        }
                    }
                }
            }
        }
        public int pHitChance = 0;
        public void SetEffect()
        {
            pHitChance++;
            if (pHitChance >= 3)
            {
                pEffect = false;
                pTimer = 0;
                pHitChance = 0;
                ShootingSpike();
            }
            else if (pHitChance == 2)
            {
                ShootingSpike(15, 10f);
            }
            else
            {
                ShootingSpike(10, 8f);
            }
        }
        public void ShootingSpike(int count = 30, float speed = 12f)
        {
            Vector2 vec = Main.MouseWorld - player.Center;
            // 角度变化
            for (float rad = 0.0f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / count)
            {
                Vector2 finalVec = (vec.ToRotation() + rad).ToRotationVector2() * speed;
                // 射出去！
                Projectile.NewProjectile(player.position, finalVec, mod.ProjectileType("污染尖刺"), 53, 4f, player.whoAmI);
            }
        }
    }
}
