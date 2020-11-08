using Entrogic.Items.Materials;
using Entrogic.Items.PollutElement.Armor;
using Entrogic.NPCs.Boss.PollutElement;

using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement
{
    public class ContaminatedElementalTreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            DisplayName.SetDefault("宝藏袋");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.expert = true;
        }
        public override int BossBagNPC => NPCType<PollutionElemental>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(ItemType<SoulofContamination>(), Main.rand.Next(20, 25)); // 污染之魂
            player.TryGettingDevArmor(); // 开发者盔甲
            player.QuickSpawnItem(ItemType<BottleofStorm>()); // 专家饰品

            var dropChooser = new WeightedRandom<int>();
            dropChooser.Add(ItemType<ContaminatedLongbow>());
            dropChooser.Add(ItemType<ContaminatedCurrent>());
            dropChooser.Add(ItemType<WaterElementalStaff>());
            int choice = dropChooser;
            player.QuickSpawnItem(choice);
            dropChooser.Clear();

            if (Main.rand.NextFloat() < .666) // 2/3掉落
            {
                dropChooser.Add(ItemType<HelmetofContamination>());
                dropChooser.Add(ItemType<HeadgearofContamination>());
                dropChooser.Add(ItemType<CrownofContamination>());
                dropChooser.Add(ItemType<MaskofContamination>());
                choice = dropChooser;
                player.QuickSpawnItem(choice);
            }
            if (Main.rand.NextFloat() < .666) // 2/3掉落
            {
                player.QuickSpawnItem(ItemType<BreastplateofContamination>());
            }
            if (Main.rand.NextFloat() < .666) // 2/3掉落
            {
                player.QuickSpawnItem(ItemType<GreavesofContamination>());
            }
        }
    }
}