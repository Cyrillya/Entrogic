using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Entrogic.Projectiles;

namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染胸甲 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/22 18:27:11
    /// </summary>
    [AutoloadEquip(EquipType.Body)]
    public class BreastplateofContamination : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ModTranslation modTranslation = Mod.CreateTranslation("PArmorBonus");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "召唤一个圈为你抵挡三次伤害，30秒后重新充能");
            modTranslation.SetDefault("Young man, there are still many things you don't understand");
            Mod.AddTranslation(modTranslation);
            modTranslation = Mod.CreateTranslation("PArmorBonus2");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "提高仆从数量上限");
            modTranslation.SetDefault("Young man, there are still many things you don't understand");
            Mod.AddTranslation(modTranslation);
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 16;
        }
        public override void UpdateEquip(Player player)
        {
            int critpoint = 15;
            player.GetCrit(DamageClass.Summon) += critpoint;
            player.GetCrit(DamageClass.Magic) += critpoint;
            player.GetCrit(DamageClass.Ranged) += critpoint;
            player.GetCrit(DamageClass.Melee) += critpoint;
            player.GetCrit(DamageClass.Throwing) += critpoint;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == ItemType<HelmetofContamination>() || head.type == ItemType<CrownofContamination>() || head.type == ItemType<MaskofContamination>() || head.type == ItemType<HeadgearofContamination>()) && legs.type == ItemType<GreavesofContamination>();
        }
        public override void UpdateArmorSet(Player player)
        {
            string bonus2 = "";
            PollutionArmorSet modPlayer = player.GetModPlayer<PollutionArmorSet>();
            modPlayer.EffectEnable = true;
            if (player.head == ItemType<CrownofContamination>())
            {
                bonus2 = "Mods.Entrogic.PArmorBonus2";
                player.maxMinions += 2;
            }
            player.setBonus = Language.GetTextValue("Mods.Entrogic.PArmorBonus") + "\n" +
                Language.GetTextValue(bonus2);
        }
    }
    public class PollutionArmorSet : ModPlayer
    {
        public int pTimer = 1800;
        public bool pEffect = false;
        public bool EffectEnable = false;
        public int pCounter = 0;
        public override void ResetEffects()
        {
            EffectEnable = false;
        }
        public override void UpdateDead()
        {
            EffectEnable = false;
        }

        public float fRingScale = 1f;
        public float flameRingRot = 0f;
        public int num = 0;
        public override void PostUpdate()
        {
            if (EffectEnable)
            {
                pTimer++;
                if (pTimer >= 1800) pEffect = true;
                else pEffect = false;
            }
            if (!EffectEnable)
            {
                pEffect = false;
            }
            if (pEffect && (!player.immune || player.immuneTime <= 2))
            {
                Rectangle rectangle4 = new Rectangle((int)((double)player.position.X + (double)player.velocity.X * 0.5 - 4.0), (int)((double)player.position.Y + (double)player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
                for (int l = 0; l < 200; l++)
                {
                    if (Main.npc[l].active && !Main.npc[l].dontTakeDamage && !Main.npc[l].friendly && Main.npc[l].immune[player.whoAmI] <= 0 && Main.npc[l].damage > 0)
                    {
                        NPC npc4 = Main.npc[l];
                        Rectangle rect4 = npc4.getRect();
                        if (rectangle4.Intersects(rect4) && (npc4.noTileCollide || player.CanHit(npc4)) && player.whoAmI == Main.myPlayer)
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
                Projectile.NewProjectile(player.position, finalVec, ProjectileType<污染尖刺>(), 53, 4f, player.whoAmI);
            }
        }
    }
}
