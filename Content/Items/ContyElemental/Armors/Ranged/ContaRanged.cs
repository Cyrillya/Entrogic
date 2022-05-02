using Entrogic.Common.Globals.Players;
using Entrogic.Content.Items.BaseTypes;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.ContyElemental.Armors.Ranged
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaRanged : Equippable
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Helmet of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染头盔");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
        }
        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.MiddleHM;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == ModContent.ItemType<ContaRanged>() && body.type == ModContent.ItemType<ContaBreastplate>() && legs.type == ModContent.ItemType<ContaGraves>();

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            DamageModify(DamageClass.Ranged, .12f);
            CritChanceModify(DamageClass.Ranged, 12);

            armorSetExtra = $"{Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet")}";
            armorSetDamages.autoSwingRanged = true;
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
