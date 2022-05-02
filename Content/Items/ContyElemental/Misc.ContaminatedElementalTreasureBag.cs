using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Items.ContyElemental.Armors;
using Entrogic.Content.Items.ContyElemental.Weapons;
using Entrogic.Content.Items.ContyElemental.Armors.Melee;
using Entrogic.Content.Items.ContyElemental.Armors.Ranged;
using Entrogic.Content.Items.ContyElemental.Armors.Magic;
using Entrogic.Content.Items.ContyElemental.Armors.Summoner;
using Entrogic.Content.Items.ContyElemental.Armors.Arcane;
using Entrogic.Content.NPCs.Enemies.ContyElemental;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Entrogic.Content.Items.ContyElemental
{
    public class ContaminatedElementalTreasureBag : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            DisplayName.SetDefault("Treasure Bag [Contaminated Elemental]");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "宝藏袋 [污染之灵]");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = -12;
            Item.expert = true;
        }
        public override int BossBagNPC => ModContent.NPCType<ContaminatedElemental>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor(); // 开发者盔甲
            player.QuickSpawnItem(ModContent.ItemType<SoulofContamination>(), Main.rand.Next(20, 25)); // 污染之魂
            player.QuickSpawnItem(ModContent.ItemType<BottleofStorm>()); // 专家饰品

            var dropChooser = new WeightedRandom<int>();
            dropChooser.Add(ModContent.ItemType<ContyLongbow>());
            dropChooser.Add(ModContent.ItemType<ContyCurrent>());
            dropChooser.Add(ModContent.ItemType<SymbioticGelatinStaff>());
            int choice = dropChooser;
            player.QuickSpawnItem(choice);
            dropChooser.Clear();

            var headChooser = new WeightedRandom<int>();
            headChooser.Add(ModContent.ItemType<ContaMelee>());
            headChooser.Add(ModContent.ItemType<ContaRanged>());
            headChooser.Add(ModContent.ItemType<ContaMagic>());
            headChooser.Add(ModContent.ItemType<ContaSummoner>());
            headChooser.Add(ModContent.ItemType<ContaArcane>());

            dropChooser.Add(headChooser);
            dropChooser.Add(ModContent.ItemType<ContaBreastplate>());
            dropChooser.Add(ModContent.ItemType<ContaGraves>());
            choice = dropChooser;
            player.QuickSpawnItem(choice);
            dropChooser.Clear();
        }
    }
}