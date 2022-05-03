using Entrogic.Common.Globals.Players;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.ContyElemental.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaMelee : Equippable
    {
        public override string Texture => $"Entrogic/Content/Items/ContyElemental/Armors/Melee/{Name}";

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mask of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染面具");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityLevelID.MiddleHM;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => head.type == ModContent.ItemType<ContaMelee>() && body.type == ModContent.ItemType<ContaBreastplate>() && legs.type == ModContent.ItemType<ContaGraves>();

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            DamageModify(DamageClass.Melee, .11f);
            CritChanceModify(DamageClass.Melee, 12);

            ArmorSetExtraTip = Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet");
            SpeedModify(DamageClass.Melee, .4f, true);
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
