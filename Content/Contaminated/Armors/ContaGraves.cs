using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Contaminated.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ContaGraves : Equippable
    {
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Greaves of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染护胫");
            ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 6);
            Item.rare = RarityLevelID.MiddleHM;
            Item.defense = 11;
        }

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            DamageModify(DamageClass.Generic, .04f);
            EquipBenefits.MoveSpeedMuiltpiler = .16f;
        }
    }
}
