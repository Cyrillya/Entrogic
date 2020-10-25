using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Items.PollutElement.Armor
{
    /// <summary>
    /// 污染头盔 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/1/22 12:53:14
    /// </summary>
    [AutoloadEquip(EquipType.Head)]
    public class HelmetofContamination : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 6);
            item.rare = RareID.LV8;
            item.defense = 11;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCrit(DamageClass.Ranged) += 7;
            player.GetDamage(DamageClass.Ranged) += 0.17f;
            //player.GetModPlayer<EntrogicPlayer>().ammoCost90 = true;
        }
        public override bool DrawHead()
        {
            return false;
        }
    }
}
