using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Contaminated.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaSummoner : Equippable
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hood of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染兜帽");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.MiddleHM;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == ModContent.ItemType<ContaSummoner>() && body.type == ModContent.ItemType<ContaBreastplate>() && legs.type == ModContent.ItemType<ContaGraves>();

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            DamageModify(DamageClass.Summon, .09f);
            EquipBenefits.MinionCapacityAdditive = 2;
            EquipBenefits.WhipRangeMultiplier = .25f;

            ArmorSetExtraTip = Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet");
            ArmorSetBenefits.WhipRangeMultiplier = .25f;
            SpeedModify(DamageClass.SummonMeleeSpeed, .40f, true);
            AutoSwingModify(DamageClass.SummonMeleeSpeed, true);
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
