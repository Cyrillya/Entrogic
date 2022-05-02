using Entrogic.Content.Items.BaseTypes;
using System;

using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.ContyElemental.Armors
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
            equipDamages.moveSpeedMuiltpiler = .16f;
        }
    }
}
