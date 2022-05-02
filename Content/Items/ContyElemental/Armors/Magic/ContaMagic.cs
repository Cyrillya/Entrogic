using Entrogic.Common.Globals.Players;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.ContyElemental.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaMagic : Equippable
    {
        public override string Texture => $"Entrogic/Content/Items/ContyElemental/Armors/Magic/{Name}";

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Headgear of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染头饰");
            Tooltip.SetDefault("Increase mana regeneration");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "增加魔力再生");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.MiddleHM;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == ModContent.ItemType<ContaMagic>() && body.type == ModContent.ItemType<ContaBreastplate>() && legs.type == ModContent.ItemType<ContaGraves>();

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            // 多运行两次魔力再生代码（加快魔力再生），所以再和魔力再生药水搭配有奇效
            player.UpdateManaRegen();
            player.UpdateManaRegen();
            CritChanceModify(DamageClass.Magic, 8);
            DamageModify(DamageClass.Magic, .13f);

            armorSetExtra = $"{Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet")}";
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
