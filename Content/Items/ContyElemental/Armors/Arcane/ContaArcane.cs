using Entrogic.Common.Globals.Players;
using Entrogic.Content.DamageClasses;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.ContyElemental.Armors.Arcane
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaArcane : Equippable
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Domino of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染面罩");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.MiddleHM;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 9;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == ModContent.ItemType<ContaArcane>() && body.type == ModContent.ItemType<ContaBreastplate>() && legs.type == ModContent.ItemType<ContaGraves>();

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            CritChanceModify(ModContent.GetInstance<ArcaneDamageClass>(), 24);
            DamageModify(ModContent.GetInstance<ArcaneDamageClass>(), .30f);

            armorSetExtra = $"{Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet")}";
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
