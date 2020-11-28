using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace Entrogic.Items.Consumables
{
    public class MagicPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            item.useAnimation = 30;
            item.useTime = 30;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.HoldUp;
=======
            item.useStyle = ItemUseStyleID.HoldingUp;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.noMelee = true;
            item.UseSound = SoundID.Item3;
            item.width = 24;
            item.height = 24;
            item.maxStack = 30;
            item.rare = -12;
            item.consumable = true;
        }
        public override bool UseItem(Player player)
        {
            player.KillMe(PlayerDeathReason.ByCustomReason(player.name + Language.GetTextValue("Mods.Entrogic.DieByMagic")), 1000.0, 0, false);
            return true;
        }
    }
}