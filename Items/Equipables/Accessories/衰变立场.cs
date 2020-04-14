using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace Entrogic.Items.Equipables.Accessories
{
    /// <summary>
    /// 衰变立场 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/24 17:53:16
    /// </summary>
    public class 衰变立场 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("人物移动速度与伤害随时间发生周期性变化，\n" +
                "且固定有一个魔法回复加快的增益\n" +
                "“毁灭后的新秩序”");
        }
        public override void SetDefaults()
        {
            item.Size = new Vector2(32, 32);
            item.rare = 7;
            item.accessory = true;
            item.value = Item.sellPrice(0, 1, 20, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaRegenDelayBonus++;
            player.manaRegenBonus += 30;
            SimpleEffectPlayer modPlayer = player.GetModPlayer<SimpleEffectPlayer>();
            modPlayer.thisEffect = true;
            player.moveSpeed += modPlayer.speedAdd;
            player.allDamage += modPlayer.damageAdd;
            player.meleeSpeed += 3f;
        }
    }
    public class SimpleEffectPlayer : ModPlayer
    {
        public bool thisEffect = false;
        public float damageAdd = 0f;
        public bool damageAdding = true;
        public float speedAdd = 0f;
        public bool speedAdding = true;
        public override void ResetEffects()
        {
            thisEffect = false;
        }
        public override void UpdateDead()
        {
            thisEffect = false;
        }
        public override void PreUpdate()
        {
            if (thisEffect)
            {
                if (damageAdding)
                {
                    damageAdd += 0.00003f;
                    if (damageAdd >= 0.2f) damageAdding = false;
                }
                else
                {
                    damageAdd -= 0.00003f;
                    if (damageAdd <= 0f) damageAdding = true;
                }
                if (speedAdding)
                {
                    speedAdd += 0.00002f;
                    if (speedAdd >= 0.1f) speedAdding = false;
                }
                else
                {
                    speedAdd -= 0.00002f;
                    if (speedAdd <= 0f) speedAdding = true;
                }
            }
        }
    }
}