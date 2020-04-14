using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染胸甲 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/22 18:27:11
    /// </summary>
    [AutoloadEquip(EquipType.Body)]
    public class 污染胸甲 : ModItem
    {
        public override void SetStaticDefaults()
        {
            ModTranslation modTranslation = mod.CreateTranslation("PArmorBonus");
            modTranslation.AddTranslation(GameCulture.Chinese, "召唤一个圈为你抵挡三次伤害，30秒后重新充能");
            modTranslation.SetDefault("Young man, there are still many things you don't understand");
            mod.AddTranslation(modTranslation);
            modTranslation = mod.CreateTranslation("PArmorBonus2");
            modTranslation.AddTranslation(GameCulture.Chinese, "提高仆从数量上限");
            modTranslation.SetDefault("Young man, there are still many things you don't understand");
            mod.AddTranslation(modTranslation);
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
            player.magicCrit += critpoint;
            player.meleeCrit += critpoint;
            player.rangedCrit += critpoint;
            player.thrownCrit += critpoint;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("污染头盔") || head.type == mod.ItemType("污染之冠") || head.type == mod.ItemType("污染面具") || head.type == mod.ItemType("污染头饰")) && legs.type == mod.ItemType("污染护胫");
        }
        public override void UpdateArmorSet(Player player)
        {
            string bonus2 = "";
            pUnDamage modPlayer = player.GetModPlayer<pUnDamage>();
            modPlayer.pEffectEnable = true;
            if (player.head == mod.ItemType("污染之冠"))
            {
                bonus2 = "Mods.Entrogic.PArmorBonus2";
                player.maxMinions += 2;
            }
            player.setBonus = Language.GetTextValue("Mods.Entrogic.PArmorBonus") + "\n" +
                Language.GetTextValue(bonus2);
        }
    }
}
