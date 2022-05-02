using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.ContyElemental.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ContaBreastplate : Equippable
    {
        public override void Load() {
            base.Load();
            Translation.RegisterTranslation("ArmorSetBonus.ContaminatedSet", GameCulture.CultureName.Chinese, "光环将保护你躲避3次攻击", "The aura will protect you from 3 attacks");
        }

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Breastplate of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染胸甲");
            Tooltip.SetDefault("Increases length of invincibility after taking damage. (Cannot stack with the effects of the Cross Necklace and its derivatives)");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "受伤后增加无敌状态时间。（与十字项链及其衍生品效果不叠加）");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = false;
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 6);
            Item.rare = RarityLevelID.MiddleHM;
            Item.defense = 16;
        }

        public override void SetBonus(Player player, bool inArmorSet) {
            base.SetBonus(player, inArmorSet);

            CritChanceModify(DamageClass.Generic, 8);
            player.longInvince = true;
        }
    }
}
