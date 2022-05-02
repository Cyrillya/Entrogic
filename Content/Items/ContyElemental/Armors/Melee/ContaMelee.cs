using Entrogic.Common.Globals.Players;
using Entrogic.Common.ModSystems;
using Entrogic.Content.Items.BaseTypes;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using static Entrogic.SpecialHeadPlayer;

namespace Entrogic.Content.Items.ContyElemental.Armors.Melee
{
    [AutoloadEquip(EquipType.Head)]
    public class ContaMelee : Equippable
    {
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

            armorSetExtra = $"\n{Language.GetTextValue("Mods.Entrogic.ArmorSetBonus.ContaminatedSet")}";
            armorSetDamages.meleeSpeedMultiplier = 0.4f;
            if (inArmorSet) player.GetModPlayer<ContaEffectPlayer>().enable = true;
        }
    }
}
